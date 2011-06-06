using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Contracts.Entities;

namespace Platform.Contracts
{
    public interface IFunction : IEntity
    {
        global::System.Int32 ID { get; set; }
        global::System.String Name { get; set; }
    }
}
