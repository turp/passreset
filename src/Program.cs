using PassReset.Commands;
using Spectre.Cli;

var app = new CommandApp();

app.Configure(config =>
{
	config.Settings.ApplicationName = "PassReset";
	config.AddCommand<UpdatePasswordCommand>("password");
	config.AddCommand<UpdateTasksCommand>("tasks");
});
//app.SetDefaultCommand<UpdatePasswordCommand>();

return app.Run(args);