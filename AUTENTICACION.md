# TP Jueves - Sistema de Reservas de Restaurantes

## Implementación realizada

### Autenticación sin confirmación de email ?

1. **Modelo de Usuario (ApplicationUser.cs)**
   - Extiende IdentityUser con Nombre y Apellido
   - Email y Contraseña almacenados automáticamente por Identity

2. **Páginas de Cuenta (Pages/Account/)**
   - **Register.cshtml**: Registro sin confirmación
   - **Login.cshtml**: Login simple
   - **Logout.cshtml**: Cierre de sesión
   - **Profile.cshtml**: Edición de perfil del usuario

3. **Base de Datos**
   - IdentityDbContext con soporte para usuarios
   - Migración inicial creada: `InitialCreate`

4. **Interfaz Visual**
   - Layout mejorado con Bootstrap 5
   - Navbar que muestra login/registro si no autenticado
   - Navbar que muestra perfil y logout si autenticado

### Requisitos para ejecutar

1. Generar la base de datos:
   ```
   cd "TP Jueves"
   dotnet ef database update
   ```

2. Ejecutar la aplicación:
   ```
   dotnet run
   ```

3. Navegar a `https://localhost:5001` (o el puerto mostrado)

### Funcionalidades de autenticación

- ? Registro de nuevos usuarios (Nombre, Apellido, Email, Contraseña)
- ? Login con email y contraseña
- ? Cerrar sesión
- ? Ver y editar perfil de usuario
- ? Protección de páginas que requieren autenticación ([Authorize])

### Próximos pasos

1. **Restaurantes**: Crear CRUD para administrar restaurantes
2. **Mesas**: Gestión de mesas por restaurante
3. **Reservas Mejoradas**: Vincular reservas a usuarios autenticados
4. **Cancelación/Modificación**: Permite a usuarios cancelar o modificar sus reservas
5. **Panel Admin**: Gestión de restaurantes y mesas

## Estructura de carpetas

```
Pages/
  ??? Account/
  ?   ??? Login.cshtml
  ?   ??? Login.cshtml.cs
  ?   ??? Register.cshtml
  ?   ??? Register.cshtml.cs
  ?   ??? Profile.cshtml
  ?   ??? Profile.cshtml.cs
  ?   ??? Logout.cshtml
  ?   ??? Logout.cshtml.cs
  ??? Reservations/
  ?   ??? List.cshtml
  ?   ??? List.cshtml.cs
  ??? Index.cshtml
  ??? Reserve.cshtml
  ??? _Layout.cshtml
Models/
  ??? ApplicationUser.cs
  ??? Reserva.cs
  ??? Mesa.cs
  ??? ...
Data/
  ??? ApplicationDbContext.cs
Services/
  ??? RestauranteService.cs
```
