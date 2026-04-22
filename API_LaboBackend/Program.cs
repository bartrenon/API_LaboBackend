using API_LaboBackend.MiddleWares;
using BLL.Interfaces;
using BLL.Services;
using DAL.Interfaces;
using DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IJoueurRepository, JoueurRepository>();
builder.Services.AddScoped<IJoueurService, JoueurService>();

builder.Services.AddScoped<ITournoiRepository, TournoiRepository>();
builder.Services.AddScoped<ITournoiService, TournoiService>();

builder.Services.AddScoped<IInscriptionRepository, InscriptionRepository>();
builder.Services.AddScoped<IInscriptionService, InscriptionService>();

builder.Services.AddScoped<ICategorieRepository, CategorieRepository>();
builder.Services.AddScoped<ICategorieService, CategorieService>();

builder.Services.AddScoped<IRencontreRepository, RencontreRepository>();
builder.Services.AddScoped<IRencontreService, RencontreService>();

builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
