call quasar build -m pwa -d
del /q "..\Napos\wwwroot\*"
FOR /D %%p IN ("..\Napos\wwwroot\*.*") DO rmdir "%%p" /s /q
Xcopy "dist\pwa" "..\Napos\wwwroot" /E /H /C /I
PAUSE
