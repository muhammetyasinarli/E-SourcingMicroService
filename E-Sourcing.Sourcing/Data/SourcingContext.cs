using E_Sourcing.Sourcing.Entities;
using E_Sourcing.Sourcing.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Sourcing.Sourcing.Data
{
    public class SourcingContext : ISourcingContext
    {
        public SourcingContext(ISourcingDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.Database);
            Auctions = database.GetCollection<Auction>("Auction");
            Bids = database.GetCollection<Bid>("Bid");

            SourcingContextSeed.SeedData(Auctions);
        }

        public IMongoCollection<Auction> Auctions { get; }

        public IMongoCollection<Bid> Bids { get; }
    }
}
