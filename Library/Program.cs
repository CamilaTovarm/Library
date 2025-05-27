using Library.Context;
using Library.Repositories;
using Library.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var conString = builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(conString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "BookHive",
        Description = ".NET 8 Web API"
    });
    // To Enable authorization using Swagger (JWT)
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}

        }
    });
});


// Repositories
builder.Services.AddScoped<IAuthorsRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorVsBookRepository, AuthorVsBookRepository>();
builder.Services.AddScoped<IBooksRepository, BooksRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IEditorialRepository, EditorialRepository>();
builder.Services.AddScoped<IFeeRepository, FeeRepository>();
builder.Services.AddScoped<ILoansRepository, LoansRepository>();
builder.Services.AddScoped<IReturnsRepository, ReturnsRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserTypeRepository, UserTypeRepository>();

// Services
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IAuthorVsBookService, AuthorVsBookService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IEditorialService, EditorialService>();
builder.Services.AddScoped<IFeeService, FeeService>();
builder.Services.AddScoped<ILoansService, LoansService>();
builder.Services.AddScoped<IReturnsService, ReturnsService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserTypeService, UserTypeService>();


var key = builder.Configuration.GetValue<string>("Jwt:key");
var keyBytes = Encoding.ASCII.GetBytes(key);


//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
