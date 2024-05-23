using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HouseRules.Data;
using HouseRules.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HouseRules.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private HouseRulesDbContext _dbContext;

    public UserProfileController(HouseRulesDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        return Ok(_dbContext
            .UserProfiles
            .Include(up => up.IdentityUser)
            .Select(up => new UserProfileDTO
            {
                Id = up.Id,
                FirstName = up.FirstName,
                LastName = up.LastName,
                Address = up.Address,
                IdentityUserId = up.IdentityUserId,
                Email = up.IdentityUser.Email,
                UserName = up.IdentityUser.UserName
            })
            .ToList());
    }

    [HttpGet("withroles")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetWithRoles()
    {
        return Ok(_dbContext.UserProfiles
        .Include(up => up.IdentityUser)
        .Select(up => new UserProfileDTO
        {
            Id = up.Id,
            FirstName = up.FirstName,
            LastName = up.LastName,
            Address = up.Address,
            Email = up.IdentityUser.Email,
            UserName = up.IdentityUser.UserName,
            IdentityUserId = up.IdentityUserId,
            Roles = _dbContext.UserRoles
            .Where(ur => ur.UserId == up.IdentityUserId)
            .Select(ur => _dbContext.Roles.SingleOrDefault(r => r.Id == ur.RoleId).Name)
            .ToList()
        }));
    }

    [HttpPost("promote/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Promote(string id)
    {
        IdentityRole role = _dbContext.Roles.SingleOrDefault(r => r.Name == "Admin");
        // This will create a new row in the many-to-many UserRoles table.
        _dbContext.UserRoles.Add(new IdentityUserRole<string>
        {
            RoleId = role.Id,
            UserId = id
        });
        _dbContext.SaveChanges();
        return NoContent();
    }

    [HttpPost("demote/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Demote(string id)
    {
        IdentityRole role = _dbContext.Roles
            .SingleOrDefault(r => r.Name == "Admin");
        IdentityUserRole<string> userRole = _dbContext
            .UserRoles
            .SingleOrDefault(ur =>
                ur.RoleId == role.Id &&
                ur.UserId == id);

        _dbContext.UserRoles.Remove(userRole);
        _dbContext.SaveChanges();
        return NoContent();
    }

    [HttpGet("{id}")]
    // [Authorize]
    public IActionResult GetUserProfile(int id)
    {
        var userProfile = _dbContext.UserProfiles
            .Include(up => up.IdentityUser)
            .Include(up => up.ChoreAssignments)
                .ThenInclude(ca => ca.Chore)
            .Include(up => up.ChoreCompletions)
                .ThenInclude(cc => cc.Chore)
            .Where(up => up.Id == id)
            .Select(up => new UserProfileDTO
            {
                Id = up.Id,
                FirstName = up.FirstName,
                LastName = up.LastName,
                Address = up.Address,
                IdentityUserId = up.IdentityUserId,
                Email = up.IdentityUser.Email,
                UserName = up.IdentityUser.UserName,
                ChoreAssignments = up.ChoreAssignments.Select(ca => new ChoreAssignmentDTO
                {
                    Id = ca.Id,
                    UserProfileId = ca.UserProfileId,
                    ChoreId = ca.ChoreId,
                    Chore = new ChoreDTO
                    {
                        Id = ca.Chore.Id,
                        Name = ca.Chore.Name,
                        Difficulty = ca.Chore.Difficulty,
                        ChoreFrequencyDays = ca.Chore.ChoreFrequencyDays
                    }

                }).ToList(),
                ChoreCompletions = up.ChoreCompletions.Select(cc => new ChoreCompletionDTO
                {
                    Id = cc.Id,
                    UserProfileId = cc.UserProfileId,
                    ChoreId = cc.ChoreId,
                    CompletedOn = cc.CompletedOn,
                    Chore = new ChoreDTO
                    {
                        Id = cc.Chore.Id,
                        Name = cc.Chore.Name,
                        Difficulty = cc.Chore.Difficulty,
                        ChoreFrequencyDays = cc.Chore.ChoreFrequencyDays
                    }
                }).ToList()
            })
            .FirstOrDefault();

        if (userProfile == null)
        {
            return NotFound();
        }

        return Ok(userProfile);
    }
}


// GET /api/userprofile/{id}
// This endpoint will get a UserProfile along with its assigned chores (through the ChoreAssignment table),as well as the user's completed chores (through the CompletedChore table).
// It should be accessible to any logged in user, regardless of role. That means using the Authorize attribute without specifying a role.
// Use Include and ThenInclude for ChoreAssignment and for CompletedChore to include the Chore data in both cases.
// If you haven't already, you will need to add collections to the UserProfile to store that related data.