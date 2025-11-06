# Gestión Completa de Restaurantes - TP Jueves

## ?? Panorama General

El sistema ahora permite a los restauranteros gestionar completamente sus restaurantes:

```
Restaurante
??? Información Básica (CRUD)
??? Mesas (CRUD)
??? Horarios Disponibles (CRUD)
```

## ??? Gestión de Mesas

### Nuevas Páginas

| Ruta | Descripción | Función |
|------|-------------|---------|
| `/Restaurants/Mesas/Index?restauranteId={id}` | Listar mesas | Restaurantero |
| `/Restaurants/Mesas/Create?restauranteId={id}` | Agregar mesa | Restaurantero |
| `/Restaurants/Mesas/Edit/{id}` | Editar capacidad | Restaurantero |
| `/Restaurants/Mesas/Delete/{id}` | Eliminar mesa | Restaurantero |

### Funcionalidades

#### 1. Index - Listar Mesas
- **Vista**: Grid de mesas con capacidad
- **Información mostrada**:
  - ID de la mesa
  - Capacidad de personas
  - Número de reservas
- **Acciones**: Editar, Eliminar
- **Botón**: Agregar nueva mesa

#### 2. Create - Agregar Mesa
- **Campos**:
  - Capacidad (1-20)
- **Atajos**: Botones rápidos (2, 4, 6, 8, 10 personas)
- **Validación**: Capacidad dentro de rango

#### 3. Edit - Editar Mesa
- **Campos editables**: Capacidad
- **Solo lectura**: Número de reservas

#### 4. Delete - Eliminar Mesa
- **Confirmación**: Muestra detalles
- **Advertencia**: Si hay reservas asignadas
- **Operación**: Elimina permanentemente

## ? Sistema de Horarios (Turnos)

### Nuevas Páginas

| Ruta | Descripción |
|------|-------------|
| `/Restaurants/Turnos/Index?restauranteId={id}` | Listar horarios |
| `/Restaurants/Turnos/Create?restauranteId={id}` | Agregar horario |
| `/Restaurants/Turnos/Edit/{id}` | Editar horario |
| `/Restaurants/Turnos/Delete/{id}` | Eliminar horario |

### Funcionalidades

#### 1. Index - Listar Horarios
- **Tabla con**:
  - Fecha
  - Horario (18:00, 20:00, 22:00)
  - Capacidad máxima
  - Capacidad usada
  - Disponible (restante)
  - Estado (Disponible, Casi Lleno, Lleno)
- **Acciones**: Editar, Eliminar

#### 2. Create - Agregar Horario
- **Campos**:
  - Fecha (date picker)
  - Horario (select)
  - Capacidad máxima (1-100)
- **Validación**: Fecha >= hoy

#### 3. Edit - Editar Horario
- **Campos editables**:
  - Fecha
  - Horario
  - Capacidad máxima
  - Estado (Activo/Inactivo)
- **Solo lectura**: Capacidad usada

#### 4. Delete - Eliminar Horario
- **Confirmación**: Detalles del horario
- **Advertencia**: Número de reservas
- **Operación**: Soft delete (IsActive = false)

## ?? Flujo de Gestión (Restaurantero)

```
1. Login como Restaurantero
   ?
2. Página de Inicio ? "Mis Restaurantes"
   ?
3. Lista de tus restaurantes
   ?
4. Haz clic en "Ver" un restaurante
   ?
5. Panel de control del restaurante:
   ??? Editar información
   ??? "Gestionar Mesas" ? CRUD de mesas
   ??? "Gestionar Horarios" ? CRUD de horarios
```

## ?? Ejemplo: Crear Restaurante Completo

### Paso 1: Crear Restaurante
1. Ve a "Mis Restaurantes"
2. Haz clic en "Agregar Restaurante"
3. Completa: Nombre, Descripción, Dirección, Teléfono, Email

### Paso 2: Agregar Mesas
1. Haz clic en "Ver" del restaurante
2. Haz clic en "Gestionar Mesas"
3. Para cada mesa:
   - Haz clic en "Agregar Mesa"
   - Selecciona capacidad (ej: 4 personas)
   - Confirma

**Ejemplo de mesas**:
```
Mesa 1: 2 personas (parejas)
Mesa 2: 2 personas (parejas)
Mesa 3: 4 personas (grupos pequeños)
Mesa 4: 4 personas (grupos pequeños)
Mesa 5: 6 personas (grupos medianos)
Mesa 6: 8 personas (grupos grandes)
```

### Paso 3: Configurar Horarios
1. Haz clic en "Ver" del restaurante
2. Haz clic en "Gestionar Horarios"
3. Para cada turno disponible:
   - Haz clic en "Agregar Horario Disponible"
   - Fecha: 15/12/2024
   - Horario: 20:00 - 22:00
   - Capacidad: 50 personas

**Ejemplo de horarios**:
```
15/12 | 18:00-20:00 | 50 personas
15/12 | 20:00-22:00 | 50 personas
15/12 | 22:00-00:00 | 50 personas
16/12 | 18:00-20:00 | 50 personas
16/12 | 20:00-22:00 | 50 personas
```

## ??? Relaciones Base de Datos

```
Restaurante
??? Mesas (1:N) [OnDelete: Cascade]
?   ??? Reservas (1:N)
??? TurnosDisponibles (1:N) [OnDelete: Cascade]
?   ??? Reservas (1:N)
??? Reservas (1:N)

Mesa
??? Restaurante (N:1)
??? Reservas (1:N)

TurnoDisponible
??? Restaurante (N:1)
??? Reservas (1:N)

Reserva
??? Restaurante (N:1)
??? Mesa (N:1) [Opcional]
??? TurnoDisponible (N:1) [Opcional]
??? Cliente (N:1)
```

## ? Validaciones

### Mesas
- Capacidad: 1-20 personas
- No se puede cambiar capacidad de mesa si tiene reservas
- Al eliminar mesa, se advierte sobre reservas asignadas

### Horarios
- Fecha: >= hoy
- Capacidad: 1-100 personas
- Solo se pueden editar horarios futuros
- Se rastrean cambios en capacidad usada

### Seguridad
- Solo el propietario puede gestionar sus mesas/horarios
- Validación: `if (Restaurante.PropietarioId != user.Id) return Forbid()`

## ?? Proximos Pasos

1. **Integración con Reservas**
   - Mostrar solo horarios disponibles en formulario
   - Asignar mesa automáticamente
   - Actualizar capacidad usada del turno

2. **Gestión de Reservas (Restaurantero)**
   - Ver todas las reservas del restaurante
   - Confirmar/Rechazar reservas
   - Enviar recordatorios

3. **Dashboard**
   - Ocupación por fecha
   - Ingresos estimados
   - Clientes frecuentes

4. **Notificaciones**
   - Email a cliente cuando se confirma
   - SMS recordatorio día anterior
   - Alertas cuando horario está casi lleno

## ?? Archivos Creados

```
Pages/Restaurants/
??? Mesas/
?   ??? Index.cshtml (.cs)
?   ??? Create.cshtml (.cs)
?   ??? Edit.cshtml (.cs)
?   ??? Delete.cshtml (.cs)
?   ??? _ViewImports.cshtml
??? Turnos/
?   ??? Index.cshtml (.cs)
?   ??? Create.cshtml (.cs)
?   ??? Edit.cshtml (.cs)
?   ??? Delete.cshtml (.cs)
?   ??? _ViewImports.cshtml
??? View.cshtml (ACTUALIZADO)
```

## ?? Tabla Resumen de Features

| Feature | Estado | Acceso |
|---------|--------|--------|
| Crear Restaurante | ? | Restaurantero |
| Editar Restaurante | ? | Restaurantero |
| Eliminar Restaurante | ? | Restaurantero |
| Agregar Mesa | ? | Restaurantero |
| Editar Mesa | ? | Restaurantero |
| Eliminar Mesa | ? | Restaurantero |
| Agregar Horario | ? | Restaurantero |
| Editar Horario | ? | Restaurantero |
| Eliminar Horario | ? | Restaurantero |
| Ver Mesas en Browse | ? | Cliente |
| Filtrar por Horario | ? | Cliente |
| Hacer Reserva | ? | Cliente |
| Ver Mis Reservas | ? | Cliente |
| Cancelar Reserva | ? | Cliente |

---

**Compilacion:** OK ?
**CRUD Mesas:** Completo ?
**CRUD Turnos:** Completo ?
**Seguridad:** Validado ?
**Interfaz:** Responsive ?
