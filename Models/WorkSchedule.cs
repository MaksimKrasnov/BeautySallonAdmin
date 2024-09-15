namespace BeautySaloon.Models
{
    public class WorkSchedule
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public DateTime Date { get; set; }
        public bool IsWorkingDay { get; set; }

        public virtual Master Master { get; set; }
    }
}
