using ElectronicShop.Infrastructure;
using ElectronicShop.Model.Domain;
using ElectronicShop.Model.RequestModels.Account;
using ElectronicShop.Resource;
using ElectronicShop.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IAPIServices _apiServices;

        protected readonly RoleManager<Role> _roleManager;
        protected readonly UserManager<Users> _userManager;
        protected readonly SignInManager<Users> _signInManager;

        public BaseController(
            UserManager<Users> userManager,
            RoleManager<Role> roleManager,
            SignInManager<Users> signInManager,
            IAPIServices apiServices)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _apiServices = apiServices;
        }

        public string? username => User?.Identity?.IsAuthenticated ?? false ? User.Identity.Name : string.Empty;

        protected IActionResult Forbid(object value) => new ObjectResult(value) { StatusCode = 403 };

        protected AppSetting _appSetting => AppSetting.Instance;

        protected async Task<CheckPermissionResult> CheckPermission(string PermissionName, string? Username = null)
        {
            var user = Username ?? username;
            if (string.IsNullOrWhiteSpace(user))
            {
                return new(false, ErrorMessage.Unauthorized);
            }

            var permissions = await _apiServices.GetPermissionByUsername(user);

            if (permissions.Any(i => i.Code == PermissionName))
            {
                return new();
            }
            var permissionResult = (await _apiServices.GetPermissions(PermissionName))?.FirstOrDefault();
            if (permissionResult == null)
            {
                return new(false, ErrorMessage.PermissionUnknow);
            }
            var permissionName = $"{permissionResult.Name} {permissionResult.Group}";
            return new(false, permissionName);
        }
    }
}