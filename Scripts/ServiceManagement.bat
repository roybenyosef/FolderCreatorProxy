@echo off
echo.
echo IMPORTANT: You must run this as administrator

IF [%1] == [] GOTO help

echo 1. Install Admin Service
echo 2. Delete Admin Service

set /p userinput=Enter choise:

if "%userinput%"=="1" (goto install)
if "%userinput%"=="2" (goto delete)

echo Error: Invalid choise
goto end

:install
sc create FolderCreatorProxy binpath="%1" start=auto DisplayName="Folder Creator Proxy"
echo IMPORTANT: Set the FolderCreatorProxy service to run as your admin user and then start it
goto end

:delete
sc delete FolderCreatorProxy
goto end

:help
echo.
echo ****************************************************************
echo Usage: ServiceManagement.bat [FolderCreatorProxy.exe full path to install service from]
echo example: ServiceManagement.bat c:\servicefolder\FolderCreatorProxy.exe
echo ****************************************************************
echo.

:end
pause 



