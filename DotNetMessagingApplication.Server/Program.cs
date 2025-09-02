using DotNetMessagingApplication.Server.Data;
using DotNetMessagingApplication.Server.Data.Repositories;
using DotNetMessagingApplication.Server.Hubs;
using DotNetMessagingApplication.Server.Services;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
		policy =>
		{
			policy.WithOrigins("https://localhost:51163")
			.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowCredentials();
		});
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SupportNonNullableReferenceTypes();

	options.MapType<IFormFile>(() => new OpenApiSchema
	{
		Type = "string",
		Format = "binary"
	});
});
builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddSignalR();


// add context, then repos, then services
builder.Services.AddDbContext<MessagingAppContext>()
				.AddScoped<IUserRepository, UserRepository>()
				.AddScoped<MessageRepository>()
				.AddScoped<ChatRepository>()
				.AddScoped<ILoginService, LoginService>()
				.AddScoped<IAccountService, AccountService>()
				.AddScoped<IMessageService, MessageService>()
				.AddScoped<IChatService, ChatService>();

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
