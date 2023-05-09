using Spectre.Cli;
using System.ComponentModel;

namespace PassReset.Commands;

[Description("Update Passwords and Tasks")]

public class UpdateAllCommand : Command<OldNewPasswordSetting>
{
	public override int Execute(CommandContext context, OldNewPasswordSetting settings)
	{
		new UpdatePasswordCommand().Execute(context, settings);
		new UpdateTasksCommand().Execute(context, new NewPasswordSetting() { Idsid = settings.Idsid, NewPassword = settings.NewPassword });

		return 1;
	}
}