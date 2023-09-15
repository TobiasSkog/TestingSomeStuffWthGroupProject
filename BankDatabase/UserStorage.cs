using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.User;

namespace GroupProject.BankDatabase
{
    [Serializable]
    public class UserStorage
    {
        protected string _userName { get; set; }
        protected string _userId { get; set; }
        protected List<AccountBase> _accounts { get; set; }
        public UserStorage() { }
        public UserStorage(string userName, string userId, List<AccountBase> accounts)
        {
            _userName = userName;
            _userId = userId;
            _accounts = accounts;
        }
        public string UserId(UserType userType)
        {
            if (userType == UserType.Admin)
            {
                return _userId;
            }
            return default;
        }
        public string UserName(UserType userType)
        {
            if (userType == UserType.Admin)
            {
                return _userName;
            }
            return default;
        }
        public List<AccountBase> Accounts(UserType userType)
        {
            if (userType == UserType.Admin)
            {
                return _accounts;
            }
            return new List<AccountBase>();
        }
    }
}

