using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTWebApi.Extensions;
using UrlShortner.Application.Interfaces;
using UrlShortner.Application.Models.Urls;
using UrlShortner.RestApi.Mappers;
using UrlShortner.RestApi.Models.Urls;

namespace RESTWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UrlController : ControllerBase
    {
        private readonly IUrlService _urlService;
        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult> GetAll(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var urlDtos = await _urlService.GetAllAsync(HttpContext.GetUserId(), token);
            return Ok(urlDtos.ToResponse());


            //var url = new UrlRes { ActualUrl = "https://www.youtube.com/watch?v=HGIdAn2h8BA&ab_channel=PatrickGod", ShortenUrl = "" };
            //return Redirect(url.ActualUrl);

            return RedirectPreserveMethod("https://youtube.com");
            // return RedirectPermanent("https://facebook.com");
        }

        [AllowAnonymous]
        [HttpGet("{url}")]
        public async Task<ActionResult> Get(string url, CancellationToken token)
        {
            var urlDto = await _urlService.GetSingleByAliasAsync(url, token);
            if (urlDto is UrlDto)
            {
                return RedirectPermanent(urlDto.Actual);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] UrlCreateRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var urlCreateDto = request.ToDto(userId);
            var urlDto = await _urlService.CreateAsync(urlCreateDto, token);
            return Ok(urlDto.ToResponse());
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UrlUpdateRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var urlUpdateDto = request.ToDto(userId);
            return Ok(await _urlService.UpdateAsync(urlUpdateDto, token));
        }
    }
}
