using FluentValidation;
using VisionHealthCareAssessment.DAL;
using VisionHealthCareAssessment.Helpers;
using VisionHealthCareAssessment.Models;
using VisionHealthCareAssessment.Services;
using Microsoft.AspNetCore.Http.Json;

namespace VisionHealthCareAssessment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var databasePath = "/data/VisionHealthCare.db";
            builder.Services.AddSqlite<ProductContext>($"Data Source={Directory.GetCurrentDirectory()}{databasePath};");

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IImportService, ImportService>();
            builder.Services.AddScoped<IValidator<Product>, ProductValidator>();

            builder.Services.Configure<ImportSettings>(builder.Configuration.GetSection("ImportSettings"));

            var app = builder.Build();

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
}