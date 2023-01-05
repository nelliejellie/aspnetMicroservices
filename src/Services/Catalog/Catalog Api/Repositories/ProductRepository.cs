using Catalog_Api.Data;
using Catalog_Api.Entities;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catalog_Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> dbcollection;
        private readonly FilterDefinitionBuilder<Product> filterBuilder = Builders<Product>.Filter;
        public ProductRepository(IMongoDatabase database, string collectionName)
        {
            dbcollection = database.GetCollection<Product>(collectionName);
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            try
            {
                var products = await dbcollection.Find(filterBuilder.Empty).ToListAsync();
                return products;

            }catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return null;
            }
        }

        public async Task<Product> GetProduct(string id)
        {
            FilterDefinition<Product> filter = filterBuilder.Eq(entity => entity.Id, id);
            return await dbcollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = filterBuilder.Eq(entity => entity.Name, name);
            return await dbcollection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductBYCategory(string category)
        {
            FilterDefinition<Product> filter = filterBuilder.Eq(entity => entity.Category, category);
            return await dbcollection.Find(filter).ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await dbcollection.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await dbcollection
                                        .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = filterBuilder.Eq(entity => entity.Id, id);
            await dbcollection.DeleteOneAsync(filter);
        }
    }
}
