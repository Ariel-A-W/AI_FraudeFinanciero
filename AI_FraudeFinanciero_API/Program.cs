using AI_FraudeFinanciero_API.Infrastructure.DIP;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API Fraude Financiero",
        Version = "v1",
        Description = "API para detección de fraude financiero usando ML",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Ariel Alejandro Wagner",
            Email = "arqdev.arielwagner@gmail.com", 
            Url = new Uri("https://www.linkedin.com/in/ariel-alejandro-w-a4834075/") 
        }
    });
});

// **********************************************************************************************
// *** Conexión a la base de datos MySQL
builder.Services.AddMySQLConnection(builder.Configuration);
// **********************************************************************************************

builder.Services.AddControllers();

// **********************************************************************************************
// *** Servicios de inyección de dependencias
builder.Services.AddDIPServices(builder.Configuration);
// **********************************************************************************************

// *************************************************************************************************
// Configuración para CORS.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   // .SetPreflightMaxAge(TimeSpan.FromHours(1)); // Cacheo de preflight por 1 hora
                   ;
        });
});
// *************************************************************************************************

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();
