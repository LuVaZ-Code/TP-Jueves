# Script para aplicar migración de horarios

$ErrorActionPreference = "Stop"

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "  APLICANDO MIGRACION: HorariosRestaurante" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

# Ubicación de la base de datos
$dbPath = "C:\Users\losmelli\Source\Repos\TP-Jueves\TP Jueves\tpjueves.db"
$sqlFile = "C:\Users\losmelli\Source\Repos\TP-Jueves\migration_horarios.sql"

# Verificar que existe la base de datos
if (-not (Test-Path $dbPath)) {
    Write-Host "ERROR: No se encontro la base de datos en: $dbPath" -ForegroundColor Red
    exit 1
}

# Verificar que existe el archivo SQL
if (-not (Test-Path $sqlFile)) {
    Write-Host "ERROR: No se encontro el archivo SQL en: $sqlFile" -ForegroundColor Red
    exit 1
}

Write-Host "Base de datos encontrada: $dbPath" -ForegroundColor Green
Write-Host "Archivo SQL encontrado: $sqlFile" -ForegroundColor Green
Write-Host ""

# Hacer backup de la base de datos
$backupPath = "$dbPath.backup_" + (Get-Date -Format "yyyyMMdd_HHmmss")
Write-Host "Creando backup en: $backupPath" -ForegroundColor Yellow
Copy-Item $dbPath $backupPath -Force
Write-Host "Backup creado exitosamente!" -ForegroundColor Green
Write-Host ""

# Leer el contenido del archivo SQL
$sqlContent = Get-Content $sqlFile -Raw -Encoding UTF8

# Aplicar la migración usando sqlite3
Write-Host "Aplicando migracion..." -ForegroundColor Yellow

try {
    # Verificar si sqlite3 está disponible
    $sqlite3Path = "sqlite3"
    
    # Intentar usar sqlite3 desde el PATH
    try {
        $null = & $sqlite3Path --version 2>&1
    } catch {
        # Si no está en PATH, buscar en ubicaciones comunes
        $possiblePaths = @(
            "C:\Program Files\SQLite\sqlite3.exe",
            "C:\SQLite\sqlite3.exe",
            "$env:LOCALAPPDATA\Programs\SQLite\sqlite3.exe"
        )
        
        $found = $false
        foreach ($path in $possiblePaths) {
            if (Test-Path $path) {
                $sqlite3Path = $path
                $found = $true
                break
            }
        }
        
        if (-not $found) {
            Write-Host "ERROR: No se encontro sqlite3.exe" -ForegroundColor Red
            Write-Host "Por favor instala SQLite desde: https://www.sqlite.org/download.html" -ForegroundColor Yellow
            Write-Host ""
            Write-Host "O aplica la migracion manualmente con este comando:" -ForegroundColor Yellow
            Write-Host "sqlite3 `"$dbPath`" < `"$sqlFile`"" -ForegroundColor Cyan
            exit 1
        }
    }
    
    # Aplicar migración
    $sqlContent | & $sqlite3Path $dbPath
    
    Write-Host ""
    Write-Host "==================================================" -ForegroundColor Green
    Write-Host "  MIGRACION APLICADA EXITOSAMENTE!" -ForegroundColor Green
    Write-Host "==================================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "La tabla HorariosRestaurante ha sido creada." -ForegroundColor Green
    Write-Host "Se agregaron horarios por defecto a todos los restaurantes." -ForegroundColor Green
    Write-Host ""
    Write-Host "Backup guardado en: $backupPath" -ForegroundColor Cyan
    
} catch {
    Write-Host ""
    Write-Host "ERROR al aplicar la migracion: $_" -ForegroundColor Red
    Write-Host ""
    Write-Host "Restaurando backup..." -ForegroundColor Yellow
    Copy-Item $backupPath $dbPath -Force
    Write-Host "Base de datos restaurada desde el backup." -ForegroundColor Green
    exit 1
}

Write-Host ""
Write-Host "Presiona cualquier tecla para continuar..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
