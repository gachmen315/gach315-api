namespace ElectronicShop.Model.ResponseModels.Account
{
    public class Permission
    {
        public string Group { get; set; } = string.Empty;
        public int GroupCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Required { get; set; }
    }

    public class PermissionBasicModel : Permission
    {
        [Obsolete("Remove in Basic Model", true)]
        public new string Group { get; set; } = string.Empty;

        [Obsolete("Remove in Basic Model", true)]
        public new int GroupCode { get; set; }

        [Obsolete("Remove in Basic Model", true)]
        public new string? Required { get; set; }
    }
}