using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Model.RequestModels.Category
{
    public class CategorySearchReq : BaseRequest
    {
        public string? Status { get; set; }
    }

    public class CategoryInsertReq
    {
        [Required(ErrorMessage = "Mã danh mục sản phẩm đang trống")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Tên danh mục sản phẩm đang trống")]
        public string Name { get; set; }

        public string? Description { get; set; }
        public string Status { get; set; }

        public CategoryInsert ConvertToRequestModel(string? UserName) => new()
        {
            Code = Code,
            Name = Name,
            Description = Description,
            Status = Status,
            User = string.IsNullOrWhiteSpace(UserName) ? "system" : UserName
        };
    }

    public class CategoryInsert : CategoryInsertReq
    {
        public string User { get; set; } = "system";
    }

    public class CategoryUpdateReq
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }

        public CategoryUpdate ConvertToRequestModel(string? UserName) => new()
        {
            Code = Code,
            Name = Name,
            Description = Description,
            Status = Status,
            User = string.IsNullOrWhiteSpace(UserName) ? "system" : UserName
        };
    }

    public class CategoryUpdate : CategoryUpdateReq
    {
        public string User { get; set; } = "system";
    }
}