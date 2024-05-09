using Microsoft.Extensions.Logging;
using Tgs.GameInterface.OBet.Client;
using Tgs.GameInterface.OBet.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.OBet.Core.ApplicationServices;

internal class PullRecordCommandService(
	ILogger<PullRecordCommandService> logger,
	IGameInterfaceOBetClient client,
	ICommandService<HandleRecordCommand> service) : ICommandService<PullRecordCommand>
{
	public async ValueTask ExecuteAsync(PullRecordCommand command, CancellationToken cancellationToken = default)
	{
		var gameRecords = await client.GameRecordAsync(
			apiUrl: command.ApiUrl,
			apiKey: command.ApiKey,
			agentId: command.AgentId,
			startDate: command.StartTime,
			endDate: command.EndTime,
			cancellationToken: cancellationToken)
			.ToArrayAsync(
			cancellationToken: cancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameRecordInfo>();

		logger.LogInformation("LogOn:{logOn} GameRecords:{gameRecords}", DateTime.Now.ToString("o"), gameRecords);

		if (!gameRecords.Any())
		{
			return;
		}

		foreach (var gameRecord in gameRecords)
		{
			try
			{
				await service
					.ExecuteAsync(
					command: new(GameRecord: gameRecord),
					cancellationToken: cancellationToken)
					.ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "LogOn:{logOn} Message: An exception occurred in this game record.({gameRecord})", DateTime.Now.ToString("o"), gameRecord);
			}
		}
	}
}
