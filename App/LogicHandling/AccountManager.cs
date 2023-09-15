using GroupProject.App.BankManagement;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.ConsoleHandling;
using ValidationUtility;

namespace GroupProject.App.LogicHandling
{
    public static class AccountManager
    {
        public static UserCustomer CreateUserCustomerAccount(UserType userType)
        {
            if (userType == UserType.Customer)
            {
                string firstName = StringValidationHelper.GetString("Enter your first name: ");
                string lastName = StringValidationHelper.GetString("Enter your last name: ");
                string socialSecurityNumber = StringValidationHelper.GetString("Enter your social security number: ");
                DateTime dateOfBirth = DateTimeValidationHelper.GetExactDateTimeAgeRestriction("Enter your date of birth ", "YYYY/MM/DD", 18);
                return new UserCustomer(firstName, lastName, socialSecurityNumber, dateOfBirth, userType);
            }
            return default;
        }

        public static UserAdmin CreateUserAdminAccount(UserType userType)
        {
            if (userType == UserType.Admin)
            {
                string firstName = StringValidationHelper.GetString("Enter your first name: ");
                string lastName = StringValidationHelper.GetString("Enter your last name: ");
                string socialSecurityNumber = StringValidationHelper.GetString("Enter your social security number: ");
                DateTime dateOfBirth = DateTimeValidationHelper.GetExactDateTimeAgeRestriction("Enter your date of birth ", "YYYY/MM/DD", 18);
                return new UserAdmin(firstName, lastName, socialSecurityNumber, dateOfBirth, userType);
            }
            return default;
        }
    }
}

