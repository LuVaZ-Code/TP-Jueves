# GUARDAR CAMBIOS - TP Jueves
# Ejecuta este script en PowerShell como Administrador

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  GUARDANDO CAMBIOS DE COPILOT" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Ir al directorio del proyecto
Set-Location "C:\Users\losmelli\Source\Repos\TP-Jueves"

Write-Host "?? Directorio actual:" -ForegroundColor Yellow
Get-Location
Write-Host ""

# Verificar estado de Git
Write-Host "?? Estado actual de Git:" -ForegroundColor Yellow
git status --short

Write-Host ""
Write-Host "¿Deseas continuar y guardar TODOS los cambios? (S/N)" -ForegroundColor Green
$respuesta = Read-Host

if ($respuesta -eq "S" -or $respuesta -eq "s") {
    Write-Host ""
    Write-Host "?? Agregando todos los archivos..." -ForegroundColor Yellow
    git add .
    
    Write-Host ""
    Write-Host "?? Creando commit..." -ForegroundColor Yellow
    git commit -m "Mejoras masivas al sistema de reservas

- Sistema de horarios simplificado sin CapacidadMaxima
- Flujo de reserva en 2 pasos con wizard
- Disponibilidad calculada en tiempo real
- Validacion de email duplicado con mensajes en español
- Correccion de HTML entities y tildes
- Dashboard mejorado con stats inteligentes
- Gestion de horarios actualizada
- UI limpiada en todas las vistas
- Sistema de migracion automatico
"
    
    Write-Host ""
    Write-Host "? CAMBIOS GUARDADOS EXITOSAMENTE!" -ForegroundColor Green
    Write-Host ""
    
    Write-Host "?? ¿Deseas subir los cambios a GitHub? (S/N)" -ForegroundColor Green
    $push = Read-Host
    
    if ($push -eq "S" -or $push -eq "s") {
        Write-Host ""
        Write-Host "?? Subiendo cambios a GitHub..." -ForegroundColor Yellow
        git push origin main
        
        Write-Host ""
        Write-Host "?? CAMBIOS SUBIDOS A GITHUB EXITOSAMENTE!" -ForegroundColor Green
    }
} else {
    Write-Host ""
    Write-Host "? Operación cancelada." -ForegroundColor Red
}

Write-Host ""
Write-Host "Presiona cualquier tecla para salir..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
