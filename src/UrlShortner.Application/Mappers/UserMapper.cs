using UrlShortner.Domain;
using UrlShortner.Application.Models.Users;

namespace UrlShortner.Application.Mappers
{
    public static class UserMapper
    {
        public static User ToDomain(this UserCreateDto userCreateDto)
        {
            return new User { FirstName = userCreateDto.FirstName, LastName = userCreateDto.LastName, Email = userCreateDto.Email };
        }

        public static User ToDomain(this UserUpdateDto userUpdateDto)
        {
            return new User { FirstName = userUpdateDto.FirstName, LastName = userUpdateDto.LastName, Email = userUpdateDto.Email };
        }

        public static UserDto ToDto(this User user)
        {
            return new UserDto { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email };
        }
    }
}
