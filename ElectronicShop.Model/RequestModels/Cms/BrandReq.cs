namespace ElectronicShop.Model.RequestModels.Cms
{
    public class BrandSearchReq : BaseRequest
    {
        public string? Status { get; set; }
    }

    public class BrandModifyReq
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public BrandModify ConvertToRequestModel(string? UserName) => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            Status = Status,
            User = string.IsNullOrWhiteSpace(UserName) ? "system" : UserName
        };
    }

    public class BrandModify : BrandModifyReq
    {
        public string User { get; set; } = "system";
    }
}