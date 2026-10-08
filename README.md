# OrderFlow

A backend portfolio project built with ASP.NET Core 10 and Clean Architecture.

## Overview

OrderFlow is an intelligent order management system designed to demonstrate modern backend development practices, including clean architecture, REST APIs, event-driven communication, external service integrations, caching, security, and automated testing.

## Technology Stack

- ASP.NET Core 10
- Clean Architecture
- Entity Framework Core 10
- PostgreSQL (Supabase)
- FluentValidation
- xUnit

## Planned Integrations

- Redis for distributed caching
- Apache Kafka for event-driven messaging
- Firebase Realtime Database for live order tracking
- Google Gemini for AI-powered features
- External payment APIs and webhooks
- OpenTelemetry for observability
- Docker and GitHub Actions

## Solution Structure

```text
src/
  OrderFlow.Api/
  OrderFlow.Application/
  OrderFlow.Domain/
  OrderFlow.Infrastructure/

tests/
  OrderFlow.UnitTests/
```

## Getting Started

### Requirements

- .NET 10 SDK
- A PostgreSQL database
- Git

### Run the application

```bash
dotnet restore
dotnet build
dotnet run --project src/OrderFlow.Api
```

Configure `ConnectionStrings:DefaultConnection` using .NET User Secrets or environment variables before running database operations.

### Run tests

```bash
dotnet test
```

## Project Status

Under active development. The current implementation includes product management, PostgreSQL persistence, basic filtering, pagination, validation, and centralized exception handling.

## Architecture

The application follows Clean Architecture principles, separating Domain, Application, Infrastructure, and Presentation responsibilities.