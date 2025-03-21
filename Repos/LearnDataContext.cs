using System;
using System.Collections.Generic;
using System.Reflection;
using JwtUser.Repos.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtUser.Repos;

public partial class LearnDataContext : DbContext
{
    public LearnDataContext()
    {
    }

    public LearnDataContext(DbContextOptions<LearnDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblImage> TblImages { get; set; }

    public virtual DbSet<TblProduct> TblProducts { get; set; }

    public virtual DbSet<TblRefreshtoken> TblRefreshtokens { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
