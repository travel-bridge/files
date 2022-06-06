using Files.Application.Commands;
using Files.Application.Queries;
using Files.Application.Responses;
using Files.Services.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Files.Services.Controllers;

[Route("files/images")]
public class ImageController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IImageQueries _imageQueries;

    public ImageController(IMediator mediator, IImageQueries imageQueries)
    {
        _mediator = mediator;
        _imageQueries = imageQueries;
    }
    
    [Authorize(AuthorizePolicies.WriteFiles)]
    [HttpPost("upload")]
    public async Task<ActionResult<UploadImageResponse>> UploadAsync([FromForm] IFormFile file)
    {
        await using var memoryStream = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(memoryStream);
        var content = memoryStream.ToArray();
        var command = new UploadImageCommand(file.FileName, file.ContentType, content);
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }

    [Authorize(AuthorizePolicies.ReadFiles)]
    [HttpGet("download")]
    public async Task<ActionResult<byte[]>> DownloadAsync([FromQuery] string groupId, [FromQuery] string sizeName)
    {
        var response = await _imageQueries.DownloadAsync(groupId, sizeName);
        if (response is null)
            return NotFound();

        return File(response.Content, response.ContentType, response.Name);
    }
}