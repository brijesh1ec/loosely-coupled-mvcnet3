using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Contracts;
using WebPlatform.Core;
using log4net;
using System.IO;

namespace ProductsRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ProductRepository()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
        }

        public IEnumerable<IProduct> GetAllByFilter(WebPlatformMVCNet.GridUtils.GridSettings filterSettings, out int totalCount)
        {
            _log.Info(String.Format("Retrieving all products for repository instance of dbContext {0}", DBContext.GetHashCode()));

            var query = DBContext.Products; //.AsQueryable();
            //filtring
            if (filterSettings.IsSearch)
            {
                //And
                if (filterSettings.Where.groupOp == "AND")
                    foreach (var rule in filterSettings.Where.rules)
                        query = query.Where(
                            rule.field, rule.data,
                            (WhereOperation)StringEnum.Parse(typeof(WhereOperation), rule.op));
                else
                {
                    //Or
                    var temp = (new List<IProduct>()).AsQueryable();
                    foreach (var rule in filterSettings.Where.rules)
                    {
                        var t = query.Where(
                        rule.field, rule.data,
                        (WhereOperation)StringEnum.Parse(typeof(WhereOperation), rule.op));
                        temp = temp.Concat(t);
                    }
                    //remove repeating records
                    query = temp.Distinct();
                }
            }

            //sorting
            query = query.OrderBy(filterSettings.SortColumn,
                filterSettings.SortOrder);

            //count
            totalCount = query.Count();

            //paging
            var data = query.Skip((filterSettings.PageIndex - 1) * filterSettings.PageSize).Take(filterSettings.PageSize).ToList();
            return data;
           
        }

        public IEnumerable<IProduct> GetAll()
        {
            throw new NotImplementedException();
        }

        public IProduct GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IDBContext DBContext { get; set; }

        public void Add(IProduct entity)
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
