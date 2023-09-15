using GroupProject.App.BankManagement.User.Customer;

namespace GroupProject.App.BankManagement.Interfaces
{
    public interface ICustomerConsole
    {
        void WriteMenu(UserCustomer user);
        void WriteLockedMenu(UserCustomer user);
    }
}
