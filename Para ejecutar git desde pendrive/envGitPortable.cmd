@echo off
echo.
for %%a in ("%cd%") do set unidad=%%~da
echo Activando utilizar %unidad%\git\.gitconfig como fichero de configuración Global(para todos los repositorios de este usuario) y así poder llevarlo en un pendrive en diferentes equipos.
echo Activado.
echo.
echo Escriba exit para volver al .gitconfig por defecto del usuario actual que debería estar en
echo %userprofile%\.gitconfig.
echo.
set homepathprevious=%homepath%
set userprofileprevious=%userprofile%
set pathprevious=%path%

set homepath=%unidad%\git
set userprofile=%unidad%\git
set path=%path%;%unidad%\git\PortableGit\cmd
cmd
set homepath=%homepathprevious%
set userprofile=%userprofileprevious%
set path=%pathprevious%