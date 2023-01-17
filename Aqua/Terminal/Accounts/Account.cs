using Aqua.Utils.Security;
using Microsoft.VisualBasic;
using System.Text;

namespace Aqua.Terminal.Accounts
{
    public class Account
    {
        private string username, password;
        private Roles role;

        public Account(string username, Roles role)
        {
            this.username = username;
            this.password = null;
            this.role = role;
        }

        public Account(string username, Roles role, string password)
        {
            this.username = username;
            this.password = password;
            this.role = role;
        }

        public string GetUsername()
        {
            return username;
        }

        public Roles GetRole()
        {
            return role;
        }

        public bool VerifyPassword(string needle)
        {
            // TODO: Implement a timing-safe verification.
            // Unless if one isn't really needed for something this simple?

            string hashed_needle = SHA256.hash(needle).ToUpper();

            return hashed_needle == password.ToUpper();
        }

        public bool PasswordIsEmpty()
        {
            return this.password == null || this.password == "";
        }
    }
}
