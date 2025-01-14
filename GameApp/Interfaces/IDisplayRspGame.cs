using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameApp.Interfaces
{
    public interface IDisplayRspGame
    {
        void ShowGameResult(Game game, double winPercentage);
        void ShowGameHistory(IEnumerable<Game> games);
    }
}
