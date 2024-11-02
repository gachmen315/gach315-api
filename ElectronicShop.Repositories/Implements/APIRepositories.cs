using ElectronicShop.Infrastructure;
using ElectronicShop.Model.RequestModels;
using ElectronicShop.Model.RequestModels.Account;
using ElectronicShop.Model.RequestModels.Category;
using ElectronicShop.Model.RequestModels.Cms;
using ElectronicShop.Model.RequestModels.Product;
using ElectronicShop.Model.ResponseModels;
using ElectronicShop.Model.ResponseModels.Account;
using ElectronicShop.Model.ResponseModels.Category;
using ElectronicShop.Model.ResponseModels.Cms;
using ElectronicShop.Model.ResponseModels.Product;
using ElectronicShop.Model.ResponseModels.ProductCategory;
using ElectronicShop.Repositories.Interfaces;

namespace ElectronicShop.Repositories.Implements
{
    public class APIRepositories : AbstractRepository<object, string>, IAPIRepositories
    {
        #region Account

        public async Task<InternalUser> FindUserCmsByUserName(string UserName)
        {
            try
            {
                var result = await base.Query<InternalUser>("SPA_Users_Find_By_UserName",
                    new
                    {
                        @UserName = UserName
                    });
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Permission>> GetPermissions(string? Code)
        {
            try
            {
                var result = await base.Query<Permission>("SPA_Permissions_List",
                    new
                    {
                        @Code = Code
                    });
                return result;
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<PermissionBasicModel>> GetPermissionByUsername(string Username)
        {
            try
            {
                var result = await base.Query<PermissionBasicModel>("SPA_Permission_List_By_Username",
                    new
                    {
                        @UserName = Username
                    });
                return result;
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<MailTemplate>> GetMailTemplates(string Title)
        {
            try
            {
                var result = await base.Query<MailTemplate>("SPA_MailTemplates_List",
                    new
                    {
                        @Title = Title
                    });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Account

        #region Role

        public async Task<int> InsertInternalUser(UserInsert req)
        {
            try
            {
                var result = await base.Execute("SPA_Users_Register",
                    new
                    {
                        @Avatar = req.Avatar,
                        @UserId = req.UserId,
                        @UserName = req.UserName,
                        @Email = req.Email,
                        @FullName = req.FullName,
                        @PhoneNumber = req.PhoneNumber,
                        @User = req.User
                    });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateUser(UserUpdate user)
        {
            try
            {
                var result = await base.Execute("SPA_Users_Update", new
                {
                    @UserId = user.UserId,
                    @Avatar = user.Avatar,
                    @FullName = user.FullName,
                    @PhoneNumber = user.PhoneNumber,
                    @Email = user.Email,
                    @Status = user.Status,
                    @User = user.User
                });
                return result;
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<InternalUsers>> GetUserInfoList(UserSearchReq req)
        {
            try
            {
                var result = await base.Query<InternalUsers>("SPA_Users_List", new
                {
                    @Keyword = req.Keyword ?? "",
                    @PageIndex = req.PageIndex ?? 1,
                    @PageSize = req.PageSize ?? 9999,
                    @Status = req.Status
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Role

        #region Category

        public async Task<IEnumerable<Categorys>> GetCategoryList(CategorySearchReq req)
        {
            try
            {
                var result = await base.Query<Categorys>("SPA_Category_GetList", new
                {
                    @Keyword = req.Keyword,
                    @Status = string.IsNullOrWhiteSpace(req.Status) ? null : req.Status,
                    @PageIndex = req.PageIndex ?? 1,
                    @PageSize = req.PageSize ?? 9999
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Category> GetCategoryDetail(string code)
        {
            try
            {
                var result = await base.Query<Category>("SPA_Category_Detail", new
                {
                    @Code = code
                });
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> InsertCategory(CategoryInsert req)
        {
            try
            {
                var result = await base.Execute("SPA_Category_Insert", new
                {
                    @Code = req.Code,
                    @CategotyName = req.Name,
                    @Description = req.Description,
                    @Status = req.Status,
                    @User = req.User
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateCategory(CategoryUpdate req)
        {
            try
            {
                var result = await base.Execute("SPA_Category_Update", new
                {
                    @Code = req.Code,
                    @CategotyName = req.Name,
                    @Description = req.Description,
                    @Status = req.Status,
                    @User = req.User
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteCategory(string code, string? username)
        {
            try
            {
                var result = await base.Execute("SPA_Category_Delete", new
                {
                    @Code = code,
                    @User = string.IsNullOrEmpty(username) ? "system" : username,
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetWebCategoryList()
        {
            try
            {
                var result = await base.Query<Category>("SPA_Web_Category_List", new
                {
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Category

        #region WebProduct

        public async Task<IEnumerable<WebProduct>> GetWebProductListDiscountedPrice(WebProductDiscountedPriceReq req)
        {
            try
            {
                var result = await base.Query<WebProduct>("SPA_Products_Web_List_Discounted_Price", new
                {
                    @ProductType = req.ProductType,
                    @PageIndex = req.PageIndex ?? 1,
                    @PageSize = req.PageSize ?? 10
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<WebProducts>> GetWebProductList(WebProductReq req)
        {
            try
            {
                var result = await base.Query<WebProducts>("SPA_Products_Web_list", new
                {
                    @KeyWord = req.Keyword,
                    @Code = req.Code,
                    @Type = req.Type,
                    @Order = req.Order,
                    @Sort = req.Sort,
                    @PageIndex = req.PageIndex ?? 1,
                    @PageSize = req.PageSize ?? 24
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<WebProduct> GetWebProductDetail(string code)
        {
            try
            {
                var result = await base.Query<WebProduct>("SPA_Products_Web_Detail", new
                {
                    @Code = code
                });
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductWebs>> GetWebProductList(ProductWebSearchReq req)
        {
            try
            {
                var result = await base.Query<ProductWebs>("SPA_Product_Web_List", new
                {
                    @Keyword = req.Keyword,
                    @CategoryCode = req.CategoryCode,
                    @OrderType = req.OrderType,
                    @PageIndex = req.PageIndex ?? 1,
                    @PageSize = req.PageSize ?? 9999
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task<ProductWeb> GetWebProductDetail(string code)
        //{
        //    try
        //    {
        //        var result = await base.Query<ProductWeb>("SPA_Product_Web_Detail", new
        //        {
        //            @Code = code,
        //        });
        //        return result.FirstOrDefault();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task<IEnumerable<ProductWebs>> GetWebProductListMostView()
        {
            try
            {
                var result = await base.Query<ProductWebs>("SPA_Product_Web_List_Most_CountTotalView", new
                {
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion WebProduct

        public async Task<IEnumerable<WebProductCategory>> GetWebProductCategoryList(string ProductTypeCode)
        {
            try
            {
                var result = await base.Query<WebProductCategory>("SPA_ProductCategory_Web_List", new
                {
                    @ProductTypeCode = ProductTypeCode
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<WebProductType>> GetWebProductTypeList()
        {
            try
            {
                var result = await base.Query<WebProductType>("SPA_ProductType_Web_List", new
                {
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<BreakCumb>> GetWebBreakCumbListByType(BreakCumbSearchReq req)
        {
            try
            {
                var result = await base.Query<BreakCumb>("SPA_BreakCumb_Web_List", new
                {
                    @Code = req.Code,
                    @Type = req.Type
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<BreakCumb>> GetWebProductCategoryListByType(BreakCumbSearchReq req)
        {
            try
            {
                var result = await base.Query<BreakCumb>("SPA_ProductCategory_Web_List_By_Type", new
                {
                    @Code = req.Code,
                    @Type = req.Type
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region *** CMS ***

        #region Dropdown

        public async Task<IEnumerable<Dropdown>> GetBrandDropDown()
        {
            try
            {
                var result = await base.Query<Dropdown>("SPA_Brand_Dropdown", new
                {
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Dropdown>> GetBrandCategoryDropDown()
        {
            try
            {
                var result = await base.Query<Dropdown>("SPA_BrandCategory_Dropdown", new
                {
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Dropdown>> GetProductTypeDropDown()
        {
            try
            {
                var result = await base.Query<Dropdown>("SPA_ProductType_Dropdown", new
                {
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Dropdown>> GetProductCategoryDropDown(string code, bool isPageProduct)
        {
            try
            {
                var result = await base.Query<Dropdown>("[SPA_ProductCategory_Dropdown]", new
                {
                    @Code = code,
                    @IsPageProduct = isPageProduct
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Dropdown>> GetSubProductCategoryDropDown(string code)
        {
            try
            {
                var result = await base.Query<Dropdown>("[SPA_SubProductCategory_Dropdown]", new
                {
                    @Code = code
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Dropdown

        #region Brand

        public async Task<IEnumerable<Brands>> GetBrandList(BrandSearchReq req)
        {
            try
            {
                var result = await base.Query<Brands>("SPA_Brand_List", new
                {
                    @Keyword = req.Keyword,
                    @PageIndex = req.PageIndex,
                    @PageSize = req.PageSize,
                    @Status = req.Status
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Brand> GetBrandDetail(int id)
        {
            try
            {
                var result = await base.Query<Brands>("SPA_Brand_Detail", new
                {
                    @Id = id
                });
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Brand> GetBrandDetailByCode(string code)
        {
            try
            {
                var result = await base.Query<Brands>("SPA_Brand_Detail_Code", new
                {
                    @Code = code
                });
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> InsertBrand(BrandModify req)
        {
            try
            {
                var result = await base.Execute("SPA_Brand_Insert", new
                {
                    @Code = req.Code,
                    @Name = req.Name,
                    @Status = req.Status,
                    @User = req.User
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateBrand(BrandModify req)
        {
            try
            {
                var result = await base.Execute("SPA_Brand_Update", new
                {
                    @Id = req.Id,
                    @Name = req.Name,
                    @Status = req.Status,
                    @User = req.User
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteBrand(int id, string user)
        {
            try
            {
                var result = await base.Execute("SPA_Brand_Delete", new
                {
                    @Id = id,
                    @User = user
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Brand

        #region BrandCategory

        public async Task<IEnumerable<BrandCategorys>> GetBrandCategoryList(BrandCategorySearchReq req)
        {
            try
            {
                var result = await base.Query<BrandCategorys>("SPA_BrandCategory_List", new
                {
                    @Keyword = req.Keyword,
                    @PageIndex = req.PageIndex,
                    @PageSize = req.PageSize,
                    @BrandCode = req.BrandCode,
                    @ProductCategoryCode = req.ProductCategoryCode,
                    @Status = req.Status
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BrandCategory> GetBrandCategoryDetail(int id)
        {
            try
            {
                var result = await base.Query<BrandCategory>("SPA_BrandCategory_Detail", new
                {
                    @Id = id
                });
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BrandCategory> GetBrandCategoryDetailByCode(string code)
        {
            try
            {
                var result = await base.Query<BrandCategory>("SPA_BrandCategory_Detail_Code", new
                {
                    @Code = code
                });
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> InsertBrandCategory(BrandCategoryModify req)
        {
            try
            {
                var result = await base.Execute("SPA_BrandCategory_Insert", new
                {
                    @Code = req.Code,
                    @Name = req.Name,
                    @BrandCode = req.BrandCode,
                    @ProductCategoryCode = req.ProductCategoryCode,
                    @Status = req.Status,
                    @User = req.User
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateBrandCategory(BrandCategoryModify req)
        {
            try
            {
                var result = await base.Execute("SPA_BrandCategory_Update", new
                {
                    @Id = req.Id,
                    @Name = req.Name,
                    @BrandCode = req.BrandCode,
                    @ProductCategoryCode = req.ProductCategoryCode,
                    @Status = req.Status,
                    @User = req.User
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteBrandCategory(int id, string user)
        {
            try
            {
                var result = await base.Execute("SPA_BrandCategory_Delete", new
                {
                    @Id = id,
                    @User = user
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion BrandCategory

        #region ProductCategory

        public async Task<IEnumerable<ProductCategorys>> GetProductCategoryList(ProductCategorySearchReq req)
        {
            try
            {
                var result = await base.Query<ProductCategorys>("SPA_ProductCategory_List", new
                {
                    @Keyword = req.Keyword,
                    @PageIndex = req.PageIndex,
                    @PageSize = req.PageSize,
                    @ProductType = req.ProductType,
                    @Status = req.Status
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductCategory> GetProductCategoryDetail(int id)
        {
            try
            {
                var result = await base.Query<ProductCategory>("SPA_ProductCategory_Detail", new
                {
                    @Id = id
                });
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductCategory> GetProductCategoryDetailByCode(string code)
        {
            try
            {
                var result = await base.Query<ProductCategory>("SPA_ProductCategory_Detail_Code", new
                {
                    @Code = code
                });
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> InsertProductCategory(ProductCategoryModify req)
        {
            try
            {
                var result = await base.Execute("SPA_ProductCategory_Insert", new
                {
                    @Code = req.Code,
                    @Name = req.Name,
                    @ProductType = req.ProductType,
                    @IsSub = req.IsSub,
                    @Status = req.Status,
                    @User = req.User
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateProductCategory(ProductCategoryModify req)
        {
            try
            {
                var result = await base.Execute("SPA_ProductCategory_Update", new
                {
                    @Id = req.Id,
                    @Name = req.Name,
                    @ProductType = req.ProductType,
                    @IsSub = req.IsSub,
                    @Status = req.Status,
                    @User = req.User
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteProductCategory(int id, string user)
        {
            try
            {
                var result = await base.Execute("SPA_ProductCategory_Delete", new
                {
                    @Id = id,
                    @User = user
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion ProductCategory

        #region ProductType

        public async Task<IEnumerable<ProductTypes>> GetProductTypeList(ProductTypeSearchReq req)
        {
            try
            {
                var result = await base.Query<ProductTypes>("SPA_ProductType_List", new
                {
                    @Keyword = req.Keyword,
                    @PageIndex = req.PageIndex,
                    @PageSize = req.PageSize,
                    @Status = req.Status
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductType> GetProductTypeDetail(int id)
        {
            try
            {
                var result = await base.Query<ProductType>("SPA_ProductType_Detail", new
                {
                    @Id = id
                });
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductType> GetProductTypeDetailByCode(string code)
        {
            try
            {
                var result = await base.Query<ProductType>("SPA_ProductType_Detail_Code", new
                {
                    @Code = code
                });
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> InsertProductType(ProductTypeModify req)
        {
            try
            {
                var result = await base.Execute("SPA_ProductType_Insert", new
                {
                    @Code = req.Code,
                    @Name = req.Name,
                    @Status = req.Status,
                    @User = req.User
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateProductType(ProductTypeModify req)
        {
            try
            {
                var result = await base.Execute("SPA_ProductType_Update", new
                {
                    @Id = req.Id,
                    @Name = req.Name,
                    @Status = req.Status,
                    @User = req.User
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteProductType(int id, string user)
        {
            try
            {
                var result = await base.Execute("SPA_ProductType_Delete", new
                {
                    @Id = id,
                    @User = user
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion ProductType

        #region SubProductCategory

        public async Task<IEnumerable<SubProductCategorys>> GetSubProductCategoryList(SubProductCategorySearchReq req)
        {
            try
            {
                var result = await base.Query<SubProductCategorys>("SPA_SubProductCategory_List", new
                {
                    @Keyword = req.Keyword,
                    @PageIndex = req.PageIndex,
                    @PageSize = req.PageSize,
                    @ProductCategoryCode = req.ProductCategoryCode,
                    @Status = req.Status
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SubProductCategory> GetSubProductCategoryDetail(int id)
        {
            try
            {
                var result = await base.Query<SubProductCategory>("SPA_SubProductCategory_Detail", new
                {
                    @Id = id
                });
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SubProductCategory> GetSubProductCategoryDetailByCode(string code)
        {
            try
            {
                var result = await base.Query<SubProductCategory>("SPA_SubProductCategory_Detail_Code", new
                {
                    @Code = code
                });
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> InsertSubProductCategory(SubProductCategoryModify req)
        {
            try
            {
                var result = await base.Execute("SPA_SubProductCategory_Insert", new
                {
                    @Code = req.Code,
                    @Name = req.Name,
                    @ProductCategoryCode = req.ProductCategoryCode,
                    @Status = req.Status,
                    @User = req.User
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateSubProductCategory(SubProductCategoryModify req)
        {
            try
            {
                var result = await base.Execute("SPA_SubProductCategory_Update", new
                {
                    @Id = req.Id,
                    @Name = req.Name,
                    @ProductCategoryCode = req.ProductCategoryCode,
                    @Status = req.Status,
                    @User = req.User
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteSubProductCategory(int id, string user)
        {
            try
            {
                var result = await base.Execute("SPA_SubProductCategory_Delete", new
                {
                    @Id = id,
                    @User = user
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion SubProductCategory

        #region Product

        public async Task<IEnumerable<Products>> GetProductList(ProductSearchReq req)
        {
            try
            {
                var result = await base.Query<Products>("SPA_Products_List", new
                {
                    @Keyword = req.Keyword,
                    @PageIndex = req.PageIndex,
                    @PageSize = req.PageSize,
                    @ProductType = req.ProductType,
                    @ProductCategoryCode = req.ProductCategoryCode,
                    @SubProductCategoryCode = req.SubProductCategoryCode,
                    @Status = req.Status
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Product> GetProductDetail(int id)
        {
            try
            {
                var result = await base.Query<Product>("SPA_Products_Detail", new
                {
                    @Id = id
                });
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Product> GetProductDetailByCode(string code)
        {
            try
            {
                var result = await base.Query<Product>("SPA_Products_Detail_Code", new
                {
                    @Code = code
                });
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> InsertProduct(ProductModify req)
        {
            try
            {
                var result = await base.Execute("SPA_Products_Insert", new
                {
                    @Code = req.Code,
                    @ProductCategoryCode = req.ProductCategoryCode,
                    @SubProductCategoryCode = req.SubProductCategoryCode,
                    @Name = req.Name,
                    @Status = req.Status,
                    @IsPublished = req.IsPublished,
                    @Description = req.Description,
                    @Image = req.Image,
                    @Price = req.Price,
                    @DiscountedPrice = req.DiscountedPrice,
                    @PercentDiscount = req.PercentDiscount,
                    @User = req.User
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateProduct(ProductModify req)
        {
            try
            {
                var result = await base.Execute("SPA_Products_Update", new
                {
                    @Id = req.Id,
                    @Code = req.Code,
                    @ProductCategoryCode = req.ProductCategoryCode,
                    @SubProductCategoryCode = req.SubProductCategoryCode,
                    @Name = req.Name,
                    @Status = req.Status,
                    @IsPublished = req.IsPublished,
                    @Description = req.Description,
                    @Image = req.Image,
                    @Price = req.Price,
                    @DiscountedPrice = req.DiscountedPrice,
                    @PercentDiscount = req.PercentDiscount,
                    @User = req.User
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteProduc(int id, string user)
        {
            try
            {
                var result = await base.Execute("SPA_Products_Delete", new
                {
                    @Id = id,
                    @User = user
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Product

        #endregion *** CMS ***
    }
}