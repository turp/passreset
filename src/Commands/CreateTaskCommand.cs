using Microsoft.Win32.TaskScheduler;
using Spectre.Cli;
using System.ComponentModel;
using System.DirectoryServices.AccountManagement;
using System.Net;
using Spectre.Console;

namespace PassReset.Commands;

[Description("Create task")]

public class CreateTaskCommand : Command<UserPass>
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
			AnsiConsole.WriteLine(
				$"{task.Folder.Path} {task.Name} {task.Definition.Principal.Account} {task.Definition.Principal.LogonType}");
			try
			{
				ts.RootFolder.RegisterTaskDefinition(task.Name,
					task.Definition,
					TaskCreation.Update,
					task.Definition.Principal.Account,
					settings.NewPassword,
					TaskLogonType.Password
				);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				while (e.InnerException != null)
				{
					e = e.InnerException;
					Console.WriteLine(e.Message);
					Console.WriteLine(e.StackTrace);
				}
			}
		}

		return 1;
	}
}