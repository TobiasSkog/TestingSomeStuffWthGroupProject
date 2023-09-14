using GroupProject.App.Tests;
using GroupProject.Bank;
using GroupProject.Bank.User;
using GroupProject.Bank.User.Admin;
using Microsoft.VisualBasic;
using ValidationUtility;

//https://spectreconsole.net/widgets/grid
//UserType newUserType = TestingConsole.GetEnumValueFromRange<UserType>();


UserAdmin administrator = new("Tobias", "Skog", "592530215634", new DateTime(1991, 10, 28));
//_password = socialSecurityNumber + firstName; 592530215634Tobias
Bank[] bankDatabase = { administrator };
BankLogin.FindUserName(bankDatabase);
//TestingConsole.TestDate("Enter Date of Birth: ", 18);
//TestingConsole.PrintConsole();