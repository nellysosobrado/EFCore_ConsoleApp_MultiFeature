using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameApp.Interfaces
{
    public interface IGameError
    {
        void HandleGameError(Exception ex);
        void ShowError(string message);
    }
}
