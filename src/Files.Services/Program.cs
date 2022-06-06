using Files.Application;
using Files.Infrastructure;
using Files.Services.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = builder.Configuration["Security:Authority"]
            ?? throw new InvalidOperationException("Security authority are not configured.");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };

        if (builder.Environment.IsDevelopment())
            options.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizePolicies.ReadFiles, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "files.read");
    });
    options.AddPolicy(AuthorizePolicies.WriteFiles, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "files.write");
    });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

var app = builder.Build();
if (!builder.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseApplication();
app.UseCors(x=> x.SetIsOriginAllowed(_ => true).AllowCredentials().AllowAnyHeader().AllowAnyMethod());
app.MapControllers();
app.MapHealthChecks("/health");

await app.RunAsync();

// TODO: Добавить обработку ошибок и логгирование
// TODO: Добавить воркер