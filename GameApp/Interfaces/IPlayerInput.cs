﻿using ClassLibrary.Models;
using ClassLibrary.Enums;

namespace GameApp.Interfaces;

public interface IPlayerInput
{
    GameMove GetPlayerMove();
    bool ShouldPlayAgain();
    void WaitForKeyPress(string message = "\nPress any key to continue...");

}