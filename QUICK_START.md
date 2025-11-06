# ?? Instalación Rápida - TP Jueves

## ? Empezar en 5 Minutos

### Paso 1: Clonar
```bash
git clone https://github.com/LuVaZ-Code/TP-Jueves
cd "TP-Jueves\TP Jueves"
```

### Paso 2: Preparar BD
```bash
dotnet ef database update
```

### Paso 3: Ejecutar
```bash
dotnet run
```

### Paso 4: Abrir
```
https://localhost:7017
```

## ?? Datos de Prueba

**Cliente:**
- Email: cliente@test.com
- Contraseña: password123

**Restaurantero:**
- Email: restaurante@test.com  
- Contraseña: password123

## ?? Lo Primero que Hacer

### Como Cliente:
1. Haz clic en "??? Ver Restaurantes"
2. Busca un restaurante
3. Haz clic en "Reservar"
4. Completa el formulario
5. ¡Reserva confirmada!

### Como Restaurantero:
1. Ve a "Mis Restaurantes"
2. Haz clic en "Agregar Restaurante"
3. Completa información
4. Haz clic en "Ver"
5. Gestiona Mesas y Horarios
6. ¡Listo para recibir reservas!

## ?? Comandos Útiles

### Reconstruir BD
```bash
# Eliminar BD actual
rm .\tpjueves.db

# Actualizar
dotnet ef database update

# Reiniciar app
dotnet run
```

### Ver Migraciones
```bash
dotnet ef migrations list
```

### Agregar Nueva Migración
```bash
dotnet ef migrations add NombreDeLaMigracion
dotnet ef database update
```

## ?? Rutas Principales

| Ruta | Descripción |
|------|-------------|
| `/` | Inicio |
| `/Restaurants/Browse` | Buscar restaurantes |
| `/Reservations/List` | Mis reservas |
| `/Restaurants/Index` | Mis restaurantes |
| `/Admin/ResetRoles` | Cambiar roles (dev) |

## ?? Problemas Comunes

### "Page is blank"
- Limpia caché: F12 ? Application ? Clear Site Data
- Recarga: Ctrl+Shift+R

### "No veo restaurantes"
- Verifica que se crearon: `dotnet ef database update`
- Reinicia la app

### "Sin rol asignado"
- Ve a `/Admin/ResetRoles`
- Asigna tu rol deseado
- Recarga página

### "Error de compilación"
```bash
dotnet clean
dotnet build
```

## ?? Documentación Completa

Ver archivos `.md` en raíz:
- `README.md` - Descripción completa
- `GUIA_EJECUCION.md` - Solucionar problemas
- `GESTION_COMPLETA.md` - Todas las features

## ?? Tips

1. **Rápido:** Registrate directamente en `/Account/Register`
2. **Admin:** Crea primer usuario, asigna Admin en `/Admin/ResetRoles`
3. **Datos:** Se crean 6 restaurantes automáticamente
4. **Dev:** Usa `/Admin/ResetRoles` para cambiar roles sin re-registrarse

---

**¡Todo listo! Ahora puedes empezar a usar TP Jueves ???**
