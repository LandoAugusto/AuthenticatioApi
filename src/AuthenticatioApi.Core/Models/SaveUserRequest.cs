namespace AuthenticatioApi.Core.Models
{
    public class SaveUserRequest
    {
        public int RoleId { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

    }
}
