namespace Tgs.GameRecord.OBet.ViewModels;

public record PullRecordViewModel(
    Uri ApiUrl,
    string ApiKey,
    string AgentId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate);
