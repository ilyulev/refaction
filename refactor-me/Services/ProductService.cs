using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using refactor_me.Models;

namespace refactor_me.Services
{
    public class ProductService : IProductService
    {
        public Products GetAll()
        {
            return new Products();
        }

        public Products SearchByName(string name)
        {
            return new Products(name);
        }

        public Product GetProduct(Guid id)
        {
            var product = new Product(id);
            if (product.IsNew)
                return null;
            return product;
        }

        public Product Create(Product product)
        {
            product.Save();
            return product;
        }

        public Product Update(Guid id, Product product)
        {
            var orig = new Product(id)
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DeliveryPrice = product.DeliveryPrice
            };
            if (!orig.IsNew)
                orig.Save();
            throw new ArgumentException("No product was found by id", nameof(id));
        }

        public void Delete(Guid id)
        {
            var product = new Product(id);
            product.Delete();
        }
    }
}