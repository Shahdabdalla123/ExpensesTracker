namespace BankProject.Response
{
    public class ResponseTemplate<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }



        public ResponseTemplate(bool success, string message, T data, List<string> errors = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors ?? new List<string>();
        }
        public static ResponseTemplate<T> SuccessMsg(T data, string message = "Request successful")
        {
            return new ResponseTemplate<T>(true, message, data);
        }

        public static ResponseTemplate<T> ErrorMsg(List<string> errors, string message = "An error occurred")
        {
            return new ResponseTemplate<T>(false, message, default(T), errors);
        }

    }
}
