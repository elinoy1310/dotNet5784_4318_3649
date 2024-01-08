using Dal;
using DalApi;

namespace DalTest
{
    enum MainManue { Exit, Engineer, Dependency, Task};
    enum SubManue { exit, create, read, readAll, update, delete };
    internal class Program
    {
        private static ITask? s_dalTask=new TaskImplementation();
        private static IEngineer? s_dalEndineer = new EngineerImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();
        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalEndineer,s_dalTask,s_dalDependency);

            }

            catch(Exception ex) 
            {

                Console.WriteLine(ex.Message);
            }
            
        }

        public void PresentMainManue()
        {
            string chooseMainManue = Console.ReadLine()!;
            MainManue optionMain = (MainManue)int.Parse(chooseMainManue);
            switch (optionMain)
            {
                case MainManue.Exit:
                    break;
                case MainManue.Engineer:
                    break;
                case MainManue.Dependency:
                    break;
                case MainManue.Task:
                    break;
                default:
                    break;
            }

        }

        public void PresentSubMenu(MainManue entity)
        {
            string chooseSubManue = Console.ReadLine()!;
            SubManue optionSubMain = (SubManue)int.Parse(chooseSubManue);
            switch (optionSubMain)
            {
                case SubManue.exit:
                    break;
                case SubManue.create:
                    break;
                case SubManue.read:
                    break;
                case SubManue.readAll:
                    break;
                case SubManue.update:
                    break;
                case SubManue.delete:
                    break;
                default:
                    break;
            }


        }
    }
}

















