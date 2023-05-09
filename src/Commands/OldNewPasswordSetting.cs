using Spectre.Cli;
using Spectre.Console;
using System.ComponentModel;
using ValidationResult = Spectre.Cli.ValidationResult;

namespace PassReset.Commands;

public class OldNewPasswordSetting : CommandSettings
{
	[CommandArgument(0, "[idsid]")]
	[Description(@"Required. IDSID without domain.")]
	public string Idsid { get; set; } = string.Empty;

	[CommandArgument(1, "[current password]")]
	[Description(@"Required.")]
	public string Password { get; set; } = string.Empty;

	[CommandArgument(2, "[new password]")]
	[Description(@"Required.")]
	public string NewPassword { get; set; } = string.Empty;

	public override ValidationResult Validate()
	{
		if (string.IsNullOrEmpty(Idsid))
		{
			var value = AnsiConsole.Ask<string>("IDSID");

			if (string.IsNullOrEmpty(value))
			{
				return ValidationResult.Error("Username must be specified");
			}

			Idsid = value;
		}

		if (string.IsNullOrEmpty(Password))
		{
			var value = AnsiConsole.Ask<string>("Current Password");

			if (string.IsNullOrEmpty(value))
			{
				return ValidationResult.Error("Current Password must be specified");
			}

			Password = value;
		}

		if (string.IsNullOrEmpty(NewPassword))
		{
			var value = AnsiConsole.Ask<string>("New Password");

			if (string.IsNullOrEmpty(value))
			{
				return ValidationResult.Error("New Password must be specified");
			}

			NewPassword = value;
		}

		return base.Validate();
	}
}

public class NewPasswordSetting : CommandSettings
{
	[CommandArgument(0, "[idsid]")]
	[Description(@"Required. IDSID without domain.")]
	public string Idsid { get; set; } = string.Empty;

	[CommandArgument(2, "[new password]")]
	[Description(@"Required.")]
	public string NewPassword { get; set; } = string.Empty;

	public override ValidationResult Validate()
	{
		if (string.IsNullOrEmpty(Idsid))
		{
			var value = AnsiConsole.Ask<string>("IDSID");

			if (string.IsNullOrEmpty(value))
			{
				return ValidationResult.Error("Username must be specified");
			}

			Idsid = value;
		}

		if (string.IsNullOrEmpty(NewPassword))
		{
			var value = AnsiConsole.Ask<string>("New Password");

			if (string.IsNullOrEmpty(value))
			{
				return ValidationResult.Error("New Password must be specified");
			}

			NewPassword = value;
		}

		return base.Validate();
	}
}