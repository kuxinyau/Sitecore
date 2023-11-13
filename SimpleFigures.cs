using System;
    
public class SimpleFigures
{
    public abstract class GeometricFigure
    {
        public abstract void Move(double deltaX, double deltaY);
        public abstract void Rotate(double rX, double rY, double angle);
    }

    public class Point : GeometricFigure 
    {
        public double x { get; set; }
        public double y { get; set; }

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override void Move(double deltaX, double deltaY)
        {
            x += deltaX;
            y += deltaY;
        }

        public override void Rotate(double rX, double rY, double angle)
        {
            // formula
            // x′ = (x−rX)⋅cos(θ)−(y−rY)⋅sin(θ) + rX
            // y′ = (x−rX)⋅sin(θ) + (y−rY)⋅cos(θ) + rY

            double rad = angle * Math.PI / 180; // Convert degrees to radians

            // Calculate new position after rotation
            double newX = (x - rX) * Math.Cos(rad) - (y - rY) * Math.Sin(rad) + rX;
            double newY = (x - rX) * Math.Sin(rad) + (y - rY) * Math.Cos(rad) + rY;

            x = newX;
            y= newY;
        }

        public override string ToString()
        {
            return string.Format("Point: ({0:0.####}, {1:0.####})", x, y);
        }
    }

    public class Line : GeometricFigure
    {
        public Point start { get; set; }
        public Point end { get; set; }

        public Line (Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }

        public override void Move(double deltaX, double deltaY)
        {
            start.Move(deltaX, deltaY);
            end.Move(deltaX, deltaY);
        }

        public override void Rotate(double rX, double rY, double angle)
        {
            start.Rotate(rX, rY, angle);
            end.Rotate(rX, rY, angle);
        }

        public override string ToString()
        {
            return string.Format("Line: ({0:0.####}, {1:0.####}), ({2:0.####}, {3:0.####})", start.x, start.y, end.x, end.y);
        }
    }

    public class Circle : GeometricFigure
    {
        public Point center { get; set; }
        public double radius { get; set; }

        public Circle(Point center, double radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public override void Move(double deltaX, double deltaY)
        {
            center.Move(deltaX, deltaY);
        }

        public override void Rotate(double rX, double rY, double angle)
        {
            center.Rotate(rX, rY, angle);
        }

        public override string ToString()
        {
            return string.Format("Circle: ({0:0.####}, {1:0.####}) with radius {2:0.####}", center.x, center.y, radius);
        }
    }

    public class Aggregation : GeometricFigure
    {
        private List<GeometricFigure> figures = new List<GeometricFigure>();

        public void AddFigure(GeometricFigure geometricFigure)
        {
            figures.Add(geometricFigure);
        }

        public override void Move(double deltaX, double deltaY)
        {
            Console.Write("\n\nTranslation: ({0:0.####}, {1:0.####})\n", deltaX, deltaY);
            foreach (var figure in figures)
            {
                figure.Move(deltaX, deltaY);
            }
        }

        public override void Rotate(double rX, double rY, double angle)
        {
            Console.Write("\n\nRotation: ({0:0.####}, {1:0.####}) with {2:0.####}°\n", rX, rY, angle);
            foreach (var figure in figures)
            {
                figure.Rotate(rX, rY, angle);
            }
        }

        public void CurrentLocation()
        {
            foreach (var figure in figures)
            {
                Console.Write(figure.ToString() + "\n");
            }
        }
    }

    public static void Main(string[] args)
    {
        // Create an Aggregation to hold the geometric figures
        var aggregation = new Aggregation();

        int choice = 0;

        while (choice != 7)
        {
            Console.Write("\nPress any key to continue....");
            Console.ReadLine();
            Console.Clear();

            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Add Point");
            Console.WriteLine("2. Add Line");
            Console.WriteLine("3. Add Circle");
            Console.WriteLine("4. Translate");
            Console.WriteLine("5. Rotate");
            Console.WriteLine("6. Display Current Location");
            Console.WriteLine("7. Exit\n");
            Console.Write("Option > ");

            choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    // Get user input for creating Point
                    Console.Write("Enter x-coordinate for Point: ");
                    double x = double.Parse(Console.ReadLine());
                    Console.Write("Enter y-coordinate for Point: ");
                    double y = double.Parse(Console.ReadLine());
                    var point = new SimpleFigures.Point(x, y);
                    aggregation.AddFigure(point);
                    break;

                case 2:
                    // Get user input for creating Line
                    Console.Write("Enter x-coordinate for start of Line: ");
                    double startX = double.Parse(Console.ReadLine());
                    Console.Write("Enter y-coordinate for start of Line: ");
                    double startY = double.Parse(Console.ReadLine());
                    Console.Write("Enter x-coordinate for end of Line: ");
                    double endX = double.Parse(Console.ReadLine());
                    Console.Write("Enter y-coordinate for end of Line: ");
                    double endY = double.Parse(Console.ReadLine());
                    var line = new SimpleFigures.Line(new SimpleFigures.Point(startX, startY), new SimpleFigures.Point(endX, endY));
                    aggregation.AddFigure(line);
                    break;

                case 3:
                    // Get user input for creating Circle
                    Console.Write("Enter x-coordinate for Circle center: ");
                    double centerX = double.Parse(Console.ReadLine());
                    Console.Write("Enter y-coordinate for Circle center: ");
                    double centerY = double.Parse(Console.ReadLine());
                    Console.Write("Enter radius for Circle: ");
                    double radius = double.Parse(Console.ReadLine());
                    var circle = new SimpleFigures.Circle(new SimpleFigures.Point(centerX, centerY), radius);
                    aggregation.AddFigure(circle);
                    break;

                case 4:
                    // Get user input for translation
                    Console.Write("Enter translation values (deltaX deltaY): ");
                    string[] translationValues = Console.ReadLine().Split(' ');
                    double deltaX = double.Parse(translationValues[0]);
                    double deltaY = double.Parse(translationValues[1]);
                    aggregation.Move(deltaX, deltaY);
                    break;

                case 5:
                    // Get user input for rotation
                    Console.Write("Enter rotation values (rX rY angle): ");
                    string[] rotationValues = Console.ReadLine().Split(' ');
                    double rX = double.Parse(rotationValues[0]);
                    double rY = double.Parse(rotationValues[1]);
                    double angle = double.Parse(rotationValues[2]);
                    aggregation.Rotate(rX, rY, angle);
                    break;

                case 6:
                    // Display the current location of figures
                    Console.WriteLine("\nCurrent Location:");
                    aggregation.CurrentLocation();
                    break;

                case 7:
                    // Exit the program
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }

        Console.Clear();
        aggregation.CurrentLocation();
        aggregation.ToString();
        Console.Write("\n\nPress any key to close....");
        Console.ReadLine();
    }
}
