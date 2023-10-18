using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.GameInterface.OBet.Protos;
using Tgs.GameRecord.Handle.Client;
using Tgs.GameRecord.Handle.Common.Protos;
using Tgs.GameRecord.OBet.Core.ApplicationServices;

namespace Tgs.GameRecord.OBet.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
    [Fact]
    public async Task Settlement_Bets_Lose()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "OBET",
            GameExternalId = "sports_book"
        });
        var fakeClient = Substitute.For<IGameRecordHandleClient>();

        var command = new HandleRecordCommand(
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

        var service = new HandleRecordCommandService(
            gameRecordInfo,
            fakeClient);

        await service.ExecuteAsync(command: command);

        await fakeClient
            .Received(1)
            .MessageAsync(
            gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
            playerCode: Arg.Is(command.GameRecord.PlayerCode),
            wagerState: Arg.Is(WagerStates.Lose),
            wagerType: Arg.Is(WagerTypes.Normal),
            freeGameState: Arg.Is(FreeGameStates.None),
            gambleState: Arg.Is(GambleStates.None),
            cashoutState: Arg.Is(CashoutStates.None),
            resettleState: Arg.Is(ResettleStates.None),
            externalRoundId: Arg.Is(command.GameRecord.ExternalRoundId),
            externalWagerId: Arg.Is(command.GameRecord.ExternalRoundId),
            externalGameId: Arg.Is(gameRecordInfo.Value.GameExternalId),
            externalCreatedOn: Arg.Is(command.GameRecord.ExternalCreatedOn.ToDateTimeOffset()),
            externalClosedOn: Arg.Is(command.GameRecord.ExternalClosedOn.ToDateTimeOffset()),
            externalPayoffOn: Arg.Is(command.GameRecord.ExternalClosedOn.ToDateTimeOffset()),
            externalResettleOn: Arg.Is(DateTimeOffset.UnixEpoch),
            bet: command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0),
            validBet: command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0),
            settle: command.GameRecord.Settle.ToDecimal().GetValueOrDefault(0),
            commission: command.GameRecord.Commission.ToDecimal().GetValueOrDefault(0),
            turnover: command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0),
            winLoss: command.GameRecord.WinLoss.ToDecimal().GetValueOrDefault(0),
            additionalData: command.GameRecord.AdditionalData,
            cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Settlement_Bets_Win()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "OBET",
            GameExternalId = "sports_book"
        });
        var fakeClient = Substitute.For<IGameRecordHandleClient>();

        var command = new HandleRecordCommand(
            GameRecord: new GameRecordInfo
            {
                ExternalRoundId = "HDP1662557219039",
                PlayerCode = "4uiyxmy8ra4a",
                IsClosed = true,
                IsVoid = false,
                ExternalCreatedOn = DateTimeOffset.Parse("2023-10-18T09:02:34.05+08:00").ToTimestamp(),
                ExternalClosedOn = DateTimeOffset.Parse("2023-10-18T10:30:38.05+08:00").ToTimestamp(),
                Bet = 500.00m,
                Commission = 0,
                Settle = 920.00m,
                WinLoss = 420.00m,
                AdditionalData = "{\"Id\":\"HDP1662557219039\",\"SDate\":\"2023-10-17T00:00:00\",\"TDate\":\"2023-10-18T09:02:34.05\",\"KDate\":\"2023-10-18T08:30:00\",\"ADate\":\"2023-10-18T09:03:16.737\",\"PDate\":\"2023-10-18T10:30:38.05\",\"User\":\"WHKTGHK4UIYXMY8RA4A\",\"Sport\":\"Football\",\"League\":\"International Friendly\",\"Home\":\"USA\",\"Away\":\"Ghana\",\"Sport_en\":\"Football\",\"League_en\":\"International Friendly\",\"Home_en\":\"USA\",\"Away_en\":\"Ghana\",\"Live\":\"True\",\"HomeScore\":\"3\",\"AwayScore\":\"0\",\"Status\":\"0\",\"FT\":\"f\",\"OddsType\":\"HK\",\"Game\":\"HDP\",\"Info\":\"-0.5\",\"Odds\":\"0.84\",\"Side\":\"h\",\"Side_en\":null,\"Bamt\":\"500.0000\",\"Wamt\":\"420.0000\",\"Tresult\":\"1\",\"Fhscore\":\"4-0\",\"Ftscore\":\"4-0\",\"FG\":\"h\",\"LG\":\"h\",\"IP\":\"203.145.89.5-HK\",\"IsMobile\":1,\"Commission\":\"0.0000\"}"
            });

        var service = new HandleRecordCommandService(
            gameRecordInfo,
            fakeClient);

        await service.ExecuteAsync(command: command);

        await fakeClient
            .Received(1)
            .MessageAsync(
            gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
            playerCode: Arg.Is(command.GameRecord.PlayerCode),
            wagerState: Arg.Is(WagerStates.Win),
            wagerType: Arg.Is(WagerTypes.Normal),
            freeGameState: Arg.Is(FreeGameStates.None),
            gambleState: Arg.Is(GambleStates.None),
            cashoutState: Arg.Is(CashoutStates.None),
            resettleState: Arg.Is(ResettleStates.None),
            externalRoundId: Arg.Is(command.GameRecord.ExternalRoundId),
            externalWagerId: Arg.Is(command.GameRecord.ExternalRoundId),
            externalGameId: Arg.Is(gameRecordInfo.Value.GameExternalId),
            externalCreatedOn: Arg.Is(command.GameRecord.ExternalCreatedOn.ToDateTimeOffset()),
            externalClosedOn: Arg.Is(command.GameRecord.ExternalClosedOn.ToDateTimeOffset()),
            externalPayoffOn: Arg.Is(command.GameRecord.ExternalClosedOn.ToDateTimeOffset()),
            externalResettleOn: Arg.Is(DateTimeOffset.UnixEpoch),
            bet: command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0),
            validBet: command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0),
            settle: command.GameRecord.Settle.ToDecimal().GetValueOrDefault(0),
            commission: command.GameRecord.Commission.ToDecimal().GetValueOrDefault(0),
            turnover: command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0),
            winLoss: command.GameRecord.WinLoss.ToDecimal().GetValueOrDefault(0),
            additionalData: command.GameRecord.AdditionalData,
            cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Unsettled_Bets()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "OBET",
            GameExternalId = "sports_book"
        });
        var fakeClient = Substitute.For<IGameRecordHandleClient>();

        var command = new HandleRecordCommand(
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

        var service = new HandleRecordCommandService(
            gameRecordInfo,
            fakeClient);

        await service
            .ExecuteAsync(
            command: command);

        await fakeClient
            .Received(1)
            .MessageAsync(
            gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
            playerCode: Arg.Is(command.GameRecord.PlayerCode),
            wagerState: Arg.Is(WagerStates.Running),
            wagerType: Arg.Is(WagerTypes.Normal),
            freeGameState: Arg.Is(FreeGameStates.None),
            gambleState: Arg.Is(GambleStates.None),
            cashoutState: Arg.Is(CashoutStates.None),
            resettleState: Arg.Is(ResettleStates.None),
            externalRoundId: Arg.Is(command.GameRecord.ExternalRoundId),
            externalWagerId: Arg.Is(command.GameRecord.ExternalRoundId),
            externalGameId: Arg.Is(gameRecordInfo.Value.GameExternalId),
            externalCreatedOn: Arg.Is(command.GameRecord.ExternalCreatedOn.ToDateTimeOffset()),
            externalClosedOn: Arg.Is(DateTimeOffset.UnixEpoch),
            externalPayoffOn: Arg.Is(DateTimeOffset.UnixEpoch),
            externalResettleOn: Arg.Is(DateTimeOffset.UnixEpoch),
            bet: command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0),
            validBet: command.GameRecord.Bet.ToDecimal().GetValueOrDefault(0),
            settle: Arg.Is(0.0m),
            commission: Arg.Is(0.0m),
            turnover: Arg.Is(0.0m),
            winLoss: Arg.Is(0.0m),
            additionalData: command.GameRecord.AdditionalData,
            cancellationToken: Arg.Any<CancellationToken>());
    }
}
