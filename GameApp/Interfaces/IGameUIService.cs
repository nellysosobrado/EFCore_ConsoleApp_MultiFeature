using ClassLibrary.Models;
using ClassLibrary.Enums;

namespace GameApp.Interfaces;

public interface IGameUIService
{
    GameMove GetPlayerMove();
    void ShowGameResult(Game game, double winPercentage);
    void ShowGameHistory(IEnumerable<Game> games);
    void ShowError(string message);
    void WaitForKeyPress(string message = "\nPress any key to continue...");
}