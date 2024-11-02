namespace ElectronicShop.Model.ResponseModels.Cms
{
    public class BrandCategory
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProductCategoryCode { get; set; } = string.Empty;
        public string BrandCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class BrandCategorys : BrandCategory
    {
        public string ProductCategoryName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public int TotalRows { get; set; }

        public BrandCategoryViewModel ConVertToViewModel() => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            ProductCategoryName = ProductCategoryName,
            BrandName = BrandName,
            Status = Status
        };
    }

    public class BrandCategoryViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProductCategoryName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}