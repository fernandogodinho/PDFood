using Microsoft.EntityFrameworkCore;
using pdfood.api.DTO;
using pdfood.api.Repository;
using pdfood.api.Repository.Files;
using pdfood.api.Repository.Products;
using pdfood.api.Services;
using pdfood.api.Services.Products;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllers();

ConfigureServices(builder.Services);
ConfigureFakeStoreService(builder.Services);
ConfigureCors(builder.Services);

builder.Services.AddAutoMapper(typeof(MappingProfile));

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
app.UseCors("allowedOrigins");

app.Run();

static void ConfigureFakeStoreService(IServiceCollection services)
{
    services.AddHttpClient<FakeStoreService>(client =>
    {
        client.BaseAddress = new Uri("https://fakestoreapi.com/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    });
}

static void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IProductService, ProductService>();
    services.AddScoped<IProductRepository, ProductRepository>();
    services.AddScoped<IImageUploadRepository, ImageUploadRepository>();
}

static void ConfigureCors(IServiceCollection services)
{
    services.AddCors(options =>
    {
        //options.AddPolicy("AllowSpecificOrigins",
        options.AddPolicy("allowedOrigins",
            builder =>
            {
                builder.WithOrigins("http://localhost:5173", "http://localhost:5173")
                       .AllowAnyMethod()
                       //.AllowAnyHeader();
                       .WithHeaders("Content-Type", "Authorization", "only-if-cached");
            });
    });
}