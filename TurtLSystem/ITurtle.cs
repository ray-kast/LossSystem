using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtLSystem {
  interface ITurtle {
    ITurtleDrawingContext Ctx { get; }
    double X { get; }
    double Y { get; }
    double Theta { get; }

    void Move(double dist, bool stroke = true);
    void Turn(double angle);
  }
}
