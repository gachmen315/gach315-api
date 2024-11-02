using ElectronicShop.Model.ResponseModels.Product;
using Newtonsoft.Json;

namespace ElectronicShop.Model.RequestModels.Cms
{
    public class ProductSearchReq : BaseRequest
    {
        public string? Status { get; set; }
        public string? ProductType { get; set; }
        public string? ProductCategoryCode { get; set; }
        public string? SubProductCategoryCode { get; set; }
    }

    public class ProductModifyReq
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string ProductCategoryCode { get; set; } = string.Empty;
        public string SubProductCategoryCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? Price { get; set; }
        public int? DiscountedPrice { get; set; }
        public int? PercentDiscount { get; set; }
        public List<Images> Images { get; set; } = new List<Images>();

        public ProductModify ConvertToRequestModel(string? UserName) => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            ProductCategoryCode = ProductCategoryCode,
            SubProductCategoryCode = SubProductCategoryCode,
            Status = Status,
            IsPublished = IsPublished,
            Description = Description,
            Price = Price,
            DiscountedPrice = DiscountedPrice,
            PercentDiscount = PercentDiscount,
            Image = JsonConvert.SerializeObject(Images),
            User = string.IsNullOrWhiteSpace(UserName) ? "system" : UserName
        };
    }

    public class ProductModify
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string ProductCategoryCode { get; set; } = string.Empty;
        public string SubProductCategoryCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? PercentDiscount { get; set; }   
        public string Image { get; set; } = string.Empty;
        public int? Price { get; set; }
        public int? DiscountedPrice { get; set; }
        public string User { get; set; } = "system";
    }
}