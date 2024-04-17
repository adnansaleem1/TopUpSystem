using CallCredit.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;


public class CallCreditContext : DbContext
{
    
    public CallCreditContext(DbContextOptions<CallCreditContext> options) : base(options)
    {
    } 
    public DbSet<User> Users { get; set; }
    public DbSet<Beneficiary> Beneficiaries { get; set; }
    public DbSet<TopUpTransaction> TopUpTransactions { get; set; }
    public DbSet<TopUpOption> TopUpOptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Beneficiary>()
            .HasOne(c => c.User)  
            .WithMany(a => a.Beneficiaries)     
            .HasForeignKey(c => c.UserId)  
            .OnDelete(DeleteBehavior.Cascade);  

        
        modelBuilder.Entity<User>().Property(a => a.AccountBalance).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<TopUpTransaction>()
            .HasOne(t => t.Beneficiary)
            .WithMany()
            .HasForeignKey(t => t.BeneficiaryId)
            .OnDelete(DeleteBehavior.Restrict); 

       
        modelBuilder.Entity<TopUpTransaction>()
            .Property(t => t.Amount)
            .HasColumnType("decimal(18,2)"); 
    }
}
