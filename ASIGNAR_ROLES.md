# Solucion: Asignar Roles a Usuarios

## Problema

Los usuarios no ven los botones en la página de inicio porque no tienen un rol asignado.

## Soluciones Disponibles

### Opcion 1: Registrarse de Nuevo (Recomendado)

Si tienes un usuario sin rol, el método más fácil es:

1. **Cerrar sesión** en la cuenta actual
2. **Registrarse de nuevo** con un email diferente
3. En el formulario de registro, selecciona el rol:
   - "Cliente - Hacer reservas"
   - "Restaurantero - Gestionar restaurante"
4. Los botones aparecerán automáticamente

**Ventaja:** Es automático y no requiere acceso admin.

---

### Opcion 2: Usar Herramienta de Desarrollo (Recomendado)

**IMPORTANTE:** Solo funciona en modo Desarrollo (cuando ejecutas `dotnet run`)

1. **Accede a:** `https://localhost:7017/Admin/ResetRoles`
2. Verás una tabla con todos los usuarios registrados
3. Usa los botones para asignar rol:
   - **Cliente** - Para acceder como cliente
   - **Restaurantero** - Para crear restaurantes
   - **Admin** - Para gestionar usuarios
   - **Limpiar** - Para remover todos los roles

**Ejemplo:**
```
Usuario: juan@test.com
Estado: Sin Rol
Accion: Haz clic en "Cliente"
Resultado: Ahora puede ver restaurantes
```

---

### Opcion 3: Base de Datos Directa (Avanzado)

Si no funciona nada, puedes:

1. Detén la aplicación
2. Elimina `tpjueves.db`
3. Ejecuta:
   ```bash
   dotnet ef database update
   ```
4. Reinicia la aplicación
5. Registrate de nuevo con un rol

---

## Paso a Paso: Asignar Rol Existente

### Metodo 1: ResetRoles (Mas Facil)

```
1. Abre https://localhost:7017/Admin/ResetRoles
2. Busca tu email en la tabla
3. Haz clic en el rol que deseas asignar
4. Listo! Recarga la pagina https://localhost:7017
5. Ahora deberias ver los botones
```

### Metodo 2: Registrarse de Nuevo (Si lo anterior no funciona)

```
1. Cierra sesion: Haz clic en tu perfil ? "Cerrar Sesion"
2. Ve a: https://localhost:7017/Account/Register
3. Ingresa nuevos datos:
   - Nombre: Juan
   - Apellido: Perez
   - Email: juan@test.com (DIFERENTE al anterior)
   - Tipo: Selecciona "Cliente" o "Restaurantero"
   - Contrasena: password123
4. Haz clic en "Crear Cuenta"
5. Listo! Ya estás autenticado con rol
```

---

## Que Deberia Ver Ahora

### Si Eres Cliente
Deberias ver en la pagina de inicio:

```
???????????????????????????
?  ??? Buscar Restaurante  ?  ? Ver Restaurantes
?  Descubre restaurantes  ?
???????????????????????????

???????????????????????????
?  ?? Mis Reservas        ?  ? Ver Mis Reservas
?  Ver tus reservas       ?
???????????????????????????

???????????????????????????
?  ?? Mi Perfil           ?  ? Ir a Perfil
?  Editar tu perfil       ?
???????????????????????????
```

En el navbar deberias ver:
- **Inicio** | **Restaurantes** | **Mis Reservas** | **[Usuario ?]**

### Si Eres Restaurantero
Deberias ver en la pagina de inicio:

```
???????????????????????????
?  ?? Mis Restaurantes    ?  ? Ir a Restaurantes
?  Gestiona restaurantes  ?
???????????????????????????

???????????????????????????
?  ?? Mi Perfil           ?  ? Ir a Perfil
?  Editar tu perfil       ?
???????????????????????????

???????????????????????????
?  ?? Estadisticas        ?  ? Proximamente
?  Reportes de reservas   ?
???????????????????????????
```

En el navbar deberias ver:
- **Inicio** | **Mis Restaurantes** | **[Usuario ?]**

---

## Solucionar Problemas

### "Aun veo la advertencia de Sin Rol"
1. Recarga la pagina (Ctrl+F5 en navegador)
2. Limpia cookies: F12 ? Application ? Clear Site Data
3. Cierra sesion y vuelve a iniciar

### "ResetRoles dice Not Found"
1. Asegurate de que estés en modo Desarrollo
2. Ejecuta con: `dotnet run` (no con IIS Express)
3. Si sigues sin poder acceder, usa Registrarse de Nuevo

### "Sigo sin ver botones"
1. Abre la consola del navegador: F12
2. Verifica que no haya errores en rojo
3. Si hay errores, copia el error y usa ResetRoles para asignar rol manualmente

---

## Rutas Importantes

| Ruta | Descripcion |
|------|-------------|
| `/` | Inicio |
| `/Account/Register` | Registrarse |
| `/Account/Login` | Iniciar sesion |
| `/Admin/ResetRoles` | Herramienta de roles (solo desarrollo) |
| `/Restaurants/Browse` | Ver restaurantes (Cliente) |
| `/Restaurants/Index` | Mis restaurantes (Restaurantero) |
| `/Reservations/List` | Mis reservas (Cliente) |

---

## Datos de Prueba Sugeridos

Para probar todo correctamente, crea 2 usuarios:

**Usuario 1 - Cliente:**
```
Email: cliente@test.com
Contrasena: password123
Nombre: Juan
Apellido: Perez
Tipo: Cliente
```
Luego accede a `/Restaurants/Browse` para ver restaurantes

**Usuario 2 - Restaurantero:**
```
Email: restaurante@test.com
Contrasena: password123
Nombre: Carlos
Apellido: Lopez
Tipo: Restaurantero
```
Luego accede a `/Restaurants/Index` para ver tus restaurantes

---

## Resumen

? Si el rol se asigna correctamente en el registro ? Los botones aparecen
? Si tienes usuario sin rol ? Usa `/Admin/ResetRoles` para asignar
? Si nada funciona ? Registrate de nuevo con otro email
? La pagina muestra advertencia "Sin Rol" si el usuario no tiene rol

**Ahora deberia funcionar todo! ??**
