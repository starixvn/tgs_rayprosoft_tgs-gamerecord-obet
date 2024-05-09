using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Tgs.GameInterface.OBet.Client;
using Tgs.GameRecord.OBet.Core.ApplicationServices;
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
			StartTime: DateTimeOffset.UtcNow.AddMinutes(-10),
			EndTime: DateTimeOffset.UtcNow);

		var service = new PullRecordCommandService(
			fakeLogger,
			fakeClient,
			fakeService);

		await service.ExecuteAsync(command: command);

		_ = await fakeClient
			.Received(1)
			.GameRecordAsync(
			apiUrl: Arg.Is(command.ApiUrl),
			apiKey: Arg.Is(command.ApiKey),
			agentId: Arg.Is(command.AgentId),
			startDate: Arg.Is(command.StartTime),
			endDate: Arg.Is(command.EndTime),
			cancellationToken: Arg.Any<CancellationToken>())
			.ToArrayAsync(
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
