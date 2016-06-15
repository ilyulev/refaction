using System;
using System.Net;
using System.Net.Http;
using Microsoft.Practices.Unity;
using System.Web.Http;
using refactor_me.Models;
using refactor_me.Services;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        [Dependency]
        public IProductService ProductService { get; set; }


        [Route]
        [HttpGet]
        public Products GetAll()
        {
            return ProductService.GetAll();
        }

        [Route]
        [HttpGet]
        public Products SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                //unfortunately, there's no 422 response code
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = "Missed expected parameter 'name'"
                });

            return ProductService.SearchByName(name);
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            var product = ProductService.GetProduct(id);
            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NoContent);

            return product;
        }

        [Route]
        [HttpPost]
        public void Create(Product product)
        {
            try
            {
                ProductService.Create(product);
            }
            catch (ArgumentException e)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = $"Incorrect parameter {e.ParamName}: {e.Message}"
                });
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = $"Cannot create the product: {e.Message}"
                });
            }
        }

        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, Product product)
        {
            if (id == Guid.Empty)
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = "Missed expected parameter 'id'"
                });

            try
            {
                ProductService.Update(id, product);
            }
            catch (ArgumentException e)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = $"Incorrect parameter {e.ParamName}: {e.Message}"
                });
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = $"Cannot update the product: {e.Message}"
                });
            }
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            if (id == Guid.Empty)
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = "Missed expected parameter 'id'"
                });

            try
            {
                ProductService.Delete(id);
            }
            catch (ArgumentException e)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = $"No product available for id= '{id}'"
                });
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = $"Cannot delete the product: {e.Message}"
                });
            }
        }
    }
}
