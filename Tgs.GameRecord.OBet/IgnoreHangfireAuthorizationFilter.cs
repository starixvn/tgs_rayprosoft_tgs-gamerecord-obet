using Hangfire.Dashboard;

namespace Tgs.GameRecord.OBet;

public class IgnoreHangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
	public bool Authorize(DashboardContext context) => true;
}
