using HelpDeskSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<AuditTrail> AuditTrails { get; set; }
    public DbSet<TicketCategory> TicketCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure Comment entity
        builder.Entity<Comment>()
            .HasOne(c => c.CreatedBy)
            .WithMany()
            .HasForeignKey(c => c.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Comment>()
            .HasOne(c => c.Ticket)
            .WithMany()
            .HasForeignKey(c => c.TicketId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure TicketCategory entity
        builder.Entity<TicketCategory>()
            .HasOne(tc => tc.ModifiedBy)
            .WithMany()
            .HasForeignKey(tc => tc.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TicketCategory>()
            .HasOne(tc => tc.CreatedBy)
            .WithMany()
            .HasForeignKey(tc => tc.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Ticket entity
        builder.Entity<Ticket>()
            .HasOne(t => t.CreatedBy)
            .WithMany()
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure AuditTrail entity
        builder.Entity<AuditTrail>()
            .HasOne(at => at.User)
            .WithMany()
            .HasForeignKey(at => at.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Additional configurations can be added here if needed
    }
}
