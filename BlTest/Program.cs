// See https://aka.ms/new-console-template for more information
using BO;


internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    private static void Main(string[] args)
    {
        try
        {
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
                DalTest.Initialization.Do();
            presentMainMenu();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private static void presentMainMenu()
    {
    }

    private static void presentSubMenu(MainMenu entity)
    {
    }

    private static void readSubMenu(MainMenu entity)
    {
    }

    private static void readAllSubMenu(MainMenu entity)
    {
    }

    private static void updateSubMenu(MainMenu entity)
    {
    }

    private static void createSubMenu(MainMenu entity)
    {
    }

    private static void deleteSubMenu(MainMenu entity)
    {
    }

}
