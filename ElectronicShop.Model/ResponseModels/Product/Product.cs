//using Newtonsoft.Json;

//namespace ElectronicShop.Model.ResponseModels.Product
//{
//    public class Product
//    {
//        public int Id { get; set; }
//        public string Code { get; set; }
//        public string CategoryCode { get; set; }
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public string ShortDescription { get; set; }
//        public string Specifications { get; set; }
//        public string Image { get; set; }
//        public int Price { get; set; }
//        public int Quanlity { get; set; }
//        public bool IsPublished { get; set; }
//        public string Status { get; set; }

//        public ProductModelView ConvertToViewModel() => new()
//        {
//            Id = Id,
//            Code = Code,
//            CategoryCode = CategoryCode,
//            Name = Name,
//            Description = Description,
//            ShortDescription = ShortDescription,
//            Specifications = Specifications,
//            Images = JsonConvert.DeserializeObject<List<Images>>(Image.Replace(@"\", string.Empty)),
//            Price = Price,
//            Quanlity = Quanlity,
//            IsPublished = IsPublished,
//            Status = Status
//        };
//    }

//    public class ProductModelView
//    {
//        public int Id { get; set; }
//        public string Code { get; set; }
//        public string CategoryCode { get; set; }
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public string ShortDescription { get; set; }
//        public string Specifications { get; set; }
//        public List<Images> Images { get; set; }
//        public int Price { get; set; }
//        public int Quanlity { get; set; }
//        public bool IsPublished { get; set; }
//        public string Status { get; set; }
//    }

//    public class Products : Product
//    {
//        public string CategoryName { get; set; }

//        public int TotalRows { get; set; }

//        public ProductListModelView ConvertToViewModel() => new()
//        {
//            Id = Id,
//            Slug = Code,
//            CategoryCode = CategoryCode,
//            CategoryName = CategoryName,
//            Name = Name,
//            ShortDescription = ShortDescription,
//            Price = Price,
//            IsPublished = IsPublished,
//            Status = Status,
//        };
//    }

//    public class ProductListModelView
//    {
//        public int Id { get; set; }
//        public string Slug { get; set; }
//        public string CategoryCode { get; set; }
//        public string CategoryName { get; set; }
//        public string Name { get; set; }
//        public string? ShortDescription { get; set; }
//        public int Price { get; set; }
//        public bool IsPublished { get; set; }
//        public string Status { get; set; }
//    }
//}