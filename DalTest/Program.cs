using Dal;
using DalApi;
using DO;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace DalTest
{
    enum MainManue { Exit, Engineer, Dependency, Task };
    enum SubManue { exit, create, read, readAll, update, delete };
    internal class Program
    {
        private static ITask? s_dalTask=new TaskImplementation();
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
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
            Console.WriteLine("Select an entity you want to check\r\n0= Exit the main menu\r\n1= engineer\r\n2=dependency\r\n3=task");
            string chooseMainManue = Console.ReadLine()!;
            MainManue optionMainManue = (MainManue)int.Parse(chooseMainManue);
            switch (optionMainManue)
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
        /// <summary>
        /// This function prompts the user to enter data in order to create an Engineer object.
        /// </summary>
        /// <returns>An Engineer object with the data entered by the user.</returns>
        public Engineer NewEngineer()
        {
            //user input
            Console.WriteLine("enter id,email,cost,name and level of the engineer");
            int id = int.Parse(Console.ReadLine() ?? "0");
            string email = Console.ReadLine() ?? "";
            double cost = int.Parse(Console.ReadLine() ?? "0");
            string name = Console.ReadLine() ?? "";
            EngineerExperience level = (EngineerExperience)int.Parse(Console.ReadLine() ?? "0");
            
            //creates new Engineer and returns it
            Engineer newEngineer = new Engineer(id, email, cost, name, level);
            return newEngineer;
        }

        /// <summary>
        /// This function prompts the user to enter data in order to create an Task object.
        /// </summary>
        /// <returns>A Task object with the data entered by the user.</returns>
        public DO.Task NewTask()
        {
            //user input
            Console.WriteLine("enter alias,description,if the task is mile stone=false, required effort time, created in date,scheduled date=null,start date, complete date, dead line, deliverables,remarks,engineer id, engineer's experience,complexity");
            string alias = Console.ReadLine() ?? "";
            string description = Console.ReadLine() ?? "";
            bool isMileStone;
            bool flag = bool.TryParse(Console.ReadLine(), out isMileStone);
            isMileStone = flag ? isMileStone : false;
            TimeSpan requiredEffortTime;
            flag = TimeSpan.TryParse(Console.ReadLine(), out requiredEffortTime);
            requiredEffortTime = flag ? requiredEffortTime : TimeSpan.Zero;
            DateTime createdInDate;
            flag = DateTime.TryParse(Console.ReadLine(), out createdInDate);
            createdInDate = flag ? createdInDate : DateTime.Today;
            DateTime scheduledDate;
            flag = DateTime.TryParse(Console.ReadLine(), out scheduledDate);
            scheduledDate = flag ? scheduledDate : DateTime.Today;
            DateTime startDate;
            flag = DateTime.TryParse(Console.ReadLine(), out startDate);
            startDate = flag ? startDate : DateTime.Today;
            DateTime completeDate;
            flag = DateTime.TryParse(Console.ReadLine(), out completeDate);
            completeDate = flag ? completeDate : DateTime.Today;
            DateTime deadline;
            flag = DateTime.TryParse(Console.ReadLine(), out deadline);
            deadline = flag ? deadline : DateTime.Today;
            string deliverables = Console.ReadLine() ?? "";
            string remarks = Console.ReadLine() ?? "";
            int engineerId = int.Parse(Console.ReadLine() ?? "");
            EngineerExperience complexity = (EngineerExperience)int.Parse(Console.ReadLine() ?? "0");
           //create new Task and returns it
            DO.Task newTask = new DO.Task(0, alias, description, isMileStone, requiredEffortTime, createdInDate, scheduledDate, startDate, completeDate, deadline, deliverables, remarks, engineerId, complexity);
            return newTask;
        }
        public void Create(MainManue entity)
        {
            try
            {

                switch (entity)
                {
                    case MainManue.Engineer:
                        Console.WriteLine( s_dalEngineer!.Create(NewEngineer()));
                        break; 
                    case MainManue.Dependency:
                        break;
                    case MainManue.Task:
                        Console.WriteLine(s_dalTask!.Create(NewTask()));


                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

        }
    }
}

















