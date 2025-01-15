using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeApp.Interfaces
{
    public interface IShapeDisplay
    {
        void ShapeHistoryDisplay(IEnumerable<Shape> shapes);
        void ShowResult(Shape shape);
        IEnumerable<Shape> GetShapeHistory();

    }
}
