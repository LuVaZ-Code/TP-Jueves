using Microsoft.EntityFrameworkCore;
using TP_Jueves.Data;
using TP_Jueves.Models;

namespace TP_Jueves.Services
{
    /// <summary>
    /// Service to seed initial data into the database.
    /// </summary>
    public class DataSeederService
    {
        private readonly ApplicationDbContext _db;

        public DataSeederService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task SeedRestaurantesAsync()
        {
            // Check if data already exists
            if (await _db.Restaurantes.AnyAsync())
                return;

            var restaurantes = new List<Restaurante>
            {
                new Restaurante
                {
                    Nombre = "La Trattoria Italiana",
                    Descripcion = "Autentica cocina italiana con pastas caseras y pizzas al horno de leña. Ambiente acogedor y servicio impecable.",
                    Direccion = "Calle Principal 123, Centro",
                    Telefono = "1123456789",
                    Email = "info@trattoria.com",
                    Mesas = new List<Mesa>
                    {
                        new Mesa { Capacidad = 2 },
                        new Mesa { Capacidad = 2 },
                        new Mesa { Capacidad = 4 },
                        new Mesa { Capacidad = 4 },
                        new Mesa { Capacidad = 6 },
                    }
                },
                new Restaurante
                {
                    Nombre = "Sakura Sushi",
                    Descripcion = "Restaurante japones especializado en sushi fresco y platos tradicionales. Chef con experiencia internacional.",
                    Direccion = "Avenida Siempre Viva 456, Palermo",
                    Telefono = "1198765432",
                    Email = "reservas@sakurasushi.com",
                    Mesas = new List<Mesa>
                    {
                        new Mesa { Capacidad = 2 },
                        new Mesa { Capacidad = 4 },
                        new Mesa { Capacidad = 4 },
                        new Mesa { Capacidad = 6 },
                        new Mesa { Capacidad = 8 },
                    }
                },
                new Restaurante
                {
                    Nombre = "El Gaucho Argentino",
                    Descripcion = "Parrilla tradicional argentina con carnes de la mejor calidad. Vinos selectos y ambiente rustico.",
                    Direccion = "Calle Lavalle 789, San Telmo",
                    Telefono = "1143215678",
                    Email = "contact@elgauchoo.com",
                    Mesas = new List<Mesa>
                    {
                        new Mesa { Capacidad = 4 },
                        new Mesa { Capacidad = 4 },
                        new Mesa { Capacidad = 6 },
                        new Mesa { Capacidad = 6 },
                        new Mesa { Capacidad = 8 },
                        new Mesa { Capacidad = 10 },
                    }
                },
                new Restaurante
                {
                    Nombre = "Brasserie Parisienne",
                    Descripcion = "Elegante brasserie francesa con cocina refinada y extensa bodega de vinos. Perfecta para cenas especiales.",
                    Direccion = "Paseo Colon 321, Puerto Madero",
                    Telefono = "1156789012",
                    Email = "reservas@braserieparis.com",
                    Mesas = new List<Mesa>
                    {
                        new Mesa { Capacidad = 2 },
                        new Mesa { Capacidad = 2 },
                        new Mesa { Capacidad = 4 },
                        new Mesa { Capacidad = 4 },
                        new Mesa { Capacidad = 6 },
                    }
                },
                new Restaurante
                {
                    Nombre = "Vegetalia Bio",
                    Descripcion = "Restaurante vegetariano y vegano con ingredientes organicos y productos locales. Cocina creativa y saludable.",
                    Direccion = "Calle Corrientes 654, Congreso",
                    Telefono = "1134567890",
                    Email = "info@vegetaliabio.com",
                    Mesas = new List<Mesa>
                    {
                        new Mesa { Capacidad = 2 },
                        new Mesa { Capacidad = 2 },
                        new Mesa { Capacidad = 4 },
                        new Mesa { Capacidad = 4 },
                    }
                },
                new Restaurante
                {
                    Nombre = "Dragon Rojo",
                    Descripcion = "Cocina asiatica fusionada con tecnicas modernas. Tailandesa, vietnamita y china en un solo lugar.",
                    Direccion = "Avenida Santa Fe 987, Barrio Norte",
                    Telefono = "1176543210",
                    Email = "dragronojo@resto.com",
                    Mesas = new List<Mesa>
                    {
                        new Mesa { Capacidad = 2 },
                        new Mesa { Capacidad = 4 },
                        new Mesa { Capacidad = 4 },
                        new Mesa { Capacidad = 6 },
                        new Mesa { Capacidad = 6 },
                    }
                },
            };

            await _db.Restaurantes.AddRangeAsync(restaurantes);
            await _db.SaveChangesAsync();
        }
    }
}
