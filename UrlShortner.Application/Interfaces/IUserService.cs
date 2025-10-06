using DataTypes.Requests;
using DataTypes.Responses;

namespace BusinessLayer.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticationRes> RegisterAsync(UserRegisterReq reqData, string ipAddress);

        Task<UserRes> GetByIdAsync(int id);

        Task<UserRes> UpdateAsync(UserRegisterReq reqData, int id);
    }
}
