@echo off
echo ============================================
echo    GUARDANDO TODOS LOS CAMBIOS DE COPILOT
echo ============================================
echo.

cd /d "C:\Users\losmelli\Source\Repos\TP-Jueves"

echo Agregando todos los archivos modificados...
git add .

echo.
echo Creando commit con resumen de cambios...
git commit -m "Mejoras masivas al sistema de reservas

- Simplificado sistema de horarios (eliminado CapacidadMaxima de HorarioRestaurante)
- Nuevo flujo de reserva en 2 pasos (fecha/cantidad -> horarios disponibles)
- Disponibilidad calculada en tiempo real segun mesas fisicas
- Validacion de email duplicado en registro con mensajes en español
- Correccion de HTML entities y tildes en todas las vistas
- Actualizacion de Dashboard para mostrar HoraReserva en lugar de enum
- Limpieza de stats (eliminadas stats con valor 0)
- Gestion de horarios actualizada para usar HorariosRestaurante
- Eliminado informacion innecesaria de reservas en gestion de mesas
- Correccion de operador null-coalescing en multiples vistas
- Mejoras visuales en cards de restaurantes
- Sistema de migracion para actualizar base de datos
"

echo.
echo Cambios guardados exitosamente!
echo.

git log -1 --oneline

echo.
echo ============================================
echo Para subir los cambios a GitHub ejecuta:
echo    git push origin main
echo ============================================
pause
