using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TurtLSystem {
  interface ITurtleDrawingContext {
    Point StartPoint { get; }

    void LineTo(double x, double y, bool stroke);

    void GetPath(StreamGeometry geom);

    StreamGeometry GetPath();

    IEnumerable<Point> GetPoints();

    IEnumerable<(Point, bool)> GetSteps();
  }
}
