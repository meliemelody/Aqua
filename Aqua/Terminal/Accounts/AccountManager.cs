using term = Aqua.Terminal.Terminal;
using System.Collections.Generic;


namespace Aqua.Terminal.Accounts
{
    public class AccountManager
    {
        public List<Account> accounts = new List<Account>();

        public int LoadUsers(string user_ini)
        {
            // Remove the old users loaded, if loaded before
            accounts.Clear();

            // Get the user sections from the list of users
            var user_sections_str = Utils.FileTypes.INI.ReadFile("LIST", "LIST", user_ini);
            var user_sections = user_sections_str.Split("|");

            // Loop through every user
            foreach (var user_key in user_sections)
            {
                var username = Utils.FileTypes.INI.ReadFile(user_key.ToUpper(), "USERNAME", user_ini);
                var password = Utils.FileTypes.INI.ReadFile(user_key.ToUpper(), "PASSWORD", user_ini);
                var role = Utils.FileTypes.INI.ReadFile(user_key.ToUpper(), "ROLE", user_ini);

                int role_int;
                if (int.TryParse(role, out role_int))
                {
                    if (password == "")
                    {
                        accounts.Add(new Account(username, (Roles)role_int));
                    }
                    else
                    {
                        accounts.Add(new Account(username, (Roles)role_int, password));
                    }
                }
                else
                {
                    term.DebugWrite($"Failed to load user '{username}' : Role ID is not a integer.", 4);
                }
            }

            // Successfully loaded users
            return 0;
        }
    }
}
