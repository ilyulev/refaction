using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Moq;
using refactor_me.Controllers;
using refactor_me.Models;
using refactor_me.Services;
using Xunit;

namespace refactor_me.Tests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _productService;
        private readonly ProductsController _ctrl;
        public ProductControllerTests()
        {
           _productService = new Mock<IProductService>();
            _ctrl = new ProductsController { ProductService = _productService.Object };
        }

        [Fact]
        public void GetAllTest()
        {
            var products = new Products();
            _productService.Setup(s => s.GetAll()).Returns(() => products);

            Assert.Same(products, _ctrl.GetAll());
        }

        [Fact]
        public void SerachByNameOkTest()
        {
            var products = new Products();
            string name = "name";
            _productService.Setup(s => s.SearchByName(name)).Returns(() => products);

            Assert.Same(products, _ctrl.SearchByName(name));

            _productService.Verify(s => s.SearchByName(name));
        }

        [Fact]
        public void SerachByNameThrowsIfNoNameReceivedTest()
        {
            string name = "";
            _productService.Setup(s => s.SearchByName(name)).Returns(() => null);

            var ex = Assert.Throws<HttpResponseException>(() => _ctrl.SearchByName(name));
            Assert.Equal(HttpStatusCode.BadRequest, ex.Response.StatusCode);
            Assert.Equal("Missed expected parameter 'name'", ex.Response.ReasonPhrase);

            _productService.Verify(s => s.SearchByName(name), Times.Never);
        }
    }
}
