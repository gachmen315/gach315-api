namespace ElectronicShop.Model.ResponseModels.Cms
{
    public class Dropdown
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsSub { get; set; }
    }
}