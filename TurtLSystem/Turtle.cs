using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TurtLSystem {
  class Turtle : ITurtle {
    public ITurtleDrawingContext Ctx { get; }
    public double Theta { get; protected set; }
    public double X { get; protected set; }
    public double Y { get; protected set; }

    public Turtle(double x, double y, double theta, ITurtleDrawingContext ctx) {
      X = x;
      Y = y;
      Theta = theta;
      Ctx = ctx;
    }

    public Turtle(double x, double y, double theta) : this(x, y, theta, new TurtleDrawingContext(x, y)) { }

    public void Move(double dist, bool stroke = true) {
      X += Math.Cos(Theta) * dist;
      Y -= Math.Sin(Theta) * dist;

      Ctx.LineTo(X, Y, stroke);
    }

    public void Turn(double angle) { Theta += angle; }
  }
}
