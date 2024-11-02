namespace ElectronicShop.Model.RequestModels.Cms
{
    public class SubProductCategorySearchReq : BaseRequest
    {
        public string? Status { get; set; }
        public string? ProductCategoryCode { get; set; }
    }

    public class SubProductCategoryModifyReq
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProductCategoryCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public SubProductCategoryModify ConvertToRequestModel(string? UserName) => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            ProductCategoryCode = ProductCategoryCode,
            Status = Status,
            User = string.IsNullOrWhiteSpace(UserName) ? "system" : UserName
        };
    }

    public class SubProductCategoryModify : SubProductCategoryModifyReq
    {
        public string User { get; set; } = "system";
    }
}