using UrlShortner.Application.Models.Users;
using UrlShortner.RestApi.Models.Users;

namespace UrlShortner.RestApi.Mappers
{
    public static class UserDtoMapper
    {
        public static UserCreateDto ToDto(this UserCreateRequest userCreateReq, string ipAddress)
        {
            return new UserCreateDto
            {
                FirstName = userCreateReq.FirstName,
                LastName = userCreateReq.LastName,
                Email = userCreateReq.Email,
                Password = userCreateReq.Password,
                IPAddress = ipAddress
            };
        }

        public static UserUpdateDto ToDto(this UserUpdateRequest userUpdateReq, string ipAddress)
        {
            return new UserUpdateDto
            {
                FirstName = userUpdateReq.FirstName,
                LastName = userUpdateReq.LastName,
                Email = userUpdateReq.Email,
                Password = userUpdateReq.Password,
                IPAddress = ipAddress
            };
        }

        public static UserResponse ToResponse(this UserDto userDto)
        {
            return new UserResponse { Id = userDto.Id, FirstName = userDto.FirstName, LastName = userDto.LastName, Email = userDto.Email };
        }
    }
}
