using System;

public class Minefield
{
    public int[,] field;

    public Minefield()
    {
    
    }

    public bool InitializeMinefield(int[,] minefield)
    {
        // Create a new 2D array to represent the minefield with an additional first row
        this.field = new int[minefield.GetLength(0) + 1, minefield.GetLength(1)];

        for (int y = 0; y < this.field.GetLength(0); y++)
        {
            for (int x = 0; x < this.field.GetLength(1); x++)
            {
                // first row, set the value to 0 (safe zone)
                if (y == 0)
                {
                    this.field[y, x] = 0;
                }
                else
                {
                    this.field[y, x] = minefield[y - 1, x];
                }
            }
        }
        return true;
    }

    public void PrintMinefield()
    {
        Console.WriteLine("Minefield:");

        for (int y = 0; y < this.field.GetLength(0); y++)
        {
            for (int x = 0; x < this.field.GetLength(1); x++)
            {
                char cellChar;

                switch (this.field[y, x])
                {
                    case 1:
                        cellChar = 'X'; // bomb
                        break;
                    case 2:
                        cellChar = '*'; // explored
                        break;
                    case 3:
                        cellChar = '√'; // completed
                        break;
                    default:
                        cellChar = ' '; // unexplored
                        break;
                }

                Console.Write(cellChar);
            }
            Console.WriteLine();
        }
    }
}

public class Position
{
    public int x, y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Position GetRelativePosition(int numberPadDirection)
    {
        // return new relative position based on number pad mapping
        switch (numberPadDirection)
        {
            case 6:
                // right
                return new Position(this.x + 1, this.y);
            case 3:
                // down right
                return new Position(this.x + 1, this.y + 1);
            case 2:
                // down
                return new Position(this.x, this.y + 1);
            case 1:
                // down left
                return new Position(this.x - 1, this.y + 1);
            case 4:
                // left
                return new Position(this.x - 1, this.y);
            case 7:
                // up left
                return new Position(this.x - 1, this.y - 1);
            case 8:
                // up
                return new Position(this.x, this.y - 1);
            case 9:
                // up right
                return new Position(this.x + 1, this.y - 1);
            default:
                return new Position(-1, -1);
        }
    }

    public override string ToString()
    {
        return string.Format("({0}, {1})", x, y);
    }
}

public class Totoshka
{
    public Position position;
    public Position allyPosition; //position of ally (last position of totoshka)
    public int[,] explored;
    public int[] pattern;

    public Totoshka()
    {
        //default search pattern
        this.pattern = new int[] { 1, 2, 3, 4, 6, 7, 8, 9 };
        this.explored = null;
    }

    public void DisplayInfo(Minefield m)
    {
        // Print out information for debugging
        Console.WriteLine("\nTotoshka at " + position.ToString());

        // Check if the explored array is not initialized
        if (this.explored == null)
        {
            return;
        }

        for (int y = 0; y < this.explored.GetLength(0); y++)
        {
            for (int x = 0; x < this.explored.GetLength(1); x++)
            {
                if (this.position.x == x && this.position.y == y)
                {
                    Console.Write("T"); // Totoshka's position
                }
                else if (this.allyPosition.x == x && this.allyPosition.y == y)
                {
                    Console.Write("A"); // Ally's position
                }
                else if (this.explored[y, x] == 1)
                {
                    Console.Write("*"); // Explored
                }
                else if (this.explored[y, x] == 2)
                {
                    Console.Write("X"); // Bomb
                }
                else
                {
                    Console.Write(" "); // Unexplored
                }
            }
            
            Console.WriteLine();
        }
    }

    public void ExploreMinefield(Minefield m)
    {
        // Find the first safe position in the first row
        int x = FindSafePositionInFirstRow(m);
        if (x == -1)
        {
            Console.WriteLine("No safe path in the first row.");
            return;
        }

        this.position = new Position(x, 0);
        this.allyPosition = new Position(x - 1, 0);
        this.explored = new int[m.field.GetLength(0), m.field.GetLength(1)];

        // Allow Totoshka and Ally to swap places if stuck
        while (!Explore(m))
        {
            Position tmp = this.position;
            this.position = this.allyPosition;
            this.allyPosition = tmp;

            // Find another safe position in the first row
            x = FindSafePositionInFirstRow(m);

            // Check if the same position is encountered again
            if (x == -1 || explored[1, x] == 1)
            {
                Console.WriteLine("No more safe paths in the first row. Stuck!");
                return;
            }

            this.position = new Position(x, 0);
            this.allyPosition = new Position(x - 1, 0);
            this.explored = new int[m.field.GetLength(0), m.field.GetLength(1)];
        }

        Console.WriteLine("\nCompleted Path:");
        m.PrintMinefield();
    }

    private int FindSafePositionInFirstRow(Minefield m)
    {
        for (int x = 0; x < m.field.GetLength(1); x++)
        {
            if (m.field[1, x] == 0)
            {
                // Found a safe position in the second row
                return x;
            }
        }

        // No safe position in the first row
        return -1;
    }

    public int Smell(Minefield m, int np)
    {
        Position a = this.position.GetRelativePosition(np);

        if (a != null && (a.x < 0 || a.x >= m.field.GetLength(1) || a.y < 0))
        {
            return 2;   // out of bound
        }
        if (a.y > m.field.GetLength(0))
        {
            return -1;  // pass through
        }
        if (a.y == 0)
        {
            return 0;   // safe since first row
        }
        return m.field[a.y, a.x];   // see whether got bomb or not
    }

    public bool Explore(Minefield m)
    {
        if (this.position.y + 1 == m.field.GetLength(0))
        {
            return true;    // reached the bottom of the minefield
        }

        for (int i = 0; i < this.pattern.Length; i++)
        {
            Position nextPosition = this.position.GetRelativePosition(pattern[i]);

            if (nextPosition.x == this.allyPosition.x && nextPosition.y == this.allyPosition.y)
            {
                continue;   // Skip positions where Totoshka would move into the Ally
            }

            switch (this.Smell(m, pattern[i]))
            {
                case 0: // safe
                    if (explored[nextPosition.y, nextPosition.x] == 1)
                    {
                        continue;
                    }

                    // ally move to totoshka position while totoshka move to a next position
                    this.allyPosition = this.position;
                    this.position = nextPosition;

                    explored[nextPosition.y, nextPosition.x] = 1;
                    Console.Clear();
                    m.PrintMinefield();
                    this.DisplayInfo(m);
                    Console.WriteLine();
                    System.Threading.Thread.Sleep(1000); // Sleep for 1 second to visualize the exploration

                    if (Explore(m))
                    {
                        // If the exploration led to the end, mark the completed path and return true
                        MarkCompletedPath(m);
                        return true;
                    }

                    // If the exploration did not lead to the end, swap places with the ally
                    Position tmp = this.position;
                    this.position = this.allyPosition;
                    this.allyPosition = tmp;

                    break;

                case -1:    // pass through
                    return true;

                case 1:     // bomb
                    explored[nextPosition.y, nextPosition.x] = 2;
                    break;
                case 2:     // out of bound
                    break;
            }
        }
        
        // No safe path found from this position, break out of the loop
        return false;
    }

    public void MarkCompletedPath(Minefield minefield)
    {
        for (int y = 0; y < this.explored.GetLength(0); y++)
        {
            for (int x = 0; x < this.explored.GetLength(1); x++)
            {
                // Check if the current position in the explored array is marked as explored (1)
                if (this.explored[y, x] == 1)
                {
                    minefield.field[y, x] = 3; // Mark the completed path
                }
            }
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Minefield minefield = new Minefield();

        int[,] mineArray = {
            {0, 0, 1, 1, 0},
            {1, 1, 0, 1, 0},
            {0, 1, 1, 0, 1},
            {1, 0, 1, 0, 1},
            {0, 1, 0, 1, 1},
        };
        minefield.InitializeMinefield(mineArray);

        Totoshka totoshka = new Totoshka();
        totoshka.ExploreMinefield(minefield);

        Console.ReadLine();
    }
}
