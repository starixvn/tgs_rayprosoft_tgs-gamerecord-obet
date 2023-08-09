using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Tgs.GameInterface.OBet.Client;
using Tgs.GameRecord.OBet.Core.ApplicationServices;
using Tgs.GameRounds.Schedule.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.OBet.Core.UnitTest.ApplicationServices;

public class PullRecordCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeLogger = NullLogger<PullRecordCommandService>.Instance;
        var fakeClient = Substitute.For<IGameInterfaceOBetClient>();
        var fakeService = Substitute.For<ICommandService<HandleRecordCommand>>();

        var command = new PullRecordCommand(
            ApiUrl: new("http://localhost/"),
            ApiKey: "apiKey",
            AgentId: "agentId",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow,
            Source: ScheduleFormSource.Schedule);

        var service = new PullRecordCommandService(
            fakeLogger,
            fakeClient,
            fakeService);

        await service
            .ExecuteAsync(
                command: command)
            .ConfigureAwait(false);

        _ = await fakeClient
            .Received(1)
            .GameRecordAsync(
                apiUrl: Arg.Is(command.ApiUrl),
                apiKey: Arg.Is(command.ApiKey),
                agentId: Arg.Is(command.AgentId),
                startDate: Arg.Is(command.StartDate),
                endDate: Arg.Is(command.EndDate),
                cancellationToken: Arg.Any<CancellationToken>())
            .ToArrayAsync(
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}
