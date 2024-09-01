using Microsoft.Extensions.Logging;
using Spectre.Cli;
using Spectre.Console;

namespace ConsoleWithSpectre.Commands;

public class HelloCommand : Command<HelloCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<NAME>")]
        public string Name { get; init; }
    }

    readonly ILogger<HelloCommand> _logger;

    public HelloCommand(ILogger<HelloCommand> logger)
    {
        _logger = logger;
    }   

    public override int Execute(CommandContext context, Settings settings)
    {
        _logger.LogDebug("Saying hello to {Name}", settings.Name);
        AnsiConsole.MarkupLine($"Hello, [bold]{settings.Name}[/]!");
        return 0;
    }
}