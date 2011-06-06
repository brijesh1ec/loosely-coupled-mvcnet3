using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Contracts;
using Microsoft.Practices.Unity;
using WebPlatform.Core;
using log4net;
using System.IO;

namespace AdminRepository
{
    public class UsersRepository : IUserRepository
    {
        private readonly ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UsersRepository()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

        }

        public IEnumerable<IUser> GetAll()
        {

            return DBContext.Users.AsEnumerable();
        }

        public IUser GetById(int id)
        {
            return DBContext.Users.FirstOrDefault(u => u.ID == id);
        }


        //[Dependency]
        public IDBContext DBContext { get; set; }

        public void Add(IUser entity)
        {
            DBContext.AddToUsers(entity);
        }

        public void Delete(IUser entity)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            DBContext.SaveChanges();
        }


        public bool HasUserAccessToModuleByUserId(int userId, byte moduleId)
        {
            try
            {
                int functionId = Convert.ToInt32(Functions.Access);

                //var q = from user in DBContext.Users
                //        join role in DBContext.Roles on user.RoleID equals role.ID
                //        join accessToModuleFunction in DBContext.AccessToModuleFunctions on role.ID equals accessToModuleFunction.RoleID
                //        join moduleFunctions in DBContext.ModulesFunctions on accessToModuleFunction.ModuleFunctionID equals moduleFunctions.ID
                //        join module in DBContext.Modules on moduleFunctions.ModuleID equals module.ID
                //        join function in DBContext.Functions on moduleFunctions.FunctionID equals function.ID
                //        where user.ID == userId && module.ID == moduleId && function.ID == functionId && accessToModuleFunction.HasAccess == true
                //        select user;

                var userAccessNoRoles = from user in DBContext.Users
                        join accessToModuleFunction in DBContext.AccessToModuleFunctions on user.ID equals accessToModuleFunction.UserID
                        join moduleFunctions in DBContext.ModulesFunctions on accessToModuleFunction.ModuleFunctionID equals moduleFunctions.ID
                        join module in DBContext.Modules on moduleFunctions.ModuleID equals module.ID
                        join function in DBContext.Functions on moduleFunctions.FunctionID equals function.ID
                        where user.ID == userId && module.ID == moduleId && function.ID == functionId 
                        select accessToModuleFunction;

                if (userAccessNoRoles.ToList().Count > 0)
                {
                    return (bool)userAccessNoRoles.First().HasAccess;
                }
                else
                {
                    var userAccessWithRoles = from user in DBContext.Users
                                                join role in DBContext.Roles on user.RoleID equals role.ID
                                                join accessToModuleFunction in DBContext.AccessToModuleFunctions on user.RoleID equals accessToModuleFunction.RoleID
                                                join moduleFunctions in DBContext.ModulesFunctions on accessToModuleFunction.ModuleFunctionID equals moduleFunctions.ID
                                                join module in DBContext.Modules on moduleFunctions.ModuleID equals module.ID
                                                join function in DBContext.Functions on moduleFunctions.FunctionID equals function.ID
                                                where user.ID == userId && module.ID == moduleId && function.ID == functionId
                                                select accessToModuleFunction;

                    if (userAccessWithRoles.ToList().Count > 0)
                    {
                        return (bool)userAccessWithRoles.First().HasAccess;
                    }
                    else
                        return false;
                }
               
              
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        
        public IUser GetByUsername(string username)
        {
            
            return DBContext.Users.FirstOrDefault(u => u.UserName == username);
        }

        public bool IsUserAuthorized(int userId, byte moduleId, byte functionId)
        {
            var userAccessQuery = from user in DBContext.Users
                             join accessToModuleFunction in DBContext.AccessToModuleFunctions on user.ID equals accessToModuleFunction.UserID
                             join moduleFunctions in DBContext.ModulesFunctions on accessToModuleFunction.ModuleFunctionID equals moduleFunctions.ID
                             join module in DBContext.Modules on moduleFunctions.ModuleID equals module.ID
                             join function in DBContext.Functions on moduleFunctions.FunctionID equals function.ID
                             where user.ID == userId && module.ID == moduleId && function.ID == functionId
                             select accessToModuleFunction;

            if (userAccessQuery.Count() > 0)
                return userAccessQuery.First().HasAccess ?? false;

            var roleAccessQuery = from user in DBContext.Users
                                  join role in DBContext.Roles on user.RoleID equals role.ID
                                  join accessToModuleFunction in DBContext.AccessToModuleFunctions on role.ID equals accessToModuleFunction.RoleID
                                  join moduleFunctions in DBContext.ModulesFunctions on accessToModuleFunction.ModuleFunctionID equals moduleFunctions.ID
                                  join module in DBContext.Modules on moduleFunctions.ModuleID equals module.ID
                                  join function in DBContext.Functions on moduleFunctions.FunctionID equals function.ID
                                  where user.ID == userId && module.ID == moduleId && function.ID == functionId
                                  select accessToModuleFunction;

            if (roleAccessQuery.Count() > 0)
                return roleAccessQuery.First().HasAccess ?? false;
            else
                return false;
        }

        public bool IsUserAuthorized(int userId, byte moduleId, List<byte> functionIds)
        {
            bool isAuthorized = true;
            foreach (byte functionId in functionIds)
            {
                isAuthorized = IsUserAuthorized(userId, moduleId, functionId);
                if (!isAuthorized) break;
            }

            return isAuthorized;
        }

        public void CreateUser(string username, string password, string firstName, string lastName, string email)
        {
            if (DBContext.Users.Count(u => u.UserName == username) > 0)
                throw new ArgumentException("Duplicate username.");

            string salt = null;
            string hash;
            WebSecurity.HashWithSalt(password, ref salt, out hash);

            var user = DBContext.CreateUser(0, username, hash, salt, firstName, lastName, email);
            user.RoleID = 1;

            Add(user);
            SaveChanges();
        }

        #region IUserRepository Members


        public IUser GetByEmail(string email)
        {
            return DBContext.Users.FirstOrDefault(u => u.Email == email);
        }

        #endregion
    }
}
