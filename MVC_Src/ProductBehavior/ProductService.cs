using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Contracts;

namespace ProductBehavior
{
    public class ProductService : IProductService
    {
        public IProductRepository ProductRepository { get; set; }


        public IEnumerable<IProduct> GetAllByFilter(WebPlatformMVCNet.GridUtils.GridSettings filter, out int totalCount)
        {
            return ProductRepository.GetAllByFilter(filter, out totalCount);
        }

        public IEnumerable<IProduct> GetAll()
        {
            throw new NotImplementedException();
        }

        public IProduct GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(IProduct entity)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
