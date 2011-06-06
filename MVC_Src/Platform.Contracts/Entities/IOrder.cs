using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Contracts.Entities;

namespace Platform.Contracts
{
    public interface IOrder : IEntity
    {
        string ClientNames { get; set; }
        int Id { get; set; }
        DateTime OrderDate { get; set; }
        int ProductId { get; set; }
        int Quantity { get; set; }
    }
}
