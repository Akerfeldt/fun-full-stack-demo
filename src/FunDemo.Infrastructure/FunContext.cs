using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FunDemo.Infrastructure;

public class FunContext(DbContextOptions<FunContext> options) : DbContext(options)
{
    public DbSet<DbPlayer> Players { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbPlayer>().ToTable("Player");
    }
}

public class DbPlayer
{
    [Column(TypeName = "int")]
    public int Id { get; set; }

    [Column(TypeName = "tinyint")]
    public required byte Class { get; set; }

    [Column(TypeName = "int")]
    public required int LocationX { get; set; }

    [Column(TypeName = "int")]
    public required int LocationY { get; set; }

    [Column(TypeName = "varchar(10)")]
    public required string Name { get; set; }

    [Column(TypeName = "tinyint")]
    public required byte Race { get; set; }

    [Column(TypeName = "varchar(30)")]
    public required string UserId { get; set; }
}