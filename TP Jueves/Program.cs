using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TP_Jueves.Data;
using TP_Jueves.Models;
using TP_Jueves.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Configure culture (es-AR)
var supportedCultures = new[] { new CultureInfo("es-AR") };
builder.Services.Configure<RequestLocalizationOptions>(opts =>
{
    opts.DefaultRequestCulture = new RequestCulture("es-AR");
    opts.SupportedCultures = supportedCultures;
    opts.SupportedUICultures = supportedCultures;
});

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=tpjueves.db"));

// Identity with Roles
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Razor Pages and runtime compilation
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Application services
builder.Services.AddScoped<RestauranteService>();
builder.Services.AddScoped<DataSeederService>();
builder.Services.AddScoped<AdminInitializerService>();

var app = builder.Build();

app.UseRequestLocalization();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// ENDPOINT TEMPORAL PARA APLICAR MIGRACION
app.MapGet("/aplicar-migracion-horarios", async (ApplicationDbContext db) =>
{
    try
    {
        var connection = db.Database.GetDbConnection();
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        
        // Verificar si la tabla existe y tiene CapacidadMaxima
        command.CommandText = "PRAGMA table_info(HorariosRestaurante)";
        var reader = await command.ExecuteReaderAsync();
        var hasCapacidadMaxima = false;
        
        while (await reader.ReadAsync())
        {
            var columnName = reader.GetString(1);
            if (columnName == "CapacidadMaxima")
            {
                hasCapacidadMaxima = true;
                break;
            }
        }
        await reader.CloseAsync();
        
        if (hasCapacidadMaxima)
        {
            // Necesita migración: recrear tabla sin CapacidadMaxima
            command.CommandText = "ALTER TABLE HorariosRestaurante RENAME TO HorariosRestaurante_Old";
            await command.ExecuteNonQueryAsync();
            
            command.CommandText = @"
                CREATE TABLE HorariosRestaurante (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    RestauranteId INTEGER NOT NULL,
                    Hora TEXT NOT NULL,
                    EstaActivo INTEGER NOT NULL,
                    Descripcion TEXT NULL,
                    CreatedAt TEXT NOT NULL,
                    FOREIGN KEY (RestauranteId) REFERENCES Restaurantes(Id) ON DELETE CASCADE
                )";
            await command.ExecuteNonQueryAsync();
            
            command.CommandText = @"
                INSERT INTO HorariosRestaurante (Id, RestauranteId, Hora, EstaActivo, Descripcion, CreatedAt)
                SELECT Id, RestauranteId, Hora, EstaActivo, Descripcion, CreatedAt
                FROM HorariosRestaurante_Old";
            await command.ExecuteNonQueryAsync();
            
            command.CommandText = "DROP TABLE HorariosRestaurante_Old";
            await command.ExecuteNonQueryAsync();
            
            command.CommandText = "CREATE INDEX IF NOT EXISTS IX_HorariosRestaurante_RestauranteId ON HorariosRestaurante (RestauranteId)";
            await command.ExecuteNonQueryAsync();
        }
        else
        {
            // Tabla ya está correcta o no existe, crearla si es necesario
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS HorariosRestaurante (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    RestauranteId INTEGER NOT NULL,
                    Hora TEXT NOT NULL,
                    EstaActivo INTEGER NOT NULL,
                    Descripcion TEXT NULL,
                    CreatedAt TEXT NOT NULL,
                    FOREIGN KEY (RestauranteId) REFERENCES Restaurantes(Id) ON DELETE CASCADE
                )";
            await command.ExecuteNonQueryAsync();
            
            command.CommandText = "CREATE INDEX IF NOT EXISTS IX_HorariosRestaurante_RestauranteId ON HorariosRestaurante (RestauranteId)";
            await command.ExecuteNonQueryAsync();
        }
        
        // Agregar columna HoraReserva (si no existe)
        try
        {
            command.CommandText = "ALTER TABLE Reservas ADD COLUMN HoraReserva TEXT NOT NULL DEFAULT ''";
            await command.ExecuteNonQueryAsync();
        }
        catch
        {
            // Columna ya existe, continuar
        }
        
        // Actualizar HoraReserva para reservas existentes
        command.CommandText = @"
            UPDATE Reservas 
            SET HoraReserva = CASE 
                WHEN Horario = 0 THEN '12:00'
                WHEN Horario = 1 THEN '13:30'
                WHEN Horario = 2 THEN '15:00'
                WHEN Horario = 3 THEN '20:00'
                WHEN Horario = 4 THEN '22:00'
                WHEN Horario = 5 THEN '00:00'
                ELSE '12:00'
            END
            WHERE HoraReserva = '' OR HoraReserva IS NULL";
        await command.ExecuteNonQueryAsync();
        
        // Insertar horarios por defecto (SIN capacidad)
        var horarios = new[]
        {
            ("12:00", "Almuerzo"),
            ("12:30", "Almuerzo"),
            ("13:00", "Almuerzo"),
            ("13:30", "Almuerzo"),
            ("14:00", "Almuerzo"),
            ("20:00", "Cena"),
            ("20:30", "Cena"),
            ("21:00", "Cena"),
            ("21:30", "Cena"),
            ("22:00", "Cena")
        };
        
        var restaurantes = await db.Restaurantes.ToListAsync();
        int horariosCreados = 0;
        
        foreach (var restaurante in restaurantes)
        {
            foreach (var (hora, descripcion) in horarios)
            {
                command.CommandText = $@"
                    SELECT COUNT(*) FROM HorariosRestaurante 
                    WHERE RestauranteId = {restaurante.Id} AND Hora = '{hora}'";
                var exists = Convert.ToInt32(await command.ExecuteScalarAsync()) > 0;
                
                if (!exists)
                {
                    command.CommandText = $@"
                        INSERT INTO HorariosRestaurante (RestauranteId, Hora, EstaActivo, Descripcion, CreatedAt)
                        VALUES ({restaurante.Id}, '{hora}', 1, '{descripcion}', datetime('now'))";
                    await command.ExecuteNonQueryAsync();
                    horariosCreados++;
                }
            }
        }
        
        return Results.Ok(new
        {
            success = true,
            message = "¡Migración aplicada exitosamente! Tabla HorariosRestaurante actualizada sin CapacidadMaxima.",
            tablaRecreada = hasCapacidadMaxima,
            restaurantesActualizados = restaurantes.Count,
            horariosCreados = horariosCreados,
            nota = "Los horarios ya no tienen capacidad máxima. La disponibilidad se determina por las mesas del restaurante."
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error al aplicar migración: {ex.Message}\n\nStack: {ex.StackTrace}");
    }
});

// Ensure DB created, roles initialized, and data seeded
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeederService>();
    var adminInitializer = scope.ServiceProvider.GetRequiredService<AdminInitializerService>();
    
    db.Database.Migrate();

    // Create roles if they don't exist
    string[] roleNames = { "Cliente", "Restaurantero", "Admin" };
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Update existing restaurants to active if they have tables
    var restaurantesConMesas = await db.Restaurantes
        .Include(r => r.Mesas)
        .Where(r => r.Mesas.Any() && r.Estado == EstadoRestaurante.EnConfiguracion)
        .ToListAsync();
    
    foreach (var restaurante in restaurantesConMesas)
    {
        restaurante.Estado = EstadoRestaurante.Activo;
        restaurante.ConfiguracionCompletada = true;
    }
    
    if (restaurantesConMesas.Any())
    {
        await db.SaveChangesAsync();
    }

    // Seed initial data
    await seeder.SeedRestaurantesAsync();
}

app.Run();
