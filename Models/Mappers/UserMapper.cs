using DataTypes.Requests;
using DataTypes.Responses;

namespace DataTypes.Mappers
{
    public static class UserMapper
    {
        public static User ToUser(this UserRegisterReq req)
        {
            return new User { FirstName = req.FirstName, LastName = req.LastName, Email = req.Email };
        }

        public static UserRes ToUserRes(this User req)
        {
            return new UserRes { Id = req.Id, FirstName = req.FirstName, LastName = req.LastName, Email = req.Email };
        }
    }
}
