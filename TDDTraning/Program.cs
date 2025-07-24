using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TDDTraning.Modles;

namespace TDDTraning;

/// <summary>
/// Main program demonstrating the repository pattern with dependency injection
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        // Create a simple console application to demonstrate the repository pattern
        var host = CreateHostBuilder(args).Build();
        
        // Get the match controller from the service provider
        var matchController = host.Services.GetRequiredService<MatchController>();
        
        Console.WriteLine("=== Match Result Management System ===");
        Console.WriteLine("Demonstrating Repository Pattern with Dependency Injection");
        Console.WriteLine();
        
        // Demonstrate the scenarios from the README
        await DemonstrateScenarios(matchController);
        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
    
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Register the repository interface with its implementation
                services.AddScoped<IMatchRepository, MockMatchRepository>();
                
                // Register the controller
                services.AddScoped<MatchController>();
            });
    
    private static async Task DemonstrateScenarios(MatchController controller)
    {
        int matchId = 91;
        
        Console.WriteLine("Scenario 1: Home Goal On the first half");
        Console.WriteLine($"Given: The current display result \"0:0 (First Half)\" (match result \"\")");
        Console.WriteLine("When: match Event is HomeGoal");
        var result1 = await controller.UpdateMatchResultAsync(matchId, MatchEvent.HomeGoal);
        Console.WriteLine($"Then: display result should be \"{result1}\" (match result \"{await controller.GetMatchResultAsync(matchId)}\")");
        Console.WriteLine();
        
        Console.WriteLine("Scenario 2: Away Goal On the second half");
        Console.WriteLine($"Given: The current display result \"{result1}\" (match result \"{await controller.GetMatchResultAsync(matchId)}\")");
        Console.WriteLine("When: match Event is AwayGoal");
        await controller.UpdateMatchResultAsync(matchId, MatchEvent.AwayGoal);
        await controller.UpdateMatchResultAsync(matchId, MatchEvent.NextPeriod);
        var result2 = await controller.UpdateMatchResultAsync(matchId, MatchEvent.AwayGoal);
        Console.WriteLine($"Then: display result should be \"{result2}\" (match result \"{await controller.GetMatchResultAsync(matchId)}\")");
        Console.WriteLine();
        
        // Reset for next scenario
        var newController = new MatchController(new MockMatchRepository());
        matchId = 92;
        
        Console.WriteLine("Scenario 3: Home Cancel");
        Console.WriteLine("Given: current display result \"2:1 (First Half)\" match result \"HAH\"");
        await newController.UpdateMatchResultAsync(matchId, MatchEvent.HomeGoal);
        await newController.UpdateMatchResultAsync(matchId, MatchEvent.AwayGoal);
        await newController.UpdateMatchResultAsync(matchId, MatchEvent.HomeGoal);
        Console.WriteLine("When: HomeCancel");
        var result3 = await newController.UpdateMatchResultAsync(matchId, MatchEvent.HomeCancel);
        Console.WriteLine($"Then: display result will change to \"{result3}\" (match result \"{await newController.GetMatchResultAsync(matchId)}\")");
        Console.WriteLine();
        
        // Reset for next scenario
        newController = new MatchController(new MockMatchRepository());
        matchId = 93;
        
        Console.WriteLine("Scenario 4: Away Cancel");
        Console.WriteLine("Given: current display result \"1:1 (Second Half)\" match result \"HA;\"");
        await newController.UpdateMatchResultAsync(matchId, MatchEvent.HomeGoal);
        await newController.UpdateMatchResultAsync(matchId, MatchEvent.AwayGoal);
        await newController.UpdateMatchResultAsync(matchId, MatchEvent.NextPeriod);
        Console.WriteLine("When: AwayCancel");
        var result4 = await newController.UpdateMatchResultAsync(matchId, MatchEvent.AwayCancel);
        Console.WriteLine($"Then: display result will change to \"{result4}\" (match result \"{await newController.GetMatchResultAsync(matchId)}\")");
        Console.WriteLine();
        
        // Reset for next scenario
        newController = new MatchController(new MockMatchRepository());
        matchId = 94;
        
        Console.WriteLine("Scenario 5: Change to next period");
        Console.WriteLine("Given: The current display result \"2:1 (First Half)\" and match result \"HAH\"");
        await newController.UpdateMatchResultAsync(matchId, MatchEvent.HomeGoal);
        await newController.UpdateMatchResultAsync(matchId, MatchEvent.AwayGoal);
        await newController.UpdateMatchResultAsync(matchId, MatchEvent.HomeGoal);
        Console.WriteLine("When: match change to second half");
        var result5 = await newController.UpdateMatchResultAsync(matchId, MatchEvent.NextPeriod);
        Console.WriteLine($"Then: The current display result should be \"{result5}\" and match result should be \"{await newController.GetMatchResultAsync(matchId)}\"");
    }
}