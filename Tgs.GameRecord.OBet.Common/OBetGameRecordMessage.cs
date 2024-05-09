namespace Tgs.GameRecord.OBet.Common;

public record OBetGameRecordMessage
{
	public Uri ApiUrl { get; init; } = default!;
	public string ApiKey { get; init; } = default!;
	public string AgentId { get; init; } = default!;
	public DateTimeOffset StartTime { get; init; } = default!;
	public DateTimeOffset EndTime { get; init; } = default!;
}
