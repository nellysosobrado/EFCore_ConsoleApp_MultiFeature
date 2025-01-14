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
        void ShowShapes(IEnumerable<Shape> shapes);
    }
}
