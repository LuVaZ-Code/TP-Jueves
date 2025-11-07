@echo off
echo ============================================
echo    LIMPIANDO ARCHIVOS PROBLEMATICOS
echo ============================================
echo.

cd /d "C:\Users\losmelli\Source\Repos\TP-Jueves"

echo Buscando archivos con nombres muy largos...
echo.

REM Limpiar archivos temporales y problemáticos
for /r %%i in (*899a3d49462144b996cce89d1ee4fb1d*) do (
    echo Eliminando: %%i
    del /f /q "%%i" 2>nul
)

REM Limpiar archivos de migraciones problemáticas
for /r "TP Jueves" %%i in (*AddRestauranteEstadoYConfiguracion*) do (
    echo Eliminando: %%i
    del /f /q "%%i" 2>nul
)

echo.
echo Limpieza completada!
echo.

REM Resetear el index de git
echo Reseteando index de Git...
git reset HEAD

echo.
echo Ahora intenta de nuevo:
echo 1. git add .
echo 2. git commit -m "Cambios guardados"
echo.

pause
