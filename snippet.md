1. Migration ef database cross
```console
    dotnet ef migrations add InitDb -c ApplicationDbContext -o Migrations/ApplicationDb -p ../Cnc.EntityFramework.PostgreSQL/Cnc.Insfrastructure.csproj
    dotnet ef database update -c ApplicationDbContext -p ../Cnc.EntityFramework.PostgreSQL/Cnc.Insfrastructure.csproj
```