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

namespace ElectronicShop.Repositories.Interfaces
{
    public interface IAPIRepositories
    {
        #region Account

        Task<InternalUser> FindUserCmsByUserName(string UserName);

        Task<IEnumerable<Permission>> GetPermissions(string? Code = null);

        Task<IEnumerable<PermissionBasicModel>> GetPermissionByUsername(string Username);

        Task<IEnumerable<MailTemplate>> GetMailTemplates(string Title);

        #endregion Account

        #region User

        Task<int> InsertInternalUser(UserInsert req);

        Task<int> UpdateUser(UserUpdate user);

        Task<IEnumerable<InternalUsers>> GetUserInfoList(UserSearchReq req);

        #endregion User

        #region Category

        Task<IEnumerable<Categorys>> GetCategoryList(CategorySearchReq req);

        Task<Category> GetCategoryDetail(string code);

        Task<int> InsertCategory(CategoryInsert req);

        Task<int> UpdateCategory(CategoryUpdate req);

        Task<int> DeleteCategory(string code, string? username);

        Task<IEnumerable<Category>> GetWebCategoryList();

        #endregion Category

        #region WebProduct

        Task<IEnumerable<WebProduct>> GetWebProductListDiscountedPrice(WebProductDiscountedPriceReq req);

        Task<IEnumerable<WebProducts>> GetWebProductList(WebProductReq req);

        Task<WebProduct> GetWebProductDetail(string code);

        Task<IEnumerable<ProductWebs>> GetWebProductList(ProductWebSearchReq req);

        Task<IEnumerable<ProductWebs>> GetWebProductListMostView();

        #endregion WebProduct

        Task<IEnumerable<WebProductCategory>> GetWebProductCategoryList(string ProductTypeCode);

        Task<IEnumerable<WebProductType>> GetWebProductTypeList();

        Task<IEnumerable<BreakCumb>> GetWebBreakCumbListByType(BreakCumbSearchReq req);

        Task<IEnumerable<BreakCumb>> GetWebProductCategoryListByType(BreakCumbSearchReq req);

        #region *** CMS ***

        #region Dropdown

        Task<IEnumerable<Dropdown>> GetBrandDropDown();

        Task<IEnumerable<Dropdown>> GetBrandCategoryDropDown();

        Task<IEnumerable<Dropdown>> GetProductTypeDropDown();

        Task<IEnumerable<Dropdown>> GetProductCategoryDropDown(string code, bool isPageProduct);
        Task<IEnumerable<Dropdown>> GetSubProductCategoryDropDown(string code);

        #endregion Dropdown

        #region Brand

        Task<IEnumerable<Brands>> GetBrandList(BrandSearchReq req);

        Task<Brand> GetBrandDetail(int id);

        Task<Brand> GetBrandDetailByCode(string code);

        Task<int> InsertBrand(BrandModify req);

        Task<int> UpdateBrand(BrandModify req);

        Task<int> DeleteBrand(int id, string user);

        #endregion Brand

        #region BrandCategory

        Task<IEnumerable<BrandCategorys>> GetBrandCategoryList(BrandCategorySearchReq req);

        Task<BrandCategory> GetBrandCategoryDetail(int id);

        Task<BrandCategory> GetBrandCategoryDetailByCode(string code);

        Task<int> InsertBrandCategory(BrandCategoryModify req);

        Task<int> UpdateBrandCategory(BrandCategoryModify req);

        Task<int> DeleteBrandCategory(int id, string user);

        #endregion BrandCategory

        #region ProductCategory

        Task<IEnumerable<ProductCategorys>> GetProductCategoryList(ProductCategorySearchReq req);

        Task<ProductCategory> GetProductCategoryDetail(int id);

        Task<ProductCategory> GetProductCategoryDetailByCode(string code);

        Task<int> InsertProductCategory(ProductCategoryModify req);

        Task<int> UpdateProductCategory(ProductCategoryModify req);

        Task<int> DeleteProductCategory(int id, string user);

        #endregion ProductCategory

        #region ProductType

        Task<IEnumerable<ProductTypes>> GetProductTypeList(ProductTypeSearchReq req);

        Task<ProductType> GetProductTypeDetail(int id);

        Task<ProductType> GetProductTypeDetailByCode(string code);

        Task<int> InsertProductType(ProductTypeModify req);

        Task<int> UpdateProductType(ProductTypeModify req);

        Task<int> DeleteProductType(int id, string user);

        #endregion ProductType

        #region SubProductCategory

        Task<IEnumerable<SubProductCategorys>> GetSubProductCategoryList(SubProductCategorySearchReq req);

        Task<SubProductCategory> GetSubProductCategoryDetail(int id);

        Task<SubProductCategory> GetSubProductCategoryDetailByCode(string code);

        Task<int> InsertSubProductCategory(SubProductCategoryModify req);

        Task<int> UpdateSubProductCategory(SubProductCategoryModify req);

        Task<int> DeleteSubProductCategory(int id, string user);

        #endregion SubProductCategory

        #region Product

        Task<IEnumerable<Products>> GetProductList(ProductSearchReq req);

        Task<Product> GetProductDetail(int id);

        Task<Product> GetProductDetailByCode(string code);

        Task<int> InsertProduct(ProductModify req);

        Task<int> UpdateProduct(ProductModify req);

        Task<int> DeleteProduc(int id, string user);

        #endregion Product

        #endregion *** CMS ***
    }
}