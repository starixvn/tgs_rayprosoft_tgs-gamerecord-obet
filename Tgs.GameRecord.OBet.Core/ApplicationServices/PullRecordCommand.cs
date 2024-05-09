namespace Tgs.GameRecord.OBet.Core.ApplicationServices;

public record PullRecordCommand(
	Uri ApiUrl,
	string ApiKey,
	string AgentId,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime);
