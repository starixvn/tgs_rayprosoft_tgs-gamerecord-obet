using Tgs.GameRounds.Schedule.Protos;

namespace Tgs.GameRecord.OBet.Core.ApplicationServices;

public record PullRecordCommand(
    Uri ApiUrl,
    string ApiKey,
    string AgentId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    ScheduleFormSource Source);
