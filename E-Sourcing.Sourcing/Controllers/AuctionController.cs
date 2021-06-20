using AutoMapper;
using E_Sourcing.Sourcing.Entities;
using E_Sourcing.Sourcing.Repositories.Interfaces;
using ESourcing.Sourcing.Repositories.Interfaces;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events.Interfaces;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace E_Sourcing.Sourcing.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        #region Variables

        private readonly IAuctionRepository _auctionRepository;
        private readonly ILogger<AuctionController> _logger;
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMQProducer _eventBus;
        #endregion

        #region Constructor

        public AuctionController(IAuctionRepository auctionRepository, 
            ILogger<AuctionController> logger,
            IBidRepository bidRepository,
            IMapper mapper,
            EventBusRabbitMQProducer eventBus
            )
        {
            _auctionRepository = auctionRepository;
            _logger = logger;
            _bidRepository = bidRepository;
            _mapper = mapper;
            _eventBus = eventBus;
        }

        #endregion

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Auction>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAuctions()
        {
            var auctions = await _auctionRepository.GetAuctions();
            return Ok(auctions);
        }

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Auction>> GetAuction(string id)
        {
            var auction = await _auctionRepository.GetAuction(id);

            if (auction == null)
            {
                _logger.LogError($"AuctionId : {id} has not been found in DB");
                return NotFound();
            }

            return Ok(auction);

        }

        [HttpPost]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Auction>> Create([FromBody] Auction auction)
        {
            await _auctionRepository.Create(auction);

            return CreatedAtRoute("GetAuction", new { id = auction.Id }, auction);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> Update([FromBody] Auction auction)
        {
            return  Ok(await _auctionRepository.Update(auction));
        }

        [HttpDelete("id:length(50)")]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> Delete(string id)
        {
            return Ok(await _auctionRepository.Delete(id));
        }

        [HttpPost("CompleteAction")]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CompleteAction(string id)
        {
            Auction auction = await _auctionRepository.GetAuction(id);
            if(auction == null)
            {
                return NotFound();
            }

            if(auction.Status != (int)Status.Active)
            {
                _logger.LogError("Auction can not be completed");
                return BadRequest();
            }

            Bid bid = await _bidRepository.GetWinnerBid(id);
            if(bid == null)
            {
                return NotFound();
            }

            OrderCreateEvent eventMessage = _mapper.Map<OrderCreateEvent>(bid);
            eventMessage.Quantity = auction.Quantity;

            auction.Status = (int)Status.Closed;

            bool updateResponse  = await _auctionRepository.Update(auction);

            if (!updateResponse)
            {
                _logger.LogError("Auction can be updated");
                return BadRequest();
            }

            try
            {
                _eventBus.Publish(EventBusConstants.OrderCreateQueue, eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR : Publishing integration event : {EventId} from {AppName}", eventMessage.Id, "Sourcing");
                throw;
            }

            return Accepted();
        }

        [HttpPost("TestEvent")]
        public ActionResult<OrderCreateEvent> TestEvent()
        {
            OrderCreateEvent orderCreateEvent = new OrderCreateEvent()
            {
                AuctionId = "dummy1",
                ProductId ="dummy_product_1",
                Price = 10,
                Quantity  =100,
                SellerUserName = "TEST@test.com"
            };

            try
            {
                _eventBus.Publish( EventBusConstants.OrderCreateQueue , orderCreateEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR : Publishing integration event : {EventId} from {AppName}", orderCreateEvent.Id, "Sourcing");
                throw;
            }

            return Accepted();
        }
    }
}
