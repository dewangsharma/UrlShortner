﻿using UrlShortner.Application.Repositories;
using UrlShortner.Application.Interfaces;
using UrlShortner.Application.Mappers;
using UrlShortner.Application.Models.Users;

namespace UrlShortner.Application.Services
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

        public async Task<UserDto> CreateAsync(UserCreateDto userCreateDto)
        {
            var encryptionResponse = _authService.EncryptCredentials(userCreateDto.Email, userCreateDto.Password);

            // var userCred = new UserCredential() { User = userCreateDto.ToUser(), Username = encryptionResponse.Username, Password = encryptionResponse.Password};
            // var userCredDb  = await _userCredentialRepository.AddAsync(userCred);
            //if (userCredDb is null)
            //    throw new Exception("Failed in registering a user");

            // return await _authService.LoginAsync(reqData.Email, reqData.Password, ipAddress);
            userCreateDto.Email = encryptionResponse.Email;
            var userEntity = userCreateDto.ToDomain();
            var user =  await _userRepository.AddAsync(userEntity);

            return user.ToDto();
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user.ToDto();
        }

        public async Task<UserDto> UpdateAsync(UserUpdateDto userUpdateDto, int id)
        {
            var userRes = await _userRepository.UpdateAsync(userUpdateDto.ToDomain());
            return userRes.ToDto();
        }
    }
}
