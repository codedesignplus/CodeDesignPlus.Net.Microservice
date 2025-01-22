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

The following instructions will help you set up the project on your local machine for development and testing purposes.

1. Clone the repository:
```bash
git clone <repository-url>
```

2. Run the MongoDB, Redis, and RabbitMQ services using Docker Compose. Clone this repository [CodeDesignPlus.Environment.Dev](https://github.com/codedesignplus/CodeDesignPlus.Environment.Dev) and run the following command:

```bash
cd resources

docker-compose up -d
```

3. Run the script to config the vault:

```bash
cd tools/vault

./config-vault.sh
```

4. Build the solution:
```bash
dotnet build
```

5. Run the desired entry point:
   
   - For REST API:
      ```bash
      dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Rest
      ```

   - For gRPC:
      ```bash
      dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.gRpc
      ```

   - For Worker:
      ```bash
      dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.AsyncWorker
      ```

## 🧪 Testing
To run the unit and integration tests, execute the following command:

```bash
dotnet test
```

## 📦 Update Packages
To update the NuGet packages, run the following script:

```bash
cd tools/update-packages

./update-packages.sh
```

## 📦 Upgrading .NET Version

To upgrade the .NET version, run the following script:

```bash
cd tools/upgrade-dotnet

./upgrade-dotnet.sh
```

## 🧪 SonarQube Analysis

To run the SonarQube analysis, follow the instructions in the sonarqube directory.

1. Replace the SonarQube URL and token in the sonarqube.sh script to analyze with SonarQube locally.
2. Run the script:

```bash
cd tools/sonarqube

./sonarqube.sh
```

🐳 Docker Support

To build and run the application using Docker, follow these steps:

1. Build the Docker image using the Dockerfile in the REST API entry point:
```bash
docker build -t ms-archetype-rest . -f src/entrypoints/CodeDesignPlus.Net.Microservice.Rest/Dockerfile

docker run -d -p 5000:5000 --network=backend -e ASPNETCORE_ENVIRONMENT=Docker --name ms-archetype-rest ms-archetype-rest
```
2. Build the Docker image using the Dockerfile in the gRPC entry point:
```bash
docker build -t ms-archetype-grpc . -f src/entrypoints/CodeDesignPlus.Net.Microservice.gRpc/Dockerfile

docker run -d -p 5001:5001 --network=backend -e ASPNETCORE_ENVIRONMENT=Docker --name ms-archetype-grpc ms-archetype-grpc
```
3. Build the Docker image using the Dockerfile in the Worker entry point:
```bash
docker build -t ms-archetype-worker . -f src/entrypoints/CodeDesignPlus.Net.Microservice.AsyncWorker/Dockerfile

docker run -d -p 5002:5002 --network=backend -e ASPNETCORE_ENVIRONMENT=Docker --name ms-archetype-worker ms-archetype-worker
```
4. Run the Docker container:


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