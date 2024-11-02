//using ElectronicShop.Model.ResponseModels.Product;
//using Newtonsoft.Json;
//using System.ComponentModel.DataAnnotations;

//namespace ElectronicShop.Model.RequestModels.Product
//{
//    public class ProductInsertReq
//    {
//        [Required(ErrorMessage = "Mã sản phẩm đang trống")]
//        public string Code { get; set; }

//        [Required(ErrorMessage = "Mã danh mục sản phẩm đang trống")]
//        public string CategoryCode { get; set; }

//        [Required(ErrorMessage = "Tên sản phẩm đang trống")]
//        public string Name { get; set; }

//        public string? Description { get; set; }
//        public string? ShortDescription { get; set; }
//        public string Specifications { get; set; }
//        public List<Images> Images { get; set; } = new List<Images>();
//        public int Price { get; set; }
//        public int Quanlity { get; set; }
//        public bool IsPublished { get; set; }
//        public string Status { get; set; }

//        public ProductInsert ConvertToRequestModel(string? UserName) => new()
//        {
//            Code = Code,
//            CategoryCode = CategoryCode,
//            Name = Name,
//            Description = Description,
//            ShortDescription = ShortDescription,
//            Specifications = Specifications,
//            Image = JsonConvert.SerializeObject(Images),
//            Price = Price,
//            Quanlity = Quanlity,
//            IsPublished = IsPublished,
//            Status = Status,
//            User = string.IsNullOrWhiteSpace(UserName) ? "system" : UserName
//        };
//    }

//    public class ProductInsert
//    {
//        public string Code { get; set; }
//        public string CategoryCode { get; set; }
//        public string Name { get; set; }
//        public string? Description { get; set; }
//        public string? ShortDescription { get; set; }
//        public string Specifications { get; set; }
//        public string Image { get; set; }
//        public int Price { get; set; }
//        public int Quanlity { get; set; }
//        public bool IsPublished { get; set; }
//        public string Status { get; set; }
//        public string User { get; set; } = "system";
//    }

//    public class ProductUpdateReq
//    {
//        public string Code { get; set; }
//        public string CategoryCode { get; set; }
//        public string Name { get; set; }
//        public string? Description { get; set; }
//        public string? ShortDescription { get; set; }
//        public string Specifications { get; set; }
//        public List<Images> Images { get; set; } = new List<Images>();
//        public int Price { get; set; }
//        public int Quanlity { get; set; }
//        public bool IsPublished { get; set; }
//        public string Status { get; set; }

//        public ProductUpdate ConvertToRequestModel(string? UserName) => new()
//        {
//            Code = Code,
//            CategoryCode = CategoryCode,
//            Name = Name,
//            Description = Description,
//            ShortDescription = ShortDescription,
//            Specifications = Specifications,
//            Image = JsonConvert.SerializeObject(Images),
//            Price = Price,
//            Quanlity = Quanlity,
//            IsPublished = IsPublished,
//            Status = Status,
//            User = string.IsNullOrWhiteSpace(UserName) ? "system" : UserName
//        };
//    }

//    public class ProductUpdate
//    {
//        public string Code { get; set; }
//        public string CategoryCode { get; set; }
//        public string Name { get; set; }
//        public string? Description { get; set; }
//        public string? ShortDescription { get; set; }
//        public string Specifications { get; set; }
//        public string Image { get; set; }
//        public int Price { get; set; }
//        public int Quanlity { get; set; }
//        public bool IsPublished { get; set; }
//        public string Status { get; set; }
//        public string User { get; set; } = "system";
//    }
//}