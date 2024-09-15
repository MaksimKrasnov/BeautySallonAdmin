namespace BeautySaloon.Models
{
	public class MasterService
	{
		public int Id { get; set; }
		public int MasterId { get; set; }
		public Master Master { get; set; }

		public int ServiceId { get; set; }
		public Service Service { get; set; }

		public Appointment Appointment { get; set; }
	}
}
