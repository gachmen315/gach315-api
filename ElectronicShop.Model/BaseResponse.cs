using ElectronicShop.Resource;

namespace ElectronicShop.Model
{
    public class BaseResponse
    {
        public BaseResponse(object? data)
        {
            Code = int.Parse(ErrorCode.Success);
            Result = true;
            Data = data;
            Message = ErrorMessage.Success;
        }

        public BaseResponse(object? data, string code, string message)
        {
            int.TryParse(code, out int c);
            Code = c;
            Result = c == 200;
            Data = data;
            Message = message;
        }

        public bool Result { get; set; }

        public int Code { get; set; }

        public object? Data { get; set; }

        public string Message { get; set; }
    }
}