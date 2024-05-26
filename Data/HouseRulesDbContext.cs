using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HouseRules.Models;
using Microsoft.AspNetCore.Identity;


namespace HouseRules.Data;
public class HouseRulesDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IConfiguration _configuration;

    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Chore> Chores { get; set; }
    public DbSet<ChoreAssignment> ChoreAssignments { get; set; }
    public DbSet<ChoreCompletion> ChoreCompletions { get; set; }

    public HouseRulesDbContext(DbContextOptions<HouseRulesDbContext> context, IConfiguration config) : base(context)
    {
        _configuration = config;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
            Name = "Admin",
            NormalizedName = "admin"
        });

        modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
        {
            Id = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
            UserName = "Administrator",
            Email = "admina@strator.comx",
            PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, _configuration["AdminPassword"])
        });

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
            UserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f"
        });
        modelBuilder.Entity<UserProfile>().HasData(new UserProfile
        {
            Id = 1,
            IdentityUserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
            FirstName = "Admina",
            LastName = "Strator",
            Address = "101 Main Street",
        });

        modelBuilder.Entity<Chore>().HasData(
               new Chore { Id = 1, Name = "Feeding the Loch Ness Monster", Difficulty = 5, ChoreFrequencyDays = 1 },
               new Chore { Id = 2, Name = "Prepping the Haggis", Difficulty = 3, ChoreFrequencyDays = 30 },
               new Chore { Id = 3, Name = "Cleaning the bagpipes", Difficulty = 4, ChoreFrequencyDays = 7 },
               new Chore { Id = 4, Name = "Cleaning the loch", Difficulty = 4, ChoreFrequencyDays = 90 },
               new Chore { Id = 5, Name = "Making noodles", Difficulty = 2, ChoreFrequencyDays = 1 }
           );

        // Seed ChoreAssignments
        modelBuilder.Entity<ChoreAssignment>().HasData(
            new ChoreAssignment { Id = 1, UserProfileId = 1, ChoreId = 1 },
            new ChoreAssignment { Id = 2, UserProfileId = 1, ChoreId = 3 }
        );

        // Seed ChoreCompletions
        modelBuilder.Entity<ChoreCompletion>().HasData(
            new ChoreCompletion { Id = 1, UserProfileId = 1, ChoreId = 1, CompletedOn = DateTime.Now }
        );
    }

}