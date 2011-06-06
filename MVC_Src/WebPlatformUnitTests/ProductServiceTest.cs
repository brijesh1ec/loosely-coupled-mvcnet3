using ProductBehavior;
using System;
using WebPlatformMVCNet.GridUtils;
using Platform.Contracts;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using Rhino.Mocks;
namespace WebPlatformUnitTests
{
    
    
    /// <summary>
    ///This is a test class for ProductServiceTest and is intended
    ///to contain all ProductServiceTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ProductServiceTest
    {

        

        [SetUp]
        public static void ProductServiceTestInitialize()
        {
          

        }

        [TearDown]
        public static void ProductServiceTestCleanup()
        {
           

        }
        /// <summary>
        ///A test for GetAllByFilter
        ///</summary>
        [Test]
        [Description("Verifies that search by criteria returns expected result for paging purposes in GUI")]
        public void GetAllByFilterTest()
        {
            GridSettings filterSettings = new GridSettings();
            filterSettings.PageIndex = 2;
            filterSettings.PageSize = 10;
            filterSettings.IsSearch = false;
            filterSettings.SortColumn = "ProductName";
            filterSettings.SortOrder = "asc";

            int totalNumebr = 0;
            IList<IProduct> list = CollectionsStubInitializer.GenerateProducts(false, 14);
            var listFiltered = list.Skip((filterSettings.PageIndex - 1) * filterSettings.PageSize).Take(filterSettings.PageSize).ToList<IProduct>(); 
            Assert.AreEqual(14, list.Count);

            MockRepository mock = new MockRepository();
            var productsRepositoryMock = mock.StrictMock<IProductRepository>();

            Expect.Call(productsRepositoryMock.GetAllByFilter(filterSettings, out totalNumebr)).IgnoreArguments().Return(listFiltered);

            mock.ReplayAll();

            ProductService productService = new ProductService();
            
            productService.ProductRepository = productsRepositoryMock;
           // var data = query.Skip((filterSettings.PageIndex - 1) * filterSettings.PageSize).Take(filterSettings.PageSize).ToList();
            
            var result = productService.GetAllByFilter(filterSettings, out totalNumebr).ToList<IProduct>();


            Assert.AreEqual(listFiltered.Count, result.Count);
            mock.Verify(productsRepositoryMock);
           
        }

    }
}
