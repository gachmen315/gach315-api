namespace ElectronicShop.Model.ResponseModels.Cms
{
    public class SubProductCategory
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProductCategoryCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class SubProductCategorys : SubProductCategory
    {
        public string ProductCategoryName { get; set; } = string.Empty;

        public int TotalRows { get; set; }

        public SubProductCategoryViewModel ConVertToViewModel() => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            ProductCategoryName = ProductCategoryName,
            Status = Status
        };
    }

    public class SubProductCategoryViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProductCategoryName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}