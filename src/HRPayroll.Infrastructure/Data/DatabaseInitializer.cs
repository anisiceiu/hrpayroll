using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HRPayroll.Domain.Entities.HR;

namespace HRPayroll.Infrastructure.Data;

public class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed default document categories if none exist
        if (!await context.DocumentCategories.AnyAsync())
        {
            var defaultCategories = new List<DocumentCategory>
            {
                new() 
                { 
                    Name = "Identity Documents", 
                    Code = "IDENTITY", 
                    NameBN = "পরিচয়পত্র",
                    Description = "National ID, Passport, Birth Certificate, etc.",
                    DisplayOrder = 1,
                    HasExpiryDate = true,
                    MaxFileSizeMB = 10,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new() 
                { 
                    Name = "Educational Certificates", 
                    Code = "EDUCATION", 
                    NameBN = "শিক্ষাগত সনদ",
                    Description = "Degrees, Transcripts, Certifications",
                    DisplayOrder = 2,
                    HasExpiryDate = false,
                    MaxFileSizeMB = 10,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new() 
                { 
                    Name = "Professional Documents", 
                    Code = "PROFESSIONAL", 
                    NameBN = "পেশাদার নথি",
                    Description = "Experience Letters, Service Books, Training Certificates",
                    DisplayOrder = 3,
                    HasExpiryDate = true,
                    MaxFileSizeMB = 10,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new() 
                { 
                    Name = "Legal Documents", 
                    Code = "LEGAL", 
                    NameBN = "আইনি নথি",
                    Description = "Contracts, Agreements, Legal Notices",
                    DisplayOrder = 4,
                    HasExpiryDate = true,
                    MaxFileSizeMB = 10,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new() 
                { 
                    Name = "Other Documents", 
                    Code = "OTHERS", 
                    NameBN = "অন্যান্য নথি",
                    Description = "Other relevant documents",
                    DisplayOrder = 5,
                    HasExpiryDate = false,
                    MaxFileSizeMB = 10,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                }
            };

            context.DocumentCategories.AddRange(defaultCategories);
            await context.SaveChangesAsync();
        }
    }
}
