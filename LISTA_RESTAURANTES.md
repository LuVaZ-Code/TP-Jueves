# Lista de Restaurantes para Clientes - TP Jueves

## Nuevas Paginas Creadas

### 1. /Restaurants/Browse.cshtml
- **Proposito**: Listar todos los restaurantes disponibles para que clientes puedan buscar y filtrar
- **Autenticacion**: Publica (sin [Authorize])
- **Caracteristicas**:
  - Buscador por nombre, descripcion o ubicacion
  - Grid responsive mostrando cada restaurante
  - Botones "Ver Detalles" y "Reservar"
  - Enlace a Login para usuarios no autenticados
  - Filtrado en tiempo real

**Campos mostrados por restaurante**:
- Nombre (destacado en rojo)
- Direccion
- Descripcion
- Telefono
- Email
- Cantidad de mesas

### 2. /Restaurants/Details.cshtml
- **Proposito**: Mostrar detalles completos de un restaurante
- **Autenticacion**: Publica (sin [Authorize])
- **Caracteristicas**:
  - Informacion completa del restaurante
  - Lista de mesas disponibles con capacidad
  - Links de contacto (telefono y email)
  - Boton para hacer reserva (si esta autenticado)
  - Placeholder para resenas/calificaciones (proximamente)

**Layout**:
- Panel izquierdo: Informacion del restaurante
- Panel derecho: Mesas disponibles
- Footer: Seccion de resenas (proximamente)

### 3. /Restaurants/Browse.cshtml.cs
- Lee todos los restaurantes no eliminados de la base de datos
- Implementa busqueda por SearchTerm
- Incluye relacion con Mesas para mostrar cantidad

### 4. /Restaurants/Details.cshtml.cs
- Lee restaurante por ID
- Valida que no este eliminado
- Carga las mesas asociadas

## Cambios en Paginas Existentes

### Reserve.cshtml.cs
- Ahora acepta parametro `RestauranteId` (optional)
- [Authorize(Roles = "Cliente")] agregado para restriccion de rol
- Si viene restauranteId, valida que exista y lo guarda
- Mantiene logica de reserva existente

### Reserve.cshtml
- Muestra encabezado especial si hay restaurante seleccionado
- Tarjeta con datos del restaurante (nombre, direccion, telefono, email)
- Mismo formulario de reserva

### Index.cshtml
- Para Cliente: Cambio de "Reservar" a "Buscar Restaurante"
- Nuevo boton que lleva a /Restaurants/Browse
- Mantiene funcionalidad para Restaurantero

### _Layout.cshtml
- Agregado link "Restaurantes" en navbar para Cliente
- Link lleva a /Restaurants/Browse
- Flujo de navegacion actualizado

## Flujo de Usuario (Cliente)

```
1. Usuario no autenticado accede a /Restaurants/Browse
   ?
2. Ve lista de restaurantes (puede buscar)
   ?
3. Elige Ver Detalles o Reservar
   ?
   Si Ver Detalles:
   - Ve info completa del restaurante
   - Puede hacer reserva
   
   Si Reservar (no autenticado):
   - Lo redirige a Login
   
   Si Reservar (autenticado):
   - Va a /Reserve?restauranteId=X
   - Ve datos del restaurante en la reserva
   - Completa formulario
   - Confirma reserva
```

## Base de Datos

No requiere migracion nueva (usa modelos existentes):
- Restaurante (ya existe)
- Mesa (ya tiene FK a Restaurante)
- Reserva (puede tener FK a Restaurante opcionalmente)

## Seguridad

- Paginas de Browse y Details: Publicas pero solo muestran restaurantes no eliminados
- Pagina Reserve: [Authorize(Roles = "Cliente")] - Solo clientes autenticados
- Validacion: RestauranteId validado en base de datos antes de procesar

## Estilos Aplicados

- Cards con diseño moderno
- Gradiente rojo para encabezado de restaurantes
- Gradiente azul para mesas disponibles
- Responsive grid que se adapta a cualquier tamaño
- Botones con iconos FontAwesome
- Buscador en formulario limpio

## Proximo Paso (Sugerido)

Después de este, se puede:

1. **Editar/Eliminar Restaurantes**
   - Pagina Edit para restaurantero
   - Validar que solo el propietario pueda editar
   - Soft delete

2. **Gestionar Mesas**
   - CRUD de mesas por restaurante
   - Agregar/quitar mesas
   - Especificar capacidad

3. **Sistema de Turnos**
   - Como mencionaste: Turno tiene fecha y horarios
   - Mesa tiene lista de turnos
   - Validar disponibilidad por turno/mesa/fecha

4. **Resenas y Calificaciones**
   - Dejar resena después de reserva
   - Calificacion con estrellas
   - Mostrar promedio de calificaciones

---

**Compilacion:** OK ?
**Rutas funcionales:** ?
  - /Restaurants/Browse
  - /Restaurants/Details/{id}
  - /Reserve?restauranteId={id}
**Roles configurados:** ?
  - Cliente puede acceder
  - Restaurantero no ve estas paginas
