using HouseRules.Data;
using Microsoft.AspNetCore.Mvc;

namespace HouseRules.Controllers;

[ApiController]
[Route("api[controller]")]

public class ChoreController : ControllerBase
{
    private HouseRulesDbContext _dbContext;

    public ChoreController(HouseRulesDbContext context)
    {
        _dbContext = context;
    }
}