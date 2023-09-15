using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.ConsoleHandling;
using GroupProject.App.Tests;
using GroupProject.BankDatabase;
using System.ComponentModel.DataAnnotations;
using ValidationUtility;

namespace GroupProject.App.BankManagement.User
{
    public abstract class UserBase : Bank
    {
        public virtual string FirstName { get; private set; }
        public virtual string LastName { get; private set; }
        public virtual string UserName { get; private set; }
        public virtual sbyte RemainingAttempts { get; private set; }
        protected virtual string _password { get; set; }
        protected virtual string _userId { get; set; }

        [StringLength(12, MinimumLength = 3, ErrorMessage = "Social security numbers must be 3 or 12 characters.")]
        protected virtual string _socialSecurityNumber { get; set; }
        protected virtual DateTime _dateOfBirth { get; set; }
        protected virtual UserType _userType { get; set; }
        protected virtual UserStatus _userStatus { get; set; }
        protected virtual List<AccountBase> _accounts { get; set; }
        protected virtual List<string> _userLog { get; set; }



        public UserBase(string firstName, string lastName, string socialSecurityNumber, DateTime dateOfBirth, UserType userType)
        {
            if (BoolValidationHelper.ValidateAgeRestriction(dateOfBirth, 1))
            {
                FirstName = firstName;
                LastName = lastName;
                UserName = firstName + lastName;
                _password = socialSecurityNumber + firstName;
                _socialSecurityNumber = socialSecurityNumber;
                _dateOfBirth = dateOfBirth;
                _userType = userType;
                _userStatus = UserStatus.Exists;
                RemainingAttempts = 3;
                _userId = StringValidationHelper.CreateRandomString();

            }
        }
        public virtual UserType GetUserType() => _userType;
        public virtual string UserId(UserType userType)
        {
            if (userType == UserType.Admin)
            {
                return _userId;
            }

            return null;
        }
        public virtual string SocialSecurityNumber(UserType userType)
        {
            if (userType == UserType.Admin)
            {
                return _socialSecurityNumber;
            }
            return null;
        }


        public UserStorage ToUserStorage(UserType userType)
        {
            if (userType == UserType.Admin)
            {
                return new UserStorage(UserName, _userId, _accounts);
            }

            return null;
        }

        public virtual void AddToLog(string log)
        {
            _userLog.Add(log);
        }
        public virtual void AddAccount(AccountBase account) => _accounts.Add(account);
        public virtual UserStatus ExistingAccount(string userName)
        {
            if (userName == UserName)
            {
                return UserStatus.Exists;
            }
            return UserStatus.DoesNotExist;
        }
        public virtual UserStatus Login(string userName)
        {
            RemainingAttempts--;

            if (RemainingAttempts <= 0 || _userStatus == UserStatus.Locked)
            {
                return UserStatus.Locked;
            }
            if (userName == UserName)
            {
                string password = PasswordValidationHelper.PasswordValidation("Enter password: ", 2, 113, false, false, false);
                if (password == _password)
                {
                    return UserStatus.Success;
                }
            }
            return UserStatus.FailedLogin;
        }
        public virtual List<string> UserLog(UserType userType)
        {
            if (userType == UserType.Admin)
            {
                return _userLog;
            }
            return null;
        }
        public virtual bool ListAccounts()
        {
            if (_accounts.Count == 0)
            {
                bool createAccount = BoolValidationHelper.PromptForYesOrNo("You have no accounts yet, would you like to create one: ");
                return createAccount;
            }
            ConsoleIO.WriteAccountList(_accounts, this);
            return false;
        }
        public virtual List<AccountBase> Accounts(UserType userType)
        {
            if (userType == UserType.Admin)
            {
                return _accounts;
            }
            return null;
        }
    }
}