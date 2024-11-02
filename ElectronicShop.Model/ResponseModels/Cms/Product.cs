using ElectronicShop.Model.ResponseModels.Product;
using Newtonsoft.Json;

namespace ElectronicShop.Model.ResponseModels.Cms
{
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public string ProductCategoryCode { get; set; } = string.Empty;
        public string SubProductCategoryCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int? Price { get; set; }
        public int? DiscountedPrice { get; set; }
        public int? PercentDiscount { get; set; }
        public int Quanlity { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsPublished { get; set; }

        public ProductModelView ConvertToViewModel() => new()
        {
            Id = Id,
            Code = Code,
            ProductType = ProductType,
            ProductCategoryCode = ProductCategoryCode,
            SubProductCategoryCode = SubProductCategoryCode,
            Name = Name,
            Description = Description,
            Images = JsonConvert.DeserializeObject<List<Images>>(Image.Replace(@"\", string.Empty)),
            Price = Price,
            DiscountedPrice = DiscountedPrice,
            PercentDiscount = PercentDiscount,
            Quanlity = Quanlity,
            Status = Status,
            IsPublished = IsPublished
        };
    }

    public class ProductModelView
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public string ProductCategoryCode { get; set; } = string.Empty;
        public string SubProductCategoryCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Images> Images { get; set; } = new List<Images>();
        public int? Price { get; set; }
        public int? DiscountedPrice { get; set; }
        public int? PercentDiscount { get; set; }
        public int Quanlity { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
    }

    public class Products : Product
    {
        public string ProductTypeName { get; set; } = string.Empty;
        public string ProductCategoryName { get; set; } = string.Empty;
        public string SubProductCategoryName { get; set; } = string.Empty;
        public int TotalRows { get; set; }

        public ProductListModelView ConvertToViewModel() => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            Status = Status,
            IsPublished = IsPublished,
            Price = Price,
            DiscountedPrice = DiscountedPrice,
            PercentDiscount = PercentDiscount,
            ProductTypeName = ProductTypeName,
            ProductCategoryName = ProductCategoryName,
            SubProductCategoryName = SubProductCategoryName,
            Images = JsonConvert.DeserializeObject<List<Images>>(Image.Replace(@"\", string.Empty)),
        };
    }

    public class ProductListModelView
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public int? Price { get; set; }
        public int? DiscountedPrice { get; set; }
        public int? PercentDiscount { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public string ProductCategoryName { get; set; } = string.Empty;
        public string SubProductCategoryName { get; set; } = string.Empty;
        public List<Images> Images { get; set; } = new List<Images>();
    }
}