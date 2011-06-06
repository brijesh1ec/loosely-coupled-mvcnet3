using System;
using System.Data.Entity;
using System.Linq;
using Platform.Contracts;
using System.Data.Objects;

namespace WebPlatform.Data
{
    public partial class MVCDbEntitiesConnection : IDBContext
    {

        #region IDBContext Members

        IQueryable<IUser> IDBContext.Users
        {
            get
            {
                return this.Users.AsQueryable();
            }
        }

        IQueryable<IAccessToModuleFunctions> IDBContext.AccessToModuleFunctions
        {
            get
            {
                return this.AccessToModuleFunctions.AsQueryable();
            }
        }

        IQueryable<IModulesFunction> IDBContext.ModulesFunctions
        {
            get
            {
                return this.ModulesFunctions.AsQueryable();
            }
        }


        IQueryable<IModule> IDBContext.Modules
        {
            get
            {
                return this.Modules.AsQueryable();
            }
        }

        IQueryable<IFunction> IDBContext.Functions
        {
            get
            {
                return this.Functions.AsQueryable();
            }
        }

        IQueryable<IRole> IDBContext.Roles
        {
            get
            {
                return this.Roles.AsQueryable();
            }
        }

        IQueryable<IProduct> IDBContext.Products
        {
            get
            {
                return this.Products.AsQueryable();
            }
        }

        IQueryable<IOrder> IDBContext.Orders
        {
            get
            {
                return this.Orders.AsQueryable();
            }
        }


        public void AddToUsers(IUser user)
        {
            this.AddToUsers((User)user);
        }

        public new void SaveChanges()
        {
            ((ObjectContext)this).SaveChanges();
        }

        #endregion


        public IUser CreateUser(int id, string userName, string hash, string salt, string firstName, string lastName, string email)
        {
            return User.CreateUser(id, userName, hash, salt, firstName, lastName, email);
        }
    }

    public partial class User : IUser
    { 
    
    }

    public partial class AccessToModuleFunction : IAccessToModuleFunctions
    {

    }

    public partial class ModulesFunction : IModulesFunction
    {
    }


    public partial class Module : IModule
    {

    }

    public partial class Function : IFunction
    {

    }


    public partial class Role : IRole
    { 
        
    }

    public partial class Product : IProduct
    { 
        
    }

    public partial class Order : IOrder
    { 
    
    }

}
