namespace WebApplication.SharedKernel
{
    public class CustomResult
    {
        public CustomResult()
        {
        }

        public bool IsValid { get; set; }
        public string Message { get; set; }
    }

    public class CustomResult<ReturnIdType>
    {
        public CustomResult()
        {
        }

        public ReturnIdType ReturnId { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }

}
