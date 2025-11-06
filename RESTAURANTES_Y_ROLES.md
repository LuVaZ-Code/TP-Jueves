# Implementacion de Restaurantes y Roles - TP Jueves

## Cambios Realizados

### 1. Nuevos Modelos

#### Restaurante.cs
- Entidad principal que representa un restaurante
- Propiedades: Nombre, Descripcion, Direccion, Telefono, Email
- FK a ApplicationUser (Propietario)
- Relaciones con Mesas y Reservas
- Soft delete con IsDeleted flag

#### Mesa.cs (Actualizado)
- Ahora tiene FK a Restaurante
- Cada mesa pertenece a un restaurante especifico

#### Reserva.cs (Actualizado)
- Ahora tiene FK a Restaurante
- Ahora tiene FK a ApplicationUser (Cliente)
- Nuevo campo IsCancelled para rastrear cancelaciones

### 2. Base de Datos

#### ApplicationDbContext.cs (Actualizado)
- Agregado DbSet<Restaurante>
- Configuradas todas las relaciones con OnDelete behaviors:
  - Restaurante -> Propietario: Restrict
  - Mesa -> Restaurante: Cascade
  - Reserva -> Restaurante: Restrict
  - Reserva -> Mesa: Restrict
  - Reserva -> Cliente: SetNull

#### Migracion: AddRestaurantesAndRoles
- Creada con `dotnet ef migrations add AddRestaurantesAndRoles`
- Tabla Restaurantes creada
- FKs configuradas en Mesa y Reserva

### 3. Autenticacion y Autorizacion

#### Program.cs (Actualizado)
- Roles agregados: Cliente, Restaurantero, Admin
- Los roles se crean automaticamente en el startup
- AddIdentity configurado para soportar roles

#### Register.cshtml / Register.cshtml.cs (Actualizado)
- Usuario elige rol al registrarse: Cliente o Restaurantero
- Rol se asigna automaticamente tras crear la cuenta
- Validacion de rol selection

### 4. Nuevas Paginas

#### /Restaurants/Index.cshtml
- Lista restaurantes del restaurantero autenticado
- Muestra: Nombre, Descripcion, Direccion, Telefono, Email, # Mesas
- Acciones: Ver, Editar, Eliminar
- Solo accesible con rol "Restaurantero"

#### /Restaurants/Create.cshtml
- Formulario para crear nuevo restaurante
- Campos: Nombre (required), Descripcion, Direccion, Telefono, Email
- El PropietarioId se asigna automaticamente del usuario autenticado
- Solo accesible con rol "Restaurantero"

### 5. Navegacion Actualizada

#### _Layout.cshtml (Actualizado)
- Navbar condicional basado en roles
- Cliente ve: Inicio, Reservar, Mis Reservas, Perfil
- Restaurantero ve: Inicio, Mis Restaurantes, Perfil
- No autenticado ve: Inicio, Ingresar, Registrarse

#### Index.cshtml (Actualizado)
- Contenido dinamico segun rol
- Cliente: Nueva Reserva, Mis Reservas, Perfil
- Restaurantero: Mis Restaurantes, Perfil, Estadisticas (proximamente)
- No autenticado: Informacion publica

### 6. Autorizacion

- [Authorize(Roles = "Restaurantero")] en paginas de restaurantes
- [Authorize] en paginas que requieren login
- User.IsInRole() en vistas para mostrar contenido condicional

## Estructura de Carpetas

```
Pages/
  ??? Restaurants/
  ?   ??? Index.cshtml (Listar restaurantes)
  ?   ??? Index.cshtml.cs
  ?   ??? Create.cshtml (Crear restaurante)
  ?   ??? Create.cshtml.cs
  ?   ??? _ViewImports.cshtml
  ??? Account/ (Actualizado con rol selection)
  ??? Reservations/
  ??? (otras paginas)
Models/
  ??? Restaurante.cs (NUEVO)
  ??? Mesa.cs (ACTUALIZADO)
  ??? Reserva.cs (ACTUALIZADO)
  ??? ApplicationUser.cs
Data/
  ??? ApplicationDbContext.cs (ACTUALIZADO)
Program.cs (ACTUALIZADO)
```

## Proximos Pasos

1. ? Crear modelo Restaurante
2. ? Agregar Roles (Cliente, Restaurantero, Admin)
3. ? CRUD basico de restaurantes (Create, List)
4. ? Edit y Delete de restaurantes
5. ? Gestionar Mesas para restaurante
6. ? Filtrar reservas por restaurante
7. ? Panel de estadisticas para restaurantero
8. ? Sistema de calificaciones/reviews

## Base de Datos

### Migracion Aplicada
```bash
dotnet ef database update
```

Esto crea:
- Tabla AspNetRoles (con Cliente, Restaurantero, Admin)
- Tabla AspNetUserRoles (vincula usuarios a roles)
- Tabla Restaurantes
- Columnas FK en Mesa (RestauranteId)
- Columnas FK en Reserva (RestauranteId, ClienteId)

## Flujo de Uso

### Cliente
1. Registrarse como "Cliente"
2. Ir a "Reservar"
3. Ver restaurantes disponibles (proximamente)
4. Hacer reserva
5. Ver "Mis Reservas"

### Restaurantero
1. Registrarse como "Restaurantero"
2. Ir a "Mis Restaurantes"
3. Crear nuevo restaurante
4. Agregar mesas al restaurante
5. Ver reservas entrantes

---

**Compilacion:** OK ?
**Base de Datos:** Migrada ?
**Autenticacion:** Activa ?
**Autorizacion:** Configurada ?
