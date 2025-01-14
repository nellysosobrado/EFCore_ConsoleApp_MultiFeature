using ClassLibrary.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClassLibrary.Models;

public class Game
{
    public int Id { get; set; }
    public GameMove PlayerMove { get; set; }
    public GameMove ComputerMove { get; set; }
    public GameResult Result { get; set; }
    public DateTime GameDate { get; set; }
}