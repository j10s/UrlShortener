using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UrlShortener.Models;

namespace UrlShortener.WebApi.Link;

[ApiController]
[Route("[controller]")]
public class LinkController(ILinkService linkService) : ControllerBase
{
    private const string GetName = "GetLink";

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Models.Link))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async ValueTask<IActionResult> CreateAsync([FromBody]CreateLinkRequest request)
    {
        var response = await linkService.CreateAsync(request);
        return CreatedAtRoute(GetName, new { Controller = "Link", response.Stub }, response);
    }

    [HttpGet("{stub}", Name = GetName)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.Link))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async ValueTask<IActionResult> GetByStubAsync([FromRoute]string stub)
    {
        var link = await linkService.GetByStubAsync(stub);

        if (link == null) return NotFound();

        return Ok(link);
    }

    [HttpPost("{stub}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.Link))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async ValueTask<IActionResult> UpdateByStubAsync([FromRoute]string stub, [FromBody]UpdateLinkRequest request)
    {
        var link = await linkService.UpdateByStubAsync(stub, request);

        if (link == null) return NotFound();

        return Ok(link);
    }

    [HttpDelete("{stub}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async ValueTask<IActionResult> DeleteByStubAsync([FromRoute]string stub)
    {
        var deleted = await linkService.DeleteByStubAsync(stub);

        if (!deleted) return NotFound();

        return Ok();
    }
}