# Sistema de Turnos y Horarios Disponibles - TP Jueves

## ?? Nueva Funcionalidad: Gestión de Horarios

### Modelos Agregados

#### 1. `TurnoDisponible.cs`
- **Propósito**: Representa un horario disponible para reservas en un restaurante
- **Campos principales**:
  - `Fecha`: Fecha del horario (sin hora)
  - `Horario`: Enum (H18_20, H20_22, etc.)
  - `CapacidadMaxima`: Máximo de personas por horario
  - `CapacidadUsada`: Personas ya reservadas
  - `CapacidadRestante`: Calculada automáticamente
  - `EstaLleno`: Bool derivado
  - `IsActive`: Disponible o no

#### 2. Actualización a `Reserva.cs`
- Agregado FK a `TurnoDisponible`
- `MesaId` ahora es nullable (opcional)
- Permite rastrear qué turno fue reservado

### Migraciones

```bash
dotnet ef migrations add AddTurnosDisponibles
dotnet ef database update
```

## ?? Rutas Nuevas

| Ruta | Descripción | Acceso |
|------|-------------|--------|
| `/Restaurants/Turnos/Index?restauranteId={id}` | Listar horarios | Restaurantero |
| `/Restaurants/Turnos/Create?restauranteId={id}` | Agregar horario | Restaurantero |
| `/Restaurants/Turnos/Edit/{id}` | Editar horario | Restaurantero |
| `/Restaurants/Turnos/Delete/{id}` | Eliminar horario | Restaurantero |

## ?? Nuevas Páginas

### 1. `/Restaurants/Turnos/Index`
- **Acceso**: Solo Restaurantero (propietario del restaurante)
- **Funcionalidad**:
  - Lista todos los horarios disponibles
  - Ordena por Fecha y Horario
  - Muestra:
    - Fecha
    - Horario
    - Capacidad máxima
    - Capacidad usada
    - Disponible (restante)
    - Estado (Lleno, Casi Lleno, Disponible)
  - Botones: Editar, Eliminar
  - Botón para agregar nuevo horario

### 2. `/Restaurants/Turnos/Create`
- **Acceso**: Solo Restaurantero
- **Formulario**:
  - Fecha (date picker, mínimo hoy)
  - Horario (select con todos los horarios disponibles)
  - Capacidad Máxima (1-100)
- **Comportamiento**: Después de crear, redirige a Index

### 3. `/Restaurants/Turnos/Edit`
- **Acceso**: Solo Restaurantero
- **Campos editables**:
  - Fecha
  - Horario
  - Capacidad Máxima
  - Estado (Activo/Inactivo)
- **Solo lectura**:
  - Capacidad Usada (se actualiza automáticamente)
  - Número de reservas

### 4. `/Restaurants/Turnos/Delete`
- **Acceso**: Solo Restaurantero
- **Confirmación**: Muestra detalles del horario antes de eliminar
- **Advertencia**: Avisa si hay reservas en este horario
- **Operación**: Soft delete (marca `IsActive` como false)

## ?? Seguridad

```csharp
// Solo el propietario del restaurante puede:
if (Restaurante.PropietarioId != user.Id)
    return Forbid();
```

- Validación en cada página
- Restaurante se obtiene desde `TurnoDisponible`
- Imposible editar turnos de otros restaurantes

## ?? Uso Paso a Paso (Restaurantero)

### Ejemplo: Agregar Horarios para El Gaucho

```
1. Inicia sesion como Restaurantero
2. Ve a "Mis Restaurantes"
3. Haz clic en "Ver" para El Gaucho
4. Ahora ves un botón "Gestionar Horarios"
5. Haz clic en "Gestionar Horarios"
6. Ves lista vacía (primer vez)
7. Haz clic en "Agregar Horario Disponible"
8. Completa:
   - Fecha: 15/12/2024
   - Horario: 20:00 - 22:00
   - Capacidad: 50
9. Haz clic en "Agregar Horario"
10. Vuelves a la lista y ves el nuevo horario
11. Puedes editar o eliminar si lo necesitas
```

## ?? Tabla de Horarios Disponibles

Ejemplo de cómo se vería:

```
Fecha      | Horario    | Cap.Máx | Cap.Usada | Disponible | Estado
-----------|------------|---------|-----------|------------|----------
15/12/2024 | 20:00-22:00|   50    |     30    |     20     | Disponible
15/12/2024 | 22:00-00:00|   50    |     50    |      0     | Lleno
16/12/2024 | 18:00-20:00|   50    |     45    |      5     | Casi Lleno
16/12/2024 | 20:00-22:00|   50    |      0    |     50     | Disponible
```

## ?? Flujo de Reserva Mejorado

Proximamente, cuando un cliente reserve:

1. Selecciona restaurante
2. Sistema muestra solo fechas/horarios disponibles
3. Sistema valida capacidad restante
4. Si hay capacidad, crea reserva
5. Incrementa `CapacidadUsada` del turno
6. Si no hay, muestra alternativas disponibles

## ? Estados de Horarios

- **Disponible**: `CapacidadRestante > 5`
- **Casi Lleno**: `1 <= CapacidadRestante <= 5`
- **Lleno**: `CapacidadRestante <= 0`

## ??? Relaciones Base de Datos

```
TurnoDisponible
??? RestauranteId (FK) ? Restaurante [Cascade]
??? Reservas (1:N)
??? IsActive (soft delete)

Reserva
??? TurnoDisponibleId (FK) ? TurnoDisponible [SetNull]
??? RestauranteId (FK) ? Restaurante
??? MesaId (FK) ? Mesa [opcional]
??? ClienteId (FK) ? ApplicationUser
```

## ?? Próximos Pasos

1. Integración con formulario de reserva
   - Mostrar solo turnos disponibles
   - Validar capacidad restante
   - Actualizar `CapacidadUsada`

2. Gestión de Mesas
   - Asignar mesas automáticamente
   - Validar capacidad de mesa vs. reserva

3. Notificaciones
   - Email cuando turno casi lleno
   - Recordatorio de reserva

4. Reportes
   - Ocupación por fecha
   - Ingresos estimados
   - Clientes regulares

---

## Archivos Creados

```
Pages/Restaurants/Turnos/
??? Index.cshtml (.cs)          - Listar horarios
??? Create.cshtml (.cs)         - Agregar horario
??? Edit.cshtml (.cs)           - Editar horario
??? Delete.cshtml (.cs)         - Eliminar horario
??? _ViewImports.cshtml         - Imports

Models/
??? TurnoDisponible.cs          - Modelo

Data/
??? ApplicationDbContext.cs     - Actualizado

Migrations/
??? AddTurnosDisponibles        - Nueva tabla
```

---

**Compilacion:** OK ?
**Migracion:** AddTurnosDisponibles ?
**Paginas:** 4 nuevas (Index, Create, Edit, Delete) ?
**Seguridad:** Validacion de propietario ?
