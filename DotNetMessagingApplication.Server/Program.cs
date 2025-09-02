using DotNetMessagingApplication.Server.Controllers;
using DotNetMessagingApplication.Server.Data;
using DotNetMessagingApplication.Server.Data.Repositories;
using DotNetMessagingApplication.Server.Services;
using DotNetMessagingApplication.Server.Hubs;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
		policy =>
		{
			policy.WithOrigins("https://localhost:51163")
			.AllowAnyHeader()
			.AllowAnyMethod();
		});
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();


// add context, then repos, then services
builder.Services.AddDbContext<MessagingAppContext>()
				.AddScoped<UserRepository>()
				.AddScoped<ILoginService, LoginService>()
				.AddScoped<IAccountService, AccountService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();


var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};
app.UseWebSockets(webSocketOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.MapFallbackToFile("/index.html");

app.Run();
