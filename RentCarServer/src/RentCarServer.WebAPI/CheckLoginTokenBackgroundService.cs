
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.LoginTokens;

namespace RentCarServer.WebAPI;

public class CheckLoginTokenBackgroundService(
    IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scoped = serviceScopeFactory.CreateScope();
        var srv = scoped.ServiceProvider;
        var loginTokenRepository = srv.GetRequiredService<ILoginTokenRepository>();
        var unitOfWork = srv.GetRequiredService<IUnitOfWork>();

        var now = DateTimeOffset.Now;
        var activeList = await loginTokenRepository
            .Where(p => p.IsActive.Value == true && p.ExpiresDate.Value < now)
            .ToListAsync(stoppingToken);

        foreach (var item in activeList)
        {
            item.SetIsActive(new(false));
        }
        if (activeList.Any())
        {
            loginTokenRepository.UpdateRange(activeList);
            await unitOfWork.SaveChangesAsync(stoppingToken);
        }

        await Task.Delay(TimeSpan.FromDays(1));
    }
}
