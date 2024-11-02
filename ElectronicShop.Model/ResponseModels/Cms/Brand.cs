namespace ElectronicShop.Model.ResponseModels.Cms
{
    public class Brand
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class Brands : Brand
    {
        public int TotalRows { get; set; }

        public Brand ConVertToViewModel() => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            Status = Status
        };
    }
}