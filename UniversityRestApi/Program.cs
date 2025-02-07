using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.JeuxDeDonnees;
using UniversiteEFDataProvider.Data;
using UniversiteEFDataProvider.RepositoryFactories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(options =>
{
    options.ClearProviders();
    options.AddConsole();
});


String connectionString = builder.Configuration.GetConnectionString("MySqlConnection") ?? throw new InvalidOperationException("Connection string 'MySqlConnection' not found.");
builder.Services.AddDbContext<UniversiteDbContext>(options =>options.UseMySQL(connectionString));
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();


var app = builder.Build();


app.UseHttpsRedirection();
app.MapControllers();
// Configuration de Swagger.
// Commentez les deux lignes ci-dessous pour désactiver Swagger (en production par exemple)
app.UseSwagger();
app.UseSwaggerUI();

/*
using(var scope = app.Services.CreateScope())
{
    // On récupère le logger pour afficher des messages. On l'a mis dans les services de l'application
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<UniversiteDbContext>>();
    // On récupère le contexte de la base de données qui est stocké sans les services
    DbContext context = scope.ServiceProvider.GetRequiredService<UniversiteDbContext>();
    logger.LogInformation("Initialisation de la base de données");
    // Suppression de la BD
    logger.LogInformation("Suppression de la BD si elle existe");
    await context.Database.EnsureDeletedAsync();
    // Recréation des tables vides
    logger.LogInformation("Création de la BD et des tables à partir des entities");
    await context.Database.EnsureCreatedAsync();
}
*/


// Initisation de la base de données
ILogger loggers = app.Services.GetRequiredService<ILogger<BdBuilder>>();
loggers.LogInformation("Chargement des données de test");
using(var scope = app.Services.CreateScope())
{
    UniversiteDbContext context = scope.ServiceProvider.GetRequiredService<UniversiteDbContext>();
    IRepositoryFactory repositoryFactory = scope.ServiceProvider.GetRequiredService<IRepositoryFactory>();   
    // C'est ici que vous changez le jeu de données pour démarrer sur une base vide par exemple
    BdBuilder seedBD = new BasicBdBuilder(repositoryFactory);
    await seedBD.BuildUniversiteBdAsync();
}


app.Run();