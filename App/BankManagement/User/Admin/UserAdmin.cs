namespace GroupProject.App.BankManagement.User.Admin
{
    public class UserAdmin : UserBase
    {
        public UserAdmin(string firstName, string lastName, string socialSecurityNumber, DateTime dateOfBirth) : base(firstName, lastName, socialSecurityNumber, dateOfBirth, UserType.Admin)
        {
        }

        public void CreateAccount()
        {
            // Choose what Type of Account
            //UserType newUserType = EnumValidationHelper.GetEnumValueFromRange<UserType>();
            // 
        }

        // if (BoolValidationHelper.ValidateNoDuplicates()) {}

    }
}