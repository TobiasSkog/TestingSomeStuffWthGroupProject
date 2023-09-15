using GroupProject.App.BankManagement;
using GroupProject.App.BankManagement.User;
using GroupProject.App.ConsoleHandling;
using GroupProject.App.LogicHandling;
using Newtonsoft.Json;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationUtility;

namespace GroupProject.BankDatabase
{
    internal class Database
    {
        private const string FilePath = "Database\\Users.txt";
        private List<UserBase> _users { get; set; }
        public Database(List<UserBase> users)
        {
            _users = users;
        }
        internal void InitiateDataBase()
        {
            ConsoleIO.StartUp();
            List<UserStorage> userStorageList = _users
                .Select(user => user.ToUserStorage())
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
                UserStorage userStorage = userStorageList.FirstOrDefault(storedUser => storedUser.UserName == userName);

                if (userStorage != null)
                {
                    UserBase userBase = _users.FirstOrDefault(user => user.UserId == userStorage.UserId && user.UserName == userStorage.UserName);

                    if (userBase != null)
                    {
                        return userBase;
                    }
                }

                return null;
            }
        }



    }
}
