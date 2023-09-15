using GroupProject.App.BankManagement.User.Admin;

namespace GroupProject.App.BankManagement.Interfaces
{
    public interface IAdminConsole
    {
        void WriteMenu(UserAdmin user);
        void WriteLockedMenu(UserAdmin user);
    }
}
