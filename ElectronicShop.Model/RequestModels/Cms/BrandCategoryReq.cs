namespace ElectronicShop.Model.RequestModels.Cms
{
    public class BrandCategorySearchReq : BaseRequest
    {
        public string? Status { get; set; }
        public string? ProductCategoryCode { get; set; }
        public string? BrandCode { get; set; }
    }

    public class BrandCategoryModifyReq
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProductCategoryCode { get; set; } = string.Empty;
        public string BrandCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public BrandCategoryModify ConvertToRequestModel(string? UserName) => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            ProductCategoryCode = ProductCategoryCode,
            BrandCode = BrandCode,
            Status = Status,
            User = string.IsNullOrWhiteSpace(UserName) ? "system" : UserName
        };
    }

    public class BrandCategoryModify : BrandCategoryModifyReq
    {
        public string User { get; set; } = "system";
    }
}