using UrlShortner.Application.Models.Urls;
using UrlShortner.RestApi.Models.Urls;

namespace UrlShortner.RestApi.Mappers
{
    public static class UrlDtoMapper
    {
        public static UrlCreateDto ToDto(this UrlCreateRequest urlCreateReq, int userId)
        {
            return new UrlCreateDto
            {
                Actual = urlCreateReq.Actual,
                UserId = userId
            };
        }

        public static UrlUpdateDto ToDto(this UrlUpdateRequest urlUpdateReq, int userId)
        {
            return new UrlUpdateDto
            {
                Id = urlUpdateReq.Id,
                Status = urlUpdateReq.Status.ToDto(),
            };
        }

        public static UrlResponse ToResponse(this UrlDto urlDto)
        {
            return new UrlResponse { Id = urlDto.Id, Actual = urlDto.Actual, Shortened = urlDto.Shortened, Status = urlDto.Status.ToResponse() };
        }

        public static IEnumerable<UrlResponse> ToResponse(this IEnumerable<UrlDto> urlDtos)
        {
            return urlDtos.Select(x => new UrlResponse { Id = x.Id, Actual = x.Actual, Shortened = x.Shortened, Status = x.Status.ToResponse() });
        }

        public static UrlStatusDto ToDto(this UrlStatusRequest status) => status switch
        {
            UrlStatusRequest.Active => UrlStatusDto.Active,
            UrlStatusRequest.Inactive => UrlStatusDto.Inactive,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };

        public static UrlStatusRequest ToResponse(this UrlStatusDto status) => status switch
        {
            UrlStatusDto.Active => UrlStatusRequest.Active,
            UrlStatusDto.Inactive => UrlStatusRequest.Inactive,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };


    }
}
