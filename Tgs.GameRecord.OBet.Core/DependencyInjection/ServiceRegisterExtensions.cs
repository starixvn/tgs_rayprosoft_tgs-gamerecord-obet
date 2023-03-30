using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Tgs.GameCommand.Client;
using Tgs.GameRecord.OBet.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceRegisterExtensions
{
    public static async Task<WebApplication> UseRegister(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var gameRecordInfo = app.Configuration.GetSection("GameRecordInfo").Get<GameRecordInfoOptions>()!;
            var utensil = scope.ServiceProvider.GetRequiredService<IUtensil>();

            var client = scope.ServiceProvider.GetRequiredService<IGameCommandClient>();

            var gameProviderId = await utensil
                .GetGameProviderIdAsync()
                .ConfigureAwait(false);

            _ = await utensil
                .GetGameIdAsync()
                .ConfigureAwait(false);
        }

        return app;
    }
}
