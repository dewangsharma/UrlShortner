using BusinessLayer.Services;
using BusinessLayer.Tests.ClassFixtures;
using DataTypes;
using DataTypes.Mappers;
using DataTypes.Repositories;
using DataTypes.Responses;
using Moq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Xunit.Abstractions;

namespace BusinessLayer.Tests
{
    public class UrlServiceShould: IClassFixture<UrlServiceFixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly UrlService _urlService;
        private readonly Mock<IUrlRepository> _urlRepository;
        public UrlServiceShould(ITestOutputHelper output, UrlServiceFixture urlFixture)
        {
            _output = output;
            _urlService = urlFixture.Sut;
            _urlRepository = urlFixture.MockUrlRepository;
        }

        [Fact]
        public async Task GetAllAsync_ForUserId()
        {
            var userId = 1;
            var url1 = new Url() { Id = 1, UserId = userId, Actual = "http://example.com/test1", Shortened = "http://t1.com", Status = UrlStatus.Active };
            var urlResponse = new List<Url>() { url1 };

            _urlRepository.Setup(url => url.FindAsync(user => user.UserId == userId)).ReturnsAsync(urlResponse);

            var response  = await _urlService.GetAllAsync(userId, new CancellationToken());

            _urlRepository.Verify(x => x.FindAsync(It.IsAny<Expression<Func<Url, bool>>>()), Times.AtLeast(1));
            
            Assert.NotNull(response);
            Assert.Equal(urlResponse.Count(), response.Count());
            Assert.Contains(url1.ToDto(), response);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmpty_WhenUserIdZero()
        {
            var userId = 0;
           
            var response = await _urlService.GetAllAsync(userId, new CancellationToken());

            Assert.NotNull(response);
            Assert.Equal(0, response?.Count());
        }

        [Fact]
        public async Task GetSingleAsync_ReturnsUrl_WhenFound() 
        {
            var actualUrl = "https://example.com/test1";
            var url = new Url() { Id = 1, UserId = 1, Actual = actualUrl, Shortened = "http://t1.com", Status = UrlStatus.Active };

            _urlRepository.Setup(repo => repo.GetSingleAsync(x => x.Actual.ToLower() == actualUrl.ToLower())).ReturnsAsync(url);

            var response = await _urlService.GetSingleAsync(actualUrl, new CancellationToken());
            
            Assert.NotNull(response);
            Assert.Equal<UrlRes>(url.ToDto(), response);
        }

        [Fact]
        public async Task GetSingleAsync_ReturnsNull_WhenNotFound()
        {
            var actualUrl = "https://example.com/t1";
            var url = new Url() { Id = 1, UserId = 1, Actual = actualUrl, Shortened = "http://t1.com", Status = UrlStatus.Active };

            _urlRepository.Setup(repo => repo.GetSingleAsync(x => x.Actual.ToLower() == actualUrl.ToLower())).ReturnsAsync(default(Url));

            var response = await _urlService.GetSingleAsync(actualUrl, new CancellationToken());
            
            Assert.Null(response);
        }

        [Fact]
        public async Task GetSingleByAliasAsync_ReturnsUrl_WhenFound()
        {
            var shortenedUrl = "https://t1.com";
            var url = new Url() { Id = 1, UserId = 1, Actual = "https://example.com/test1", Shortened = shortenedUrl, Status = UrlStatus.Active };

            _urlRepository.Setup(repo => repo.GetSingleAsync(x => x.Shortened.ToLower() == shortenedUrl.ToLower())).ReturnsAsync(url);

            var response = await _urlService.GetSingleByAliasAsync(shortenedUrl, new CancellationToken());

            Assert.NotNull(response);
            Assert.Equal<UrlRes>(url.ToDto(), response);
        }

        [Fact]
        public async Task GetSingleByAliasAsync_ReturnsNull_WhenNotFound()
        {
            var shortenedUrl = "https://t1.com";
            var url = new Url() { Id = 1, UserId = 1, Actual = "https://example.com/test1", Shortened = "https://t2.com", Status = UrlStatus.Active };

            _urlRepository.Setup(repo => repo.GetSingleAsync(x => x.Shortened.ToLower() == shortenedUrl.ToLower())).ReturnsAsync(default(Url));

            var response = await _urlService.GetSingleByAliasAsync(shortenedUrl, new CancellationToken());

            Assert.Null(response);
        }

        

    }
}
