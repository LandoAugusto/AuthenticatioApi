namespace AuthenticatioApi.Core.Models
{
    public class UpdateUserRequest
    {
        public string? Id { get; set; }
        public int RoleId { get; set; }
        public string? Email { get; set; }
    }
}
