namespace TP.NA.UserService.Application.Commands.User.Request
{
    public class UpdateUserRequest
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}