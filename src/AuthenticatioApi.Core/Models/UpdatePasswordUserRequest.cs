namespace AuthenticatioApi.Core.Models
{
    public class UpdatePasswordUserRequest
    {
        public int? Id { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
