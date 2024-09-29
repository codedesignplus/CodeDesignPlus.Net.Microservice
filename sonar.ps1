#!/usr/bin/env pwsh
#echo "Install dotnet-sonarscanner ----------------------------------------------------------------------------------------------------------------------"
#dotnet tool install --global dotnet-sonarscanner 

$env:SONAR_TOKEN="82588378259add8808c44526a291124d47ab2a74"

Write-Host "Start Sonarscanner -------------------------------------------------------------------------------------------------------------------------------"

$org = "codedesignplus"
$key = "CodeDesignPlus.Net.Microservice"
$csproj = "CodeDesignPlus.Net.Microservice.sln"
$report = "tests/**/coverage.opencover.xml"
$server = "https://sonarcloud.io"

dotnet test $csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
dotnet sonarscanner begin /o:$org /k:$key /d:sonar.host.url=$server /d:sonar.coverage.exclusions="**Tests*.cs" /d:sonar.cs.opencover.reportsPaths=$report
dotnet build
dotnet sonarscanner end