using E_Sourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
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
    public class BidController : ControllerBase
    {
        #region Variables

        private readonly IBidRepository _bidRepository;
        private readonly ILogger<BidController> _logger;

        #endregion

        #region Constructor

        public BidController(IBidRepository bidRepository, ILogger<BidController> logger)
        {
            _bidRepository = bidRepository;
            _logger = logger;
        }

        #endregion

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> SenBid([FromBody]Bid bid)
        {
            await _bidRepository.SenBid(bid);
            return Ok();
        }

        [HttpGet("GetBidsByAuctionId")]
        [ProducesResponseType(typeof(IEnumerable<Bid>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBidsByAuctionId(string id)
        {
            return Ok( await _bidRepository.GetBidsByAuctionId(id));
        }

        [HttpGet("GetWinnerBid")]
        [ProducesResponseType(typeof(Bid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Bid>> GetWinnerBid(string id)
        {
            return Ok(await _bidRepository.GetWinnerBid(id));
        }

    }
}
