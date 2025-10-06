using BusinessLayer.Services;
using DataTypes;
using DataTypes.Mappers;
using DataTypes.Repositories;
using Moq;
using System.Linq.Expressions;
using UrlShortner.Application.Tests.ClassFixtures;

namespace UrlShortner.Application.Tests.Services
{
    public class UrlServiceShould : IClassFixture<UrlServiceFixture>
    {
        private readonly UrlService _urlService;
        private readonly Mock<IUrlRepository> _urlRepository;
        public UrlServiceShould(UrlServiceFixture urlFixture)
        {
            _urlService = urlFixture.Sut;
            _urlRepository = urlFixture.MockUrlRepository;
        }

        [Fact]
        public async Task GetAllAsync_ForUserId()
        {
            var userId = 1;
            var url1 = new Url()
            {
                Id = 1,
                UserId = userId,
                Actual = "http://example.com/test1-url",
                Shortened = "http://t1.com",
                Status = UrlStatus.Active
            };
            var urlResponse = new List<Url>() { url1 };

            _urlRepository.Setup(url => url.FindAsync(user => user.UserId == userId)).ReturnsAsync(urlResponse);

            var response = await _urlService.GetAllAsync(userId, new CancellationToken());

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
            var url = new Url()
            {
                Id = 1,
                UserId = 1,
                Actual = actualUrl,
                Shortened = "http://t1.com",
                Status = UrlStatus.Active
            };

            _urlRepository.Setup(repo => repo.GetSingleAsync(x => x.Actual.ToLower() == actualUrl.ToLower())).ReturnsAsync(url);

            var response = await _urlService.GetSingleAsync(actualUrl, new CancellationToken());

            Assert.NotNull(response);
            Assert.Equal(url.ToDto(), response);
        }

        [Fact]
        public async Task GetSingleAsync_ReturnsNull_WhenNotFound()
        {
            var actualUrl = "https://example.com/t1";
            var url = new Url()
            {
                Id = 1,
                UserId = 1,
                Actual = actualUrl,
                Shortened = "http://t1.com",
                Status = UrlStatus.Active
            };

            _urlRepository.Setup(repo => repo.GetSingleAsync(x => x.Actual.ToLower() == actualUrl.ToLower())).ReturnsAsync(default(Url));

            var response = await _urlService.GetSingleAsync(actualUrl, new CancellationToken());

            Assert.Null(response);
        }

        [Fact]
        public async Task GetSingleByAliasAsync_ReturnsUrl_WhenFound()
        {
            var shortenedUrl = "https://t1.com";
            var url = new Url()
            {
                Id = 1,
                UserId = 1,
                Actual = "https://example.com/test1-null",
                Shortened = shortenedUrl,
                Status = UrlStatus.Active
            };

            _urlRepository.Setup(repo => repo.GetSingleAsync(x => x.Shortened.ToLower() == shortenedUrl.ToLower())).ReturnsAsync(url);

            var response = await _urlService.GetSingleByAliasAsync(shortenedUrl, new CancellationToken());

            Assert.NotNull(response);
            Assert.Equal(url.ToDto(), response);
        }

        [Fact]
        public async Task GetSingleByAliasAsync_ReturnsNull_WhenNotFound()
        {
            var shortenedUrl = "https://t1.com";
            var url = new Url()
            {
                Id = 1,
                UserId = 1,
                Actual = "https://example.com/test1",
                Shortened = "https://t2.com",
                Status = UrlStatus.Active
            };

            _urlRepository.Setup(repo => repo.GetSingleAsync(x => x.Shortened.ToLower() == shortenedUrl.ToLower())).ReturnsAsync(default(Url));

            var response = await _urlService.GetSingleByAliasAsync(shortenedUrl, new CancellationToken());

            Assert.Null(response);
        }

        [Fact]
        public async Task CreateAsync_Success()
        {
            var actualUrl = "https://url.com/test1_create";
            var userId = 101;

            var url = new Url()
            {
                Id = 11,
                UserId = userId,
                Actual = actualUrl,
                Shortened = "https://t1.com/create",
                Status = UrlStatus.Active
            };

            _urlRepository.Setup(repo => repo.GetSingleAsync(x => x.Actual == actualUrl)).ReturnsAsync(default(Url));
            _urlRepository.Setup(repo => repo.AddAsync(It.IsAny<Url>())).ReturnsAsync(url);

            var response = await _urlService.CreateAsync(actualUrl, userId, new CancellationToken());

            Assert.Equal(actualUrl, response.Actual);
        }

        [Fact]
        public async Task CreateAsync_Exception_WhenActualUrl_AlreadyExists()
        {
            var actualUrl = "https://example.com/test1-alreadyexist";
            var userId = 101;

            var url = new Url()
            {
                Id = 1,
                UserId = userId,
                Actual = actualUrl,
                Shortened = "https://t2.com",
                Status = UrlStatus.Active
            };

            _urlRepository.Setup(repo => repo.GetSingleAsync(x => x.Actual.ToLower() == actualUrl.ToLower())).ReturnsAsync(url);
            _urlRepository.Setup(repo => repo.AddAsync(It.IsAny<Url>())).ReturnsAsync(url);

            var ex = await Assert.ThrowsAsync<Exception>(async () => await _urlService.CreateAsync(actualUrl, userId, new CancellationToken()));

            Assert.Equal($"Url {url.Actual} already exist with Shortned url {url.Shortened}", ex.Message);
        }

    }
}
