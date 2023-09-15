using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.ConsoleHandling;
using ValidationUtility;

namespace GroupProject.App.BankManagement.User.Admin
{
    public class UserAdmin : UserBase
    {
        public UserAdmin(string firstName, string lastName, string socialSecurityNumber, DateTime dateOfBirth, UserType userType) : base(firstName, lastName, socialSecurityNumber, dateOfBirth, userType)
        {
        }

        public UserChoice UpdateCurrencyExchange()
        {
            return UserChoice.Back;
        }

    }
}