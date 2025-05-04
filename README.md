# ingredients-tracker-api
Ingredients Tracker API is a backend service that powers Ingredients Tracker system, focusing on inventory management and recipe processing for both home and professional kitchens. It interfaces with IoT devices and utilizes machine learning for product recognition, providing a foundation for tracking and managing kitchen inventories and recipes.

## Table of Contents
- [Features](#features)
- [Stack](#stack)
- [Installation](#installation)
  - [Prerequisites](#prerequisites)
  - [Setup Instructions](#setup-instructions)
- [Configuration](#configuration)

## Features
- User authentication, registration, and role management.
- Group management with user membership and roles.
- CRUD operations for devices, devices status management, and integration with Azure IoT Hub.
- Product (ingredient) management including create, update, and paginated search.
- Recipe management with support for thumbnails, ingredients, categories, and paginated retrieval.
- Category management with search and pagination.
- Image recognition using Azure Cognitive Services to extract tags from images.
- Integration with IoT devices for automatic product recognition.
- Upload and store images efficiently with cloud object storage (AWS S3 compatible).
- Pagination support for users, roles, products, recipes, categories, and devices.
- Middleware for global exception handling and user context injection.
- Azure App Service deployment CI/CD pipeline via GitHub Actions.
- Comprehensive integration tests covering API endpoints and operations.
- Devcontainer support for streamlined development environment setup.

## Stack
- Language: C# (.NET 8.0)
- Database: MongoDB
- Services: 
   - Azure Cognitive Services - Computer Vision
   - AWS S3 compatible Cloud Object Storage SDK

## Installation

### Prerequisites
- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [MongoDB](https://www.mongodb.com/try/download/community) instance running or connection string to managed service
- Azure Cognitive Services Computer Vision API key and endpoint
- AWS S3-compatible cloud storage account and credentials (or equivalent)

### Setup Instructions

Clone the repository:
```bash
git clone https://github.com/Shchoholiev/ingredients-tracker-api.git
cd ingredients-tracker-api
```

Restore dependencies and build solution:
```bash
dotnet restore IngredientsTrackerApi.sln
dotnet build IngredientsTrackerApi.sln
```

Run the API locally (for example in RestApi project):
```bash
dotnet run --project IngredientsTrackerApi.RestApi/IngredientsTrackerApi.RestApi.csproj
```

Or use the provided devcontainer for consistent dev environment:
- Open in VS Code and reopen in container (requires Docker installed)
- Extensions and .NET SDK configured automatically

To run tests:
```bash
dotnet test IngredientsTrackerApi.IntegrationTests/IngredientsTrackerApi.IntegrationTests.csproj
```

## Configuration

The application requires the following environment variables or appsettings configuration:

### Azure Cognitive Services
- `AzureCognitiveServices:ComputerVision:Key` — Your Azure Computer Vision API key.
- `AzureCognitiveServices:ComputerVision:Endpoint` — The endpoint URL for the Computer Vision API.

### Cloud Object Storage (AWS S3 compatible)
- `CloudObjectStorage:BucketName` — The target bucket name for storing images.
- `CloudObjectStorage:StorageEndpoint` — The S3-compatible storage service endpoint URL.
- `CloudObjectStorage:AccessKey` — Access key for cloud object storage.
- `CloudObjectStorage:SecretKey` — Secret key for cloud object storage.
- `CloudObjectStorage:ObjectUrl` — Base URL to access uploaded objects publicly.

### JWT Authentication
- Configuration for JWT tokens is expected in the application configuration (e.g., `appsettings.json`) and used internally for securing the API.

Make sure to configure these settings before running the application to enable all features such as authentication, image recognition, and cloud storage.
