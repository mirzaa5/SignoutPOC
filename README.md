# Signout Service POC

This repository is a **proof of concept (POC)** for a **sign-out** flow in a microservice-style setup. It demonstrates how a client can call a single entry point, have JWTs validated at the edge, and revoke a session by writing to a **token blacklist** backed by **Redis**, all without pretending to be production-grade.

## What it illustrates

- **API Gateway as a reverse proxy** — A .NET YARP–based gateway fronts the system, routes traffic to the auth API, and runs JWT checks before forwarding.
- **Containerized services** — The **API Gateway**, **Auth Service**, and **Redis** are built as images and run as containers for consistent, portable deployments.
- **Service orchestration (fundamental)** — **Docker Compose** defines the services, a shared network, and startup order (`depends_on`) so the stack can be started and torn down as one unit for local and demo use.

## Stack (high level)

| Piece | Role |
|--------|------|
| API Gateway | Reverse proxy, JWT validation, route to sign-out |
| Auth Service | Extract token / user, blacklist token in Redis |
| Redis | Token blacklist (e.g. `SETEX` with TTL to token lifetime) |

## Run with Docker Compose

From the repository root:

```bash
docker compose up --build
```

The gateway is exposed (by default) on **port 5000**; adjust in `docker-compose.yml` as needed. Send `POST` requests to the gateway’s sign-out path (per your reverse-proxy configuration) with `Authorization: Bearer <JWT>`.

## Solution layout

- `ApiGateway` — .NET app with YARP reverse proxy and JWT middleware  
- `AuthService` — .NET Web API for sign-out and Redis interaction  
- `docker-compose.yml` — Redis, authservice, and apigateway on a shared bridge network

This POC is for learning and design discussion; hardening, auth standards, and operational practices would come in a real product.
