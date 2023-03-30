using Google.Protobuf.WellKnownTypes;
using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameInterface.OBet.Protos;
using Tgs.GameRecord.OBet.Core.ApplicationServices;
using Tgs.GameRounds.Work.Manual.Client;
using Tgs.GameRounds.Work.Manual.Common.Protos;

namespace Tgs.GameRecord.OBet.Core.UnitTest.ApplicationServices;

public class ManualHandleRecordCommandServiceUnitTest
{
    [Fact]
    public async Task Settlement_Bets()
    {
        var fakeUtensil = Substitute.For<IUtensil>();
        var fakeClient = Substitute.For<IGameRoundsWorkManualClient>();

        var command = new ManualHandleRecordCommand(
            GameRecord: new GameRecordInfo
            {
                ExternalRoundId = "OU1459290213398",
                PlayerCode = "4ucmqplxb029",
                IsClosed = true,
                IsVoid = false,
                ExternalCreatedOn = DateTimeOffset.Parse("2023-03-29T17:26:43.257+08:00").ToTimestamp(),
                ExternalClosedOn = DateTimeOffset.Parse("2023-03-29T17:51:40.717+08:00").ToTimestamp(),
                Bet = 10,
                Commission = 0,
                Settle = 0,
                WinLoss = -10,
                AdditionalData = "{\"Id\":\"OU1459290213398\"}"
            });

        var gameProviderId = Guid.NewGuid();
        _ = fakeUtensil
            .GetGameProviderIdAsync(
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderId);

        var gameProviderCode = "OBET";
        _ = fakeUtensil
            .GetGameProviderCode()
            .Returns(gameProviderCode);

        var gameExternalId = "sports_book";
        _ = fakeUtensil
            .GetGameExternalId()
            .Returns(gameExternalId);

        var gameId = 1;
        _ = fakeUtensil
            .GetGameIdAsync(
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameId);

        var player = new PlayerInfo(
            ResellerId: Guid.NewGuid(),
            ResellerCode: "TGS",
            LicenseeId: Guid.NewGuid(),
            LicenseeCode: "TGS",
            BrandId: Guid.NewGuid(),
            BrandCode: "GPI",
            PlayerCode: "4ucmqplxb029",
            Username: "4ucmqplxb029",
            DefaultWalletId: Guid.NewGuid(),
            DefaultWalletCurrencyCode: "CNY");
        _ = fakeUtensil
            .GetPlayerAsync(
                playerCode: Arg.Is(command.GameRecord.PlayerCode))
            .Returns(player);

        var gameProviderConfigurationId = (Guid?)Guid.NewGuid();
        _ = fakeUtensil
            .GetBrandGameProviderConfigurationIdAsync(
                brandId: Arg.Is(player.BrandId),
                gameProviderId: Arg.Is(gameProviderId))
            .Returns(gameProviderConfigurationId);

        var service = new ManualHandleRecordCommandService(
            fakeUtensil,
            fakeClient);

        await service
            .ExecuteAsync(
                command: command)
            .ConfigureAwait(false);

        await fakeClient
            .Received(1)
            .RoundBidding(
                gameProviderId: Arg.Is(gameProviderId),
                gameProviderCode: Arg.Is(gameProviderCode),
                externalRoundId: Arg.Is(command.GameRecord.ExternalRoundId),
                gameId: Arg.Is(gameId),
                externalGameId: Arg.Is(gameExternalId),
                playerCode: Arg.Is(player!.PlayerCode),
                walletId: Arg.Is(player!.DefaultWalletId),
                currencyCode: Arg.Is(player!.DefaultWalletCurrencyCode),
                brandId: Arg.Is(player!.BrandId),
                gameProviderConfigurationId: Arg.Is(gameProviderConfigurationId!.Value),
                status: Arg.Is(RoundStatus.Closed),
                clientStatus: Arg.Is(RoundClientStatus.Loss),
                externalCreatedOn: Arg.Is(command.GameRecord.ExternalCreatedOn.ToDateTimeOffset()),
                externalClosedOn: Arg.Is(command.GameRecord.ExternalClosedOn.ToDateTimeOffset()),
                bet: command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0),
                validBet: command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0),
                settle: command.GameRecord.Settle.ToDecimal().GetValueOrDefault(0),
                commission: command.GameRecord.Commission.ToDecimal().GetValueOrDefault(0),
                turnover: command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0),
                playerWinLoss: command.GameRecord.WinLoss.ToDecimal().GetValueOrDefault(0),
                additionalData: command.GameRecord.AdditionalData,
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task Unsettled_Bets()
    {
        var fakeUtensil = Substitute.For<IUtensil>();
        var fakeClient = Substitute.For<IGameRoundsWorkManualClient>();

        var command = new ManualHandleRecordCommand(
            GameRecord: new GameRecordInfo
            {
                ExternalRoundId = "OU1459290213398",
                PlayerCode = "4ucmqplxb029",
                IsClosed = false,
                IsVoid = false,
                ExternalCreatedOn = DateTimeOffset.Parse("2023-03-29T17:18:51.82+08:00").ToTimestamp(),
                ExternalClosedOn = null,
                Bet = 10,
                Commission = 0,
                Settle = 0,
                WinLoss = 0,
                AdditionalData = "{\"Id\":\"OU1459720030614\"}"
            });

        var gameProviderId = Guid.NewGuid();
        _ = fakeUtensil
            .GetGameProviderIdAsync(
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderId);

        var gameProviderCode = "OBET";
        _ = fakeUtensil
            .GetGameProviderCode()
            .Returns(gameProviderCode);

        var gameExternalId = "sports_book";
        _ = fakeUtensil
            .GetGameExternalId()
            .Returns(gameExternalId);

        var gameId = 1;
        _ = fakeUtensil
            .GetGameIdAsync(
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameId);

        var player = new PlayerInfo(
            ResellerId: Guid.NewGuid(),
            ResellerCode: "TGS",
            LicenseeId: Guid.NewGuid(),
            LicenseeCode: "TGS",
            BrandId: Guid.NewGuid(),
            BrandCode: "GPI",
            PlayerCode: "4ucmqplxb029",
            Username: "4ucmqplxb029",
            DefaultWalletId: Guid.NewGuid(),
            DefaultWalletCurrencyCode: "CNY");
        _ = fakeUtensil
            .GetPlayerAsync(
                playerCode: Arg.Is(command.GameRecord.PlayerCode))
            .Returns(player);

        var gameProviderConfigurationId = (Guid?)Guid.NewGuid();
        _ = fakeUtensil
            .GetBrandGameProviderConfigurationIdAsync(
                brandId: Arg.Is(player.BrandId),
                gameProviderId: Arg.Is(gameProviderId))
            .Returns(gameProviderConfigurationId);

        var service = new ManualHandleRecordCommandService(
            fakeUtensil,
            fakeClient);

        await service
            .ExecuteAsync(
                command: command)
            .ConfigureAwait(false);

        await fakeClient
            .Received(1)
            .RoundBidding(
                gameProviderId: Arg.Is(gameProviderId),
                gameProviderCode: Arg.Is(gameProviderCode),
                externalRoundId: Arg.Is(command.GameRecord.ExternalRoundId),
                gameId: Arg.Is(gameId),
                externalGameId: Arg.Is(gameExternalId),
                playerCode: Arg.Is(player!.PlayerCode),
                walletId: Arg.Is(player!.DefaultWalletId),
                currencyCode: Arg.Is(player!.DefaultWalletCurrencyCode),
                brandId: Arg.Is(player!.BrandId),
                gameProviderConfigurationId: Arg.Is(gameProviderConfigurationId!.Value),
                status: Arg.Is(RoundStatus.Open),
                clientStatus: Arg.Is(RoundClientStatus.Open),
                externalCreatedOn: Arg.Is(command.GameRecord.ExternalCreatedOn.ToDateTimeOffset()),
                externalClosedOn: null,
                bet: command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0),
                validBet: command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0),
                settle: command.GameRecord.Settle.ToDecimal().GetValueOrDefault(0),
                commission: command.GameRecord.Commission.ToDecimal().GetValueOrDefault(0),
                turnover: command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0),
                playerWinLoss: command.GameRecord.WinLoss.ToDecimal().GetValueOrDefault(0),
                additionalData: command.GameRecord.AdditionalData,
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}
