using Newtonsoft.Json;

namespace ElectronicShop.Model.ResponseModels.Product
{
    public class WebProduct
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int? Price { get; set; }
        public int? DiscountedPrice { get; set; }
        public int? PercentDiscount { get; set; }
        public int Quanlity { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ProductCategoryCode { get; set; } = string.Empty;

        public WebProductDetailViewModel ConvertToDetailViewModel() => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            Images = JsonConvert.DeserializeObject<IEnumerable<Images>>(Image.Replace(@"\", string.Empty)),
            Price = Price,
            DiscountedPrice = DiscountedPrice,
            PercentDiscount = PercentDiscount,
            Quanlity = Quanlity,
            Description = Description,
            Status = Status,
            ProductCategoryCode = ProductCategoryCode
        };

        public WebProductDiscountedPrice ConvertToViewModel() => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            Images = JsonConvert.DeserializeObject<IEnumerable<Images>>(Image.Replace(@"\", string.Empty)),
            Price = Price,
            DiscountedPrice = DiscountedPrice,
            PercentDiscount = PercentDiscount
        };
    }

    public class WebProducts : WebProduct
    {
        public int TotalRows { get; set; }

        public new WebProductDiscountedPrice ConvertToViewModel() => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            Images = JsonConvert.DeserializeObject<IEnumerable<Images>>(Image.Replace(@"\", string.Empty)),
            Price = Price,
            DiscountedPrice = DiscountedPrice,
            PercentDiscount = PercentDiscount
        };
    }

    public class WebProductDiscountedPrice
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? Price { get; set; }
        public int? DiscountedPrice { get; set; }
        public int? PercentDiscount { get; set; }
        public IEnumerable<Images> Images { get; set; } = new List<Images>();
    }

    public class WebProductDetailViewModel : WebProductDiscountedPrice
    {
        public int Quanlity { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ProductCategoryCode { get; set; } = string.Empty;
    }
}