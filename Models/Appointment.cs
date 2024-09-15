namespace BeautySaloon.Models
{

	public class Appointment
	{
		public int Id { get; set; }
		public string? ClientName { get; set; }
		public string? Phone { get; set; }
		public DateTime DateTime { get; set; }
		public int MasterServiceId { get; set; }
		public MasterService MasterService { get; set; }
		public int? UserId { get; set; }
		public User User { get; set; }
	}

}
