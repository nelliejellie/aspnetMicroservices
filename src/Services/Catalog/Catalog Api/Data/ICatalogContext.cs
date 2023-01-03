using Catalog_Api.Entities;
using MongoDB.Driver;

namespace Catalog_Api.Data
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products{get;}
    }
}
