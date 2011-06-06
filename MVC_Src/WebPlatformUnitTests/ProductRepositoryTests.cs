using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using WebPlatformMVCNet.GridUtils;
using Platform.Contracts;

namespace WebPlatformUnitTests
{
    [TestFixture]
    public class ProductRepositoryTests
    {

        [Test]
        [Description("")]
        public void GetAllProductsByCritearia()
        {
            IList<IProduct> list = CollectionsStubInitializer.GenerateProducts(false, 14);

            MockRepository mock = new MockRepository();
            var dbContextMock = mock.PartialMock<DBFakeContext>();
            SetupResult.For(dbContextMock.Products).Return(list.AsQueryable());
            mock.ReplayAll();

            ProductsRepository.ProductRepository repo = new ProductsRepository.ProductRepository();
            repo.DBContext = dbContextMock;
            int i = 0;
            GridSettings filterSettings = new GridSettings();
            filterSettings.PageIndex = 1;
            filterSettings.PageSize = 10;
            filterSettings.IsSearch = false;
            filterSettings.SortColumn = "ProductName";
            filterSettings.SortOrder = "asc";

            var products = repo.GetAllByFilter(filterSettings, out i);
            Assert.AreEqual(10, products.Count()); //it is filtered
            mock.Verify(dbContextMock);

        }
    }
}
