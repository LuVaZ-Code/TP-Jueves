# Actualizacion: CRUD Completo de Restaurantes - TP Jueves

## Nuevas Caracteristicas

### 1. Data Seeding
**Archivo:** `Services/DataSeederService.cs`
- Se crean 6 restaurantes de ejemplo automaticamente en el primer inicio
- Cada restaurante tiene multiples mesas con diferentes capacidades
- Se ejecuta al iniciar la aplicacion si la base de datos esta vacia

**Restaurantes creados:**
1. La Trattoria Italiana - Italiana
2. Sakura Sushi - Japonesa
3. El Gaucho Argentino - Parrilla Argentina
4. Brasserie Parisienne - Francesa
5. Vegetalia Bio - Vegetariana/Vegana
6. Dragon Rojo - Cocina Asiatica

### 2. Paginas de CRUD para Restaurantero

#### Mis Restaurantes (Index)
- **Ruta:** `/Restaurants/Index`
- **Acceso:** Solo Restaurantero
- Lista todas sus restaurantes
- Muestra: Nombre, Descripcion, Direccion, Telefono, Email, #Mesas
- Botones: Ver, Editar, Eliminar

#### Crear Restaurante
- **Ruta:** `/Restaurants/Create`
- **Acceso:** Solo Restaurantero
- Formulario para nuevo restaurante
- El PropietarioId se asigna automaticamente
- Redirige a Index tras crear

#### Ver Restaurante (Restaurantero)
- **Ruta:** `/Restaurants/View/{id}`
- **Acceso:** Solo el propietario
- Muestra detalles completos
- Lista de mesas con capacidades
- Placeholder para "Agregar Mesa" (proximamente)

#### Editar Restaurante
- **Ruta:** `/Restaurants/Edit/{id}`
- **Acceso:** Solo el propietario
- Permite editar: Nombre, Descripcion, Direccion, Telefono, Email
- Validacion de propietario antes de permitir edicion

#### Eliminar Restaurante
- **Ruta:** `/Restaurants/Delete/{id}`
- **Acceso:** Solo el propietario
- Confirmacion antes de eliminar (Soft delete)
- Advertencia clara: "Esta accion no se puede deshacer"
- Utiliza IsDeleted flag (soft delete, no borra de BD)

### 3. Paginas Publicas para Cliente

#### Buscar Restaurantes
- **Ruta:** `/Restaurants/Browse`
- **Acceso:** Publico
- Lista restaurantes no eliminados
- Buscador por nombre, descripcion o ubicacion
- Botones: Ver Detalles, Reservar (si autenticado)
- No autenticados ven boton Ingresar

#### Detalles Restaurante (Cliente)
- **Ruta:** `/Restaurants/Details/{id}`
- **Acceso:** Publico
- Informacion completa del restaurante
- Lista de mesas con capacidades
- Boton para hacer reserva
- Placeholder para resenas (proximamente)

### 4. Validaciones de Seguridad

#### En Restaurantero (Edit, Delete, View)
```csharp
// Verificar que existe el restaurante
if (restaurante == null) return NotFound();

// Verificar que es el propietario
if (restaurante.PropietarioId != user.Id) return Forbid();
```

#### En Cliente (Browse, Details)
```csharp
// Solo restaurantes no eliminados
.Where(r => !r.IsDeleted)
```

### 5. Mejoras en Navegacion

#### Navbar actualizado
- Restaurantero: Ve link "Mis Restaurantes"
- Cliente: Ve link "Restaurantes" (Browse)
- Links condicionales segun rol

#### Index dinamico
- Restaurantero: Card "Mis Restaurantes"
- Cliente: Card "Buscar Restaurante"

## Base de Datos

### Migracion
- Ya existe: `AddRestaurantesAndRoles`
- Ejecutar: `dotnet ef database update`

### Soft Delete
- Campo `IsDeleted` en Restaurante (false por defecto)
- Queries siempre filtran donde `IsDeleted == false`
- Permite recuperacion futura si es necesario

## Flujo de Uso

### Para Restaurantero
```
1. Login como Restaurantero
2. Ve Index con "Agregar Restaurante"
3. Crea nuevo restaurante
4. Puede Ver, Editar o Eliminar
5. Proximamente: Agregar mesas, ver reservas
```

### Para Cliente
```
1. Login como Cliente
2. Accede a "Restaurantes" (Browse)
3. Busca por nombre/descripcion/ubicacion
4. Ve detalles de restaurante
5. Hace reserva
6. Confirma en formulario con datos del restaurante
```

### Datos de Prueba
Al iniciar, se crean 6 restaurantes de ejemplo:
- Todos estan disponibles en Browse
- Cada uno tiene 4-6 mesas
- Se pueden editar o eliminar por el "admin"
- **Nota:** Para editar/eliminar, necesitas ser el propietario

## Archivos Creados/Modificados

### Nuevos:
- `Services/DataSeederService.cs` - Generador de datos de prueba
- `Pages/Restaurants/Edit.cshtml(.cs)` - Editar restaurante
- `Pages/Restaurants/Delete.cshtml(.cs)` - Eliminar restaurante
- `Pages/Restaurants/View.cshtml(.cs)` - Ver detalles (restaurantero)
- `Pages/Restaurants/Browse.cshtml(.cs)` - Listar publico
- `Pages/Restaurants/Details.cshtml(.cs)` - Detalles (cliente)

### Modificados:
- `Program.cs` - Agregar DataSeederService y llamar al startup
- `Pages/Restaurants/Index.cshtml` - Links a Edit/Delete, usar View
- `Pages/Shared/_Layout.cshtml` - Links condicionales
- `Pages/Index.cshtml` - Dinamico segun rol

## Proximos Pasos

1. **Gestionar Mesas** - CRUD de mesas por restaurante
2. **Sistema de Turnos** - Fecha + horarios como pediste
3. **Vincular Reservas** - Reservas por restaurante especifico
4. **Estadisticas** - Panel para restaurantero con reservas
5. **Resenas/Calificaciones** - Clientes califican restaurantes

---

**Compilacion:** OK ?
**Data Seeding:** Activo ?
**CRUD Restaurantes:** Completo ?
**Seguridad:** Propietario validado ?
**Soft Delete:** Implementado ?
