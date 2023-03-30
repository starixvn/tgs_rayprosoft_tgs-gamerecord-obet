using Microsoft.Extensions.Logging;
using Tgs.GameInterface.OBet.Client;
using Tgs.GameInterface.OBet.Protos;
using Tgs.GameRounds.Schedule.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.OBet.Core.ApplicationServices;

internal class PullRecordCommandService : ICommandService<PullRecordCommand>
{
    private readonly IGameInterfaceOBetClient _client;
    private readonly ILogger<PullRecordCommandService> _logger;
    private readonly ICommandService<ManualHandleRecordCommand> _manualService;
    private readonly ICommandService<ScheduleHandleRecordCommand> _scheduleService;

    public PullRecordCommandService(
        ILogger<PullRecordCommandService> logger,
        IGameInterfaceOBetClient client,
        ICommandService<ScheduleHandleRecordCommand> scheduleService,
        ICommandService<ManualHandleRecordCommand> manualService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _scheduleService = scheduleService ?? throw new ArgumentNullException(nameof(scheduleService));
        _manualService = manualService ?? throw new ArgumentNullException(nameof(manualService));
    }

    public async ValueTask ExecuteAsync(PullRecordCommand command, CancellationToken cancellationToken = default)
    {
        var gameRecords = await _client
            .GameRecordAsync(
                apiUrl: command.ApiUrl,
                apiKey: command.ApiKey,
                agentId: command.AgentId,
                startDate: command.StartDate,
                endDate: command.EndDate,
                cancellationToken: cancellationToken)
            .ToArrayAsync(
                cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordInfo>();

        _logger.LogInformation("LogOn:{logOn} GameRecords:{gameRecords}", DateTime.Now.ToString("o"), gameRecords);

        if (!gameRecords.Any())
        {
            return;
        }

        foreach (var gameRecord in gameRecords)
        {
            try
            {
                if (command.Source == ScheduleFormSource.Schedule)
                {
                    await _scheduleService
                        .ExecuteAsync(
                            command: new(GameRecord: gameRecord),
                            cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
                }
                else
                {
                    await _manualService
                        .ExecuteAsync(
                            command: new(GameRecord: gameRecord),
                            cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "遊戲紀錄處理發生例外.({gameRecord})", gameRecord);
            }
        }
    }
}
