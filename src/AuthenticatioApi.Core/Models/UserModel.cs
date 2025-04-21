using AuthenticatioApi.Core.Entities.Enumrators;

namespace AuthenticatioApi.Core.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public int ProfileId { get; set; }
        public DocumentTypeEnum DocumentTypeId { get; set; }
        public string Document { get; set; }        
        public string? LegacyCode { get; set; }
    }
}
