using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Contracts;
using Microsoft.Practices.Unity;
using System.Net.Mail;
using WebPlatform.Core;

namespace AdminBehavior
{
    public class UserService : IUserService
    {
        #region IUserService Members

        
        public IUserRepository UserRepository { get; set; }

        #endregion

        #region IService<IUser> Members

        public IEnumerable<IUser> GetAll()
        {
            return UserRepository.GetAll();
        }

        public IUser GetById(int id)
        {
            return UserRepository.GetById(id);
        }

        public void Delete(IUser entity)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            UserRepository.SaveChanges();
        }

        #endregion

        #region IUserService Members

        public void AddUser(IUser user)
        {
            UserRepository.Add(user);
        }

        public IUser GetByUsername(string username)
        {
            return UserRepository.GetByUsername(username);
        }

        #endregion


        public bool HasUserAccessToModule(int userId, byte moduleId)
        {
            return UserRepository.HasUserAccessToModuleByUserId(userId, moduleId);
        }

        public bool IsUserAuthorized(int userId, byte moduleId, List<byte> functionIds)
        {
            return UserRepository.IsUserAuthorized(userId, moduleId, functionIds);
        }

        public void CreateUser(string username, string password, string firstName, string lastName, string email)
        {
            UserRepository.CreateUser(username, password, firstName, lastName, email);
        }


        #region IUserService Members


        public IUser GetByEmail(string email)
        {
            return UserRepository.GetByEmail(email);
        }

        public void RecoverPasswordByEmail(string email)
        {
            var user = GetByEmail(email);
            if (user == null)
                throw new ArgumentException("The email that you provided does not exist in our records.");

            string guid = Guid.NewGuid().ToString("N");
            string salt = null;
            string hash;
            WebSecurity.HashWithSalt(guid, ref salt, out hash);

            user.Salt = salt;
            user.Hash = hash;

            SaveChanges();

            MailMessage msg = new MailMessage("nik.raykov@gmail.com", email);
            msg.Subject = "Your new password!";
            msg.Body = guid;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Send(msg);
        }

        #endregion
    }
}
