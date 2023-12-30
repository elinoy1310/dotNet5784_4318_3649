partial class Program
{
    private static void Main(string[] args)
    {
        Welcome4318();
        Welcome3649();
        Console.ReadKey();
    }
    static partial void Welcome3649();
    private static void Welcome4318()
    {
        Console.WriteLine("Enter your name: ");
        string name = Console.ReadLine() ?? " ";
        Console.WriteLine("{0},welcome to my first console application", name);
    }

   
}