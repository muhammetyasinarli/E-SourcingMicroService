using E_Sourcing.Sourcing.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Repositories.Interfaces
{
    public interface IBidRepository
    {
        Task SenBid(Bid bid);
        Task<List<Bid>> GetBidsByAuctionId(string auctionId);
        Task<Bid> GetWinnerBid(string id);
    }
}
