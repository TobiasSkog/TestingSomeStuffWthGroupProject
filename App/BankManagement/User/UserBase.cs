using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.Tests;
using GroupProject.BankDatabase;
using System.ComponentModel.DataAnnotations;
using ValidationUtility;

namespace GroupProject.App.BankManagement.User
{
    public abstract class UserBase : Bank
    {
        protected virtual string _firstName { get; set; }
        protected virtual string _lastName { get; set; }
        protected virtual string _userName { get; set; }
        protected virtual string _password { get; set; }
        protected virtual string _userId { get; set; }

        [StringLength(12, MinimumLength = 3, ErrorMessage = "Social security numbers must be 3 or 12 characters.")]
        protected virtual string _socialSecurityNumber { get; set; }
        protected virtual DateTime _dateOfBirth { get; set; }
        protected virtual UserType _userType { get; set; }
        protected virtual UserStatus _userStatus { get; set; }
        protected virtual sbyte _remainingAttempts { get; set; }
        protected virtual List<AccountBase> _accounts { get; set; }

        public UserBase(string firstName, string lastName, string socialSecurityNumber, DateTime dateOfBirth, UserType userType)
        {
            if (BoolValidationHelper.ValidateAgeRestriction(dateOfBirth, 1))
            {
                _firstName = firstName;
                _lastName = lastName;
                _userName = firstName + lastName;
                _password = socialSecurityNumber + firstName;
                _socialSecurityNumber = socialSecurityNumber;
                _dateOfBirth = dateOfBirth;
                _userType = userType;
                _userStatus = UserStatus.Exists;
                _remainingAttempts = 3;
                _userId = StringValidationHelper.CreateRandomString();

            }
        }
        public virtual string FirstName => _firstName;
        public virtual string UserName => _userName;
        public virtual sbyte RemainingAttempts => _remainingAttempts;
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
                return new UserStorage(_userName, _userId, _accounts);
            }

            return null;
        }
        public virtual void AddAccount(AccountBase account) => _accounts.Add(account);
        public virtual UserStatus ExistingAccount(string userName)
        {
            if (userName == _userName)
            {
                return UserStatus.Exists;
            }
            return UserStatus.DoesNotExist;
        }
        public virtual UserStatus Login(string userName)
        {
            _remainingAttempts--;

            if (_remainingAttempts <= 0 || _userStatus == UserStatus.Locked)
            {
                return UserStatus.Locked;
            }
            if (userName == _userName)
            {
                string password = PasswordValidationHelper.PasswordValidation("Enter password: ", 2, 113, false, false, false);
                if (password == _password)
                {
                    return UserStatus.Success;
                }
            }
            return UserStatus.FailedLogin;
        }

    }
}