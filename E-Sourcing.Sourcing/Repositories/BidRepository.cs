using E_Sourcing.Sourcing.Data;
using E_Sourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly ISourcingContext _context;

        public BidRepository(ISourcingContext context)
        {
            _context = context;
        }

        public async Task<List<Bid>> GetBidsByAuctionId(string auctionId)
        {
            var filter = Builders<Bid>.Filter.Eq(m => m.AuctionId, auctionId);
            List<Bid> bids = await _context.Bids.Find(filter).ToListAsync();

            bids = bids.OrderByDescending(a => a.CreatedAt).GroupBy(a => a.SellerUserName).Select(a => new
                    Bid()
                    {
                          AuctionId = a.FirstOrDefault().AuctionId,
                          Price = a.FirstOrDefault().Price,
                          SellerUserName = a.FirstOrDefault().SellerUserName,
                          CreatedAt = a.FirstOrDefault().CreatedAt,
                          ProductId = a.FirstOrDefault().ProductId,
                          Id = a.FirstOrDefault().Id
                    }
            ).ToList();

            return bids;
        }

        public async Task<Bid> GetWinnerBid(string id)
        {
            List<Bid> bids = await GetBidsByAuctionId(id);

            return bids.OrderByDescending(r => r.Price).FirstOrDefault();
        }

        public async Task SenBid(Bid bid)
        {
            await _context.Bids.InsertOneAsync(bid);
        }
    }
}
