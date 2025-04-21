namespace AuthenticatioApi.Core.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProfileId { get; set; }
        public int DocumentTypeId { get; set; }
        public string Document { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? Code { get; set; }
    }
}
