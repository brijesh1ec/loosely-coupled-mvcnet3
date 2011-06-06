using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebPlatformMVCNet.GridUtils;

namespace Platform.Contracts
{
    public interface IProductRepository : IRepository<IProduct>
    {
        IEnumerable<IProduct> GetAllByFilter(GridSettings filterSettings, out int totalCount);
    }
}
