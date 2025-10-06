using DataTypes.Requests;
using DataTypes.Responses;
using System;

namespace DataTypes.Mappers
{
    public static class UrlMappers
    {
        public static UrlRes ToDto( this Url url)
        {
            return new UrlRes { Id = url.Id, Actual = url.Actual, Shortened = url.Shortened, Status = url.Status };
        }

        public static IEnumerable<UrlRes> ToDto(this IEnumerable<Url> url)
        {
            return url.Select( x => new UrlRes {Id = x.Id, Actual = x.Actual, Shortened = x.Shortened, Status = x.Status });
        }

        public static Url ToDb(this UrlRes url)
        {
            return new Url { Id = url.Id, Actual = url.Actual, Shortened = url.Shortened, Status = url.Status };
        }
    }
}
