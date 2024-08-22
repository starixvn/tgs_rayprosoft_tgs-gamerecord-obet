using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.GameInterface.OBet.Protos;
using Tgs.GameRecord.Handle.Client;
using Tgs.GameRecord.OBet.Core.ApplicationServices;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.OBet.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
	[Fact]
	public async Task Settlement_Bets_Lose()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "OBET",
			GameExternalId = "sports_book",
			HangfirePrefix = "gamerecord-obet-hangfire:",
			HangfireLocking = "gamerecord-obet-locking:",
			IntervalMinutes = 3,
			IntervalMinutesMax = 15,
			IntervalMinutesMinimum = 1
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

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

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command: command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.GameRecord.PlayerCode,
				ExternalRoundId = command.GameRecord.ExternalRoundId,
				ExternalBetId = command.GameRecord.ExternalRoundId,
				ExternalGameId = fakeGameRecordInfoOptions.Value.GameExternalId,
				ExternalCreatedOn = command.GameRecord.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.GameRecord.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.GameRecord.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Loss,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 10.0m,
				ValidBet = 10.0m,
				Settle = 0.0M,
				Commission = 0.0M,
				Turnover = 10.0m,
				WinLoss = -10.0m,
				Jackpot = 0.0M,
				AdditionalData = command.GameRecord.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Settlement_Bets_Win()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "OBET",
			GameExternalId = "sports_book",
			HangfirePrefix = "gamerecord-obet-hangfire:",
			HangfireLocking = "gamerecord-obet-locking:",
			IntervalMinutes = 3,
			IntervalMinutesMax = 15,
			IntervalMinutesMinimum = 1
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

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

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command: command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.GameRecord.PlayerCode,
				ExternalRoundId = command.GameRecord.ExternalRoundId,
				ExternalBetId = command.GameRecord.ExternalRoundId,
				ExternalGameId = fakeGameRecordInfoOptions.Value.GameExternalId,
				ExternalCreatedOn = command.GameRecord.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.GameRecord.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.GameRecord.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Win,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 500.00m,
				ValidBet = 500.00m,
				Settle = 920.00m,
				Commission = 0.0M,
				Turnover = 500.00m,
				WinLoss = 420.00m,
				Jackpot = 0.0M,
				AdditionalData = command.GameRecord.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Unsettled_Bets()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "OBET",
			GameExternalId = "sports_book",
			HangfirePrefix = "gamerecord-obet-hangfire:",
			HangfireLocking = "gamerecord-obet-locking:",
			IntervalMinutes = 3,
			IntervalMinutesMax = 15,
			IntervalMinutesMinimum = 1
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

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

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command: command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.GameRecord.PlayerCode,
				ExternalRoundId = command.GameRecord.ExternalRoundId,
				ExternalBetId = command.GameRecord.ExternalRoundId,
				ExternalGameId = fakeGameRecordInfoOptions.Value.GameExternalId,
				ExternalCreatedOn = command.GameRecord.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = DateTimeOffset.UnixEpoch,
				ExternalPayoffOn = DateTimeOffset.UnixEpoch,
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Running,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 10.0m,
				ValidBet = 10.0m,
				Settle = 0.0m,
				Commission = 0.0M,
				Turnover = 0.0m,
				WinLoss = 0.0m,
				Jackpot = 0.0M,
				AdditionalData = command.GameRecord.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
