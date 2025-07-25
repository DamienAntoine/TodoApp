using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoReact.Server.Models;
using TodoReact.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
	});

builder.Services.AddDbContext<TodoContext>(opt =>
	opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
	.AddEntityFrameworkStores<TodoContext>().AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFrontend", policy =>
	{
		policy.WithOrigins("http://localhost:3000", "https://localhost:3000") // Your frontend URLs
			  .AllowCredentials()
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});

builder.Services.AddScoped<TaskManager>();
builder.Services.AddScoped<AppUserManager>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var dbContext = services.GetRequiredService<TodoContext>();

	await dbContext.Database.MigrateAsync();

	await IdentitySeeder.SeedRolesAndAdminAsync(services);
}

app.Run();