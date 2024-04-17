namespace CallCredit.API.Models
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }

        public ResponseModel(bool success, string message, object? data =null)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ResponseModel SuccessResponse(string message, object? data=null)
        {
            return new ResponseModel(true, message, data);
        }

        public static ResponseModel ErrorResponse(string message, object? data=null)
        {
            return new ResponseModel(false, message, data);
        }
    }

}
