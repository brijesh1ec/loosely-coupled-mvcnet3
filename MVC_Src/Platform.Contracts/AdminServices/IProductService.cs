using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebPlatformMVCNet.GridUtils;

namespace Platform.Contracts
{
    public interface IProductService : IService<IProduct>
    {
        IEnumerable<IProduct> GetAllByFilter(GridSettings filter, out int totalCount);

        IProductRepository ProductRepository { get; set; }

    }
}
