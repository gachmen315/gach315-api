using ElectronicShop.Model;
using ElectronicShop.Model.Domain;
using ElectronicShop.Model.RequestModels.Product;
using ElectronicShop.Model.ResponseModels.Product;
using ElectronicShop.Resource;
using ElectronicShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly IConfiguration _configuration;

        public ProductsController(
            UserManager<Users> userManager,
            RoleManager<Role> roleManager,
            SignInManager<Users> signInManager,
            IAPIServices apiServices,
            IConfiguration configuration) : base(userManager, roleManager, signInManager, apiServices)
        {
            _configuration = configuration;
        }

        #region CMS

        #endregion

        #region WEB

        [AllowAnonymous]
        [HttpPost("web-get-product-list-discounted-price")]
        public async Task<IActionResult> GetWebProductListDiscountedPrice(WebProductDiscountedPriceReq req)
        {
            try
            {
                var result = await _apiServices.GetWebProductListDiscountedPrice(req);

                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var listProduct = result.Select(i => i.ConvertToViewModel());

                return Ok(new BaseResponse(listProduct, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("web-get-product-list")]
        public async Task<IActionResult> GetWebProductList(WebProductReq req)
        {
            try
            {
                var result = await _apiServices.GetWebProductList(req);

                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var listProduct = result.Select(i => i.ConvertToViewModel());

                var reponse = new PagingResponse<WebProductDiscountedPrice>()
                {
                    TotalRecords = result.First().TotalRows,
                    PageIndex = req.PageIndex ?? 1,
                    PageSize = req.PageSize ?? 24,
                    Data = listProduct
                };
                return Ok(new BaseResponse(reponse, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet("web-get-product-detail")]
        public async Task<IActionResult> GetWebProductDetail(string code)
        {
            try
            {
                var result = await _apiServices.GetWebProductDetail(code);

                if (result == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }
                return Ok(new BaseResponse(result.ConvertToDetailViewModel(), ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task<IActionResult> GetWebProductList(ProductWebSearchReq req)
        //{
        //    try
        //    {
        //        var result = await _apiServices.GetWebProductList(req);

        //        if (result == null || !result.Any())
        //        {
        //            return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
        //        }
        //        //var image = new Images();
        //        //image.Base_url = "https://salt.tikicdn.com/ts/product/a2/6d/e9/2c1b3634c350424be4c6f6bda7a4c28e.jpg";

        //        //List<Images> images = new List<Images>();
        //        //images.Add(image);
        //        //string imagesjson = JsonConvert.SerializeObject(images);

        //        var listProduct = result.Select(i => i.ConvertToViewModel());
        //        var listProductViewModel = new PagingResponse<ProductWebListModelView>()
        //        {
        //            PageIndex = req.PageIndex.Value,
        //            PageSize = req.PageSize.Value,
        //            TotalRecords = result.First().TotalRows,
        //            Data = result.Select(i => i.ConvertToViewModel()),
        //        };

        //        return Ok(new BaseResponse(listProductViewModel, ErrorCode.Success, ErrorMessage.Success));
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        #endregion WEB
    }
}