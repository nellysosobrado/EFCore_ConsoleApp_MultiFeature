using System.ComponentModel.DataAnnotations;
using ClassLibrary.Enums.RpsGameEnums;

namespace ClassLibrary.Models;

public class Game
{
    [Key]
    public int Id { get; set; }

    [Required]
    public GameMove PlayerMove { get; set; }

    [Required]
    public GameMove ComputerMove { get; set; }

    [Required]
    public string Winner { get; set; } = string.Empty;

    public double AverageWinRate { get; set; }

    [Required]
    public DateTime GameDate { get; set; } = DateTime.Now;


}