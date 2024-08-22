using Tgs.GameRecord.OBet.Core.JobApplicationServices;

namespace Tgs.GameRecord.OBet.Core;

public interface IHangfireUtensilRepository
{
	public Task JobCheckAsync();

	public Task JobExecuteAsync(JobExecuteCommand source);

	public Task JobInitAsync();
}
