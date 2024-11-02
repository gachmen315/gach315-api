using ElectronicShop.Model;
using ElectronicShop.Model.Domain;
using ElectronicShop.Model.RequestModels;
using ElectronicShop.Model.ResponseModels;
using ElectronicShop.Model.ResponseModels.ProductCategory;
using ElectronicShop.Resource;
using ElectronicShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategorysController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public CategorysController(
            UserManager<Users> userManager,
            RoleManager<Role> roleManager,
            SignInManager<Users> signInManager,
            IAPIServices apiServices,
            IConfiguration configuration,
            IWebHostEnvironment environment) : base(userManager, roleManager, signInManager, apiServices)
        {
            _configuration = configuration;
            _environment = environment;
        }
        #region CMS

        #endregion

        #region Web

        [AllowAnonymous]
        [HttpGet("get-web-product-category-list")]
        public async Task<IActionResult> GetWebProductCategoryList(string ProductTypeCode)
        {
            try
            {
                var result = await _apiServices.GetWebProductCategoryList(ProductTypeCode);

                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var listWebProductCategory = new ProductCategoryConverter().ConvertToViewModel(result);

                return Ok(new BaseResponse(listWebProductCategory, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet("get-web-product-type-list")]
        public async Task<IActionResult> GetWebProductTypeList()
        {
            try
            {
                var result = await _apiServices.GetWebProductTypeList();

                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("get-web-breakcumb-list")]
        public async Task<IActionResult> GetWebBreakCumbList(BreakCumbSearchReq req)
        {
            try
            {
                var breakCumbList = await _apiServices.GetWebBreakCumbListByType(req);
                var productCategory = await _apiServices.GetWebProductCategoryListByType(req);

                BreakCumbWebViewModek result = new BreakCumbWebViewModek()
                {
                    BreakCumb = breakCumbList,
                    ProductCategory = productCategory
                };

                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Web
    }
}