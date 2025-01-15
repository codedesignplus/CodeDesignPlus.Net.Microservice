#!/bin/bash

dotnet tool install -g upgrade-assistant

proyectos=$(find ./../../ -name "*.csproj")

for proyecto in $proyectos
do
    echo -e "\e[32mUpdate $proyecto\e[0m"
    upgrade-assistant upgrade --operation Inplace --targetFramework net9.0  --non-interactive $proyecto
done