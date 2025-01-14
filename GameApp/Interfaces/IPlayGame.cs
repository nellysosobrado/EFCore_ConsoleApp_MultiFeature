using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameApp.Interfaces
{
    public interface IPlayGame
    {
        Game CreateGame();
        void ProcessGameResult(Game game);
        bool ShouldPlayAgain();
        void HandleGameError(Exception ex);
    }
}
