# CodeDesignPlus .NET 9 Microservice Template

A production-ready microservice template built with .NET 9, implementing Clean Architecture, DDD, and CQRS patterns. This template provides a robust foundation for building scalable and maintainable microservices.

## 🚀 Key Features

- Clean Architecture implementation
- Domain-Driven Design (DDD) principles
- Command Query Responsibility Segregation (CQRS)
- Multiple entry points (REST API, gRPC, Worker)
- Distributed caching with Redis
- Message broker integration with RabbitMQ
- MongoDB database integration
- OAuth2/OpenID Connect security
- Swagger API documentation
- Application monitoring and tracing
- Docker containerization
- Unit and integration testing
- Load testing scripts

## 🛠️ Technology Stack

- .NET 9
- MongoDB
- Redis
- RabbitMQ
- Vault
- Swagger/OpenAPI
- gRPC
- MediatR
- FluentValidation
- Mapster
- Docker

## ⚙️ Prerequisites

- .NET 9 SDK
- Docker and Docker Compose
- MongoDB
- Redis
- RabbitMQ
- Vault (optional)

## 🚀 Getting Started

1. Clone the repository:
```bash
git clone <repository-url>
```

2. Configure environment settings in `appsettings.json`:

```json
{
  "Core": {
    "AppName": "your-service-name",
    "Version": "v1"
  },
  "MongoDb": {
    "ConnectionString": "mongodb://localhost:27017",
    "Database": "your-database"
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Port": 5672
  }
}
```

3. Build the solution:
```bash
dotnet build
```

4. Run the desired entry point:
For REST API:
```bash
dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Rest
```

For gRPC:
```bash
dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.gRpc
```

For Worker:
```bash
dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.AsyncWorker
```

## 🧪 Testing
Run unit tests:
```bash
dotnet test tests/CodeDesignPlus.Net.Microservice.Application.Test
```

Run integration tests:
```bash
dotnet test tests/integration
```

Run load tests:
```bash
cd tests/load
k6 run load-test.js
```

🐳 Docker Support
Build the Docker image:
```bash
docker build -t codedesignplus-microservice .
```

Run with Docker Compose:

```bash
docker-compose up -d
```

## 📚 Documentation
API documentation available at `/swagger`
gRPC service definitions in Protos

## 🤝 Contributing
Please read our Contributing Guide for details on our code of conduct and development process.

## 📄 License
This project is licensed under the MIT License - see the LICENSE.md file for details.

## 🔧 Tools
The repository includes several utility scripts in the tools directory:

- `convert-crlf-to-lf.sh`: Converts line endings
- `update-packages/`: Updates NuGet packages
- `upgrade-dotnet/`: Upgrades .NET version
- `vault/`: Vault configuration scripts
- `sonarqube/`: SonarQube analysis configuration

## 📦 CodeDesignPlus Packages
This template uses the following CodeDesignPlus NuGet packages:

- CodeDesignPlus.Net.Core
- CodeDesignPlus.Net.Redis
- CodeDesignPlus.Net.RabbitMQ
- CodeDesignPlus.Net.Logger
- CodeDesignPlus.Net.Vault