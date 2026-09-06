using Arkive_API.Application.Interfaces;
using Arkive_API.Application.UseCases;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Infrastructure.Data;
using Arkive_API.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options => {
    options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
});

builder.Services.AddScoped<ICategoriaDoencaRepository, CategoriaDoencaRepository>();
builder.Services.AddScoped<ICategoriaDoencaUseCase, CategoriaDoencaUseCase>();
builder.Services.AddScoped<IEspecieRepository, EspecieRepository>();
builder.Services.AddScoped<IEspecieUseCase, EspecieUseCase>();
builder.Services.AddScoped<IDoencaRepository, DoencaRepository>();
builder.Services.AddScoped<IDoencaUseCase, DoencaUseCase>();
builder.Services.AddScoped<IRacaRepository, RacaRepository>();
builder.Services.AddScoped<IRacaUseCase, RacaUseCase>();
builder.Services.AddScoped<IPredisposicaoRepository, PredisposicaoRepository>();
builder.Services.AddScoped<IPredisposicaoUseCase, PredisposicaoUseCase>();
builder.Services.AddScoped<IFeedbackNPSRepository, FeedbackNPSRepository>();
builder.Services.AddScoped<IFeedbackNPSUseCase, FeedbackNPSUseCase>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.EnableAnnotations();
    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

builder.Services.AddResponseCompression(options => {
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options => {
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options => {
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

builder.Services.AddRateLimiter(options => {

    options.AddFixedWindowLimiter(policyName: "leitura", opt => {
        opt.PermitLimit = 60;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    options.AddFixedWindowLimiter(policyName: "escrita", opt => {
        opt.PermitLimit = 10;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter();
app.UseResponseCompression();

app.MapControllers();

app.Run();

public partial class Program { }