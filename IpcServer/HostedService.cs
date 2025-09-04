using System.Net.WebSockets;
using System.Text;
using IpcServer;
using IpcServer.Domain;
using IpcServer.Domain.Entities;
using IpcServer.Domain.Interfaces;
using IpcServer.WorkFlow.Workflows;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

public class HostedService : BackgroundService
{
    private readonly IHubContext<WorkHub> _hubContext;
    private readonly ILogger<HostedService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly List<ErrorInfo> _errorInfos;
    public HostedService(
        IHubContext<WorkHub> hubContext,
        ILogger<HostedService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _hubContext = hubContext;
        _logger = logger;
        _scopeFactory = scopeFactory;
        _errorInfos = new List<ErrorInfo>()
        {
            new ErrorInfo(){Level = Level.warning,time = DateTime.Now,description = "hahhaha"},
            new ErrorInfo(){Level = Level.info ,time = DateTime.Now,description = "test"}, 
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        List<StepDetail> steps=new ();
        using (var scope = _scopeFactory.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<StationRecipe>>();
            var recipeExecutor = scope.ServiceProvider.GetRequiredService<IRecipeExecutor>();

            var stationRecipes = await repository.FindAsync(x => x.StationCode == "OP050");
            recipeExecutor.ExecuteRecipe(stationRecipes.ToList());
            foreach (var stationRecipe in stationRecipes)
            {
                steps.Add(new StepDetail(stationRecipe.ProcessStep,stationRecipe.ProcessName,stationRecipe.OperationType,"30"));
            }
        }

        var currentStep = 1;
        while (!stoppingToken.IsCancellationRequested)
        {
           
            await _hubContext.Clients.All.SendAsync("heartbeat", new
            {
                time = DateTime.Now.ToString("HH:mm:ss"),
                currentStation = "OP050"
            });

            await _hubContext.Clients.All.SendAsync("workStepUpdate", new
            {
                steps,
                time = DateTime.Now
            });
            
            
            await _hubContext.Clients.All.SendAsync("currentStep", new
            {
                currentStep,
                currentStepInfo=steps[currentStep].stepType+steps[currentStep].stepName
                
            });

            await _hubContext.Clients.All.SendAsync("errorList", new
            {
                errorList=_errorInfos
            });
            
            
            currentStep++;
            if (currentStep > steps.Count-1)
            {
                currentStep = 1;
            }
            
         

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}