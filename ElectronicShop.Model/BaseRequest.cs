namespace ElectronicShop.Model
{
    public class BaseRequest
    {
        public string? Keyword { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}