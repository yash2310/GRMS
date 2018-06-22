namespace AMS.WebApi.Models
{
	public class LoginRequest
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}

	public class ResetPassword
	{
		public int UserId { get; set; }
		public string email { get; set; }
		public string Password { get; set; }
	}
}