using UrlShortner.Domain;
using UrlShortner.Application.Models.Urls;

namespace UrlShortner.Application.Mappers
{
    public static class UrlMapper
    {
        public static Url ToDomain(this UrlCreateDto urlCreateDto, string shortnedUrl)
        {
            return new Url { UserId = urlCreateDto.UserId, Status = UrlStatus.Active, Actual = urlCreateDto.Actual, Shortened = shortnedUrl };
        }

        public static UrlDto ToDto(this Url url)
        {
            return new UrlDto { Id = url.Id, Actual = url.Actual, Shortened = url.Shortened, Status = url.Status.ToDto() };
        }

        public static IEnumerable<UrlDto> ToDto(this IEnumerable<Url> url)
        {
            return url.Select(x => new UrlDto { Id = x.Id, Actual = x.Actual, Shortened = x.Shortened, Status = x.Status.ToDto() });
        }

        public static UrlStatus ToDomain(this UrlStatusDto status) => status switch
        {
            UrlStatusDto.Active => UrlStatus.Active,
            UrlStatusDto.Inactive => UrlStatus.Inactive,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };

        public static UrlStatusDto ToDto(this UrlStatus status) => status switch
        {
            UrlStatus.Active => UrlStatusDto.Active,
            UrlStatus.Inactive => UrlStatusDto.Inactive,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}
