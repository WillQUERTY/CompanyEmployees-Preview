#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Script para ejecutar/desplegar la versión preview

.EXAMPLE
    .\run.ps1
    .\run.ps1 -Build
#>

param([switch]$Build)

$ErrorActionPreference = "Stop"

Write-Host "
╔════════════════════════════════════════════════════════════╗
║  CompanyEmployees Preview - CQRS Architecture Demo       ║
╚════════════════════════════════════════════════════════════╝
" -ForegroundColor Cyan

Write-Host "📦 Iniciando Docker Compose..." -ForegroundColor Green

if ($Build) {
    Write-Host "🔨 Build con --no-cache..." -ForegroundColor Yellow
    docker compose -f docker-compose.yml build --no-cache
}

docker compose -f docker-compose.yml up -d

Start-Sleep -Seconds 5

Write-Host "`n✅ Servicios corriendo:" -ForegroundColor Green
docker compose ps

Write-Host "`n╔════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║  📍 API:     http://localhost:5000                    ║" -ForegroundColor Cyan
Write-Host "║  📚 Swagger: http://localhost:5000/swagger            ║" -ForegroundColor Cyan
Write-Host "║  💾 SQL:     localhost:1433 (sa/Strong@Password123)  ║" -ForegroundColor Cyan
Write-Host "║                                                        ║" -ForegroundColor Cyan
Write-Host "║  ⏹️  Para detener: docker compose down               ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
