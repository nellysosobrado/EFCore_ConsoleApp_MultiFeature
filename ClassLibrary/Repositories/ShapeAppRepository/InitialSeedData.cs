
using ClassLibrary.Models;
using ClassLibrary.Enums;
using ClassLibrary.Interface;

namespace ClassLibrary.Repositories.ShapeAppRepository
{
    public class InitialSeedData
    {
        private readonly IApplicationDbContext _context;

        public InitialSeedData(IApplicationDbContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            if (_context.Shapes.Any()) return;

            var shapes = new List<Shape>
        {
            new()
            {
                ShapeType = ShapeType.Rectangle,
                Width = 4,
                Height = 5,
                Area = 20,
                Perimeter = 18,
                CalculationDate = DateTime.Now
            },
            new()
            {
                ShapeType = ShapeType.Parallelogram,
                BaseLength = 6,
                Height = 4,
                Side = 4,
                Area = 24,
                Perimeter = 20,
                CalculationDate = DateTime.Now
            },
            new()
            {
                ShapeType = ShapeType.Triangle,
                SideA = 4,
                SideB = 4,
                SideC = 4,
                Height = 6,
                Area = 12,
                Perimeter = 12,
                CalculationDate = DateTime.Now
            },
            new()
            {
                ShapeType = ShapeType.Rhombus,
                Side = 4,
                Height = 4,
                Area = 16,
                Perimeter = 16,
                CalculationDate = DateTime.Now
            }
        };

            _context.Shapes.AddRange(shapes);
            _context.SaveChanges();
        }
    }
}
