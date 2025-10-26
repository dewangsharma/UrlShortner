using UrlShortner.Application.Models.Users;

namespace UrlShortner.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateAsync(UserCreateDto userCreateDto);

        Task<UserDto> GetByIdAsync(int id);

        Task<UserDto> UpdateAsync(UserUpdateDto userUpdateDto, int id);
    }
}
