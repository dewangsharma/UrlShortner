using BusinessLayer.Interfaces;
using DataTypes;
using DataTypes.Mappers;
using DataTypes.Repositories;
using DataTypes.Requests;
using DataTypes.Responses;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IUserCredentialRepository _userCredentialRepository;
        public UserService(IUserRepository userRepository, IAuthService authService, IUserCredentialRepository userCredentialRepository)
        {
            _userRepository = userRepository;
            _authService = authService;
            _userCredentialRepository = userCredentialRepository;
        }

        public async Task<AuthenticationRes> RegisterAsync(UserRegisterReq reqData, string ipAddress)
        {
            var authResponse = _authService.EncryptCredentials(reqData.Email, reqData.Password);
            var userCred = new UserCredential() { User = reqData.ToUser(), Username = authResponse.Username, Password = authResponse.Password};
            var userCredDb  = await _userCredentialRepository.AddAsync(userCred);
            if (userCredDb is null)
                throw new Exception("Failed in registering a user");

            return await _authService.LoginAsync(reqData.Email, reqData.Password, ipAddress);
        }

        public async Task<UserRes> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user.ToUserRes();
        }

        public async Task<UserRes> UpdateAsync(UserRegisterReq reqData, int id)
        {
            var userRes = await _userRepository.UpdateAsync(reqData.ToUser());
            return userRes.ToUserRes();
        }
    }
}
