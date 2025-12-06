namespace Project.WebApi.Controllers.Users.RequestBodys
{
    public class UserDisableRequestBody
    {
        public long UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
