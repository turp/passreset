using System.ComponentModel;
using System.DirectoryServices.AccountManagement;
using Microsoft.Win32.TaskScheduler;
using Spectre.Cli;
using Spectre.Console;

namespace PassReset.Commands;

[Description("Change password")]
public class ChangePasswordCommand : Command<UserPass>
{
	private readonly ActiveDirectoryProvider _ad = new(new PrincipalContext(ContextType.Domain));

	public override int Execute(CommandContext context, UserPass settings)
	{
		AnsiConsole.WriteLine($"{settings.Idsid}");

		var users = new List<string>(){ settings.Idsid, $"mfg_{settings.Idsid}", $"ad_{settings.Idsid}" };

		//UpdatePassword(settings.Idsid, settings.Password, settings.NewPassword);
		//UpdatePassword($"ad_{settings.Idsid}", $"{settings.Password}{settings.Password}", $"{settings.NewPassword}{settings.NewPassword}");
		//UpdatePassword($"mfg_{settings.Idsid}", $"{settings.Password}{settings.Password}", $"{settings.NewPassword}{settings.NewPassword}");

		UpdateTasks(settings.Idsid, settings.NewPassword);
		Console.ReadKey();

		return 0;
	}

	private void UpdateTasks(string idsid, string newPassword)
	{
		using (var ts = new TaskService())
		{
			Directory.CreateDirectory("tasks");

			foreach (var task in ts.AllTasks.Where(t => t.Definition.Principal.LogonType == TaskLogonType.Password 
			                                            && t.Definition.Principal.Account.Contains(idsid)
			                                            )
			    )
			{
				AnsiConsole.WriteLine($"{task.Folder.Path} {task.Name} {task.Definition.Principal.Account} {task.Definition.Principal.LogonType}");
				try
				{
					ts.RootFolder.RegisterTaskDefinition(task.Folder.Path,
						task.Definition,
						TaskCreation.Update,
						task.Definition.Principal.Account,
						newPassword,
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
		}
	}

	private void UpdatePassword(string idsid, string password, string newPassword)
	{
		var user = _ad.FindUserByName(idsid);

		//user.ChangePassword(settings.Password, settings.NewPassword);
		AnsiConsole.WriteLine($"Updated password for: {user.SamAccountName} from {password} to {newPassword}");
	}
}