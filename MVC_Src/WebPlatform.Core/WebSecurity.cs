using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web.Security;

namespace WebPlatform.Core
{
    public class WebSecurity
    {
        private const string DefCryptoAlg = "sha1";

        public static void HashWithSalt(
            string plaintext, ref string salt, out string hash)
        {
            const int SALT_BYTE_COUNT = 16;
            if (salt == null || salt == "")
            {
                byte[] saltBuf = new byte[SALT_BYTE_COUNT];
                RNGCryptoServiceProvider rng =
                    new RNGCryptoServiceProvider();
                rng.GetBytes(saltBuf);

                StringBuilder sb =
                    new StringBuilder(saltBuf.Length);
                for (int i = 0; i < saltBuf.Length; i++)
                    sb.Append(string.Format("{0:X2}", saltBuf[i]));
                salt = sb.ToString();
            }

            hash = FormsAuthentication.
                HashPasswordForStoringInConfigFile(
                salt + plaintext, DefCryptoAlg);
        }
        public static string Encrypt(string plaintext)
        {
            /* Although designed to encrypt time-stamped tickets, 
             * using FormsAuthentication.Encrypt is by far the simplest 
             * way to encrypt strings. It does incur a small amount 
             * of additional space to store two date-time values and 
             * the size of the FormsAuthenticationTicket itself. 
             * The other advantage of this technique is that the 
             * encryption key is auto-generated and stored as an 
             * LSA secret for you. Be aware that the key is 
             * server-specific, and if you need to scale the 
             * application to a web farm you should set the 
             * decryption key in machine.config on all machines 
             * in the farm so that cross-machine 
             * encryption/decryption works properly */

            FormsAuthenticationTicket ticket;
            ticket = new FormsAuthenticationTicket(1, "", DateTime.Now,
                DateTime.Now, false, plaintext, "");

            return FormsAuthentication.Encrypt(ticket);
        }

        public static string Decrypt(string ciphertext)
        {
            FormsAuthenticationTicket ticket;
            ticket = FormsAuthentication.Decrypt(ciphertext);
            return ticket.UserData;
        }

    }
}
