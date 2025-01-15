dotnet tool install --global dotnet-outdated-tool

$proyectos = Get-ChildItem -Path "./../../" -Recurse -Filter "*.csproj"

foreach ($proyecto in $proyectos) 
{
    Write-Host "Update Package $($proyecto.Name)" -ForegroundColor Green

    dotnet outdated -u $proyecto.FullName 
}