namespace ElectronicShop.Model.ResponseModels.Cms
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public bool IsSub { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class ProductCategorys : ProductCategory
    {
        public string ProductTypeName { get; set; } = string.Empty;
        public int TotalRows { get; set; }

        public ProductCategoryViewModel ConVertToViewModel() => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            ProductTypeName = ProductTypeName,
            IsSub = IsSub,
            Status = Status
        };
    }

    public class ProductCategoryViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
        public bool IsSub { get; set; }
        public string Status { get; set; } = string.Empty;

    }
}