$filesToDelete = @(
    'src/domain/CodeDesignPlus.Net.Microservice.Application/Order',
    'src/domain/CodeDesignPlus.Net.Microservice.Domain/OrderAggregate.cs',
    'src/domain/CodeDesignPlus.Net.Microservice.Domain/DataTransferObjects',
    'src/domain/CodeDesignPlus.Net.Microservice.Domain/DomainEvents',
    'src/domain/CodeDesignPlus.Net.Microservice.Domain/Entities',
    'src/domain/CodeDesignPlus.Net.Microservice.Domain/Enums',
    'src/domain/CodeDesignPlus.Net.Microservice.Domain/Repositories',
    'src/domain/CodeDesignPlus.Net.Microservice.Infrastructure/Repositories',
    'src/entrypoints/CodeDesignPlus.Net.Microservice.AsyncWorker/Consumers',
    'src/entrypoints/CodeDesignPlus.Net.Microservice.gRpc/Protos',
    'src/entrypoints/CodeDesignPlus.Net.Microservice.gRpc/Services',
    'src/entrypoints/CodeDesignPlus.Net.Microservice.Rest/Controllers',
    'tests/integration/CodeDesignPlus.Net.Microservice.AsyncWorker.Test/Consumers',
    'tests/integration/CodeDesignPlus.Net.Microservice.gRpc.Test/Protos',
    'tests/integration/CodeDesignPlus.Net.Microservice.gRpc.Test/Services',
    'tests/integration/CodeDesignPlus.Net.Microservice.Rest.Test/Controllers',
    'tests/unit/CodeDesignPlus.Net.Microservice.Application.Test/Order',
    'tests/unit/CodeDesignPlus.Net.Microservice.Domain.Test/DomainEvents',
    'tests/unit/CodeDesignPlus.Net.Microservice.Domain.Test/OrderAggregateTest.cs',
    'tests/unit/CodeDesignPlus.Net.Microservice.Infrastructure.Test/Repositories',
    'tests/unit/CodeDesignPlus.Net.Microservice.AsyncWorker.Test/Consumers',
    'tests/unit/CodeDesignPlus.Net.Microservice.gRpc.Test/Services',
    'tests/unit/CodeDesignPlus.Net.Microservice.Rest.Test/Controllers'
)

foreach ($file in $filesToDelete) {
    Remove-Item -Path $file -Recurse -Force -ErrorAction SilentlyContinue
    if ($?) {
        Write-Host "Deleted $file"
    } else {
        Write-Host "Failed to delete $file"
    }
}

# Clean Usings

# Obtener la ruta al directorio actual
$rootPath = Get-Location

# Buscar todos los archivos Usings.cs
$usingsFiles = Get-ChildItem -Path $rootPath -Recurse -Filter "Usings.cs"

# Mostrar la ruta de cada archivo encontrado
$usingsFiles | ForEach-Object {
    Write-Host $_.FullName
}

# Patrón para buscar y eliminar las líneas de using específicas
$patterns = @(
    'global using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;',
    'global using CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById;',
    'global using CodeDesignPlus.Net.Microservice.Domain;',
    'global using CodeDesignPlus.Net.Microservice.Domain.Entities;',
    'global using CodeDesignPlus.Net.Microservice.Domain.Repositories;',
    'global using CodeDesignPlus.Net.Microservice.Domain.Enums;',
    'global using CodeDesignPlus.Net.Microservice.Domain.DataTransferObjects;',
    'global using CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder;',
    'global using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CancelOrder;',
    'global using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder;',
    'global using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;',
    'global using CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct;',
    'global using CodeDesignPlus.Net.Microservice.Application.Order.Commands.UpdateQuantityProduct;',
    'global using CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById;',
    'global using CodeDesignPlus.Net.Microservice.Application.Order.Queries.GetAllOrders;',
    'global using CodeDesignPlus.Microservice.Api.Dtos;',
    'global using CodeDesignPlus.Net.Microservice.Domain.DomainEvents;',
    'global using CodeDesignPlus.Net.Microservice.Infrastructure.Repositories;',
    'global using CodeDesignPlus.Net.Microservice.Rest.Controllers;',
    'global using CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;',
    'global using CodeDesignPlus.Net.Microservice.gRpc.Services;'
)

foreach ($file in $usingsFiles) {
    # Leer el contenido del archivo
    $fileContent = Get-Content -Path $file.FullName -Raw
    
    foreach ($pattern in $patterns) {
        # Remover las líneas de using específicas
        $fileContent = $fileContent -replace [regex]::Escape($pattern), ''
    }

    # Escribir el contenido modificado de nuevo en el archivo
    $fileContent | Set-Content -Path $file.FullName

    Write-Host "Usings eliminados exitosamente en: $($file.FullName)"
}

# Remove reference proto

# Buscar todos los archivos .csproj
$csprojFiles = Get-ChildItem -Path $rootPath -Recurse -Filter "*.csproj"

# Patrón para buscar y eliminar el contenido específico, incluyendo saltos de línea
$pattern = '<Protobuf Include="Protos\\orders.proto" GrpcServices="(Server|Client)" />'


foreach ($file in $csprojFiles) {
    # Leer el contenido del archivo
    $fileContent = Get-Content -Path $file.FullName -Raw

    # Remover el contenido específico
    $fileContent = $fileContent -replace $pattern, ''

    # Escribir el contenido modificado de nuevo en el archivo
    $fileContent | Set-Content -Path $file.FullName

    Write-Host "Contenido eliminado exitosamente en: $($file.FullName)"
}

# Clean mapster config

# Buscar todos los archivos MapsterConfig.cs
$mapsterConfigFiles = Get-ChildItem -Path $rootPath -Recurse -Filter "MapsterConfig.cs"

foreach ($file in $mapsterConfigFiles) {
    # Leer el contenido del archivo
    $fileContent = Get-Content -Path $file.FullName -Raw
    
    # Reemplazar el contenido del método Configure
    $fileContent = $fileContent -replace 'public static void Configure\(\)\s*\{[^}]*\}', 'public static void Configure() { }'

    # Escribir el contenido modificado de nuevo en el archivo
    $fileContent | Set-Content -Path $file.FullName

    Write-Host "Método Configure vaciado en: $($file.FullName)"
}

# Clean Errors

# Buscar todos los archivos Errors.cs
$errorFiles = Get-ChildItem -Path $rootPath -Recurse -Filter "Errors.cs"

foreach ($file in $errorFiles) {
    # Leer el contenido del archivo
    $fileContent = Get-Content -Path $file.FullName -Raw

    # Reemplazar el contenido de la clase Errors para eliminar las constantes
    $fileContent = $fileContent -replace 'public class Errors: IErrorCodes\s*\{[^}]*\}', 'public class Errors: IErrorCodes { }'

    # Escribir el contenido modificado de nuevo en el archivo
    $fileContent | Set-Content -Path $file.FullName

    Write-Host "Constantes de error eliminadas en: $($file.FullName)"
}

