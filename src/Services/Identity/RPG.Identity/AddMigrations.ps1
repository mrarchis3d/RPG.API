dotnet ef migrations add InitialIdentity --context UserIdentityDbContext
dotnet ef migrations add InitialConfiguration --context ConfigurationDbContext
dotnet ef migrations add InitialPersistenGrant --context PersistedGrantDbContext
dotnet ef database update --context PersistedGrantDbContext
dotnet ef database update --context ConfigurationDbContext
dotnet ef database update --context UserIdentityDbContext