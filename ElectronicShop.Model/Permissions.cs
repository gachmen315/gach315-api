namespace ElectronicShop.Model
{
    public class Permissions
    {
        public string Group { get; set; } = string.Empty;
        public int GroupCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Required { get; set; }
    }

    public class GroupPermissionsViewModel
    {
        public string GroupName { get; set; } = string.Empty;
        public int GroupCode { get; set; }
        public IEnumerable<PermissionsViewModel> Permissions { get; set; } = Enumerable.Empty<PermissionsViewModel>();
    }

    public class PermissionsViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public IEnumerable<string> Required { get; set; } = Enumerable.Empty<string>();
    }
}