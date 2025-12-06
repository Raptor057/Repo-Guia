namespace Project.WebApi.Controllers.Users.RequestBodys
{
    public class UserUpdateRequestBody
    {
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? Password { get; set; }
        public long RoleId { get; set; }
    }
}
