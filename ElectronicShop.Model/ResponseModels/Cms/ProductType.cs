namespace ElectronicShop.Model.ResponseModels.Cms
{
    public class ProductType
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class ProductTypes : ProductType
    {
        public int TotalRows { get; set; }

        public ProductType ConVertToViewModel() => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            Status = Status
        };
    }
}