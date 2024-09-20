using Microsoft.Extensions.Configuration;
using src.Infrastructure.Db;
using System;
using System.IO;

public class DbContextFixture : IDisposable
{
    public MyDbContext DbContext { get; private set; }

    public DbContextFixture()
    {
        DbContext = CriarContextoDeTeste();
    }

    private MyDbContext CriarContextoDeTeste()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Testing.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        var configuration = builder.Build();

        return new MyDbContext(configuration);
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}
