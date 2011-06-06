using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Platform.Contracts
{
    public interface IUserService : IService<IUser>
    {
        void AddUser(IUser user);
        bool HasUserAccessToModule(int userId, byte moduleId);
        IUser GetByUsername(string username);
        bool IsUserAuthorized(int userId, byte moduleId, List<byte> functionIds);
        void CreateUser(string username, string password, string firstName, string lastName, string email);
        IUser GetByEmail(string email);
        void RecoverPasswordByEmail(string email);
    }
}
