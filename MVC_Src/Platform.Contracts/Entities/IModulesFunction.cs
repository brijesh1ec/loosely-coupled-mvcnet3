using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Contracts.Entities;

namespace Platform.Contracts
{
    public interface IModulesFunction : IEntity
    {
        global::System.Int32 ID { get; set; }
        global::System.Int32 ModuleID { get; set; }
        global::System.Int32 FunctionID { get; set; }
    }
}
