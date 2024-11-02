using ElectronicShop.Model.Domain;
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
using ElectronicShop.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ElectronicShop.Services.Implements
{
    public class APIServices : IAPIServices
    {
        public readonly IAPIRepositories _repo;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ElectronicShopDbContext _context;

        public APIServices(UserManager<Users> userManager, SignInManager<Users> signInManager, IConfiguration configuration, IAPIRepositories repo, ElectronicShopDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _repo = repo;
            _context = context;
        }

        #region Account

        public async Task<CmsLoginRes> CreateToken(Users user)
        {
            try
            {
                var sectionConfig = _configuration.GetSection("JWT");

                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var secretKey = sectionConfig.GetValue("Secret", "ThisIsTheSecureKey1234567890");
                var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                var tokenExpire = sectionConfig.GetValue("TokenExpireAfter", 15);
                var refreshTokenExpire = sectionConfig.GetValue("RefreshTokenExpireAfter", 15);
                var claims = new List<Claim>
                {
                      new Claim(ClaimTypes.Name, user.UserName),
                      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var userRole in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Issuer = sectionConfig.GetValue("ValidIssuer", "https://localhost:7092/"),
                    Audience = sectionConfig.GetValue("ValidAudience", "User"),
                    Expires = DateTime.UtcNow.AddMinutes(tokenExpire),
                    SigningCredentials = new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                };
                var token = jwtTokenHandler.CreateToken(tokenDescriptor);
                var accessToken = jwtTokenHandler.WriteToken(token);
                var refreshToken = GenerateRefreshToken();
                var refreshTokenEntity = new RefreshToken
                {
                    Id = Guid.NewGuid(),
                    JwtId = token.Id,
                    UserId = user.UserName,
                    Token = refreshToken,
                    IsUsed = false,
                    IsRevoked = false,
                    IssuedAt = DateTime.UtcNow,
                    ExpiredAt = DateTime.UtcNow.AddMinutes(refreshTokenExpire)
                };
                await _context.AddAsync(refreshTokenEntity);
                await _context.SaveChangesAsync();
                return new CmsLoginRes
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        public async Task<InternalUser> FindUserCmsByUserName(string UserName)
        {
            try
            {
                return await _repo.FindUserCmsByUserName(UserName);
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<Permission>> GetPermissions(string? Code = null)
        {
            try
            {
                return await _repo.GetPermissions(Code);
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<PermissionBasicModel>> GetPermissionByUsername(string Username)
        {
            try
            {
                return await _repo.GetPermissionByUsername(Username);
            }
            catch (Exception) { throw; }
        }

        public async Task<RefreshTokenRes?> RenewToken(RefreshTokenReq model, string AccessToken)
        {
            try
            {
                RefreshTokenRes res = new RefreshTokenRes();
                var jwtTokenHandler = new JwtSecurityTokenHandler();

                var Secret = _configuration["JWT:Secret"] ?? "ThisIsTheSecureKey1234567890";
                var secretKeyBytes = Encoding.UTF8.GetBytes(Secret);
                var tokenValidateParam = new TokenValidationParameters
                {
                    //tự cấp token
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    //ký vào token
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                    ClockSkew = TimeSpan.Zero,

                    ValidateLifetime = false //ko kiểm tra token hết hạn
                };

                //check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(AccessToken, tokenValidateParam, out var validatedToken);

                //check 2: Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        return new RefreshTokenRes()
                        {
                            Message = "Invalid token"
                        };
                    }
                }

                //check 3: Check accessToken expire?
                var exp = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp);
                if (exp == null) return default;
                var utcExpireDate = long.Parse(exp.Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    return new RefreshTokenRes()
                    {
                        Message = "Access token has not yet expired"
                    };
                }

                //check 4: Check refreshtoken exist in DB
                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
                if (storedToken == null)
                {
                    return new RefreshTokenRes()
                    {
                        Message = "Refresh token does not exist",
                    };
                }

                //check 5: check refreshToken is used/revoked?
                if (storedToken.IsUsed)
                {
                    return new RefreshTokenRes()
                    {
                        Message = "Refresh token has been used",
                    };
                }
                if (storedToken.IsRevoked)
                {
                    return new RefreshTokenRes()
                    {
                        Message = "Refresh token has been revoked"
                    };
                }

                //check 6: AccessToken id == JwtId in RefreshToken
                var jti = tokenInVerification?.Claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
                if (storedToken.JwtId != jti)
                {
                    return new RefreshTokenRes()
                    {
                        Message = "Token doesn't match"
                    };
                }

                //Update token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                //create new token
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == storedToken.UserId);
                if (user != null)
                {
                    var token = await CreateToken(user);
                    return new RefreshTokenRes()
                    {
                        Message = "Success",
                        Result = new Token()
                        {
                            AccessToken = token.AccessToken,
                            RefreshToken = token.RefreshToken
                        }
                    };
                }
                return default;
            }
            catch (Exception) { throw; }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }

        public async Task<IEnumerable<MailTemplate>> GetMailTemplates(string Title)
        {
            try
            {
                var result = await _repo.GetMailTemplates(Title);
                return result;
            }
            catch (Exception) { throw; }
        }

        #endregion Account

        #region User

        public async Task<int> InsertInternalUser(UserInsert req)
        {
            try
            {
                return await _repo.InsertInternalUser(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> UpdateUser(UserUpdate user)
        {
            try
            {
                return await _repo.UpdateUser(user);
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<InternalUsers>> GetUserInfoList(UserSearchReq req)
        {
            try
            {
                return await _repo.GetUserInfoList(req);
            }
            catch (Exception) { throw; }
        }

        #endregion User

        #region Category

        public async Task<IEnumerable<Categorys>> GetCategoryList(CategorySearchReq req)
        {
            try
            {
                return await _repo.GetCategoryList(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<Category> GetCategoryDetail(string code)
        {
            try
            {
                return await _repo.GetCategoryDetail(code);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> InsertCategory(CategoryInsert req)
        {
            try
            {
                return await _repo.InsertCategory(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> UpdateCategory(CategoryUpdate req)

        {
            try
            {
                return await _repo.UpdateCategory(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> DeleteCategory(string code, string? username)

        {
            try
            {
                return await _repo.DeleteCategory(code, username);
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<Category>> GetWebCategoryList()

        {
            try
            {
                return await _repo.GetWebCategoryList();
            }
            catch (Exception) { throw; }
        }

        #endregion Category

        #region WebProduct

        public async Task<IEnumerable<WebProduct>> GetWebProductListDiscountedPrice(WebProductDiscountedPriceReq req)
        {
            try
            {
                return await _repo.GetWebProductListDiscountedPrice(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<WebProducts>> GetWebProductList(WebProductReq req)
        {
            try
            {
                return await _repo.GetWebProductList(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<WebProduct> GetWebProductDetail(string code)
        {
            try
            {
                return await _repo.GetWebProductDetail(code);
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<ProductWebs>> GetWebProductList(ProductWebSearchReq req)
        {
            try
            {
                return await _repo.GetWebProductList(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<ProductWebs>> GetWebProductListMostView()
        {
            try
            {
                return await _repo.GetWebProductListMostView();
            }
            catch (Exception) { throw; }
        }

        #endregion WebProduct

        public async Task<IEnumerable<WebProductCategory>> GetWebProductCategoryList(string ProductTypeCode)
        {
            try
            {
                return await _repo.GetWebProductCategoryList(ProductTypeCode);
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<WebProductType>> GetWebProductTypeList()
        {
            try
            {
                return await _repo.GetWebProductTypeList();
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<BreakCumb>> GetWebBreakCumbListByType(BreakCumbSearchReq req)
        {
            try
            {
                return await _repo.GetWebBreakCumbListByType(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<BreakCumb>> GetWebProductCategoryListByType(BreakCumbSearchReq req)
        {
            try
            {
                return await _repo.GetWebProductCategoryListByType(req);
            }
            catch (Exception) { throw; }
        }

        #region *** CMS ***

        #region Dropdown

        public async Task<IEnumerable<Dropdown>> GetBrandDropDown()
        {
            try
            {
                return await _repo.GetBrandDropDown();
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<Dropdown>> GetBrandCategoryDropDown()
        {
            try
            {
                return await _repo.GetBrandCategoryDropDown();
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<Dropdown>> GetProductTypeDropDown()
        {
            try
            {
                return await _repo.GetProductTypeDropDown();
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<Dropdown>> GetProductCategoryDropDown(string code, bool isPageProduct)
        {
            try
            {
                return await _repo.GetProductCategoryDropDown(code, isPageProduct);
            }
            catch (Exception) { throw; }
        }
        public async Task<IEnumerable<Dropdown>> GetSubProductCategoryDropDown(string code)
        {
            try
            {
                return await _repo.GetSubProductCategoryDropDown(code);
            }
            catch (Exception) { throw; }
        }

        #endregion Dropdown

        #region Brand

        public async Task<IEnumerable<Brands>> GetBrandList(BrandSearchReq req)
        {
            try
            {
                return await _repo.GetBrandList(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<Brand> GetBrandDetail(int id)
        {
            try
            {
                return await _repo.GetBrandDetail(id);
            }
            catch (Exception) { throw; }
        }

        public async Task<Brand> GetBrandDetailByCode(string code)
        {
            try
            {
                return await _repo.GetBrandDetailByCode(code);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> InsertBrand(BrandModify req)
        {
            try
            {
                return await _repo.InsertBrand(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> UpdateBrand(BrandModify req)
        {
            try
            {
                return await _repo.UpdateBrand(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> DeleteBrand(int id, string user)
        {
            try
            {
                return await _repo.DeleteBrand(id, user);
            }
            catch (Exception) { throw; }
        }

        #endregion Brand

        #region BrandCategory

        public async Task<IEnumerable<BrandCategorys>> GetBrandCategoryList(BrandCategorySearchReq req)
        {
            try
            {
                return await _repo.GetBrandCategoryList(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<BrandCategory> GetBrandCategoryDetail(int id)
        {
            try
            {
                return await _repo.GetBrandCategoryDetail(id);
            }
            catch (Exception) { throw; }
        }

        public async Task<BrandCategory> GetBrandCategoryDetailByCode(string code)
        {
            try
            {
                return await _repo.GetBrandCategoryDetailByCode(code);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> InsertBrandCategory(BrandCategoryModify req)
        {
            try
            {
                return await _repo.InsertBrandCategory(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> UpdateBrandCategory(BrandCategoryModify req)
        {
            try
            {
                return await _repo.UpdateBrandCategory(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> DeleteBrandCategory(int id, string user)
        {
            try
            {
                return await _repo.DeleteBrandCategory(id, user);
            }
            catch (Exception) { throw; }
        }

        #endregion BrandCategory

        #region ProductCategory

        public async Task<IEnumerable<ProductCategorys>> GetProductCategoryList(ProductCategorySearchReq req)
        {
            try
            {
                return await _repo.GetProductCategoryList(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<ProductCategory> GetProductCategoryDetail(int id)
        {
            try
            {
                return await _repo.GetProductCategoryDetail(id);
            }
            catch (Exception) { throw; }
        }

        public async Task<ProductCategory> GetProductCategoryDetailByCode(string code)
        {
            try
            {
                return await _repo.GetProductCategoryDetailByCode(code);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> InsertProductCategory(ProductCategoryModify req)
        {
            try
            {
                return await _repo.InsertProductCategory(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> UpdateProductCategory(ProductCategoryModify req)
        {
            try
            {
                return await _repo.UpdateProductCategory(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> DeleteProductCategory(int id, string user)
        {
            try
            {
                return await _repo.DeleteProductCategory(id, user);
            }
            catch (Exception) { throw; }
        }

        #endregion ProductCategory

        #region ProductType

        public async Task<IEnumerable<ProductTypes>> GetProductTypeList(ProductTypeSearchReq req)
        {
            try
            {
                return await _repo.GetProductTypeList(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<ProductType> GetProductTypeDetail(int id)
        {
            try
            {
                return await _repo.GetProductTypeDetail(id);
            }
            catch (Exception) { throw; }
        }

        public async Task<ProductType> GetProductTypeDetailByCode(string code)
        {
            try
            {
                return await _repo.GetProductTypeDetailByCode(code);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> InsertProductType(ProductTypeModify req)
        {
            try
            {
                return await _repo.InsertProductType(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> UpdateProductType(ProductTypeModify req)
        {
            try
            {
                return await _repo.UpdateProductType(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> DeleteProductType(int id, string user)
        {
            try
            {
                return await _repo.DeleteProductType(id, user);
            }
            catch (Exception) { throw; }
        }

        #endregion ProductType

        #region SubProductCategory

        public async Task<IEnumerable<SubProductCategorys>> GetSubProductCategoryList(SubProductCategorySearchReq req)
        {
            try
            {
                return await _repo.GetSubProductCategoryList(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<SubProductCategory> GetSubProductCategoryDetail(int id)
        {
            try
            {
                return await _repo.GetSubProductCategoryDetail(id);
            }
            catch (Exception) { throw; }
        }

        public async Task<SubProductCategory> GetSubProductCategoryDetailByCode(string code)
        {
            try
            {
                return await _repo.GetSubProductCategoryDetailByCode(code);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> InsertSubProductCategory(SubProductCategoryModify req)
        {
            try
            {
                return await _repo.InsertSubProductCategory(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> UpdateSubProductCategory(SubProductCategoryModify req)
        {
            try
            {
                return await _repo.UpdateSubProductCategory(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> DeleteSubProductCategory(int id, string user)
        {
            try
            {
                return await _repo.DeleteSubProductCategory(id, user);
            }
            catch (Exception) { throw; }
        }

        #endregion SubProductCategory

        #region Product

        public async Task<IEnumerable<Products>> GetProductList(ProductSearchReq req)
        {
            try
            {
                return await _repo.GetProductList(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<Product> GetProductDetail(int id)
        {
            try
            {
                return await _repo.GetProductDetail(id);
            }
            catch (Exception) { throw; }
        }

        public async Task<Product> GetProductDetailByCode(string code)

        {
            try
            {
                return await _repo.GetProductDetailByCode(code);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> InsertProduct(ProductModify req)
        {
            try
            {
                return await _repo.InsertProduct(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> UpdateProduct(ProductModify req)
        {
            try
            {
                return await _repo.UpdateProduct(req);
            }
            catch (Exception) { throw; }
        }

        public async Task<int> DeleteProduct(int id, string user)
        {
            try
            {
                return await _repo.DeleteProduc(id, user);
            }
            catch (Exception) { throw; }
        }

        #endregion Product

        #endregion *** CMS ***
    }
}