# Real-Time Task Management Platform

This repository contains a production-ready architecture for a real-time collaborative task management platform built with ASP.NET Core and Angular.

## Backend

* **Architecture:** Clean Architecture, CQRS with MediatR, FluentValidation, Domain-driven design.
* **Projects:**
  * `TaskManagement.Domain` – Entities, value objects, domain events.
  * `TaskManagement.Application` – Commands, queries, DTOs, pipeline behaviors, AutoMapper profiles.
  * `TaskManagement.Infrastructure` – EF Core DbContext, configurations, services, dependency injection.
  * `TaskManagement.SignalRHubs` – SignalR hubs and extension methods for real-time communication.
  * `TaskManagement.API` – REST API, JWT authentication setup, controllers, and startup configuration.

## Frontend

* **Framework:** Angular 18 standalone components with TailwindCSS styling.
* **Structure:** Core services (authentication, SignalR, notifications, presence), feature modules for dashboard, projects, tasks, and authentication flows.
* **Real-time:** Client-side SignalR service with providers for notifications, presence, and task updates.

## Getting Started

1. Update the connection string and JWT settings in `backend/src/TaskManagement.API/appsettings.json`.
2. Restore and build the .NET solution (requires .NET 8 SDK or later).
3. Install Node.js 20+ and run `npm install` inside the `frontend` directory.
4. Start the backend API and Angular dev server (`npm start`).

## Security & Observability

* JWT authentication with refresh tokens and SignalR token forwarding.
* Configurable CORS policy with secure defaults.
* Logging and performance pipeline behaviors.
* Presence tracking, notification broadcasting, and optimistic UI scaffolding.
