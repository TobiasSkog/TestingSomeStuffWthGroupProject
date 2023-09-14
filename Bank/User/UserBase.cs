using GroupProject.App.Tests;
using System.ComponentModel.DataAnnotations;
using ValidationUtility;

namespace GroupProject.Bank.User
{
    public abstract class UserBase : Bank
    {
        protected virtual string _firstName { get; set; }
        protected virtual string _lastName { get; set; }
        protected virtual string _userName { get; set; }
        protected virtual string _password { get; set; }

        [StringLength(12, MinimumLength = 8, ErrorMessage = "Social security numbers must be 8 or 12 characters.")]
        protected virtual string _socialSecurityNumber { get; set; }
        protected virtual DateTime _dateOfBirth { get; set; }
        protected virtual UserType _userType { get; set; }
        protected virtual UserStatus _userStatus { get; set; }
        protected sbyte _remainingAttempts { get; set; }

        public UserBase(string firstName, string lastName, string socialSecurityNumber, DateTime dateOfBirth, UserType userType)
        {
            if (BoolValidationHelper.ValidateAgeRestriction(dateOfBirth, 18))
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
            }
        }
        public virtual string FirstName => _firstName;
        public virtual string LastName => _lastName;
        public virtual sbyte RemainingAttempts => _remainingAttempts;

        public virtual UserStatus ExistingAccount(string userName)
        {
            if (userName == _userName)
            {
                return UserStatus.Exists;
            }
            return UserStatus.DoesNotExist;
        }
        public virtual UserStatus Login(string userName, string password)
        {
            _remainingAttempts--;

            if (_remainingAttempts <= 0 || _userStatus == UserStatus.Locked)
            {
                return UserStatus.Locked;
            }



            if (userName == _userName)
            {
                if (password == _password)
                {
                    return UserStatus.Success;
                }
            }
            return UserStatus.FailedLogin;
        }
        public virtual string SocialSecurityNumber(UserType userType)
        {
            if (userType == UserType.Admin)
            {
                return _socialSecurityNumber;
            }
            return default;
        }

    }
}
