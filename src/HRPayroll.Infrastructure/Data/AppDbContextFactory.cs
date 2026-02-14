using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HRPayroll.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Server=.;Database=HRPayrollDb;User Id=sa;Password=Sa123;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=true");
        return new AppDbContext(optionsBuilder.Options);
    }
}
