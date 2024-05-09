namespace Tgs.GameRecord.OBet.Core;

public record GameRecordInfoOptions
{
	public string GameProviderCode { get; set; } = default!;
	public string GameExternalId { get; set; } = default!;
	public string GameProviderConfiguration { get; init; } = default!;
	public string HangfirePrefix { get; set; } = "gamerecord-obet-hangfire:";
	public string HangfireLocking { get; set; } = "gamerecord-obet-locking:";
	public double IntervalMinutes { get; init; } = 10;
	public double IntervalMinutesMax { get; init; } = 15;
	public double IntervalMinutesMinimum { get; init; } = 1;
}
