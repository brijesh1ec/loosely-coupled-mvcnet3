using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Contracts.Entities;

namespace Platform.Contracts
{
    public interface IUser : IEntity
    {
        int ID { get; set; }
        string UserName { get; set; }
        string Hash { get; set; }
        string Salt { get; set; }
        Nullable<global::System.Int32> RoleID { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
    }
}
