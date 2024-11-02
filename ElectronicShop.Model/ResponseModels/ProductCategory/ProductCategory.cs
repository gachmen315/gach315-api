namespace ElectronicShop.Model.ResponseModels.ProductCategory
{
    public class WebProductCategory
    {
        public string ProductCategoryCode { get; set; } = string.Empty;
        public string ProductTypeCode { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
        public string ProductCategoryName { get; set; } = string.Empty;
        public string? SubProductCategoryCode { get; set; }
        public string? SubProductCategoryName { get; set; }
    }

    public class ProductCategoryConverter
    {
        public WebProductTypeViewModel ConvertToViewModel(IEnumerable<WebProductCategory> categories)
        {
            var groupedByProductType = categories
                .GroupBy(c => new { c.ProductTypeCode, c.ProductTypeName })
                .Select(g => new WebProductTypeViewModel
                {
                    ProductTypeCode = g.Key.ProductTypeCode,
                    ProductTypeName = g.Key.ProductTypeName,
                    Type = 1,
                    ProductCategory = g
                        .GroupBy(c => new { c.ProductCategoryCode, c.ProductCategoryName })
                        .Select(pcGroup => new WebProductCategoryViewModel
                        {
                            ProductCategoryCode = pcGroup.Key.ProductCategoryCode,
                            ProductCategoryName = pcGroup.Key.ProductCategoryName,
                            Type = 2,
                            SubProductCategories = pcGroup
                                .Select(c => new WebSubProductCategoryViewModel
                                {
                                    SubProductCategoryCode = string.IsNullOrWhiteSpace(c.SubProductCategoryCode) ? null : c.SubProductCategoryCode,
                                    SubProductCategoryName = c.SubProductCategoryName,
                                    Type = 3,
                                })
                                .Where(sub => sub.SubProductCategoryCode != null) // Filter out entries with null SubProductCategoryCode
                                .ToList()
                        })
                        .ToList()
                })
                .FirstOrDefault(); // Assuming you only need one WebProductTypeViewModewal

            return groupedByProductType;
        }
    }

    public class WebProductTypeViewModel
    {
        public string ProductTypeCode { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
        public int Type { get; set; }
        public IEnumerable<WebProductCategoryViewModel> ProductCategory { get; set; } = Enumerable.Empty<WebProductCategoryViewModel>();
    }

    public class WebProductCategoryViewModel
    {
        public string ProductCategoryCode { get; set; } = string.Empty;
        public string ProductCategoryName { get; set; } = string.Empty;
        public int Type { get; set; }
        public IEnumerable<WebSubProductCategoryViewModel>? SubProductCategories { get; set; } = Enumerable.Empty<WebSubProductCategoryViewModel>();
    }

    public class WebSubProductCategoryViewModel
    {
        public string? SubProductCategoryCode { get; set; }
        public string? SubProductCategoryName { get; set; }
        public int Type { get; set; }
    }
}