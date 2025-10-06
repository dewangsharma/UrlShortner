using BusinessLayer.Interfaces;
using DataTypes.Requests;
using DataTypes.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTWebApi.Extensions;

namespace RESTWebApi.Controllers
{
    [Route("api/[controller]")]
    [Route("/[controller]")]
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
            return Ok(await _urlService.GetAllAsync(HttpContext.GetUserId(), token));


            //var url = new UrlRes { ActualUrl = "https://www.youtube.com/watch?v=HGIdAn2h8BA&ab_channel=PatrickGod", ShortenUrl = "" };
            //return Redirect(url.ActualUrl);

            return RedirectPreserveMethod("https://youtube.com");
            // return RedirectPermanent("https://facebook.com");
        }

        [AllowAnonymous]
        [HttpGet("{url}")]
        public async Task<ActionResult> Get(string url, CancellationToken token)
        {
            var result = await _urlService.GetSingleByAliasAsync(url, token);
            if (result is UrlRes)
            {
                return RedirectPermanent(result.Actual);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] UrlCreateReq request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            return Ok(await _urlService.CreateAsync(request.Actual, userId, token));
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UrlUpdateReq request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            return Ok(await _urlService.UpdateAsync(request, token));
        }
    }
}
