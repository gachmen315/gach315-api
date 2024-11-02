namespace ElectronicShop.Model.RequestModels.Cms
{
    public class ProductTypeSearchReq : BaseRequest
    {
        public string? Status { get; set; }
    }

    public class ProductTypeModifyReq
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public ProductTypeModify ConvertToRequestModel(string? UserName) => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            Status = Status,
            User = string.IsNullOrWhiteSpace(UserName) ? "system" : UserName
        };
    }

    public class ProductTypeModify : ProductTypeModifyReq
    {
        public string User { get; set; } = "system";
    }
}