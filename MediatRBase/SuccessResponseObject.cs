namespace MediatrExample
{
    public class SuccessResponseObject
    {
        public SuccessResponseObject(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; } 

        public string Message { get; set; }
    }
}
