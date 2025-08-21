namespace AuthenticatioApi.Core.Models
{
    public class SaveMenuModel
    {
        public required string RoleName { get; set; }
        public List<int> MenuIds { get; set; } = new List<int>();
    }
}
