using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Contracts.Entities;

namespace Platform.Contracts
{
    public interface IAccessToModuleFunctions : IEntity
    {
        global::System.Int32 ID { get; set; }
        Nullable<global::System.Int32> UserID {get; set;}
        Nullable<global::System.Int32> RoleID { get; set; }
        Nullable<global::System.Int32> ModuleFunctionID { get; set; }
        Nullable<global::System.Boolean> HasAccess { get; set; }
    }
}
