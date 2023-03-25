using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using RickAndMortyAPI.BL;
using RickAndMortyAPI.BL.Externals;
using RickAndMortyAPI.BL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IPersonProvider, HttpPersonProvider>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddMemoryCache();
var sp = builder.Services.BuildServiceProvider();
builder.Services.AddScoped<IPersonProvider>(c => new CachePersonDecorator(new HttpPersonProvider(), sp.GetService<IMemoryCache>(), sp.GetService<IMemoryCache>()));
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(RickAndMortyProfile));

// var mapperConfig = new MapperConfiguration(mc =>
// {
//     mc.AddProfile(new RickAndMortyProfile());
// });
//
// IMapper mapper = mapperConfig.CreateMapper();
// builder.Services.AddSingleton(mapper);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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