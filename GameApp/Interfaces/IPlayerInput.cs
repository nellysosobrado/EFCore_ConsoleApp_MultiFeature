using ClassLibrary.Models;
using ClassLibrary.Enums;

namespace GameApp.Interfaces;

public interface IPlayerInput
{
    GameMove GetPlayerMove();
  
    void WaitForKeyPress(string message = "\nPress any key to continue...");
}