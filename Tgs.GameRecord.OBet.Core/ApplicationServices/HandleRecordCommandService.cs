using System.Text.Json;
using Tgs.GameRounds.Work.Client;
using Tgs.GameRounds.Work.Common.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.OBet.Core.ApplicationServices;

internal class HandleRecordCommandService : ICommandService<HandleRecordCommand>
{
    private readonly IGameRoundsWorkClient _client;
    private readonly IUtensil _utensil;

    public HandleRecordCommandService(
        IUtensil utensil,
        IGameRoundsWorkClient client)
    {
        _utensil = utensil ?? throw new ArgumentNullException(nameof(utensil));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask ExecuteAsync(HandleRecordCommand command, CancellationToken cancellationToken = default)
    {
        var gameProviderId = await _utensil
            .GetGameProviderIdAsync(
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        var gameProviderCode = _utensil
            .GetGameProviderCode();
        var gameExternalId = _utensil
            .GetGameExternalId();
        var gameId = await _utensil
            .GetGameIdAsync(
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var player = await _utensil
            .GetPlayerAsync(
                playerCode: command.GameRecord.PlayerCode,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        bool success = (player != null);

        Guid? gameProviderConfigurationId = default;
        if (player != null)
        {
            gameProviderConfigurationId = await _utensil
                .GetBrandGameProviderConfigurationIdAsync(
                    brandId: player.BrandId,
                    gameProviderId: gameProviderId,
                    cancellationToken: cancellationToken).ConfigureAwait(false);
            if (!gameProviderConfigurationId.HasValue)
            {
                success = false;
            }
        }

        if (success)
        {
            var status = command.GameRecord.IsClosed || command.GameRecord.IsVoid ? RoundStatus.Closed : RoundStatus.Open;
            var externalCreatedOn = command.GameRecord.ExternalCreatedOn.ToDateTimeOffset();
            DateTimeOffset? externalClosedOn = (status == RoundStatus.Closed) ? command.GameRecord.ExternalClosedOn.ToDateTimeOffset() : null;
            var bet = command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0);
            var validBet = command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0);
            var settle = command.GameRecord.Settle.ToDecimal().GetValueOrDefault(0);
            var commission = command.GameRecord.Commission.ToDecimal().GetValueOrDefault(0);
            var turnover = command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0);
            var winLoss = command.GameRecord.WinLoss.ToDecimal().GetValueOrDefault(0);

            var clientStatus = RoundClientStatus.Open;
            if (status == RoundStatus.Closed)
            {
                clientStatus = command.GameRecord.IsVoid ? RoundClientStatus.Void : winLoss <= 0 ? RoundClientStatus.Loss : RoundClientStatus.Win;
            }

            await _client
            .RoundBidding(
                gameProviderId: gameProviderId,
                gameProviderCode: gameProviderCode,
                externalRoundId: command.GameRecord.ExternalRoundId,
                gameId: gameId,
                externalGameId: gameExternalId,
                playerCode: player!.PlayerCode,
                walletId: player!.DefaultWalletId,
                currencyCode: player!.DefaultWalletCurrencyCode,
                brandId: player!.BrandId,
                gameProviderConfigurationId: gameProviderConfigurationId!.Value,
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
        else
        {
            await _client
            .RoundMissing(
                gameProviderId: gameProviderId,
                gameProviderCode: gameProviderCode,
                externalRoundId: command.GameRecord.ExternalRoundId,
                gameId: gameId,
                externalGameId: gameExternalId,
                playerCode: player?.PlayerCode ?? string.Empty,
                data: JsonSerializer.Serialize(command.GameRecord),
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        }
    }
}
