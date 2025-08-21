namespace AuthenticatioApi.Core.Models
{
    public class MenuListModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Icon { get; set; }
        public string? Url { get; set; }
        public int? Code { get; set; }
        public List<MenuListModel>? MenuItem { get; set; } = new List<MenuListModel>();
    }
}
