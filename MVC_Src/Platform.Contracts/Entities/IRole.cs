using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Contracts.Entities;

namespace Platform.Contracts
{
    public interface IRole : IEntity
    {
        int ID { get; set; }
        string Name { get; set; }
    }
}
