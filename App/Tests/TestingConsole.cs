using GroupProject.Bank.User;
using Spectre.Console;
using System.Reflection;
using ValidationUtility;

namespace GroupProject.App.Tests
{
    public class TestingConsole
    {


        public static void TestDate(string prompt, int ageRestriction)
        {
            DateTime userDateOfBirth = DateTimeValidationHelper.GetDateTimeAgeRestriction(prompt, ageRestriction);
            Console.WriteLine(userDateOfBirth);
        }




        public static void PrintConsole()
        {

            //var columns = new List<Text>()
            //{
            //        new Text("LOGGO", new Style(Color.Red, Color.Black)),
            //        new Text("Item 2"),
            //        new Text("Item 3")
            //};
            //var grid = new Grid();

            var grid = new Grid();

            // For i < accounts[].Length Add a Collumn
            // Add columns 
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();

            // Add header row 
            // AccountName
            //            grid.AddRow(new Text[]{
            //    new Text("Account Name:", new Style(Color.Red, Color.Black)).LeftJustified(),
            //    new Text("Currency:", new Style(Color.Green, Color.Black)).Centered(),
            //    new Text("Account Type:", new Style(Color.Blue, Color.Black)).RightJustified()
            //});
            grid.AddRow(new Text[]{
    new Text($"", new Style(Color.Red, Color.Black)).LeftJustified(),
    new Text("Currency:", new Style(Color.Green, Color.Black)).Centered(),
    new Text("Account Type:", new Style(Color.Blue, Color.Black)).RightJustified()
});

            //var embedded = new Grid();

            //embedded.AddColumn();
            //embedded.AddColumn();

            //embedded.AddRow(new Text("Embedded I"), new Text("Embedded II"));
            //embedded.AddRow(new Text("Embedded III"), new Text("Embedded IV"));

            // Add content row 
            grid.AddRow(
                new Text("Personkonto").LeftJustified(),
                new Text("4500SEK").Centered(),
                new Text("Privatkonto").RightJustified()
            );
            grid.AddRow(
                new Text("Personkonto").LeftJustified(),
                new Text("150USD").Centered(),
                new Text("Valutakonto").RightJustified()
            );
            grid.AddRow(
                new Text("Sparkonto").LeftJustified(),
                new Text("125000SEK").Centered(),
                new Text("Privatkonto").RightJustified()
            );

            // Write centered cell grid contents to Console
            AnsiConsole.Write(grid);
            Console.ReadKey();

            /*
             * 
             * foreach(var acc in Accounts){
             * grid.AddColumn;
             * new Text
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             */

            //AnsiConsole.Write(new Columns(columns));
        }

    }
}

