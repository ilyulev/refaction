using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using refactor_me.Models;
using refactor_me.Services;
using Xunit;

namespace refactor_me.Tests
{
    public class ProductServiceTests
    {
        private readonly IProductService _productService;
        Mock<ProductContext> mockContext = new Mock<ProductContext>();
        private static IQueryable<Product> _products = new List<Product>
        {
            new Product {Name = "BBB", Id = new Guid("434b8d53-4521-4061-a121-409eabcb6ad6")},
            new Product {Name = "ZZZ", Id = new Guid("534b8d53-4521-4061-a121-409eabcb6ad6")},
            new Product {Name = "AAA", Id = new Guid("634b8d53-4521-4061-a121-409eabcb6ad6")},
        }.AsQueryable();
        Mock<DbSet<Product>> mockSet;

        public ProductServiceTests()
        {
            var queribleProducts = _products.AsQueryable();
            mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(queribleProducts.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(queribleProducts.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(queribleProducts.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => queribleProducts.GetEnumerator());

            _productService = new ProductService {DbContext = mockContext.Object};
        }

        [Fact]
        public void GetAllTest()
        {
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            var products = _productService.GetAll();

            Assert.Equal(3, products.Items.Count);
            Assert.True(products.Items.Any(p => p.Name == "AAA"));
            Assert.True(products.Items.Any(p => p.Name == "BBB"));
            Assert.True(products.Items.Any(p => p.Name == "ZZZ"));
        }


        [Fact]
        public void SearchByNameTest()
        {
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            var products = _productService.SearchByName("aa");

            Assert.Equal(1, products.Items.Count);
            Assert.True(products.Items.Any(p => p.Name == "AAA"));
        }
    }
    
}