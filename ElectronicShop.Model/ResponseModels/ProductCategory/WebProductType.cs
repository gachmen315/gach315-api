namespace ElectronicShop.Model.ResponseModels.ProductCategory
{
    public class WebProductType
    {
        public int Id { get; set; }
        public string ProductTypeCode { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
        public int Type { get; set; } = 1;
    }
}