namespace ElectronicShop.Model.RequestModels.Cms
{
    public class ProductCategorySearchReq : BaseRequest
    {
        public string? Status { get; set; }
        public string? ProductType { get; set; }
    }

    public class ProductCategoryModifyReq
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public bool IsSub { get; set; }
        public string Status { get; set; } = string.Empty;

        public ProductCategoryModify ConvertToRequestModel(string? UserName) => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            ProductType = ProductType,
            IsSub = IsSub,
            Status = Status,
            User = string.IsNullOrWhiteSpace(UserName) ? "system" : UserName
        };
    }

    public class ProductCategoryModify : ProductCategoryModifyReq
    {
        public string User { get; set; } = "system";
    }
}