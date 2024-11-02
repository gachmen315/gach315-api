using ElectronicShop.Common.Helper;
using ElectronicShop.Model;
using ElectronicShop.Model.ComplexType;
using ElectronicShop.Model.Domain;
using ElectronicShop.Model.RequestModels.Account;
using ElectronicShop.Model.ResponseModels.Account;
using ElectronicShop.Model.RoleModels;
using ElectronicShop.Resource;
using ElectronicShop.Services.Implements;
using ElectronicShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ElectronicShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IConfiguration _configuration;

        public UsersController(
            UserManager<Users> userManager,
            RoleManager<Role> roleManager,
            SignInManager<Users> signInManager,
            IAPIServices apiServices,
            IConfiguration configuration) : base(userManager, roleManager, signInManager, apiServices)
        {
            _configuration = configuration;
        }

        #region Account

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    return Unauthorized(new BaseResponse(null, ErrorCode.Error, ErrorMessage.Unauthorized));
                }
                var internalUser = await _apiServices.FindUserCmsByUserName(username);
                if (internalUser == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.DataNotFound));
                }
                //Get Info
                var res = new ProfileViewModel()
                {
                    Id = internalUser.Id,
                    UserId = internalUser.UserId,
                    //Avatar = !string.IsNullOrWhiteSpace(internalUser.Avatar) ? _appSetting.ImgUrl + internalUser.Avatar : null,
                    Avatar = internalUser.Avatar,
                    Email = internalUser.Email,
                    FullName = internalUser.FullName,
                    PhoneNumber = internalUser.PhoneNumber,
                    UserName = internalUser.UserName,
                    Status = internalUser.Status,
                };

                //Get Role & GrantedAuthorities/Permission
                var user = await _userManager.FindByNameAsync(username).ConfigureAwait(false);
                var roleName = (await _userManager.GetRolesAsync(user).ConfigureAwait(false)).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(roleName))
                {
                    res.RoleName = "Chưa phân quyền!";
                }
                else
                {
                    var role = await _roleManager.FindByNameAsync(roleName).ConfigureAwait(false);
                    res.RoleId = role.Id;
                    res.RoleName = role.Name;
                    var claims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
                    res.GrantedAuthorities = claims.Select(i => i.Value);
                }

                return Ok(new BaseResponse(res, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(ex, ErrorCode.Error, ErrorMessage.Undefined));
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CmsLoginReq req)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var error = ModelState.Where(e => e.Value?.Errors.Count > 0)
                      .Select(e => e.Value?.Errors?.FirstOrDefault()?.ErrorMessage)
                      .FirstOrDefault();
                    return Ok(new BaseResponse(null, ErrorCode.Error, error ?? ErrorMessage.Undefined));
                }

                var internalUser = await _apiServices.FindUserCmsByUserName(req.UserName);
                var user = await _userManager.FindByNameAsync(req.UserName);
                if (internalUser == null || user == null || user.UserType == UserType.WEB)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.AccountNotExist));
                }

                if (internalUser.Status == "INACTIVE")
                {
                    return Forbid(new BaseResponse(null, ErrorCode.Forbidden, ErrorMessage.AccountInactive));
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, req.Password, false);
                if (!result.Succeeded)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.PasswordFail));
                }

                var token = await _apiServices.CreateToken(user);
                if (token == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.LoginFail));
                }
                var sectionConfig = _configuration.GetSection("JWT");
                var tokenExpire = sectionConfig.GetValue("TokenExpireAfter", 15);
                var response = new
                {
                    accessToken = token.AccessToken,
                    refreshToken = token.RefreshToken,
                    expiresIn = (int)(DateTime.UtcNow.AddMinutes(tokenExpire) - DateTime.UtcNow).TotalSeconds
                };
                return Ok(new BaseResponse(response, ErrorCode.Success, ErrorMessage.LoginSuccess));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(ex, ErrorCode.Error, ErrorMessage.Undefined));
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenReq req)
        {
            try
            {
                string authHeader = Request.Headers["Authorization"].ToString();
                if (authHeader != null && authHeader.StartsWith("Bearer "))
                {
                    string token = authHeader.Substring("Bearer ".Length);

                    var result = await _apiServices.RenewToken(req, token);
                    if (result?.Message == "Success" && result.Result != null)
                    {
                        var sectionConfig = _configuration.GetSection("JWT");
                        var tokenExpire = sectionConfig.GetValue("TokenExpireAfter", 15);
                        var response = new
                        {
                            access_token = result.Result.AccessToken,
                            refresh_token = result.Result.RefreshToken,
                            expires_in = (int)(DateTime.UtcNow.AddMinutes(tokenExpire) - DateTime.UtcNow).TotalSeconds
                        };

                        return Ok(new BaseResponse(response, ErrorCode.Success, ErrorMessage.Success));
                    }
                    return Ok(new BaseResponse(String.Empty, ErrorCode.Error, result?.Message ?? ErrorMessage.Undefined));
                }

                return Ok(new BaseResponse(String.Empty, ErrorCode.Error, ErrorMessage.Failed));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(ex, ErrorCode.Error, ErrorMessage.Undefined));
            }
        }
        //[AllowAnonymous]
        //[HttpPost("forget-password")]
        //public async Task<IActionResult> ForgetPassword([FromBody] CmsForgetPasswordReq req)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            var error = ModelState.Where(e => e.Value?.Errors.Count > 0)
        //              .Select(e => e.Value?.Errors?.FirstOrDefault()?.ErrorMessage)
        //              .FirstOrDefault();
        //            return Ok(new BaseResponse(null, ErrorCode.Error, error ?? ErrorMessage.Undefined));
        //        }

        //        //Cache check limit Send Mail
        //        var cacheKey = $":authen:cms:forget-password:{req.UserName}";
        //        //var attemp = await _cache.GetCacheHardMode<int?>(cacheKey);
        //        var attemp = 1;
        //        var limitCacheSendMail = _configuration.GetSection("AppSetting").GetValue<int>("LimitCacheSendMail", 10);
        //        if (attemp >= limitCacheSendMail)
        //        {
                   
        //            return Ok(new BaseResponse(null, ErrorCode.Error, "Bạn đã vượt quá số lần nhận mail cho phép trong ngày.\nVui lòng liên hệ nhumaidao2024@gmail.com cấp lại mật khẩu email hoặc quay lại vào ngày mai."));
        //        }

        //        var user = await _userManager.FindByNameAsync(req.UserName);
        //        if (user == null || user.UserType == UserType.WEB)
        //        {
        //            return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.DataNotFound));
        //        }

              
        //        if (string.IsNullOrWhiteSpace(user.Email))
        //        {
        //            return Ok(new BaseResponse(null, ErrorCode.Error, "Bạn chưa cung cấp Email để nhận mật khẩu mới"));
        //        }

        //        var newPass = PasswordHelper.CreateRandomPassword();
        //        var userInfo = await _apiServices.FindUserCmsByUserName(user.UserName);
        //        await _userManager.RemovePasswordAsync(user);
        //        await _userManager.AddPasswordAsync(user, newPass);

        //        var mailTemplate = (await _apiServices.GetMailTemplates("FORGOT_PASSWORD")).FirstOrDefault();

        //        var content = mailTemplate.MailContent
        //           .Replace("{imgUrl}", _appSetting.ImgUrl)
        //           .Replace("{feUrl}", _appSetting.FrontEndDomain)
        //           .Replace("{{username}}", user.UserName)
        //           .Replace("{{password}}", newPass)
        //           .Replace("{{FirstName}}", userInfo.FullName);

        //        var emailConfig = (await _apiServices.GetEmailConfigs("3")).FirstOrDefault();
        //        var mailHelper = new MailHelper(emailConfig);

        //        await mailHelper.Send(user.Email, mailTemplate.MailSubject, content).ConfigureAwait(false);

        //        //Cache redis limit
        //        TimeSpan timeLeft = DateTime.Today.AddDays(1) - DateTime.Now;
        //        await _cache.SetCacheHardMode(cacheKey, (attemp ?? 0) + 1, timeLeft, keepExpiration: attemp.HasValue);
        //        return Ok(new BaseResponse<string>(user.Email, ErrorCode.Success, ErrorMessage.Success));
        //    }
        //    catch (Exception ex)
        //    {
        //        var log = new LoggerInternal()
        //        {
        //            TypeLog = "Error",
        //            Package = GetType().Namespace,
        //            ClassName = GetType().Name,
        //            Method = MethodBase.GetCurrentMethod()?.ReflectedType?.Name,
        //            Parameters = JsonConvert.SerializeObject(req),
        //            Message = JsonConvert.SerializeObject(ex)
        //        };
        //        await _elastic.IndexDocumentAsync(log);
        //        return Ok(new BaseResponse<object>(string.Empty, ErrorCode.Error, ErrorMessage.Undefined));
        //    }
        //}
        #endregion Account

        #region User

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(UserInsertReq req)
        {
            try
            {
                //var checkPermission = await base.CheckPermission(PermissionsResx.CreateUser);
                //if (!checkPermission.IsPass)
                //{
                //    return Forbid(new BaseResponse<object>(checkPermission.Message, ErrorCode.Forbidden, ErrorMessage.AccessDenied));
                //}

                if (!ModelState.IsValid)
                {
                    var error = ModelState.Where(e => e.Value?.Errors.Count > 0)
                      .Select(e => e.Value?.Errors?.FirstOrDefault()?.ErrorMessage)
                      .FirstOrDefault();
                    return Ok(new BaseResponse(null, ErrorCode.Error, error ?? ErrorMessage.Undefined));
                }

                var existUsername = await _userManager.FindByNameAsync(req.UserName);
                if (existUsername != null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.UserNameExist));
                }

                //Create AspNetUser
                var user = new Users
                {
                    UserName = req.UserName,
                    UserType = UserType.CMS,
                    Email = !string.IsNullOrWhiteSpace(req.Email) ? req.Email : null,
                    PhoneNumber = !string.IsNullOrWhiteSpace(req.PhoneNumber) ? req.PhoneNumber : null,
                };

                var result = await _userManager.CreateAsync(user, req.Password);

                if (!result.Succeeded)
                {
                    throw new Exception("Create AspNetUsers fail!");
                }

                //Create User
                var param = new UserInsert()
                {
                    Avatar = req.Avatar,
                    Email = req.Email,
                    FullName = req.FullName,
                    PhoneNumber = req.PhoneNumber,
                    UserId = user.Id,
                    UserName = user.UserName,
                    User = username ?? "system",
                };

                var iUserResult = await _apiServices.InsertInternalUser(param);
                if (iUserResult == 0)
                {
                    throw new Exception("Create Users fail!");
                }

                //Add role
                if (!string.IsNullOrWhiteSpace(req.RoleId))
                {
                    var role = await _roleManager.FindByIdAsync(req.RoleId);
                    if (role != null)
                    {
                        var aRoleResult = await _userManager.AddToRoleAsync(user, role.Name);
                        if (!aRoleResult.Succeeded)
                        {
                            return Ok(new BaseResponse(null, ErrorCode.Success, "Tài khoản đã được tạo, tuy nhiên việc gán quyền đã bị lỗi!"));
                        }
                    }
                }

                //var mailResult = "";
                ////sendmail
                //var mailConfig = (await _cmsServices.GetEmailConfigs("3")).FirstOrDefault();
                //if (mailConfig != null && !string.IsNullOrWhiteSpace(req.Email))
                //{
                //    var mail = new MailHelper(mailConfig);
                //    var template = (await _cmsServices.GetMailTemplates("NEW_ACCOUNT")).FirstOrDefault();

                //    var content = template.MailContent.Replace("{imgUrl}", _appSetting.ImgUrl)
                //                                      .Replace("{feUrl}", _appSetting.FrontEndDomain)
                //                                      .Replace("{{FirstName}}", req.FullName ?? req.UserName)
                //                                      .Replace("{{username}}", req.UserName)
                //                                      .Replace("{{password}}", req.Password);
                //    mailResult = await mail.Send(req.Email, template.MailSubject, content);
                //}

                return Ok(new BaseResponse(null, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(string.Empty, ErrorCode.Error, ex.Message ?? ErrorMessage.Undefined));
            }
        }

        [HttpGet("list-users/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetUserList(string? keyword, int pageIndex = 1, int pageSize = 10, string? status = null)
        {
            var req = new UserSearchReq()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Status = status
            };
            try
            {
                //var checkPermission = await base.CheckPermission(PermissionsResx.ViewListUser);
                //if (!checkPermission.IsPass)
                //{
                //    return Forbid(new BaseResponse(checkPermission.Message, ErrorCode.Forbidden, ErrorMessage.AccessDenied));
                //}

                var listUsers = await _apiServices.GetUserInfoList(req);

                if (listUsers == null || !listUsers.Any())
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                listUsers.ToList().ForEach(i =>
                {
                    i.Avatar = i.Avatar;
                    i.UserType = i.UserType == ((int)UserType.WEB).ToString() ? nameof(UserType.WEB) : i.UserType == ((int)UserType.CMS).ToString() ? nameof(UserType.CMS) : string.Empty;
                });

                var listUserInfo = new PagingResponse<InternalUsersViewModel>()
                {
                    PageIndex = req.PageIndex.Value,
                    PageSize = req.PageSize.Value,
                    TotalRecords = listUsers.First().TotalRows,
                    Data = listUsers.Select(i => i.ConvertToViewModel()),
                };

                return Ok(new BaseResponse(listUserInfo, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(ex, ErrorCode.Error, ErrorMessage.Undefined));
            }
        }

        [HttpGet("detail-user/{userName}")]
        public async Task<IActionResult> GetUserDetail(string userName)
        {
            try
            {
                var checkPermission = await base.CheckPermission(PermissionsResx.ViewUser);
                if (!checkPermission.IsPass)
                {
                    return Forbid(new BaseResponse(checkPermission.Message, ErrorCode.Forbidden, ErrorMessage.AccessDenied));
                }

                var internalUser = await _apiServices.FindUserCmsByUserName(userName);
                if (internalUser == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }
                var user = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);
                //Get Info
                var res = new UserViewModel()
                {
                    Id = internalUser.Id,
                    Avatar = internalUser.Avatar,
                    Email = internalUser.Email,
                    FullName = internalUser.FullName,
                    PhoneNumber = internalUser.PhoneNumber,
                    UserName = internalUser.UserName,
                    Status = internalUser.Status,
                    UserId = internalUser.UserId
                };

                //Get Role
                var roleName = (await _userManager.GetRolesAsync(user).ConfigureAwait(false)).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(roleName))
                {
                    res.RoleId = null;
                    res.RoleName = "Chưa phân quyền!";
                }
                else
                {
                    var role = await _roleManager.FindByNameAsync(roleName).ConfigureAwait(false);
                    res.RoleId = role.Id;
                    res.RoleName = role.Name;
                }

                return Ok(new BaseResponse(res, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(ex, ErrorCode.Error, ErrorMessage.Undefined));
            }
        }

        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser(UserUpdateReq req)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var error = ModelState.Where(e => e.Value?.Errors.Count > 0)
                      .Select(e => e.Value?.Errors?.FirstOrDefault()?.ErrorMessage)
                      .FirstOrDefault();
                    return Ok(new BaseResponse(string.Empty, ErrorCode.Error, error ?? ErrorMessage.Undefined));
                }

                if (req.UserName != username)
                {
                    var checkPermission = await base.CheckPermission(PermissionsResx.EditUser);
                    if (!checkPermission.IsPass)
                    {
                        return Forbid(new BaseResponse(checkPermission.Message, ErrorCode.Forbidden, ErrorMessage.AccessDenied));
                    }
                }

                //Update User
                var param = new UserUpdate()
                {
                    Avatar = req.Avatar,
                    Email = req.Email,
                    FullName = req.FullName,
                    PhoneNumber = req.PhoneNumber,
                    UserId = req.UserId,
                    Status = req.Status,
                    User = string.IsNullOrWhiteSpace(username) ? "system" : username
                };
                var result = await _apiServices.UpdateUser(param);

                var user = await _userManager.FindByIdAsync(req.UserId).ConfigureAwait(false);
                if (user.PhoneNumber != req.PhoneNumber)
                {
                    await _userManager.SetPhoneNumberAsync(user, req.PhoneNumber);
                }

                if (user.Email != req.Email)
                {
                    await _userManager.SetEmailAsync(user, req.Email);
                }

                if (result == 0)
                {
                    throw new Exception(ErrorMessage.UpdateFail);
                }

                //Update Role
                if (!string.IsNullOrWhiteSpace(req.RoleId))
                {
                    var currentRole = (await _userManager.GetRolesAsync(user).ConfigureAwait(false)).FirstOrDefault();

                    var roleName = (await _roleManager.FindByIdAsync(req.RoleId).ConfigureAwait(false)).Name;
                    if (string.IsNullOrWhiteSpace(currentRole))
                    {
                        await _userManager.AddToRoleAsync(user, roleName).ConfigureAwait(false);
                    }
                    else if (currentRole != roleName)
                    {
                        await _userManager.RemoveFromRoleAsync(user, currentRole).ConfigureAwait(false);
                        await _userManager.AddToRoleAsync(user, roleName).ConfigureAwait(false);
                    }
                }

                return Ok(new BaseResponse(result, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(ex, ErrorCode.Error, ex.Message ?? ErrorMessage.Undefined));
            }
        }

        #endregion User

        #region Role

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole(RoleInsertReq req)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var error = ModelState.Where(e => e.Value?.Errors.Count > 0)
                      .Select(e => e.Value?.Errors?.FirstOrDefault()?.ErrorMessage)
                      .FirstOrDefault();
                    return Ok(new BaseResponse(string.Empty, ErrorCode.Error, error ?? ErrorMessage.Undefined));
                }

                var roleExists = await _roleManager.FindByNameAsync(req.Name);
                if (roleExists != null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.Error, ErrorMessage.RoleExisted));
                }

                var role = new Role(req.Name, req.Description);

                if (req.PermissionCodes != null && req.PermissionCodes.Any())
                {
                    var claims = req.PermissionCodes.Select(c => new IdentityRoleClaim<string>() { ClaimType = "permission", ClaimValue = c });
                    role.Claims = claims.ToList();
                }

                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    throw new Exception("Create Role fail!");
                }
                return Ok(new BaseResponse(role, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(ex, ErrorCode.Error, ErrorMessage.Undefined));
            }
        }

        [HttpGet("list-roles/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetRoleList(string? keyword, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var result = _roleManager.Roles.Include(r => r.Claims).ToList();

                var listData = new List<Role>();
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    listData = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    listData = result.Where(r => r.Name.ToLower().Contains(keyword.ToLower().Trim()))
                    .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }

                if (listData == null || !listData.Any())
                {
                    return Ok(new BaseResponse(listData, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var response = new PagingResponse<Role>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalRecords = result.Count,
                    Data = listData
                };
                return Ok(new BaseResponse(response, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(ex, ErrorCode.Error, ErrorMessage.Undefined));
            }
        }

        [HttpGet("role-detail/{roleId}")]
        public async Task<IActionResult> GetRoleDetail(string roleId)
        {
            try
            {
                var role = _roleManager.Roles.Where(k => k.Id == roleId).Include(r => r.Claims).FirstOrDefault();

                if (role == null)
                {
                    return Ok(new BaseResponse(role, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                return Ok(new BaseResponse(role, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(ex, ErrorCode.Error, ErrorMessage.Undefined));
            }
        }

        [HttpPut("update-role")]
        public async Task<IActionResult> UpdateRole(RoleUpdateReq req)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var error = ModelState.Where(e => e.Value?.Errors.Count > 0)
                      .Select(e => e.Value?.Errors?.FirstOrDefault()?.ErrorMessage)
                      .FirstOrDefault();
                    return Ok(new BaseResponse(string.Empty, ErrorCode.Error, error ?? ErrorMessage.Undefined));
                }

                var roleExisted = _roleManager.Roles.Include(r => r.Claims).FirstOrDefault(i => i.Id == req.Id);

                if (roleExisted == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                roleExisted.Name = req.Name;
                roleExisted.Description = req.Description;
                var result = await _roleManager.UpdateAsync(roleExisted);
                if (!result.Succeeded)
                {
                    throw new Exception("Update Role fail!");
                }

                //Sync Permission a.k.a RoleClaims
                if (req.PermissionCodes != null && req.PermissionCodes.Any())
                {
                    var claims = req.PermissionCodes.Select(c => new IdentityRoleClaim<string>() { ClaimType = "permission", ClaimValue = c }).ToList();

                    var needToRemove = roleExisted.Claims.Where(r => claims.All(r2 => r.ClaimValue != r2.ClaimValue)).ToList();
                    if (needToRemove.Any())
                    {
                        foreach (var claim in needToRemove)
                        {
                            await _roleManager.RemoveClaimAsync(roleExisted, claim.ToClaim());
                        }
                    }

                    var needToAdd = claims.Where(r => roleExisted.Claims.All(r2 => r.ClaimValue != r2.ClaimValue)).ToList();
                    if (needToAdd.Any())
                    {
                        foreach (var claim in needToAdd)
                        {
                            await _roleManager.AddClaimAsync(roleExisted, claim.ToClaim());
                        }
                    }
                }

                return Ok(new BaseResponse(roleExisted, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(ex, ErrorCode.Error, ErrorMessage.Undefined));
            }
        }

        [HttpDelete("delete-role/{roleId}")]
        public async Task<IActionResult> DeleteRole([Required(ErrorMessage = "Mã Vai trò trống!")] string roleId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var error = ModelState.Where(e => e.Value?.Errors.Count > 0)
                      .Select(e => e.Value?.Errors?.FirstOrDefault()?.ErrorMessage)
                      .FirstOrDefault();
                    return BadRequest(new BaseResponse(string.Empty, ErrorCode.Error, error ?? ErrorMessage.Undefined));
                }

                var roleExisted = await _roleManager.FindByIdAsync(roleId);

                if (roleExisted == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                var result = await _roleManager.DeleteAsync(roleExisted);
                if (!result.Succeeded)
                {
                    throw new Exception("Delete Role fail!");
                }
                return Ok(new BaseResponse(roleExisted, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(ex, ErrorCode.Error, ErrorMessage.Undefined));
            }
        }

        [AllowAnonymous]
        [HttpGet("list-permissions")]
        public async Task<IActionResult> GetPermissionList(bool? json)
        {
            try
            {
                var result = await _apiServices.GetPermissions();
                if (json ?? true)
                {
                    var group = result.GroupBy(g => new { g.GroupCode, g.Group })
                                  .Select(g => new GroupPermissionViewModel()
                                  {
                                      GroupCode = g.Key.GroupCode,
                                      GroupName = g.Key.Group,
                                      Permissions = result.Where(p => p.GroupCode == g.Key.GroupCode)
                                                          .Select(pd => new PermissionViewModel()
                                                          {
                                                              Code = pd.Code,
                                                              Name = pd.Name,
                                                              Required = !string.IsNullOrWhiteSpace(pd.Required) ? pd.Required.Split(',') : new List<string>(),
                                                          })
                                  });

                    return Ok(new BaseResponse(group, ErrorCode.Success, ErrorMessage.Success));
                }
                else
                {
                    return Ok(new BaseResponse(result.Select(i => i.Code), ErrorCode.Success, ErrorMessage.Success));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(ex, ErrorCode.Error, ErrorMessage.Undefined));
            }
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(RoleAssignReq req)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var error = ModelState.Where(e => e.Value?.Errors.Count > 0)
                      .Select(e => e.Value?.Errors?.FirstOrDefault()?.ErrorMessage)
                      .FirstOrDefault();
                    return BadRequest(new BaseResponse(string.Empty, ErrorCode.Error, error ?? ErrorMessage.Undefined));
                }

                var user = await _userManager.FindByNameAsync(req.Username);

                var role = await _roleManager.FindByIdAsync(req.RoleId);

                if (user == null || role == null)
                {
                    return Ok(new BaseResponse(null, ErrorCode.DataNotFound, ErrorMessage.DataNotFound));
                }

                //remove existed roles
                var existedRoles = await _userManager.GetRolesAsync(user);
                if (existedRoles != null && existedRoles.Any())
                {
                    foreach (var r in existedRoles)
                    {
                        await _userManager.RemoveFromRoleAsync(user, r);
                    }
                }

                var result = await _userManager.AddToRoleAsync(user, role.Name);

                if (!result.Succeeded)
                {
                    throw new Exception("Assign Role fail!");
                }
                return Ok(new BaseResponse(role, ErrorCode.Success, ErrorMessage.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(ex, ErrorCode.Error, ErrorMessage.Undefined));
            }
        }

        #endregion Role

        [AllowAnonymous]
        [HttpPost("check-permission")]
        public async Task<IActionResult> CheckPermission(CheckPermissionReq req)
        {
            try
            {
                if (req == null || string.IsNullOrWhiteSpace(req.Permission) || string.IsNullOrWhiteSpace(req.Username))
                {
                    return BadRequest(new BaseResponse(null, ErrorCode.Error, ErrorMessage.ParameterNull));
                }

                var result = await base.CheckPermission(req.Permission, req.Username);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(ex, ErrorCode.Error, ErrorMessage.Undefined));
            }
        }
    }
}