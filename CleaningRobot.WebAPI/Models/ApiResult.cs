namespace CleaningRobot.WebAPI.Models
{
    public class ApiResult<TBody>
    {
        public ApiResult() { }

        public ApiResult(TBody body) 
        {
            Body = body;
        }

        public ApiResult(ApiError error) 
        {
            Error = error;
        }

        public TBody Body { get; set; }
        public ApiError Error { get; set; }
    }
}