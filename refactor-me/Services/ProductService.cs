using System;
using System.Linq;
using Microsoft.Practices.Unity;
using refactor_me.Models;

namespace refactor_me.Services
{
    public class ProductService : IProductService
    {
        [Dependency]
        public ProductContext DbContext { get; set; }

        public Products GetAll()
        {
            return new Products
            {
                Items = DbContext.Products.AsQueryable().ToList()
            };
        }

        public Products SearchByName(string name)
        {
            return new Products
            {
                Items = DbContext.Products.AsQueryable().Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList()
            };

        }

        public Product GetProduct(Guid id)
        {
            return DbContext.Products.AsQueryable().FirstOrDefault(p => p.Id == id);
        }

        public Product Create(Product product)
        {
            if (product.Id == Guid.Empty)
                product.Id = Guid.NewGuid();

            DbContext.Products.Add(product);
            DbContext.SaveChanges();
            return product;
        }

        public Product Update(Guid id, Product product)
        {
            var savedProduct = DbContext.Products.AsQueryable().FirstOrDefault(p => p.Id == id);
            if (savedProduct == null)
                throw new ArgumentException("No product was found by id", nameof(id));

            //TODO: better use any of the mapping frameworks, but for now I do manual mapping
            savedProduct.Name = product.Name;
            savedProduct.Description = product.Description;
            savedProduct.Price = product.Price;
            savedProduct.DeliveryPrice = product.DeliveryPrice;

            DbContext.SaveChanges();
            return product;
        }

        public void Delete(Guid id)
        {
            var product = new Product {Id = id};
            DbContext.Products.Attach(product);
            DbContext.Products.Remove(product);

            DbContext.SaveChanges();
        }
    }
}