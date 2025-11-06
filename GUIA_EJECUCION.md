# Guia de Ejecucion - TP Jueves

## Problema: Pagina de Inicio Vacia

Si la pagina `https://localhost:7017/` se ve vacia, sigue estos pasos:

### 1. Detener la Aplicacion
Presiona `Ctrl+C` en la terminal donde está corriendo la aplicación.

### 2. Limpiar la Base de Datos (Opcional pero Recomendado)
```bash
cd "C:\Users\losmelli\Source\Repos\TP-Jueves\TP Jueves"

# Si la BD está bloqueada, cierra Visual Studio
# Luego elimina el archivo:
rm .\tpjueves.db
```

### 3. Crear Nueva Migracion (si agregaste cambios a modelos)
```bash
dotnet ef migrations add YourMigrationName
```

### 4. Actualizar la Base de Datos
```bash
dotnet ef database update
```

### 5. Compilar el Proyecto
```bash
dotnet build
```

### 6. Ejecutar la Aplicacion
```bash
dotnet run
```

### 7. Limpiar Cache del Navegador
- Presiona `F12` en tu navegador
- Ve a Application/Storage
- Haz clic en "Clear site data"
- Recarga la pagina con `Ctrl+Shift+R` (hard refresh)

---

## Que Deberia Ver

### En la Pagina de Inicio (Sin Autenticar)

```
[NAVBAR]
- Logo "TP Jueves"
- Links: Inicio, Ingresar, Registrarse

[HERO SECTION]
- Titulo: "TP Jueves"
- Subtitulo: "Sistema de Reservas de Restaurantes"

[ALERTA AZUL]
"Para continuar, debes iniciar sesion o crear una cuenta"
- Boton: "Ingresar"
- Boton: "Registrarse"

[CARACTERISTICAS]
6 Cards con:
1. Rapido y Facil - ?
2. Seguro - ??
3. Accesible - ??
4. Confirmacion - ??
5. Gestiona - ??
6. Grupos - ???????????

[FOOTER]
"© 2024 TP Jueves - Sistema de Reservas de Restaurantes..."
```

### Despues de Autenticar como Cliente

```
[NAVBAR]
- Links: Inicio, Restaurantes, Mis Reservas, [Usuario]

[CARDS]
1. Buscar Restaurante ??? ? Link a /Restaurants/Browse
2. Mis Reservas ?? ? Link a /Reservations/List
3. Mi Perfil ?? ? Link a /Account/Profile
```

### Despues de Autenticar como Restaurantero

```
[NAVBAR]
- Links: Inicio, Mis Restaurantes, [Usuario]

[CARDS]
1. Mis Restaurantes ?? ? Link a /Restaurants/Index
2. Mi Perfil ?? ? Link a /Account/Profile
3. Estadisticas ?? ? Proximamente (Disabled)
```

---

## Pasos para Probar Completamente

### 1. Registrarse como Cliente
- URL: `https://localhost:7017/Account/Register`
- Nombre: Juan
- Apellido: Perez
- Email: cliente@test.com
- Tipo de Cuenta: "Cliente - Hacer reservas"
- Contrasena: password123

### 2. Verifica la Pagina de Inicio
- Deberia ver 3 cards: Buscar Restaurante, Mis Reservas, Mi Perfil

### 3. Ve a Restaurantes (Browse)
- URL: `https://localhost:7017/Restaurants/Browse`
- Deberia ver 6 restaurantes de ejemplo (creados por el seeder)
- Puedes buscar por nombre, descripcion o ubicacion

### 4. Haz una Reserva
- Haz clic en "Reservar" en cualquier restaurante
- O ve a `/Reserve?restauranteId=1`
- Completa el formulario:
  - DNI: 12345678
  - Cantidad: 4
  - Fecha: Proxima (manana o despues)
  - Horario: 20:00 - 22:00
  - Dieta: Normal
- Haz clic en "Confirmar Reserva"

### 5. Ve Mis Reservas
- URL: `https://localhost:7017/Reservations/List`
- Deberia ver la reserva que acabas de hacer

### 6. Prueba como Restaurantero
- Registrate como otro usuario:
  - Email: restaurante@test.com
  - Tipo: "Restaurantero - Gestionar restaurante"
- Ve a `/Restaurants/Index`
- Deberia estar vacio (Restaurantero solo ve sus propios restaurantes)
- Haz clic en "Agregar Restaurante"
- Completa el formulario y crea uno nuevo

---

## Rutas Principales

| Ruta | Descripcion |
|------|-------------|
| `/` | Pagina de inicio |
| `/Account/Register` | Registrarse |
| `/Account/Login` | Iniciar sesion |
| `/Restaurants/Browse` | Listar restaurantes (Cliente) |
| `/Restaurants/Details/{id}` | Ver detalles (Cliente) |
| `/Restaurants/Index` | Mis restaurantes (Restaurantero) |
| `/Restaurants/Create` | Crear restaurante (Restaurantero) |
| `/Restaurants/Edit/{id}` | Editar restaurante (Restaurantero) |
| `/Restaurants/Delete/{id}` | Eliminar restaurante (Restaurantero) |
| `/Reserve` | Hacer reserva (Cliente) |
| `/Reserve?restauranteId={id}` | Reservar en restaurante especifico |
| `/Reservations/List` | Ver mis reservas (Cliente) |
| `/Account/Profile` | Mi perfil (Autenticado) |

---

## Datos de Prueba Automaticos

Se crean 6 restaurantes automaticamente en el primer inicio:

1. **La Trattoria Italiana** - Cocina Italiana
   - Telefono: 1123456789
   - 5 mesas

2. **Sakura Sushi** - Cocina Japonesa
   - Telefono: 1198765432
   - 5 mesas

3. **El Gaucho Argentino** - Parrilla
   - Telefono: 1143215678
   - 6 mesas

4. **Brasserie Parisienne** - Cocina Francesa
   - Telefono: 1156789012
   - 5 mesas

5. **Vegetalia Bio** - Vegetariana
   - Telefono: 1134567890
   - 4 mesas

6. **Dragon Rojo** - Asiatica
   - Telefono: 1176543210
   - 5 mesas

---

## Solucionar Problemas Comunes

### La pagina se ve sin estilos (muy basica)
- Verifica que Bootstrap se está cargando: Presiona F12, ve a Network
- Si `/css/site.css` falla (404), ejecuta: `dotnet run` nuevamente

### No puedo registrar un usuario
- Asegurate de que la contrasena tenga al menos 6 caracteres
- Debe tener numeros y minusculas

### No veo restaurantes en Browse
- Verifica que la BD se migró correctamente
- Ejecuta: `dotnet ef database update`
- Reinicia la aplicacion

### Las reservas no aparecen
- Verifica que estés autenticado como Cliente
- Ve a `/Reservations/List`
- Si está vacio, haz una nueva reserva en `/Restaurants/Browse`

### Puedo ver mis restaurantes como Restaurantero pero estan vacios
- Eso es normal. Cada restaurantero solo ve sus propios restaurantes
- Los 6 de ejemplo fueron creados por el seeder (sin propietario)
- Crea un nuevo restaurante para verlo en tu lista

---

**Compilacion:** OK ?
**Base de Datos:** Migrada ?
**Data Seeding:** Activo ?
**Estilos:** Cargados desde CDN ?
