using Tgs.GameRecord.OBet.Core.ApplicationServices;

namespace Tgs.GameRecord.OBet.Core;

public interface IPullRecordDispatcher
{
	ValueTask PushCommandAsync(PullRecordCommand command, CancellationToken cancellationToken = default);
}
