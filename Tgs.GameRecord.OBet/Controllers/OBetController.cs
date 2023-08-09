using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.OBet.Core.ApplicationServices;
using Tgs.GameRecord.OBet.ViewModels;
using Tgs.GameRounds.Schedule.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.OBet.Controllers;

[Route("[controller]")]
[ApiController]
public class OBetController : ControllerBase
{
    /// <summary>
    /// Executions the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
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
    [HttpPost]
    public ValueTask ExecutionAsync(
        [FromServices] ICommandService<PullRecordCommand> service,
        [FromBody] PullRecordViewModel source)
        => service
        .ExecuteAsync(
            command: new PullRecordCommand(
                source.ApiUrl,
                source.ApiKey,
                source.AgentId,
                source.StartDate,
                source.EndDate,
                ScheduleFormSource.Manual),
            cancellationToken: HttpContext.RequestAborted);
}
