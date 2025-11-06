# Navegacion Rapida - TP Jueves

## ?? Pagina de Inicio (Sin Autenticacion)

Ahora en la pagina principal verás **3 botones principales**:

```
???????????????????????????????
?  ??? Ver Restaurantes        ?  ? NUEVO: Para explorar sin cuenta
???????????????????????????????

???????????????????????????????
?  Ingresar                   ?  ? Para iniciar sesion
???????????????????????????????

???????????????????????????????
?  Registrarse                ?  ? Para crear cuenta
???????????????????????????????
```

## ?? Flujo de Navegacion

### Sin Autenticacion (Cualquiera)
```
1. Inicio (/) 
   ?
2. Haz clic en "Ver Restaurantes"
   ?
3. Ves lista publica de restaurantes (/Restaurants/Browse)
   ?
4. Puedes:
   - Buscar por nombre/descripcion/ubicacion
   - Ver detalles de cada restaurante
   - Ver mesas disponibles
   ?
5. Para RESERVAR debes iniciar sesion
```

### Inicio Rapido (Recomendado)

**Para Clientes:**
```
1. Accede a la pagina: https://localhost:7017
2. Haz clic en "Ver Restaurantes"
3. Explora y busca restaurantes
4. Cuando encuentres uno que te guste, haz clic en "Reservar"
5. Si no estás autenticado, te pedirá que inicies sesion
6. Después de iniciar sesion, completa la reserva
```

**Para Restauranteros:**
```
1. Accede a: https://localhost:7017
2. Haz clic en "Registrarse"
3. Selecciona "Restaurantero"
4. Ahora irás a "Mis Restaurantes"
5. Crea tu primer restaurante
6. Listo!
```

## ?? Rutas Principales (SIN AUTENTICACION)

| Ruta | Descripcion | Boton |
|------|-------------|-------|
| `/` | Pagina de inicio | En navbar |
| `/Restaurants/Browse` | **Ver todos los restaurantes** | ??? Ver Restaurantes |
| `/Restaurants/Details/{id}` | Detalles de un restaurante | En Browse |
| `/Account/Login` | Iniciar sesion | Ingresar |
| `/Account/Register` | Crear cuenta | Registrarse |

## ?? Lo que Puedes Hacer SIN Cuenta

? Ver lista de restaurantes
? Buscar restaurantes por nombre/descripcion/ubicacion
? Ver detalles completos de un restaurante
? Ver mesas disponibles
? Ver informacion de contacto

? Hacer reservas (necesitas cuenta)
? Ver tus reservas (necesitas cuenta)
? Crear restaurante (necesitas ser Restaurantero)

## ?? Lo que Necesitas Cuenta Para

?? **Hacer Reservas**
- Debes estar logueado como Cliente
- Accedes desde cualquier restaurante con boton "Reservar"

?? **Crear Restaurantes**
- Debes estar logueado como Restaurantero
- Ve a "Mis Restaurantes" en la pagina de inicio

## ?? Consejos

1. **Primero explora:** Sin cuenta puedes ver todos los restaurantes
2. **Luego registrate:** Con tu rol preferido (Cliente o Restaurantero)
3. **Finalmente reserva:** Si eres cliente, haz reservas
4. **O crea:** Si eres restaurantero, agrega tu restaurante

---

## Ejemplo Completo (Flujo Usuario Cliente)

```
PASO 1: Llego a la pagina
https://localhost:7017

PASO 2: Veo 3 botones grandes
- ??? Ver Restaurantes ? Hago clic
- Ingresar
- Registrarse

PASO 3: Aparece lista de restaurantes
Puedo buscar, ver detalles, mesas, etc.

PASO 4: Encuentro un restaurante que me gusta
Hago clic en "Reservar"

PASO 5: Me dice que debo iniciar sesion
Hago clic en "Ingresar"

PASO 6: Me lleva a login o registro
Creo cuenta como "Cliente"

PASO 7: Completo formulario de reserva
DNI, cantidad, fecha, horario, dieta

PASO 8: Confirmacion
Mi reserva está hecha!

PASO 9: Voy a "Mis Reservas" para ver mis reservas
```

---

## Buscador de Restaurantes

En `/Restaurants/Browse` tienes un buscador que busca en:

- **Nombre:** "La Trattoria", "Sakura", etc.
- **Descripcion:** "italiano", "sushi", "parrilla", etc.
- **Ubicacion:** "Centro", "Palermo", "San Telmo", etc.

**Ejemplo de busquedas:**
- "italiano" ? Encuentra La Trattoria Italiana
- "parrilla" ? Encuentra El Gaucho Argentino
- "centro" ? Encuentra restaurantes en el centro

---

**¡Ahora está claro donde esta el boton! ??**

En la pagina de inicio, el primer boton grande azul dice "??? Ver Restaurantes"
