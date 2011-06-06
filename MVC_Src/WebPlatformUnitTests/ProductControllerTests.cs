using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WebPlatformMVCNet.Controllers;
using WebPlatformMVCNet.GridUtils;
using Platform.Contracts;
using Rhino.Mocks;
using System.Web.Mvc;

namespace WebPlatformUnitTests
{
    [TestFixture]
    public class ProductControllerTests
    {
        public static MockRepository mocks { get; set; }

        [SetUp]
        public static void ProductControllerTestsInitialize()
        {
            mocks = new MockRepository();
        }

        [TearDown]
        public static void ProductControllerTestsCleanup()
        {
            mocks = null;
        }


        [Test]
        [Description("Verifies that search by criteria returns expected result in JSON format")]
        public void GetDataTest()
        {
            ProductController controller = new ProductController();
            GridSettings filterSettings = new GridSettings();
            filterSettings.PageIndex = 2;
            filterSettings.PageSize = 10;
            filterSettings.IsSearch = false;
            filterSettings.SortColumn = "ProductName";
            filterSettings.SortOrder = "asc";
            
            var products = CollectionsStubInitializer.GenerateProducts(false, 14);
            int totalNumber = 0;
            IProductService productServiceMock = mocks.StrictMock<IProductService>();

            Expect.Call(productServiceMock.GetAllByFilter(filterSettings, out totalNumber)).IgnoreArguments().Return(products).OutRef( products.Count);

            mocks.ReplayAll();

            controller.ProductService = productServiceMock;
            JsonResult result = controller.GetData(filterSettings);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);

            //get expected data properties and make assert assumptions
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var output = serializer.Serialize(result.Data);
            var obj = serializer.Deserialize<ResultSet>(output);

            Assert.AreEqual(14, obj.Rows.Count());
            Assert.AreEqual(products[0].Id, obj.Rows[0].Id);
            Assert.AreEqual(products[0].ProductName, obj.Rows[0].ProductName);
            Assert.AreEqual(2, obj.Page);
            Assert.AreEqual(2, obj.Total);
            Assert.AreEqual(14, obj.Records);

            mocks.Verify(productServiceMock);

        }
    }
}
