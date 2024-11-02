namespace ElectronicShop.Model.RequestModels.Account
{
    public class CheckPermissionResult
    {
        public CheckPermissionResult(bool isPass = true, string message = "Ok!")
        {
            IsPass = isPass;
            Message = message;
        }

        public bool IsPass { get; set; }
        public string Message { get; set; } = "Ok!";
    }
}