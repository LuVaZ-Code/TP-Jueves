# TP Jueves - Sistema Completo de Reservas de Restaurantes ???

## ?? Resumen del Proyecto

**TP Jueves** es un sistema web completo de reservas de restaurantes construido con **ASP.NET Core 9** y **Razor Pages**, que permite:

- ? Clientes: Buscar, explorar y reservar en restaurantes
- ? Restauranteros: Crear y gestionar sus restaurantes, mesas y horarios
- ? Administradores: Gestionar usuarios y roles

## ??? Arquitectura

### Tecnología
- **Framework**: ASP.NET Core 9 (Razor Pages)
- **Base de Datos**: SQLite con Entity Framework Core 9
- **Autenticación**: ASP.NET Identity
- **Estilos**: Bootstrap 5 + CSS personalizado
- **Lenguaje**: C# 11.0

### Patrón MVC
```
Pages/
??? Restaurants/      ? Gestión de restaurantes
??? Reservations/     ? Sistema de reservas
??? Account/          ? Autenticación
??? Admin/            ? Administración
??? Shared/           ? Layouts compartidos

Models/               ? Modelos de datos
Data/                 ? DbContext
Services/             ? Lógica de negocio
```

## ?? Tipos de Usuarios y Roles

### 1. Cliente
- Registrarse con rol "Cliente"
- Ver restaurantes públicos
- Buscar y filtrar restaurantes
- Ver detalles del restaurante
- Hacer reservas con DNI
- Ver mis reservas
- Cancelar reservas
- Editar perfil

**Rutas principales:**
- `/Restaurants/Browse` - Explorar restaurantes
- `/Restaurants/Details/{id}` - Ver detalles
- `/Reserve` - Hacer reserva
- `/Reservations/List` - Mis reservas

### 2. Restaurantero
- Registrarse con rol "Restaurantero"
- Crear restaurantes
- Editar información del restaurante
- Eliminar restaurantes (soft delete)
- **Gestionar Mesas**: CRUD completo
- **Gestionar Horarios**: CRUD completo
- Ver detalles del restaurante

**Rutas principales:**
- `/Restaurants/Index` - Mis restaurantes
- `/Restaurants/Create` - Crear restaurante
- `/Restaurants/Edit/{id}` - Editar restaurante
- `/Restaurants/View/{id}` - Ver detalles
- `/Restaurants/Mesas/Index` - Gestionar mesas
- `/Restaurants/Turnos/Index` - Gestionar horarios

### 3. Admin
- Gestionar usuarios
- Asignar/cambiar roles
- Ver estadísticas

**Rutas principales:**
- `/Admin/Users` - Gestionar usuarios
- `/Admin/ResetRoles` - Cambiar roles (desarrollo)

## ??? Modelos de Datos

### ApplicationUser (ASP.NET Identity)
```csharp
Id, UserName, Email, PasswordHash
+ Nombre, Apellido
```

### Restaurante
```csharp
Id, Nombre, Descripcion, Direccion, Telefono, Email
PropietarioId (FK) ? ApplicationUser
Mesas (1:N), Reservas (1:N), CreatedAt, IsDeleted
```

### Mesa
```csharp
Id, Capacidad (1-20)
RestauranteId (FK) ? Restaurante
Reservas (1:N)
```

### TurnoDisponible
```csharp
Id, Fecha, Horario, CapacidadMaxima, CapacidadUsada
RestauranteId (FK) ? Restaurante
Reservas (1:N), IsActive
```

### Reserva
```csharp
Id (Guid), DniCliente, CantPersonas
Fecha, Horario, Dieta
ClienteId (FK) ? ApplicationUser
RestauranteId (FK) ? Restaurante
MesaId (FK) ? Mesa (nullable)
TurnoDisponibleId (FK) ? TurnoDisponible (nullable)
CreatedAt, IsCancelled
```

## ?? Autenticación y Autorización

### Roles
- **Cliente**: Acceso a reservas
- **Restaurantero**: Acceso a gestión de restaurante
- **Admin**: Acceso total

### Protecciones
- `[Authorize]` - Requiere login
- `[Authorize(Roles = "Cliente")]` - Solo clientes
- `[Authorize(Roles = "Restaurantero")]` - Solo restauranteros
- Validación de propietario en operaciones sensibles

### Seguridad
- Passwords hasheados con Identity
- Soft deletes (no se borra físicamente)
- Validación de DNI (8 dígitos)
- CSRF tokens en formularios
- Inyección de dependencias

## ?? Funcionalidades Principales

### 1. Sistema de Búsqueda
- Buscador público de restaurantes
- Filtro por nombre, descripción, ubicación
- Vista de mesas disponibles
- Información de contacto

### 2. Sistema de Reservas
- Reserva con DNI + cantidad + fecha + horario + dieta
- Validación de disponibilidad
- Asignación automática de mesa
- Sugerencias de alternativas si no hay disponibilidad
- Vista de "Mis Reservas" con detalles completos
- Cancelación de reservas futuras

### 3. Gestión de Restaurantes
- CRUD de restaurantes
- Editar: nombre, descripción, dirección, teléfono, email
- Ver detalles con panel de control
- Soft delete

### 4. Gestión de Mesas
- Agregar mesas con capacidad (1-20)
- Editar capacidad
- Eliminar mesas
- Ver número de reservas asignadas
- Grid responsive

### 5. Gestión de Horarios (Turnos)
- Agregar horarios disponibles
- Especificar fecha, horario (18:00, 20:00, 22:00)
- Capacidad máxima por turno
- Rastrear capacidad usada
- Ver estado (Disponible, Casi Lleno, Lleno)
- Editar activos/inactivos
- Soft delete

### 6. Panel de Administración
- Listar usuarios
- Asignar roles
- Herramienta de reseteo (desarrollo)

## ?? Flujos de Usuario

### Cliente: Hacer una Reserva
```
1. Acceso / ? Ve botón "Ver Restaurantes"
2. /Restaurants/Browse ? Ve lista de restaurantes
3. Busca por nombre/descripción/ubicación
4. /Restaurants/Details/{id} ? Ve detalles + mesas
5. Haz clic en "Reservar" o /Reserve?restauranteId={id}
6. Completa formulario:
   - DNI (8 dígitos)
   - Cantidad de personas
   - Fecha (date picker)
   - Horario (select)
   - Dieta (select)
7. Sistema valida disponibilidad
8. Crea reserva o muestra alternativas
9. /Reservations/List ? Ve todas sus reservas
```

### Restaurantero: Crear Negocio
```
1. /Account/Register ? Selecciona "Restaurantero"
2. /Restaurants/Index ? Panel de tus restaurantes
3. Haz clic en "Agregar Restaurante"
4. Completa formulario de restaurante
5. /Restaurants/View/{id} ? Panel de control del restaurante
6. Haz clic en "Gestionar Mesas"
7. /Restaurants/Mesas/Index ? Agregar mesas
8. Vuelve y haz clic en "Gestionar Horarios"
9. /Restaurants/Turnos/Index ? Agregar horarios disponibles
10. ¡Listo para recibir reservas!
```

## ?? Rutas Completas

### Públicas (Sin autenticación)
- `/` - Página de inicio
- `/Account/Login` - Iniciar sesión
- `/Account/Register` - Registrarse
- `/Restaurants/Browse` - Listar restaurantes
- `/Restaurants/Details/{id}` - Detalles de restaurante

### Cliente (Autenticado)
- `/` - Inicio personalizado
- `/Restaurants/Browse` - Buscar restaurantes
- `/Restaurants/Details/{id}` - Ver detalles
- `/Reserve[?restauranteId={id}]` - Hacer reserva
- `/Reservations/List` - Mis reservas
- `/Account/Profile` - Mi perfil

### Restaurantero (Autenticado)
- `/Restaurants/Index` - Mis restaurantes
- `/Restaurants/Create` - Crear restaurante
- `/Restaurants/Edit/{id}` - Editar restaurante
- `/Restaurants/Delete/{id}` - Eliminar restaurante
- `/Restaurants/View/{id}` - Ver detalles
- `/Restaurants/Mesas/Index` - Gestionar mesas
- `/Restaurants/Mesas/Create` - Agregar mesa
- `/Restaurants/Mesas/Edit/{id}` - Editar mesa
- `/Restaurants/Mesas/Delete/{id}` - Eliminar mesa
- `/Restaurants/Turnos/Index` - Gestionar horarios
- `/Restaurants/Turnos/Create` - Agregar horario
- `/Restaurants/Turnos/Edit/{id}` - Editar horario
- `/Restaurants/Turnos/Delete/{id}` - Eliminar horario

### Admin (Autenticado)
- `/Admin/Users` - Gestionar usuarios
- `/Admin/ResetRoles` - Cambiar roles

## ?? Estructura de Carpetas

```
TP Jueves/
??? Pages/
?   ??? Index.cshtml               # Inicio
?   ??? Reserve.cshtml             # Reservas
?   ??? Account/
?   ?   ??? Login.cshtml
?   ?   ??? Register.cshtml
?   ?   ??? Profile.cshtml
?   ?   ??? Logout.cshtml
?   ??? Restaurants/
?   ?   ??? Index.cshtml           # Mis restaurantes
?   ?   ??? Create.cshtml
?   ?   ??? Edit.cshtml
?   ?   ??? Delete.cshtml
?   ?   ??? View.cshtml            # Detalles
?   ?   ??? Browse.cshtml          # Buscar (público)
?   ?   ??? Details.cshtml         # Detalles (público)
?   ?   ??? Mesas/
?   ?   ?   ??? Index.cshtml
?   ?   ?   ??? Create.cshtml
?   ?   ?   ??? Edit.cshtml
?   ?   ?   ??? Delete.cshtml
?   ?   ??? Turnos/
?   ?       ??? Index.cshtml
?   ?       ??? Create.cshtml
?   ?       ??? Edit.cshtml
?   ?       ??? Delete.cshtml
?   ??? Reservations/
?   ?   ??? List.cshtml            # Mis reservas
?   ??? Admin/
?   ?   ??? Users.cshtml
?   ?   ??? ResetRoles.cshtml
?   ??? Shared/
?       ??? _Layout.cshtml
??? Models/
?   ??? ApplicationUser.cs
?   ??? Restaurante.cs
?   ??? Mesa.cs
?   ??? TurnoDisponible.cs
?   ??? Reserva.cs
?   ??? Dieta.cs (enum)
?   ??? Horario.cs (enum)
?   ??? Turno.cs (value object)
??? Data/
?   ??? ApplicationDbContext.cs
??? Services/
?   ??? RestauranteService.cs
?   ??? DataSeederService.cs
?   ??? AdminInitializerService.cs
??? Migrations/
?   ??? InitialCreate
?   ??? AddRestaurantesAndRoles
?   ??? AddTurnosDisponibles
??? wwwroot/
?   ??? css/
?       ??? site.css
??? Program.cs
```

## ?? Cómo Ejecutar

### Requisitos
- .NET 9 SDK
- SQLite (incluido)

### Pasos
```bash
# 1. Clonar repositorio
git clone https://github.com/LuVaZ-Code/TP-Jueves
cd TP-Jueves\TP\ Jueves

# 2. Instalar dependencias
dotnet restore

# 3. Actualizar base de datos
dotnet ef database update

# 4. Ejecutar
dotnet run

# 5. Abrir navegador
https://localhost:7017
```

## ?? Datos de Prueba

Se crean automáticamente 6 restaurantes:
1. La Trattoria Italiana
2. Sakura Sushi
3. El Gaucho Argentino
4. Brasserie Parisienne
5. Vegetalia Bio
6. Dragon Rojo

### Usuarios de prueba sugeridos
```
Cliente: cliente@test.com / password123
Restaurantero: restaurante@test.com / password123
```

## ?? Roadmap Futuro

- [ ] Notificaciones por email
- [ ] SMS recordatorios
- [ ] Sistema de calificaciones
- [ ] Dashboard de estadísticas
- [ ] Integración de pago
- [ ] Múltiples horarios por día
- [ ] Cancelación automática
- [ ] Sistema de puntos/lealtad
- [ ] API REST

## ?? Documentación Adicional

Ver archivos MD en la raíz del proyecto:
- `RESTAURANTES_Y_ROLES.md` - Arquitectura de roles
- `LISTA_RESTAURANTES.md` - Browse y detalles
- `CRUD_RESTAURANTES.md` - Gestión de restaurantes
- `SISTEMA_TURNOS.md` - Horarios disponibles
- `GESTION_COMPLETA.md` - Gestión total de restaurantes
- `GUIA_EJECUCION.md` - Cómo ejecutar
- `ASIGNAR_ROLES.md` - Gestión de roles
- `NAVEGACION_RAPIDA.md` - Navegación del sistema

## ? Checklist de Funcionalidad

- [x] Autenticación y Roles
- [x] Registro de usuarios
- [x] CRUD de restaurantes
- [x] CRUD de mesas
- [x] CRUD de horarios (turnos)
- [x] Sistema de reservas
- [x] Búsqueda de restaurantes
- [x] Gestión de reservas
- [x] Data seeding
- [x] Soft delete
- [x] Validaciones
- [x] Estilos responsive
- [x] Administración de usuarios
- [ ] Notificaciones
- [ ] Calificaciones
- [ ] Reportes

## ?? Soporte

Para problemas o dudas, ver:
- `GUIA_EJECUCION.md` - Solucionar problemas
- `ASIGNAR_ROLES.md` - Problemas de roles
- `NAVEGACION_RAPIDA.md` - Navegación rápida

---

**Status:** ? Fully Functional
**Última actualización:** 2024
**Versión:** 1.0.0
