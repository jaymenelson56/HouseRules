using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseRules.Models;

public class ChoreCompletion
{
    public int Id { get; set; }
    [Required]
    public int UserProfileId { get; set; }
    [Required]
    public int ChoreId { get; set; }
    [Required]
    public DateTime CompletedOn { get; set; }
    [ForeignKey(nameof(ChoreId))]
    public Chore? Chore { get; set; }
    [ForeignKey(nameof(UserProfileId))]
    public UserProfile? UserProfile { get; set; }
}