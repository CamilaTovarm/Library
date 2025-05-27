using Library.Context;
using Library.Repositories;
using Library.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var conString = builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(conString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
