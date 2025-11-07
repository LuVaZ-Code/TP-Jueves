# Script para corregir encoding UTF-8 en todos los archivos .cshtml

$replacements = @{
    '?' = 'ó'
    '?' = 'á'
    '?' = 'é'
    '?' = 'í'
    '?' = 'ú'
    '?' = 'ñ'
    '?' = 'Á'
    '?' = 'É'
    '?' = 'Í'
    '?' = 'Ó'
    '?' = 'Ú'
    '?' = 'Ñ'
    '?' = '¿'
    '?' = '¡'
    '??' = ''
    '???' = ''
    '????' = ''
    '?????' = ''
    '??????' = ''
    '???????' = ''
    '????????' = ''
    '?????????' = ''
    '??????????' = ''
    '???????????' = ''
}

$files = Get-ChildItem -Path "Pages" -Filter "*.cshtml" -Recurse -File

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw -Encoding UTF8
    $originalContent = $content
    
    foreach ($key in $replacements.Keys) {
        $content = $content -replace [regex]::Escape($key), $replacements[$key]
    }
    
    if ($content -ne $originalContent) {
        Write-Host "Corrigiendo: $($file.FullName)" -ForegroundColor Yellow
        [System.IO.File]::WriteAllText($file.FullName, $content, [System.Text.Encoding]::UTF8)
    }
}

Write-Host "`nCorrección completada!" -ForegroundColor Green
