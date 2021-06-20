using E_Sourcing.Sourcing.Data;
using E_Sourcing.Sourcing.Entities;
using E_Sourcing.Sourcing.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Sourcing.Sourcing.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly ISourcingContext _context;
        public AuctionRepository(ISourcingContext context)
        {
            _context = context;
        }

        public async Task Create(Auction auction)
        {
            await _context.Auctions.InsertOneAsync(auction);
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Auction>.Filter.Eq(m => m.Id, id);
            DeleteResult deleteResult = await _context.Auctions.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<Auction> GetAuction(string id)
        {
            return await _context.Auctions.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Auction>> GetAuctionByName(string name)
        {
            var filter = Builders<Auction>.Filter.Eq(m => m.Name, name);
            return await _context.Auctions.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetAuctions()
        {
            return await _context.Auctions.Find(r => true).ToListAsync();
        }

        public async Task<bool> Update(Auction auction)
        {
            var updateResult = await _context.Auctions.ReplaceOneAsync(filter: g => g.Id == auction.Id, replacement: auction);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
