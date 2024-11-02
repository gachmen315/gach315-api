using Newtonsoft.Json;

namespace ElectronicShop.Model.ResponseModels.Product
{
    public class ProductWeb
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string CategoryCode { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Specifications { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }

        public ProductWebModelView ConvertToViewModel() => new()
        {
            Id = Id,
            Slug = Code,
            Categories = CategoryCode,
            Name = Name,
            Short_description = ShortDescription,
            Description = Description,
            Specifications = Specifications,
            Images = JsonConvert.DeserializeObject<List<Images>>(Image.Replace(@"\", string.Empty)),
            Price = Price
        };
    }

    public class ProductWebModelView
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Categories { get; set; }
        public string Name { get; set; }
        public string? Short_description { get; set; }
        public string Description { get; set; }
        public string Specifications { get; set; }
        public List<Images> Images { get; set; }
        public int Price { get; set; }
    }

    public class ProductWebs : ProductWeb
    {
        public int TotalRows { get; set; }

        public ProductWebListModelView ConvertToViewModel() => new()
        {
            Id = Id,
            Slug = Code,
            Categories = CategoryCode,
            Name = Name,
            ShortDescription = ShortDescription,
            Images = JsonConvert.DeserializeObject<List<Images>>(Image.Replace(@"\", string.Empty)),
            Price = Price
        };
    }

    public class ProductWebListModelView
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Categories { get; set; }
        public string Name { get; set; }
        public string? ShortDescription { get; set; }
        public List<Images> Images { get; set; }
        public int Price { get; set; }
    }

    public class Images
    {
        public string? Base_url { get; set; }
    }
}