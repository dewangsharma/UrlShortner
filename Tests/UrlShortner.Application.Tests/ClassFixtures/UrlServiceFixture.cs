using BusinessLayer.Services;
using DataTypes.Repositories;
using Moq;

namespace UrlShortner.Application.Tests.ClassFixtures
{
    public class UrlServiceFixture: IDisposable
    {
        internal UrlService Sut { get; init; }
        internal Mock<IUrlRepository> MockUrlRepository { get; init; }

        public UrlServiceFixture()
        {
            MockUrlRepository = new Mock<IUrlRepository>();
            Sut = new UrlService(MockUrlRepository.Object);
        }

        public void Dispose()
        {
        }
    }
}
