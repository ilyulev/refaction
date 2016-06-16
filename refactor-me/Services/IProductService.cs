using System;
using System.Collections.Generic;
using refactor_me.Models;

namespace refactor_me.Services
{
    public interface IProductService
    {
        Products GetAll();
        Products SearchByName(string name);
        Product GetProduct(Guid id);
        Product Create(Product product);
        Product Update(Guid id, Product product);
        void Delete(Guid id);
    }
}