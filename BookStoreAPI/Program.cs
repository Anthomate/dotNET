using BookStoreAPI.Data;
using BookStoreAPI.Entities.ClientEntities;
using Microsoft.AspNetCore.Identity;
using System;

namespace BookStoreAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                        .AddIdentityCookies();
        builder.Services.AddAuthorizationBuilder();
        builder.Services.AddDbContext<ApplicationDbContext>();
        builder.Services.AddIdentityCore<Client>()
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddApiEndpoints();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        app.MapIdentityApi<Client>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}