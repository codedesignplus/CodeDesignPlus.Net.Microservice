#!/usr/bin/env pwsh
#echo "Install dotnet-sonarscanner ----------------------------------------------------------------------------------------------------------------------"
#dotnet tool install --global dotnet-sonarscanner 

Write-Host "Start Sonarscanner -------------------------------------------------------------------------------------------------------------------------------"

$org = "codedesignplus"
$key = "CodeDesignPlus.Net.Microservice"
$csproj = "CodeDesignPlus.Net.Microservice.sln"
$report = "tests/**/coverage.opencover.xml"
$server = "http://localhost:9000"
$token = "sqa_12f3d20d51de2b4c9639db0035d1c68dc4f2fff1"

cd ..
dotnet test $csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
dotnet sonarscanner begin /o:$org /k:$key /d:sonar.host.url=$server /d:sonar.coverage.exclusions="**Tests*.cs,**/tests/load/*.js" /d:sonar.cs.opencover.reportsPaths=$report /d:sonar.login=$token
dotnet build
dotnet sonarscanner end /d:sonar.login=$token