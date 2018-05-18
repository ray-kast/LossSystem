using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TurtLSystem {
  class TurtleDrawingContext : ITurtleDrawingContext {
    struct DrawingStep {
      public Point Point { get; }
      public bool Stroke { get; }

      public DrawingStep(Point point, bool stroke) {
        Point = point;
        Stroke = stroke;
      }
    }

    protected Point Pos => steps.Count == 0 ? startPoint : steps[steps.Count - 1].Point;

    public Point StartPoint => GetOffsetPoint(startPoint);

    Point startPoint;
    List<DrawingStep> steps = new List<DrawingStep>();
    double minX, minY;

    public TurtleDrawingContext(double x, double y) {
      startPoint = new Point(minX = x, minY = y);
    }

    public void LineTo(double x, double y, bool stroke) {
      steps.Add(new DrawingStep(new Point(x, y), stroke));

      if (x < minX) minX = x;
      if (y < minY) minY = y;
    }

    Point GetOffsetPoint(Point point) {
      return new Point(point.X - minX, point.Y - minY);
    }

    public void GetPath(StreamGeometry geom) {
      var ctx = geom.Open();

      ctx.BeginFigure(GetOffsetPoint(startPoint), false, false);

      //Console.WriteLine($"Start at {GetOffsetPoint(startPoint)}");

      bool lastStroke = false;
      foreach (DrawingStep step in steps) {
        //Console.WriteLine($"{(step.Stroke ? "Draw" : "Move")} to {GetOffsetPoint(step.Point)}");
        ctx.LineTo(GetOffsetPoint(step.Point), step.Stroke, lastStroke);
        lastStroke = step.Stroke;
      }

      ctx.Close();
    }

    public StreamGeometry GetPath() {
      StreamGeometry geom = new StreamGeometry();
      GetPath(geom);
      return geom;
    }

    public IEnumerable<Point> GetPoints() {
      yield return StartPoint;

      foreach (DrawingStep step in steps) {
        if (step.Stroke) yield return GetOffsetPoint(step.Point);
      }
    }

    public IEnumerable<(Point, bool)> GetSteps() {
      foreach (DrawingStep step in steps) yield return (GetOffsetPoint(step.Point), step.Stroke);
    }
  }
}
