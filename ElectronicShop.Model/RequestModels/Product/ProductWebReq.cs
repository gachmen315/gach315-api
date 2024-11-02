using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Model.RequestModels.Product
{
    public class ProductWebSearchReq : BaseRequest
    {
        public string? CategoryCode { get; set; }

        [Required(ErrorMessage = "Loại sắp xếp đang trống!")]
        public string OrderType { get; set; }
    }

    //public class ProductSearchReq : BaseRequest
    //{
    //    public string? CategoryCode { get; set; }

    //    public string? Status { get; set; }
    //}

    public class WebProductDiscountedPriceReq : BaseRequest
    {
        public string ProductType { get; set; } = string.Empty;
    }

    public class WebProductReq : BaseRequest
    {
        public string Code { get; set; } = string.Empty;
        public int Type { get; set; }
        public string Order { get; set; } = string.Empty;
        public string Sort { get; set; } = string.Empty;
    }
}