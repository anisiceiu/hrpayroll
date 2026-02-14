using HRPayroll.Domain.Entities;
using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Entities.Payroll;
using Microsoft.EntityFrameworkCore;

namespace HRPayroll.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // HR Tables
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Designation> Designations { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<EmployeeShift> EmployeeShifts { get; set; }    
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<Leave> Leaves { get; set; }
    public DbSet<LeaveBalance> LeaveBalances { get; set; }
    public DbSet<Holiday> Holidays { get; set; }
    public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
    public DbSet<DocumentCategory> DocumentCategories { get; set; }
    public DbSet<Recruitment> Recruitments { get; set; }
    public DbSet<Onboarding> Onboardings { get; set; }
    public DbSet<PerformanceAppraisal> PerformanceAppraisals { get; set; }

    // Payroll Tables
    public DbSet<SalaryStructure> SalaryStructures { get; set; }
    public DbSet<SalaryComponent> SalaryComponents { get; set; }
    public DbSet<PayrollRun> PayrollRuns { get; set; }
    public DbSet<PayrollDetail> PayrollDetails { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<LoanRepayment> LoanRepayments { get; set; }
    public DbSet<Bonus> Bonuses { get; set; }
    public DbSet<Overtime> Overtimes { get; set; }
    public DbSet<TaxConfig> TaxConfigs { get; set; }
    public DbSet<TaxSlab> TaxSlabs { get; set; }
    public DbSet<TaxRebate> TaxRebates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Employee Configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => e.EmployeeCode).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasOne(e => e.Department)
                  .WithMany(d => d.Employees)
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Designation)
                  .WithMany()
                  .HasForeignKey(e => e.DesignationId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Shift)
                  .WithMany()
                  .HasForeignKey(e => e.ShiftId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Manager)
                  .WithMany()
                  .HasForeignKey(e => e.ManagerId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Supervisor)
                  .WithMany()
                  .HasForeignKey(e => e.SupervisorId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Documents)
                  .WithOne()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Leaves)
                  .WithOne()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Loans)
                  .WithOne()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Overtimes)
                  .WithOne()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.PerformanceAppraisals)
                  .WithOne()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Department Configuration
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => e.Code).IsUnique();
        });

        // Designation Configuration
        modelBuilder.Entity<Designation>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Shift Configuration
        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Attendance Configuration
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasIndex(e => new { e.EmployeeId, e.Date }).IsUnique();
            entity.HasOne(e => e.Approver)
                  .WithMany()
                  .HasForeignKey(e => e.ApprovedBy)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Leave Configuration
        modelBuilder.Entity<Leave>(entity =>
        {
            entity.HasOne(e => e.LeaveType)
                  .WithMany()
                  .HasForeignKey(e => e.LeaveTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // LeaveBalance Configuration
        modelBuilder.Entity<LeaveBalance>(entity =>
        {
            entity.HasIndex(e => new { e.EmployeeId, e.LeaveTypeId, e.Year }).IsUnique();
        });

        // PayrollRun Configuration
        modelBuilder.Entity<PayrollRun>(entity =>
        {
            entity.HasIndex(e => new { e.Month, e.Year }).IsUnique();
        });

        // PayrollDetail Configuration
        modelBuilder.Entity<PayrollDetail>(entity =>
        {
            entity.HasIndex(e => new { e.PayrollRunId, e.EmployeeId }).IsUnique();
            entity.HasOne(e => e.PayrollRun)
                  .WithMany()
                  .HasForeignKey(e => e.PayrollRunId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Loan Configuration
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasOne(e => e.Employee)
                  .WithMany()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // LoanRepayment Configuration
        modelBuilder.Entity<LoanRepayment>(entity =>
        {
            entity.HasOne(e => e.Loan)
                  .WithMany()
                  .HasForeignKey(e => e.LoanId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Bonus Configuration
        modelBuilder.Entity<Bonus>(entity =>
        {
            entity.HasOne(e => e.Employee)
                  .WithMany()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.PayrollDetail)
                  .WithMany()
                  .HasForeignKey(e => e.PayrollDetailId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Overtime Configuration
        modelBuilder.Entity<Overtime>(entity =>
        {
            entity.HasOne(e => e.Employee)
                  .WithMany()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.ApprovedBy)
                  .WithMany()
                  .HasForeignKey(e => e.ApprovedById)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.PayrollDetail)
                  .WithMany()
                  .HasForeignKey(e => e.PayrollDetailId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // SalaryStructure Configuration
        modelBuilder.Entity<SalaryStructure>(entity =>
        {
            entity.HasOne(e => e.Employee)
                  .WithOne(s => s.SalaryStructure)
                  .HasForeignKey<SalaryStructure>(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Recruitment Configuration
        modelBuilder.Entity<Recruitment>(entity =>
        {
            entity.HasOne(e => e.Department)
                  .WithMany()
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Designation)
                  .WithMany()
                  .HasForeignKey(e => e.DesignationId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Application Configuration
        modelBuilder.Entity<HRPayroll.Domain.Entities.HR.Application>(entity =>
        {
            entity.HasOne(e => e.Recruitment)
                  .WithMany()
                  .HasForeignKey(e => e.RecruitmentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
