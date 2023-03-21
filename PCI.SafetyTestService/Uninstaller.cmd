echo off
cls
SETLOCAL EnableDelayedExpansion
for /F "tokens=1,2 delims=#" %%a in ('"prompt #$H#$E# & echo on & for %%b in (1) do rem"') do (
  set "DEL=%%a"
)

call :ColorText 0a "                                            Powered by M.Zulfikar Isnaen"
echo(
call :ColorText 0e "                                                Services uninstaller"
echo(
call :ColorText 0b "                                                 Copyright(c) 2023"
echo(
call :ColorText 2F "======================================================================================================================="
echo(
call :ColorText 0a "Current Directory=" && <nul set /p=%~dp0%
echo(
cd %~dp0%
PCI.SafetyTestService.exe uninstall
call :ColorText 0a "                                            Uninstall Finished" && <nul set /p=":)"
echo(
@PAUSE

goto :eof
:ColorText
echo off
<nul set /p ".=%DEL%" > "%~2"
findstr /v /a:%1 /R "^$" "%~2" nul
del "%~2" > nul 2>&1
goto :eof