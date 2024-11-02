using ElectronicShop.Model;
using ElectronicShop.Model.Domain;
using ElectronicShop.Model.RequestModels.Cms;
using ElectronicShop.Model.ResponseModels.Cms;
using ElectronicShop.Resource;
using ElectronicShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CmsController : BaseController
    {
        private readonly IConfiguration _configuration;
        private static string ApplicationName = "Gach315";

        public CmsController(
            UserManager<Users> userManager,
            RoleManager<Role> roleManager,
            SignInManager<Users> signInManager,
            IAPIServices apiServices,
            IConfiguration configuration) : base(userManager, roleManager, signInManager, apiServices)
        {
            _configuration = configuration;
        }

        #region Dropdown

        [HttpGet("get-dropdown-brand")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBrandDropDown()
        {
            try
            {
                var result = await _apiServices.GetBrandDropDown();

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

        [HttpGet("get-dropdown-brand-category")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBrandCategoryDropDown()
        {
            try
            {
                var result = await _apiServices.GetBrandCategoryDropDown();

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

        [HttpGet("get-dropdown-product-type")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductTypeDropDown()
        {
            try
            {
                var result = await _apiServices.GetProductTypeDropDown();

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

        [HttpGet("get-dropdown-product-category")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductCategoryDropDown(string code, bool isPageProduct = false)
        {
            try
            {
                var result = await _apiServices.GetProductCategoryDropDown(code, isPageProduct);

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

        [HttpGet("get-dropdown-sub-product-category")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubProductCategoryDropDown(string code)
        {
            try
            {
                var result = await _apiServices.GetSubProductCategoryDropDown(code);

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

        #endregion Dropdown

        #region Brand

        [HttpPost("get-brand-list")]
        public async Task<IActionResult> GetBrandList(BrandSearchReq req)
        {
            try
            {
                var result = await _apiServices.GetBrandList(req);

                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var reponse = new PagingResponse<Brand>()
                {
                    PageIndex = req.PageIndex ?? 1,
                    PageSize = req.PageSize ?? 10,
                    TotalRecords = result.First().TotalRows,
                    Data = result.Select(i => i.ConVertToViewModel())
                };

                return Ok(new BaseResponse(reponse, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get-brand-detail/{id}")]
        public async Task<IActionResult> GetBrandDetail(int id)
        {
            try
            {
                var result = await _apiServices.GetBrandDetail(id);

                if (result == null)
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

        [HttpPost("insert-brand")]
        public async Task<IActionResult> InsertBrand(BrandModifyReq req)
        {
            try
            {
                var detail = await _apiServices.GetBrandDetailByCode(req.Code);
                if (detail != null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.CodeExisted));
                }
                var result = await _apiServices.InsertBrand(req.ConvertToRequestModel(username));

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.InsertFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("update-brand")]
        public async Task<IActionResult> UpdateBrand(BrandModifyReq req)
        {
            try
            {
                var detail = await _apiServices.GetBrandDetail(req.Id);
                if (detail == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }
                var result = await _apiServices.UpdateBrand(req.ConvertToRequestModel(username));

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.UpdateFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("delete-brand/{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            try
            {
                string userName = string.IsNullOrEmpty(username) ? "system" : username;

                var detail = await _apiServices.GetBrandDetail(id);
                if (detail == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }
                var result = await _apiServices.DeleteBrand(id, userName);

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.UpdateFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Brand

        #region BrandCategory

        [HttpPost("get-brand-category-list")]
        public async Task<IActionResult> GetBrandCategoryList(BrandCategorySearchReq req)
        {
            try
            {
                var result = await _apiServices.GetBrandCategoryList(req);

                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var reponse = new PagingResponse<BrandCategoryViewModel>()
                {
                    PageIndex = req.PageIndex ?? 1,
                    PageSize = req.PageSize ?? 10,
                    TotalRecords = result.First().TotalRows,
                    Data = result.Select(i => i.ConVertToViewModel())
                };

                return Ok(new BaseResponse(reponse, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get-brand-category-detail/{id}")]
        public async Task<IActionResult> GetBrandCategoryDetail(int id)
        {
            try
            {
                var result = await _apiServices.GetBrandCategoryDetail(id);

                if (result == null)
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

        [HttpPost("insert-brand-category")]
        public async Task<IActionResult> InsertBrandCategory(BrandCategoryModifyReq req)
        {
            try
            {
                var detail = await _apiServices.GetBrandCategoryDetailByCode(req.Code);
                if (detail != null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.CodeExisted));
                }

                var result = await _apiServices.InsertBrandCategory(req.ConvertToRequestModel(username));

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.InsertFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("update-brand-category")]
        public async Task<IActionResult> UpdateBrandCategory(BrandCategoryModifyReq req)
        {
            try
            {
                var detail = await _apiServices.GetBrandCategoryDetail(req.Id);
                if (detail == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var result = await _apiServices.UpdateBrandCategory(req.ConvertToRequestModel(username));

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.UpdateFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("delete-brand-category/{id}")]
        public async Task<IActionResult> DeleteBrandCategory(int id)
        {
            try
            {
                string userName = string.IsNullOrEmpty(username) ? "system" : username;
                var detail = await _apiServices.GetBrandCategoryDetail(id);
                if (detail == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }
                var result = await _apiServices.DeleteBrandCategory(id, userName);

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.UpdateFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion BrandCategory

        #region ProductCategory

        [HttpPost("get-product-category-list")]
        public async Task<IActionResult> GetProductCategoryList(ProductCategorySearchReq req)
        {
            try
            {
                var result = await _apiServices.GetProductCategoryList(req);

                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var reponse = new PagingResponse<ProductCategoryViewModel>()
                {
                    PageIndex = req.PageIndex ?? 1,
                    PageSize = req.PageSize ?? 10,
                    TotalRecords = result.First().TotalRows,
                    Data = result.Select(i => i.ConVertToViewModel())
                };

                return Ok(new BaseResponse(reponse, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get-product-category-detail/{id}")]
        public async Task<IActionResult> GetProductCategoryDetail(int id)
        {
            try
            {
                var result = await _apiServices.GetProductCategoryDetail(id);

                if (result == null)
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

        [HttpPost("insert-product-category")]
        public async Task<IActionResult> InsertProductCategory(ProductCategoryModifyReq req)
        {
            try
            {
                var detail = await _apiServices.GetProductCategoryDetailByCode(req.Code);
                if (detail != null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.CodeExisted));
                }

                var result = await _apiServices.InsertProductCategory(req.ConvertToRequestModel(username));

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.InsertFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("update-product-category")]
        public async Task<IActionResult> UpdateProductCategory(ProductCategoryModifyReq req)
        {
            try
            {
                var detail = await _apiServices.GetProductCategoryDetail(req.Id);
                if (detail == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var result = await _apiServices.UpdateProductCategory(req.ConvertToRequestModel(username));

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.UpdateFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("delete-product-category/{id}")]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            try
            {
                string userName = string.IsNullOrEmpty(username) ? "system" : username;
                var detail = await _apiServices.GetProductCategoryDetail(id);
                if (detail == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }
                var result = await _apiServices.DeleteProductCategory(id, userName);

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.UpdateFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion ProductCategory

        #region ProductType

        [HttpPost("get-product-type-list")]
        public async Task<IActionResult> GetProductTypeList(ProductTypeSearchReq req)
        {
            try
            {
                var result = await _apiServices.GetProductTypeList(req);

                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var reponse = new PagingResponse<ProductType>()
                {
                    PageIndex = req.PageIndex ?? 1,
                    PageSize = req.PageSize ?? 10,
                    TotalRecords = result.First().TotalRows,
                    Data = result.Select(i => i.ConVertToViewModel())
                };

                return Ok(new BaseResponse(reponse, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get-product-type-detail/{id}")]
        public async Task<IActionResult> GetProductTypeDetail(int id)
        {
            try
            {
                var result = await _apiServices.GetProductTypeDetail(id);

                if (result == null)
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

        [HttpPost("insert-product-type")]
        public async Task<IActionResult> InsertProductType(ProductTypeModifyReq req)
        {
            try
            {
                var detail = await _apiServices.GetProductTypeDetailByCode(req.Code);
                if (detail != null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.CodeExisted));
                }

                var result = await _apiServices.InsertProductType(req.ConvertToRequestModel(username));

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.InsertFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("update-product-type")]
        public async Task<IActionResult> UpdateProductType(ProductTypeModifyReq req)
        {
            try
            {
                var detail = await _apiServices.GetProductTypeDetail(req.Id);
                if (detail == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var result = await _apiServices.UpdateProductType(req.ConvertToRequestModel(username));

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.UpdateFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("delete-product-type/{id}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            try
            {
                string userName = string.IsNullOrEmpty(username) ? "system" : username;
                var detail = await _apiServices.GetProductTypeDetail(id);
                if (detail == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }
                var result = await _apiServices.DeleteProductType(id, userName);

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.UpdateFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion ProductType

        #region SubProductCategory

        [HttpPost("get-sub-product-category-list")]
        public async Task<IActionResult> GetSubProductCategoryList(SubProductCategorySearchReq req)
        {
            try
            {
                var result = await _apiServices.GetSubProductCategoryList(req);

                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var reponse = new PagingResponse<SubProductCategoryViewModel>()
                {
                    PageIndex = req.PageIndex ?? 1,
                    PageSize = req.PageSize ?? 10,
                    TotalRecords = result.First().TotalRows,
                    Data = result.Select(i => i.ConVertToViewModel())
                };

                return Ok(new BaseResponse(reponse, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get-sub-product-category-detail/{id}")]
        public async Task<IActionResult> GetSubProductCategoryDetail(int id)
        {
            try
            {
                var result = await _apiServices.GetSubProductCategoryDetail(id);

                if (result == null)
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

        [HttpPost("insert-sub-product-category")]
        public async Task<IActionResult> InsertSubProductCategory(SubProductCategoryModifyReq req)
        {
            try
            {
                var detail = await _apiServices.GetSubProductCategoryDetailByCode(req.Code);
                if (detail != null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.CodeExisted));
                }

                var result = await _apiServices.InsertSubProductCategory(req.ConvertToRequestModel(username));

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.InsertFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("update-sub-product-category")]
        public async Task<IActionResult> UpdateSubProductCategory(SubProductCategoryModifyReq req)
        {
            try
            {
                var detail = await _apiServices.GetSubProductCategoryDetail(req.Id);
                if (detail == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var result = await _apiServices.UpdateSubProductCategory(req.ConvertToRequestModel(username));

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.UpdateFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("delete-sub-product-category/{id}")]
        public async Task<IActionResult> DeleteSubProductCategory(int id)
        {
            try
            {
                string userName = string.IsNullOrEmpty(username) ? "system" : username;
                var detail = await _apiServices.GetSubProductCategoryDetail(id);
                if (detail == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }
                var result = await _apiServices.DeleteSubProductCategory(id, userName);

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.UpdateFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion SubProductCategory

        #region Product

        [HttpPost("get-product-list")]
        public async Task<IActionResult> GetProductList(ProductSearchReq req)
        {
            try
            {
                var result = await _apiServices.GetProductList(req);

                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var reponse = new PagingResponse<ProductListModelView>()
                {
                    PageIndex = req.PageIndex ?? 1,
                    PageSize = req.PageSize ?? 10,
                    TotalRecords = result.First().TotalRows,
                    Data = result.Select(i => i.ConvertToViewModel())
                };

                return Ok(new BaseResponse(reponse, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get-product-detail/{id}")]
        public async Task<IActionResult> GetProductDetail(int id)
        {
            try
            {
                var result = await _apiServices.GetProductDetail(id);

                if (result == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }
                return Ok(new BaseResponse(result.ConvertToViewModel(), ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("insert-product")]
        public async Task<IActionResult> InsertProduct(ProductModifyReq req)
        {
            try
            {
                var detail = await _apiServices.GetProductDetailByCode(req.Code);
                if (detail != null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.CodeExisted));
                }

                var result = await _apiServices.InsertProduct(req.ConvertToRequestModel(username));

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.InsertFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProduct(ProductModifyReq req)
        {
            try
            {
                var detail = await _apiServices.GetProductDetail(req.Id);
                if (detail == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var result = await _apiServices.UpdateProduct(req.ConvertToRequestModel(username));

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.UpdateFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduc(int id)
        {
            try
            {
                string userName = string.IsNullOrEmpty(username) ? "system" : username;
                var detail = await _apiServices.GetProductDetail(id);
                if (detail == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }
                var result = await _apiServices.DeleteProduct(id, userName);

                if (result == 0)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.UpdateFail));
                }
                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Product
    }
}