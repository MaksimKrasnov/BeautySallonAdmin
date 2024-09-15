namespace BeautySaloon.Models
{
	public class Master
	{
		public int Id { get; set; }
		public string Name { get; set; }
		
		public string Position { get; set; }
		public virtual ICollection<MasterService> MasterServices { get; set; }
        public virtual ICollection<WorkSchedule> WorkSchedules { get; set; }


        // Другие свойства, если необходимо
    }
}
