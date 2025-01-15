#!/bin/bash

dotnet tool install --global dotnet-outdated-tool

proyectos=$(find ./../../ -name "*.csproj")

for proyecto in $proyectos
do
    echo -e "\e[32mUpdate Package $(basename $proyecto)\e[0m"
    dotnet outdated -u $proyecto
done