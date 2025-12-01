
Migracion desde el raiz de la aplición:

dotnet ef --verbose migrations add InitialCreate -p src/Arquetipo.Infrastructure/ -s src/Arquetipo.Api


dotnet ef database update -p src/Arquetipo.Infrastructure -s src/Arquetipo.Api



docker run -d   --name postgres -e POSTGRES_PASSWORD=postgres  -e POSTGRES_DB=postgres -p 5432:5432  postgres:16
