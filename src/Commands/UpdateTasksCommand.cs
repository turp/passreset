using Microsoft.Win32.TaskScheduler;
using Spectre.Cli;
using System.ComponentModel;
using Spectre.Console;

namespace PassReset.Commands;

[Description("Update Tasks")]

public class UpdateTasksCommand : Command<UserPass>
{
	public override int Execute(CommandContext context, UserPass settings)
	{
		using var ts = new TaskService();

		foreach (var task in ts.AllTasks.Where(t => 
				t.Definition.Principal.LogonType == TaskLogonType.Password
				&& t.Definition.Principal.Account.Contains(settings.Idsid)
		    )
		)
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