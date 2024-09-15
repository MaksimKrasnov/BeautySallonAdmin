namespace BeautySaloon.Models
{
    public class AdminViewModel

    {
        public List<Master> Masters { get; set; }
        public List<Service> Services { get; set; }
         public  Dictionary<Master, Dictionary<DateTime, string>> Appointments { get; set; }
        public int ServiceId { get; set; }
        public DateTime Date { get; set; }
    }
}


