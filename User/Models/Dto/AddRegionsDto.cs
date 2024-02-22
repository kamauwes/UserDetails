
namespace RegionsUser.Models.Dto
{
    public class AddRegionsDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Guid Id { get; internal set; }
    }
}
