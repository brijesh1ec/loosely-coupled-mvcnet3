using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Contracts.Entities;

namespace Platform.Contracts
{
    public interface IProduct : IEntity
    {
        int Id { get; set; }
        string ProductDescription { get; set; }
        string ProductName { get; set; }
        decimal UnitPrice { get; set; }
    }
}
