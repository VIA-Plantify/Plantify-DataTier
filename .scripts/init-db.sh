#!/usr/bin/env bash
set -Eeuo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "${SCRIPT_DIR}/.." && pwd)"
COMPOSE_FILE="${PROJECT_ROOT}/docker-compose.prod.yml"
PROJECT_NAME="plantify_production"
SERVICE="postgres"
DB_NAME="plantify"
DB_USER="dev"

cd "${PROJECT_ROOT}"

echo "Starting ${SERVICE}..."
docker compose -p "${PROJECT_NAME}" -f "${COMPOSE_FILE}" up -d "${SERVICE}"

echo "Waiting for PostgreSQL to become ready..."
until docker compose -p "${PROJECT_NAME}" -f "${COMPOSE_FILE}" exec -T "${SERVICE}" \
  pg_isready -U "${DB_USER}" -d "${DB_NAME}" >/dev/null 2>&1; do
  sleep 1
done

echo "Checking whether database '${DB_NAME}' exists..."
DB_EXISTS="$(
  docker compose -p "${PROJECT_NAME}" -f "${COMPOSE_FILE}" exec -T "${SERVICE}" \
    psql -U "${DB_USER}" -d postgres -tAc "SELECT 1 FROM pg_database WHERE datname='${DB_NAME}';" \
    | tr -d '[:space:]'
)"

if [ "${DB_EXISTS}" = "1" ]; then
  echo "Database '${DB_NAME}' exists."
else
  echo "Database '${DB_NAME}' was not found."
  exit 1
fi

echo "PostgreSQL setup is ready."

echo "Restoring .NET dependencies..."

dotnet restore ./Plantify-DataTier.sln

echo "Applying EF Core migrations..."

dotnet ef database update \
  --project ./EFC/EFC.csproj \
  --startup-project ./GrpcService/GrpcService.csproj

echo "Migrations applied successfully."