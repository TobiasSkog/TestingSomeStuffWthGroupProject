using GroupProject.App.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationUtility;

namespace GroupProject.Bank.User.Admin
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
