# 🚀 Guía de Implementación del Nuevo Diseño

## ✅ Cambios Completados

1. ✓ **site.css** - Sistema de diseño completo renovado
2. ✓ **animations.css** - Biblioteca de animaciones profesionales
3. ✓ **site.js** - JavaScript interactivo mejorado
4. ✓ **_Layout.cshtml** - Layout principal actualizado
5. ✓ **Index.cshtml** - Página de inicio renovada

## 📝 Próximos Pasos para Aplicar el Diseño

### 1. Páginas de Reservas

Actualiza las páginas en `Pages/Reservations/` para usar las nuevas clases:

**Antes:**
```html
<div style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 2rem;">
```

**Después:**
```html
<div class="hero fade-in">
```

**Clases a reemplazar:**
- Inline styles → Usar clases CSS definidas
- Colores viejos (#667eea, #764ba2, etc.) → Variables CSS (var(--primary-500), etc.)
- Box-shadows hardcoded → var(--shadow-md), var(--shadow-lg), etc.
- Border-radius hardcoded → var(--radius-lg), var(--radius-xl), etc.

### 2. Páginas de Restaurantes

En `Pages/Restaurants/`:

**Dashboard.cshtml:**
```html
<!-- Reemplazar stat cards inline styles -->
<div class="stat-card">
    <div class="stat-icon" style="background: linear-gradient(135deg, var(--primary-500), var(--primary-600)); color: white;">
        📊
    </div>
    <div class="stat-value" style="color: var(--primary-600);">100</div>
    <div class="stat-label">Total Reservas</div>
</div>
```

**Cards de reservas:**
```html
<div class="reserva-card">
    <!-- contenido -->
</div>
```

### 3. Formularios

Actualizar todos los formularios para usar:
```html
<div style="margin-bottom: var(--space-4);">
    <label class="form-label">Campo</label>
    <input type="text" class="form-control" placeholder="Placeholder">
    <small class="form-text">Texto de ayuda</small>
</div>
```

### 4. Botones

Reemplazar botones inline:
```html
<!-- Antes -->
<a href="#" style="background: linear-gradient(...); padding: 1rem 2rem; ...">Botón</a>

<!-- Después -->
<a href="#" class="btn btn-primary">Botón</a>
```

### 5. Alertas y Mensajes

```html
<!-- Éxito -->
<div class="alert alert-success">
    ✓ Operación exitosa
</div>

<!-- Error -->
<div class="alert alert-danger">
    ✕ Error encontrado
</div>

<!-- Info -->
<div class="alert alert-info">
    ℹ️ Información importante
</div>

<!-- Warning -->
<div class="alert alert-warning">
    ⚠️ Advertencia
</div>
```

### 6. Agregar Animaciones

Añade clases de animación a elementos:
```html
<!-- Hero sections -->
<div class="hero fade-in">

<!-- Cards -->
<div class="card scale-in">

<!-- Feature cards -->
<div class="feature-card slide-in delay-200">

<!-- Stats -->
<div class="stat-card fade-in-up delay-300">
```

## 🎨 Paleta de Colores para Migración

### Mapeo de Colores Antiguos → Nuevos

```
Viejo Purple (#667eea, #764ba2) → var(--primary-600) o var(--accent-600)
Viejo Pink (#f093fb, #f5576c) → var(--accent-500)
Viejo Green (#11998e, #38ef7d) → var(--primary-500)
Viejo Orange (#f39c12) → var(--warning)
Viejo Red (#e74c3c) → var(--error)
Viejo Dark (#2d3436) → var(--neutral-900)
Viejo Light (#f8f9fa) → var(--neutral-50)
```

## 🔧 Variables CSS Útiles

### Colores
```css
--primary-500    /* Verde principal */
--accent-500     /* Indigo acento */
--neutral-700    /* Texto normal */
--neutral-900    /* Texto oscuro */
--success        /* Verde éxito */
--warning        /* Naranja advertencia */
--error          /* Rojo error */
--info           /* Azul información */
```

### Espaciado
```css
--space-1: 0.25rem  /* 4px */
--space-2: 0.5rem   /* 8px */
--space-3: 0.75rem  /* 12px */
--space-4: 1rem     /* 16px */
--space-6: 1.5rem   /* 24px */
--space-8: 2rem     /* 32px */
```

### Bordes
```css
--radius-sm: 0.375rem
--radius-md: 0.5rem
--radius-lg: 0.75rem
--radius-xl: 1rem
--radius-2xl: 1.5rem
--radius-full: 9999px
```

### Sombras
```css
--shadow-sm   /* Sombra sutil */
--shadow-md   /* Sombra estándar */
--shadow-lg   /* Sombra elevada */
--shadow-xl   /* Sombra modal */
--shadow-2xl  /* Sombra flotante */
```

## 📋 Checklist de Migración por Página

### Lista.cshtml (Reservas)
- [ ] Actualizar hero section
- [ ] Migrar stat cards
- [ ] Actualizar reserva cards
- [ ] Reemplazar botones
- [ ] Agregar animaciones

### Details.cshtml (Detalle Reserva)
- [ ] Hero section
- [ ] Info cards
- [ ] Badges de estado
- [ ] Botones de acción

### Dashboard.cshtml (Restaurantero)
- [ ] Stat cards
- [ ] Reserva cards
- [ ] Gráficos/tablas
- [ ] Quick actions

### Browse.cshtml (Buscar Restaurantes)
- [ ] Cards de restaurantes
- [ ] Filtros
- [ ] Grid layout
- [ ] Animaciones de entrada

## 💡 Tips de Implementación

1. **Migra página por página**: No intentes cambiar todo a la vez
2. **Usa el inspector**: Chrome DevTools para verificar estilos
3. **Mantén consistencia**: Usa siempre las mismas clases para elementos similares
4. **Test responsive**: Verifica en mobile, tablet y desktop
5. **Verifica accesibilidad**: Contraste de colores y navegación por teclado

## 🎯 Patrón de Migración Recomendado

```html
<!-- ANTES -->
<div style="background: #fff; padding: 2rem; border-radius: 12px; box-shadow: 0 5px 15px rgba(0,0,0,0.1);">
    <h3 style="color: #2c3e50; margin-bottom: 1rem;">Título</h3>
    <p style="color: #636e72;">Contenido</p>
    <a href="#" style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 0.75rem 1.5rem; border-radius: 8px; text-decoration: none;">
        Acción
    </a>
</div>

<!-- DESPUÉS -->
<div class="card fade-in">
    <div class="card-body">
        <h3 class="card-title">Título</h3>
        <p class="card-text">Contenido</p>
        <a href="#" class="btn btn-primary">Acción</a>
    </div>
</div>
```

## 📞 Recursos

- **Guía Visual**: Abre `GUIA_DISEÑO.html` en tu navegador
- **Documentación**: Lee `DISEÑO_RESTAURANCITO.md`
- **CSS Reference**: Revisa `wwwroot/css/site.css` para todas las clases disponibles
- **Animaciones**: Consulta `wwwroot/css/animations.css` para efectos disponibles

## 🐛 Troubleshooting

**Problema**: Los estilos no se aplican
- Solución: Verifica que `site.css` y `animations.css` estén en _Layout.cshtml
- Limpia caché del navegador (Ctrl+Shift+R)

**Problema**: Las animaciones no funcionan
- Solución: Asegúrate de que `site.js` esté cargado correctamente
- Verifica la consola del navegador por errores

**Problema**: Responsive no funciona bien
- Solución: Usa clases responsive de Bootstrap combinadas con las nuevas
- Verifica meta viewport en el head

---

**¡Éxito con la migración!** 🎉

El nuevo diseño está listo y funcionando en la página de inicio. Ahora puedes aplicar gradualmente estos estilos al resto de las páginas del sistema.
