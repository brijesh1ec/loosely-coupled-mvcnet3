using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platform.Contracts
{
    public interface IUserRepository : IRepository<IUser>
    {

        bool HasUserAccessToModuleByUserId(int userId, byte moduleId);

        IUser GetByUsername(string username);

        bool IsUserAuthorized(int userId, byte moduleId, byte functionId);

        bool IsUserAuthorized(int userId, byte moduleId, List<byte> functionIds);

        void CreateUser(string username, string password, string firstName, string lastName, string email);

        IUser GetByEmail(string email);
    }
}
