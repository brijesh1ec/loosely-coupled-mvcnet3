using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Contracts;

namespace WebPlatformUnitTests
{
    public class ProductStub : IProduct
    {
        public int Id { get; set; }

        public string ProductDescription { get; set; }

        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }
        
    }
}
