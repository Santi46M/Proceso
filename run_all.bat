@echo off
echo ==============================
echo Iniciando Proceso System Monitor
echo ==============================

echo.
echo [1] Iniciando Backend (.NET)...
start cmd /k "cd api && dotnet run --urls http://0.0.0.0:5180"

timeout /t 4 >nul

echo.
echo [2] Iniciando Frontend (React)...
start cmd /k "cd dashboard && npm run dev -- --host"

timeout /t 5 >nul

echo.
echo [3] Abriendo navegador...

start http://localhost:5173

echo.
echo ==============================
echo Sistema listo 🚀
echo ==============================

pause