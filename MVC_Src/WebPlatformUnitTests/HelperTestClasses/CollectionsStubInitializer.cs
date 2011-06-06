using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Contracts;


namespace WebPlatformUnitTests
{
    public class CollectionsStubInitializer
    {
        public static IList<IProduct> GenerateProducts(bool emptyCollection, int size)
        {
            IList<IProduct> productsList = new List<IProduct>();
            if (emptyCollection) return productsList;


            for(int i = 0; i < size; i++)
            {
                IProduct product = new ProductStub();
                product.Id = i + 1;
                product.ProductDescription = "Who cares about this";
                product.ProductName = String.Concat(i.ToString(), " Name");
                product.UnitPrice = RandomPositiveDecimal();

                productsList.Add(product);
            }


            return productsList;
        }

        #region helper
        private static decimal RandomPositiveDecimal()
        {
            Random rnd = new Random();
            var value = rnd.Next(5) + rnd.Next(99) / 100d;
            return (decimal)value;
        }
        #endregion
    }

    public abstract class DBFakeContext : WebPlatform.Data.MVCDbEntitiesConnection, IDBContext
    {
        public DBFakeContext()
            : base("metadata=res://*/MVCDbModel.csdl|res://*/MVCDbModel.ssdl|res://*/MVCDbModel.msl;provider=System.Data.SqlClient;provider connection string='Data Source=(local);Initial Catalog=MVCNet;Integrated Security=True;MultipleActiveResultSets=True'")
        { }

        //public abstract new IList<IProduct> Products { get; set; }
        public abstract new IQueryable<IProduct> Products { get; set; }
    }
  
}
