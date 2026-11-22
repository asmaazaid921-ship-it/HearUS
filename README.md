# HearUS — Deaf Community, Learning & Job Platform

A full-stack / back-end platform built solo from scratch using **ASP.NET Core**, **Clean Architecture**, **CQRS with MediatR**, and **SignalR**, designed for the deaf community under the principle *"Nothing About Us Without Us"* to learn, explore jobs, share lived experiences, and connect in real time.

## Architecture & Tech Stack
- **Architecture:** Clean Architecture (`Domain`, `Application`, `Infrastructure`, `API`), CQRS (`MediatR`), SOLID Principles, Repository Pattern
- **Real-Time Communication:** SignalR
- **Security & Access Control:** Custom Authorization policies (including restricted lived-experience sharing for eligible community members)
- **Data & Framework:** .NET / ASP.NET Core, Entity Framework Core, SQL Server

## Key Features
- **Lived-Experience Space with Custom Authorization:** A dedicated space where community members share daily challenges and lived experiences, protected by custom authorization so only authentic/eligible experience owners can publish there.
- **Real-Time Messaging & Connection:** Instant chat and peer connection powered by SignalR with low latency.
- **Learning & Job Discovery:** Core workflows supporting accessible learning and job opportunities tailored for the deaf community.
- **Modular, Maintainable Back-End:** Separation of read/write responsibilities via CQRS and MediatR, keeping controllers clean and features independent.

## linkedIn
https://lnkd.in/p/eP4WhfpH
