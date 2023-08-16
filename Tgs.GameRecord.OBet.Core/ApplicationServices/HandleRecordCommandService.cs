using Microsoft.Extensions.Options;
using Tgs.GameRecord.Handle.Client;
using Tgs.GameRecord.Handle.Common.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.OBet.Core.ApplicationServices;

internal class HandleRecordCommandService : ICommandService<HandleRecordCommand>
{
    private readonly IGameRecordHandleClient _client;
    private readonly GameRecordInfoOptions _gameRecordInfo;

    public HandleRecordCommandService(
         IOptions<GameRecordInfoOptions> options,
        IGameRecordHandleClient client)
    {
        _gameRecordInfo = options.Value;
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask ExecuteAsync(HandleRecordCommand command, CancellationToken cancellationToken = default)
    {
        var status = command.GameRecord.IsClosed || command.GameRecord.IsVoid ? GameRecordStatus.Closed : GameRecordStatus.Open;
        var externalCreatedOn = command.GameRecord.ExternalCreatedOn.ToDateTimeOffset();
        DateTimeOffset? externalClosedOn = (status == GameRecordStatus.Closed) ? command.GameRecord.ExternalClosedOn.ToDateTimeOffset() : null;
        var bet = command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0);
        var validBet = command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0);
        var settle = command.GameRecord.Settle.ToDecimal().GetValueOrDefault(0);
        var commission = command.GameRecord.Commission.ToDecimal().GetValueOrDefault(0);
        var turnover = command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0);
        var winLoss = command.GameRecord.WinLoss.ToDecimal().GetValueOrDefault(0);

        var clientStatus = GameRecordClientStatus.Open;
        if (status == GameRecordStatus.Closed)
        {
            clientStatus = command.GameRecord.IsVoid ? GameRecordClientStatus.Void : winLoss <= 0 ? GameRecordClientStatus.Loss : GameRecordClientStatus.Win;
        }

        await _client
            .MessageAsync(
            gameProviderCode: _gameRecordInfo.GameProviderCode,
            externalRoundId: command.GameRecord.ExternalRoundId,
            externalGameId: _gameRecordInfo.GameExternalId,
            playerCode: command.GameRecord.PlayerCode,
            status: status,
            clientStatus: clientStatus,
            externalCreatedOn: externalCreatedOn,
            externalClosedOn: externalClosedOn,
            bet: bet,
            validBet: validBet,
            settle: settle,
            commission: commission,
            turnover: turnover,
            playerWinLoss: winLoss,
            additionalData: command.GameRecord.AdditionalData,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);
    }
}
