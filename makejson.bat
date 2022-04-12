@ECHO OFF

.\FFXIVWeatherResourceGenerator\bin\Debug\net6.0\FFXIVWeatherResourceGenerator.exe

COPY .\FFXIVWeather\Data\*.json ..\ffxivweather-py\ffxivweather\store

git add .

git commit -m "JSON store update"

git push origin master

cd ..\ffxivweather-py

git add .

git commit -m "JSON store update"

git push origin master

ECHO Done!

PAUSE
