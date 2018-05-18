using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TurtLSystem {
  class TurtleStackDrawingContext : TurtleDrawingContext {
    Stack<Point> pointStack = new Stack<Point>();

    public TurtleStackDrawingContext(double x, double y) : base(x, y) { }

    public void Push() => pointStack.Push(Pos);
    public void Pop() {
      Point pop = pointStack.Pop();

      LineTo(pop.X, pop.Y, false);
    }
  }
}
