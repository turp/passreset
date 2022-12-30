using PassReset.Commands;
using Spectre.Cli;

var app = new CommandApp();

app.Configure(config =>
{
	config.Settings.ApplicationName = "PassReset";
	config.AddCommand<ChangePasswordCommand>("change");
	config.AddCommand<CreateTaskCommand>("create");
});
app.SetDefaultCommand<ChangePasswordCommand>();

return app.Run(args);