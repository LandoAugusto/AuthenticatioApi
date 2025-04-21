namespace AuthenticatioApi.Core.Models
{
    public class RoleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> MenuIds { get; set; }

        public RoleResponse(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public RoleResponse(int id, string name, string description, List<int> menuIds)
        {
            Id = id;
            Name = name;
            Description = description;
            MenuIds = menuIds;
        }
    }
}
