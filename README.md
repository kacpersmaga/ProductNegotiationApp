# ProductNegotiationApp

Welcome to **ProductNegotiationApp**, a .NET 9 Web API application designed to facilitate a price negotiation process for products in an online store. This application allows customers to propose prices for products, while store employees can accept or reject these proposals. Built as a modular monolith, it leverages MSSQL for data persistence and incorporates modern practices like API response wrapping, Serilog for logging, and a custom exception-handling middleware for robust error management.

## Features

- **Product Module**: Supports adding, retrieving, and listing products.
- **Negotiation Module**: Implements a price negotiation process with the following rules:
  - Customers (unauthenticated users) can propose a price for a product (up to 3 attempts).
  - Store employees (authenticated users) can accept or reject proposed prices.
  - Customers have 7 days to propose a new price after a rejection; otherwise, the negotiation is canceled.
- **RESTful API**: Adheres to REST standards for all endpoints.
- **Request Validation**: Ensures inputs like product names are not empty and proposed prices are greater than zero.
- **Authentication**: Employees must log in using JWT-based authentication; customers do not require authentication.
- **MSSQL Database**: Persistent storage for products, negotiations, and user data.
- **API Response Wrapper**: Uses `ApiResponse<T>` for consistent API responses.
- **Serilog**: Structured logging for better observability.
- **Exception Handling**: Centralized middleware for consistent error handling.
- **API Documentation**: Comprehensive Swagger documentation available at `/swagger`.
- **Unit Tests**: Extensive unit tests for all modules (integration tests coming soon).
- **Version Control**: Managed with Git for reliable source control.

## Prerequisites

- **Docker**: Ensure Docker and Docker Compose are installed.
- **.NET SDK 9.0**: Required for running tests locally.
- **Git**: For cloning the repository.

## Getting Started

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/kacpersmaga/ProductNegotiationApp.git
   cd ProductNegotiationApp
   ```

2. **Run the Application**:
   Use Docker Compose to start the application and MSSQL database:
   ```bash
   docker-compose up --build
   ```
   The API will be available at `http://localhost:7000`. Access Swagger documentation at `http://localhost:7000/swagger`.

3. **Run Unit Tests**:
   To execute the unit tests, use:
   ```bash
   dotnet test
   ```

## Project Structure

The application follows a **modular monolith** architecture, with clear separation of concerns:

- **Modules**: Core business logic, divided into:
  - `Products`: Handles product-related operations.
  - `Negotiations`: Manages the price negotiation process.
  - `Identity`: Manages employee authentication.
- **Shared**: Common utilities, middleware (`ExceptionHandlingMiddleware`), and services (e.g., `ApiResponse<T>`, `DateTimeService`).
- **Tests**: Unit tests for all modules (integration tests in development).
- **WebApi**: The main entry point for the API, configured with Serilog and Swagger.

## Best Practices

The negotiation module is designed for future extensibility with the following practices:
- **Modular Monolith**: Organizes code into independent modules for scalability and maintainability.
- **Clean Architecture**: Separates concerns into Domain, Application, Infrastructure, and API layers.
- **CQRS Pattern**: Uses MediatR for Commands and Queries to separate write and read operations.
- **Dependency Injection**: Ensures loose coupling and testability.
- **Fluent Validation**: Validates all incoming requests.
- **Domain-Driven Design**: Models the business domain with entities and enums.
- **Serilog Logging**: Structured logging for debugging and monitoring.
- **Exception Handling Middleware**: Centralized error handling for consistent API responses.
- **Unit Testing**: Comprehensive unit tests using xUnit and Moq.

## API Endpoints

### Products
- `GET /api/products`: Retrieve all products.
- `GET /api/products/{id}`: Retrieve a specific product by ID.
- `POST /api/products`: Create a new product (employee-only, requires JWT).

### Negotiations
- `POST /api/negotiations`: Start a new negotiation (customer).
- `GET /api/negotiations/{id}`: Retrieve negotiation details.
- `POST /api/negotiations/{id}/propose`: Propose a new price (customer).
- `POST /api/negotiations/{id}/accept`: Accept a proposed price (employee-only, requires JWT).
- `POST /api/negotiations/{id}/reject`: Reject a proposed price (employee-only, requires JWT).

### Authentication
- `POST /api/auth/register`: Register a new employee.
- `POST /api/auth/login`: Log in to obtain a JWT token.

For detailed request/response formats, refer to the Swagger documentation at `http://localhost:7000/swagger`.

## Technical Details

- **Database**: MSSQL Server, configured via Entity Framework Core with migrations for each module.
- **Authentication**: JWT-based authentication for employee endpoints, configured in the `Identity` module.
- **Logging**: Serilog with structured logging to console and file (configurable in `appsettings.json`).
- **Response Wrapper**: `ApiResponse<T>` ensures consistent success/error responses across all endpoints.
- **Exception Handling**: Custom `ExceptionHandlingMiddleware` catches and formats errors for client-friendly responses.
- **Docker**: The application and MSSQL database are containerized, with configuration in `docker-compose.yml`.

## Future Work

- **Integration Tests**: Planned to ensure end-to-end functionality.
- **CI/CD for Tests**: Implement continuous integration and deployment pipelines to automate testing.

## Author

Kacper Smaga  
Email: kacper.smaga@onet.pl  
GitHub: [kacpersmaga](https://github.com/kacpersmaga)
