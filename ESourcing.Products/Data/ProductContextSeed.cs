using ESourcing.Products.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.Products.Data
{
    public class ProductContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(r => true).Any();
            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetConfigureProducts());
            }
        }

        private static IEnumerable<Product> GetConfigureProducts()
        {
            return new List<Product>
            {
                new Product(){Name="IPhone X", Summary ="This product is the ...", Category="Smart Phone", Description = "Lorem ....", Price=950.00M, ImageFile="phone.png"},
                new Product(){Name="Huawei Plus", Summary ="This product is the ...", Category="Smart Phone", Description = "Lorem ....", Price=950.00M, ImageFile="phone2.png"},
                new Product(){Name="Samsung 10", Summary ="This product is the ...", Category="Smart Phone", Description = "Lorem ....", Price=950.00M, ImageFile="phone3.png"}
            };
        }
    }
}
