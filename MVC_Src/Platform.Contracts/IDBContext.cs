using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platform.Contracts
{
    public interface IDBContext
    {
        IQueryable<IUser> Users { get; }
        IQueryable<IAccessToModuleFunctions> AccessToModuleFunctions { get; }
        IQueryable<IModulesFunction> ModulesFunctions { get; }

        IQueryable<IModule> Modules { get; }
        IQueryable<IFunction> Functions { get; }

        IQueryable<IRole> Roles { get; }
        IQueryable<IProduct> Products { get; }
        IQueryable<IOrder> Orders { get; }


        void AddToUsers(IUser user);
        IUser CreateUser(Int32 id, String userName, String hash, String salt, String firstName, String lastName, String email);

        void SaveChanges();
    }
}
