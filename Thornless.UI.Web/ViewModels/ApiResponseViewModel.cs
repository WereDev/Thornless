namespace Thornless.UI.Web.ViewModels
{
    public class ApiResponseViewModel
    {
        public object? Data { get; set; }
        public ErrorDetailsModel[]? Errors { get; set; }
    }
}
