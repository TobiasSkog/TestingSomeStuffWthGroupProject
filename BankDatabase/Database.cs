using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.User;
using GroupProject.App.ConsoleHandling;
using Newtonsoft.Json;
using Spectre.Console;
using System.ComponentModel;
using ValidationUtility;

namespace GroupProject.BankDatabase
{
    internal class Database
    {
        private const string FilePath = "Database\\Users.txt";
        private const UserType _admin = UserType.Admin;
        private List<UserBase> _users { get; set; }
        public Database(List<UserBase> users)
        {
            _users = users;
        }
        internal void InitiateDataBase()
        {
            ConsoleIO.StartUp();
            List<UserStorage> userStorageList = _users
                .Select(user => user.ToUserStorage(_admin))
                .ToList();

            using (StreamWriter sw = File.CreateText(FilePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, userStorageList);
            }
        }
        //string json = JsonConvert.SerializeObject(UserBase, Formatting.Indented);
        internal static void AddBankAccountToUser()
        {


        }

        internal UserBase FindUserInDatabase(string prompt = "Enter username: ")
        {
            string userName = StringValidationHelper.GetString(prompt);
            using (StreamReader sr = new(FilePath))
            {
                List<UserStorage> userStorageList = JsonConvert.DeserializeObject<List<UserStorage>>(sr.ReadToEnd());
                UserStorage userStorage = userStorageList.FirstOrDefault(storedUser => storedUser.UserName(_admin) == userName);

                if (userStorage != null)
                {
                    UserBase userBase = _users.FirstOrDefault(user => user.UserId(_admin) == userStorage.UserId(_admin) && user.UserName == userStorage.UserName(_admin));

                    if (userBase != null)
                    {
                        return userBase;
                    }
                }

                return null;
            }
        }

        internal void SaveAccount(AccountBase account)
        {
            List<UserStorage> userStorageList;
            using (StreamReader sr = new(FilePath))
            {
                userStorageList = JsonConvert.DeserializeObject<List<UserStorage>>(sr.ReadToEnd());
                UserStorage userStorage = userStorageList.FirstOrDefault(storedUser => storedUser.UserId(_admin) == account.AccountOwner(_admin).UserId(_admin));
                if (userStorage != null)
                {
                    if (userStorage.Accounts(_admin) != null)
                    {
                        userStorage.Accounts(_admin).Add(account);

                        using (StreamWriter sw = File.CreateText(FilePath))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Serialize(sw, userStorageList);
                        }
                    }
                }
            }
        }
        internal void SaveUser(UserBase user)
        {
            List<UserStorage> userStorageList;
            using (StreamReader sr = new(FilePath))
            {
                userStorageList = JsonConvert.DeserializeObject<List<UserStorage>>(sr.ReadToEnd());
                UserStorage userStorage = userStorageList.FirstOrDefault(storedUser => storedUser.UserId(_admin) == user.UserId(_admin));
                if (userStorage != null)
                {
                    using (StreamWriter sw = File.CreateText(FilePath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(sw, userStorageList);
                    }
                }
            }
        }

    }
}
