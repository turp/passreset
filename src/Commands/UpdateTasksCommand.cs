using Microsoft.Win32.TaskScheduler;
using Spectre.Cli;
using Spectre.Console;
using System.ComponentModel;

namespace PassReset.Commands;

[Description("Update Tasks")]

public class UpdateTasksCommand : Command<NewPasswordSetting>
{
	public override int Execute(CommandContext context, NewPasswordSetting settings)
	{
		AnsiConsole.WriteLine($"Updating Tasks");
		using var ts = new TaskService();

		AnsiConsole.WriteLine($"Looking for tasks to update");
		var tasks = ts.AllTasks
			.Where(t => t.Definition.Principal.LogonType == TaskLogonType.Password
						&& t.Definition.Principal.Account.Contains(settings.Idsid)
			);

		foreach (var task in tasks)
		{
			AnsiConsole.WriteLine($"Updating {task.Folder.Path} {task.Name} {task.Definition.Principal.Account}");

			ts.RootFolder.RegisterTaskDefinition(task.Name,
				task.Definition,
				TaskCreation.Update,
				task.Definition.Principal.Account,
				settings.NewPassword,
				TaskLogonType.Password
			);
		}

		return 1;
	}
}