using TP.NA.UserService.Application.Commons;
using TP.NA.UserService.Application.Models;

namespace TP.NA.UserService.Application.Commands.User.Response
{
    public class CreateUserResponse : BaseResponse
    {
        public UserModel User { get; set; }
    }
}
