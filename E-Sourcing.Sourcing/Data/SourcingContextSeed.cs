using E_Sourcing.Sourcing.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Sourcing.Sourcing.Data
{
    public class SourcingContextSeed
    {
        public static void SeedData(IMongoCollection<Auction> auctionCollection)
        {
            bool existAuction = auctionCollection.Find(r => true).Any();
            if (!existAuction)
            {
                auctionCollection.InsertManyAsync(GetConfigureProducts());
            }
        }

        private static IEnumerable<Auction> GetConfigureProducts()
        {
            return new List<Auction> {
                new Auction()
                {
                    Name = "Auction 1",
                    Description = "Bla bla bla ...",
                    CreatedAt = DateTime.Now,
                    StartedAt = DateTime.Now,
                    FinishedAt = DateTime.Now,
                    ProductId = "60093337093d7352d5467341",
                    IncludedSellers = new List<string>()
                    {
                        "abc@test.com",
                        "bbc@test.com",
                        "ccc@test.com"
                    },
                    Quantity = 5,
                    Status = (int)Status.Active

                },
                 new Auction()
                {
                    Name = "Auction 2",
                    Description = "Bla bla bla ...",
                    CreatedAt = DateTime.Now,
                    StartedAt = DateTime.Now,
                    FinishedAt = DateTime.Now,
                    ProductId = "60093337093d7352d5467341",
                    IncludedSellers = new List<string>()
                    {
                        "abc@test.com",
                        "bbc@test.com",
                        "ccc@test.com"
                    },
                    Quantity = 5,
                    Status = (int)Status.Active

                },
                  new Auction()
                {
                    Name = "Auction 3",
                    Description = "Bla bla bla ...",
                    CreatedAt = DateTime.Now,
                    StartedAt = DateTime.Now,
                    FinishedAt = DateTime.Now,
                    ProductId = "60093337093d7352d5467341",
                    IncludedSellers = new List<string>()
                    {
                        "abc@test.com",
                        "bbc@test.com",
                        "ccc@test.com"
                    },
                    Quantity = 5,
                    Status = (int)Status.Active

                }
            };
        }
    }
}
