using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BeautySaloon.Models
{
	public class User
	{
		public int Id { get; set; }

		//[EmailAddress(ErrorMessage = "Неверный формат адреса электронной почты")]
		public string Email { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }
		public string PhoneNumber { get; set; }

		public ICollection<Appointment> Appointments { get; set; }
	}
}
