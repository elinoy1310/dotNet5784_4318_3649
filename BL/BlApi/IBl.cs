using BO;

namespace BlApi;
public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }
    public DateTime? ProjectStartDate { get; set; }
    #region Clock
    public DateTime Clock { get; }
    public void PromoteTime(Time addTime);
    public void ResetClock();
    #endregion 
    public ProjectStatus CheckProjectStatus();
    public void CreateSchedule(CreateScheduleOption option=CreateScheduleOption.Automatically, int taskId=-1);
    public void InitializeDB();
    public void ResetDB();

    
}
