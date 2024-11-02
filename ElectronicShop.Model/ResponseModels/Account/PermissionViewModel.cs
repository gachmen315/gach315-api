namespace ElectronicShop.Model.ResponseModels.Account
{
    public class PermissionViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public IEnumerable<string> Required { get; set; } = new List<string>();
    }

    public class GroupPermissionViewModel
    {
        public int GroupCode { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public IEnumerable<PermissionViewModel> Permissions { get; set; } = new List<PermissionViewModel>();
    }
}