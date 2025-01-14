﻿using ClassLibrary.Models;
using ClassLibrary.Enums;

namespace GameApp.Interfaces;

public interface IGameService
{
    GameMove GetComputerMove();
    string DetermineWinner(GameMove playerMove, GameMove computerMove);
    void SaveGame(Game game);
    IEnumerable<Game> GetGameHistory();
    double CalculateWinPercentage();
}