using System.ComponentModel;
using System.DirectoryServices.AccountManagement;
using Microsoft.Win32.TaskScheduler;
using Spectre.Cli;
using Spectre.Console;

namespace PassReset.Commands;

[Description("Update passwords")]
public class UpdatePasswordCommand : Command<OldNewPasswordSetting>
{
	private readonly ActiveDirectoryProvider _ad = new(new PrincipalContext(ContextType.Domain));

	public override int Execute(CommandContext context, OldNewPasswordSetting settings)
	{
		UpdatePassword(settings.Idsid, settings.Password, settings.NewPassword);
		UpdatePassword($"ad_{settings.Idsid}", $"{settings.Password}{settings.Password}", $"{settings.NewPassword}{settings.NewPassword}");
		UpdatePassword($"mfg_{settings.Idsid}", $"{settings.Password}{settings.Password}", $"{settings.NewPassword}{settings.NewPassword}");

		Console.ReadKey();

		return 0;
	}

	private void UpdatePassword(string idsid, string password, string newPassword)
	{
		AnsiConsole.WriteLine($"Updating {idsid}");

		var user = _ad.FindUserByName(idsid);
		user.ChangePassword(password, newPassword);

		AnsiConsole.WriteLine($"Updated password from {password} to {newPassword}");
	}
}