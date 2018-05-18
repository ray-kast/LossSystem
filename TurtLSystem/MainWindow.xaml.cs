using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TurtLSystem {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() => InitializeComponent();

    Point Lerp(Point a, Point b, double amt) => new Point(a.X + amt * (b.X - a.X), a.Y + amt * (b.Y - a.Y));

    void Window_Loaded(object sender, RoutedEventArgs e) {
      ITurtleDrawingContext drawCtx = Loss(5);

      var figures = new List<Point[]>();

      {
        var figure = new List<Point> { drawCtx.StartPoint };

        foreach ((Point pos, bool stroke) in drawCtx.GetSteps()) {
          if (!stroke) {
            if (figure.Count > 1) figures.Add(figure.ToArray());
            figure.Clear();
          }

          figure.Add(pos);
        }

        if (figure.Count > 1) figures.Add(figure.ToArray());
      }

      using (StreamGeometryContext ctx = TurtlePath.Open()) {
        foreach (Point[] figure in figures) {
          ctx.BeginFigure(figure[0], false, false);
          for (int i = 1; i < figure.Length; ++i) ctx.LineTo(figure[i], true, true);
        }
      }
    }

    ITurtleDrawingContext Loss(int iters) {
      var lSys = new LSystem("L", "S", "R", "F", "[2", "[M", "]", "{", "}", "-", "+", "^", "$");

      lSys.AddRules(new Dictionary<string, string>() {
        ["L"] = "{S-S+ [2 [2 F+F-][M L] -F [2[2 R]+ L -F+ [M L] -[2 R]+] R [2 -[2 R]+ [M L] -F [M L] [2 R]] R [2[2 R]+ [M L] -F+ [M L] -[2 R]+]]} ",
      });

      string[] result = lSys.Iterate("L", iters);

      var turtle = new TurtleStack(0, 0, Math.PI * 0.5);
      var len = new Stack<double>();

      len.Push(1.0);

      foreach (string str in result) {
        switch (str) {
          case "L":
          case "S":
            turtle.Move(len.Peek() * -0.5, false);
            turtle.Move(len.Peek());
            //turtle.Push();
            //turtle.Turn(Math.PI * 0.75);
            //turtle.Move(len.Peek() * 0.15);
            //turtle.Pop();
            //turtle.Push();
            //turtle.Turn(Math.PI * -0.75);
            //turtle.Move(len.Peek() * 0.15);
            //turtle.Pop();
            turtle.Move(len.Peek() * -0.5, false);
            break;
          case "R":
            turtle.Move(-len.Peek(), false);
            break;
          case "F":
            turtle.Move(len.Peek(), false);
            break;
          case "[2":
            len.Push(len.Peek() / 2.0);
            break;
          case "[M":
            len.Push(len.Peek() * 0.95);
            break;
          case "]":
            len.Pop();
            break;
          case "{":
            turtle.Push();
            break;
          case "}":
            turtle.Pop();
            break;
          case "-":
            turtle.Turn(-Math.PI * 0.5);
            break;
          case "+":
            turtle.Turn(Math.PI * 0.5);
            break;
          case "^":
            turtle.Turn(-Math.PI * 0.15);
            break;
          case "$":
            turtle.Turn(Math.PI * 0.15);
            break;
        }
      }

      return turtle.Ctx;
    }
  }
}
