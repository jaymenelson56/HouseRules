using HouseRules.Data;
using HouseRules.Models;
using HouseRules.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseRules.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ChoreController : ControllerBase
{
    private HouseRulesDbContext _dbContext;

    public ChoreController(HouseRulesDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    // [Authorize]

    public IActionResult Get()
    {
        return Ok(_dbContext
            .Chores
            .Select(c => new ChoreDTO
            {
                Id = c.Id,
                Name = c.Name,
                Difficulty = c.Difficulty,
                ChoreFrequencyDays = c.ChoreFrequencyDays
            })
            .ToList());
    }

    [HttpGet("{id}")]
    // [Authorize]

    public IActionResult GetbyId(int id)
    {
        var chore = _dbContext.Chores
            .Include(c => c.ChoreAssignments)
            .ThenInclude(ca => ca.UserProfile)
            .Include(c => c.ChoreCompletions)
            .SingleOrDefault(c => c.Id == id);

        if (chore == null)
        {
            return NotFound();
        }

        var choreDto = new ChoreDTO
        {
            Id = chore.Id,
            Name = chore.Name,
            Difficulty = chore.Difficulty,
            ChoreFrequencyDays = chore.ChoreFrequencyDays,
            ChoreAssignments = chore.ChoreAssignments.Select(ca => new ChoreAssignmentDTO
            {
                Id = ca.Id,
                UserProfileId = ca.UserProfileId,
                ChoreId = ca.ChoreId,
                UserProfile = new UserProfileDTO
                {
                    Id = ca.UserProfile.Id,
                    FirstName = ca.UserProfile.FirstName,
                    LastName = ca.UserProfile.LastName,
                    Address = ca.UserProfile.Address,
                    IdentityUserId = ca.UserProfile.IdentityUserId
                }
            }).ToList(),
            ChoreCompletions = chore.ChoreCompletions.Select(cc => new ChoreCompletionDTO
            {
                Id = cc.Id,
                UserProfileId = cc.UserProfileId,
                ChoreId = cc.ChoreId,
                CompletedOn = cc.CompletedOn
            }).ToList()
        };

        return Ok(choreDto);
    }

    
    [HttpPost("{id}/complete")]
    // [Authorize]
    public IActionResult CompleteChore(int id, int userId)
    {
        var chore = _dbContext.Chores.SingleOrDefault(c => c.Id == id);
        if (chore == null)
        {
            return NotFound();
        }

        var userProfile = _dbContext.UserProfiles.SingleOrDefault(up => up.Id == userId);
        if (userProfile == null)
        {
            return BadRequest("Invalid userId");
        }

        var choreCompletion = new ChoreCompletion
        {
            ChoreId = id,
            UserProfileId = userProfile.Id,
            CompletedOn = DateTime.Now
        };

        _dbContext.ChoreCompletions.Add(choreCompletion);
        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Post([FromBody] ChoreDTO choreDto)
    {
        if (choreDto == null)
        {
            return BadRequest("Chore data is null");
        }

        var chore = new Chore
        {
            Name = choreDto.Name,
            Difficulty = choreDto.Difficulty,
            ChoreFrequencyDays = choreDto.ChoreFrequencyDays
        };

        _dbContext.Chores.Add(chore);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetbyId), new { id = chore.Id }, chore);
    }


    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Put(int id, ChoreDTO choreDto)
    {
        if (choreDto == null)
        {
            return BadRequest("Chore data is null");
        }

        var chore = _dbContext.Chores.SingleOrDefault(c => c.Id == id);
        if (chore == null)
        {
            return NotFound();
        }

        chore.Name = choreDto.Name;
        chore.Difficulty = choreDto.Difficulty;
        chore.ChoreFrequencyDays = choreDto.ChoreFrequencyDays;

        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var chore = _dbContext.Chores.SingleOrDefault(c => c.Id == id);
        if (chore == null)
        {
            return NotFound();
        }

        _dbContext.Chores.Remove(chore);
        _dbContext.SaveChanges();

        return NoContent();
    }
    [HttpPost("{id}/assign")]
    // [Authorize(Roles = "Admin")]
    public IActionResult AssignChore(int id, int userId)
    {
        var chore = _dbContext.Chores.SingleOrDefault(c => c.Id == id);
        if (chore == null)
        {
            return NotFound();
        }

        var userProfile = _dbContext.UserProfiles.SingleOrDefault(up => up.Id == userId);
        if (userProfile == null)
        {
            return BadRequest("Invalid userId");
        }

        var choreAssignment = new ChoreAssignment
        {
            ChoreId = id,
            UserProfileId = userProfile.Id
        };

        _dbContext.ChoreAssignments.Add(choreAssignment);
        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpPost("{id}/unassign")]
    [Authorize(Roles = "Admin")]
    public IActionResult UnassignChore(int id, int userId)
    {
        var chore = _dbContext.Chores.SingleOrDefault(c => c.Id == id);
        if (chore == null)
        {
            return NotFound();
        }

        var userProfile = _dbContext.UserProfiles.SingleOrDefault(up => up.Id == userId);
        if (userProfile == null)
        {
            return BadRequest("Invalid userId");
        }

        var choreAssignment = _dbContext.ChoreAssignments
            .SingleOrDefault(ca => ca.ChoreId == id && ca.UserProfileId == userProfile.Id);

        if (choreAssignment == null)
        {
            return NotFound("The chore is not assigned to the user.");
        }

        _dbContext.ChoreAssignments.Remove(choreAssignment);
        _dbContext.SaveChanges();

        return NoContent();
    }

    
}


