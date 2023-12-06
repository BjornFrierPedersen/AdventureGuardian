using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using static System.String;

// var service = new OpenAiCommunicatorService();
var dbContext = new AdventureGuardianDbContext();
dbContext.Database.EnsureCreated();

// while (true)
// {
//     Console.WriteLine("Please insert your prompt here");
//     var prompt = Console.ReadLine() ?? Empty;
//     var response = await service.SendRequestAsync(prompt);
//     Console.WriteLine(response);
//     Console.WriteLine("\nDo you want to ask another question?P");
// }