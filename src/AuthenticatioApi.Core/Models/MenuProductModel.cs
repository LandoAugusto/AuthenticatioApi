namespace AuthenticatioApi.Core.Models
{
    public class MenuProductModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CoverageId { get; set; }
        public int Code { get; set; }
        public string? Name { get; set; }
    }
}
