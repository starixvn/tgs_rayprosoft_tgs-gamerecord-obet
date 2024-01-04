using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.OBet.Common;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.OBet.Controllers;

[Route("[controller]")]
[ApiController]
public class OBetController : ControllerBase
{
    /// <summary>
    /// Games the record manual asynchronous.
    /// </summary>
    /// <param name="messageSender">The message sender.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://sports-api.ob33club.com/",
    ///             "apiKey": "G4sL7r0kKFI5Mm317w99",
    ///             "agentId": "YYTES12",
    ///             "startDate": "2023-03-29T17:26:00+08:00",
    ///             "endDate": "2023-03-29T18:00:00+08:00"
    ///         }
    /// </remarks>
    [HttpPost("manual")]
    public ValueTask GameRecordManualAsync(
        [FromServices] IMessageSender messageSender,
        OBetGameRecordMessage source)
        => messageSender.PublishAsync(
            subject: OBetNatsConst.ManualPublish,
            data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Games the record schedule asynchronous.
    /// </summary>
    /// <param name="messageSender">The message sender.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://sports-api.ob33club.com/",
    ///             "apiKey": "G4sL7r0kKFI5Mm317w99",
    ///             "agentId": "YYTES12",
    ///             "startDate": "2023-03-29T17:26:00+08:00",
    ///             "endDate": "2023-03-29T18:00:00+08:00"
    ///         }
    /// </remarks>
    [HttpPost("schedule")]
    public ValueTask GameRecordScheduleAsync(
        [FromServices] IMessageSender messageSender,
        OBetGameRecordMessage source)
        => messageSender.PublishAsync(
            subject: OBetNatsConst.SchedulePublish,
            data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
            cancellationToken: HttpContext.RequestAborted);
}
