using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TurtLSystem {
  class TurtleStack : ITurtle {
    Stack<Turtle> stack = new Stack<Turtle>();

    public TurtleStackDrawingContext Ctx { get; }
    ITurtleDrawingContext ITurtle.Ctx => Ctx;

    public Turtle Peek => stack.Peek();
    public double X => Peek.X;
    public double Y => Peek.Y;
    public double Theta => Peek.Theta;

    public TurtleStack(double x, double y, double theta) {
      Ctx = new TurtleStackDrawingContext(x, y);

      stack.Push(new Turtle(x, y, theta, Ctx));
    }

    public void Move(double dist, bool draw = true) => Peek.Move(dist, draw);

    public void Turn(double angle) => Peek.Turn(angle);

    public void Push() {
      stack.Push(new Turtle(Peek.X, Peek.Y, Peek.Theta, Ctx));
      Ctx.Push();
    }

    public void Pop() {
      stack.Pop();
      Ctx.Pop();
    }
  }
}
