using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace _8PuzzleSolver
{
    class Program
    {

        static int permutations = 0;

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("Enter Puzzle Size (IE. 9, 16, 25)");
            int puzzleSize = int.Parse(Console.ReadLine().Trim());

            Console.WriteLine("Enter Initial Configuration (with 'e' as the empty spot)");
            string InitialConfig = Console.ReadLine();

            Console.WriteLine("Enter the Final Configuration (with 'e' as the empty spot)");
            string FinalConfig = Console.ReadLine();

            int count = 1;

            int sqrt = (int)Math.Sqrt(puzzleSize);
            if (puzzleSize < 16)
            {
                stopwatch.Start();
                List<string> result = FindSolutionDijkstra(InitialConfig, FinalConfig, puzzleSize);
                stopwatch.Stop();
                Console.WriteLine("Dijkstra's:");
                Console.WriteLine("Time taken = " + stopwatch.ElapsedMilliseconds + " ms");
                Console.WriteLine("Permutations = " + permutations);
                permutations = 0;

                foreach (var path in result)
                {
                    Console.WriteLine($"Move {count}:");
                    Console.WriteLine(path.Substring(0, sqrt * 2));
                    Console.WriteLine(path.Substring(sqrt * 2, sqrt * 2));
                    Console.WriteLine(path.Substring(sqrt * 4));
                    count++;
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            stopwatch.Reset();
            stopwatch.Start();
            List<string> resultAStar = FindSolutionAStar(InitialConfig, FinalConfig, puzzleSize, HeuristicType.ManhattanConflict);
            stopwatch.Stop();
            Console.WriteLine("Time taken = " + stopwatch.ElapsedMilliseconds + " ms");
            Console.WriteLine("Permutations = " + permutations);
            permutations = 0;
            count = 1;

            Console.WriteLine("A*:");

            foreach (var path in resultAStar)
            {
                Console.WriteLine($"Move {count}:");
                int temp = 0;
                foreach (var c in path.Split(' '))
                {
                    if (temp == sqrt)
                    {
                        temp = 0;
                        Console.WriteLine();
                    }
                    Console.Write(c + " ");

                    temp++;
                }
                Console.WriteLine();
                count++;
            }


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();


            stopwatch.Reset();
            stopwatch.Start();
            List<string> resultIDAStar = FindSolutionIDAStar(InitialConfig, FinalConfig, puzzleSize, HeuristicType.ManhattanConflict);
            stopwatch.Stop();
            Console.WriteLine("Time taken = " + stopwatch.ElapsedMilliseconds + " ms");
            Console.WriteLine("Permutations = " + permutations);
            permutations = 0;
            count = 1;

            Console.WriteLine("IDA*:");

            foreach (var path in resultIDAStar)
            {
                Console.WriteLine($"Move {count}:");
                int temp = 0;
                foreach (var c in path.Split(' '))
                {
                    if (temp == sqrt)
                    {
                        temp = 0;
                        Console.WriteLine();
                    }
                    Console.Write(c + " ");

                    temp++;
                }
                Console.WriteLine();
                count++;
            }


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();


            stopwatch.Reset();
            stopwatch.Start();
            List<string> resultRealIDAStar = FindSolutionRealIDAStar(InitialConfig, FinalConfig, puzzleSize, HeuristicType.ManhattanConflict);
            stopwatch.Stop();
            Console.WriteLine("Time taken = " + stopwatch.ElapsedMilliseconds + " ms");
            Console.WriteLine("Permutations = " + permutations);
            permutations = 0;
            count = 1;

            Console.WriteLine("Real IDA*:");

            foreach (var path in resultRealIDAStar)
            {
                Console.WriteLine($"Move {count}:");
                int temp = 0;
                foreach (var c in path.Split(' '))
                {
                    if (temp == sqrt)
                    {
                        temp = 0;
                        Console.WriteLine();
                    }
                    Console.Write(c + " ");

                    temp++;
                }
                Console.WriteLine();
                count++;
            }

        }

        static List<string> FindSolutionDijkstra(string start, string end, int PuzzleSize)
        {
            Dictionary<string, string> parentTable = new Dictionary<string, string>();

            parentTable.Add(start, start);

            Queue<string> paths = new Queue<string>();
            paths.Enqueue(start);
            string currPuzzle = start;

            while (currPuzzle != end)
            {
                currPuzzle = paths.Dequeue();
                string[] splitValues = currPuzzle.Split(' ');


                int emptyIndex = -1;
                for (int i = 0; i < splitValues.Length; i++)
                {
                    if (splitValues[i] == "e")
                    {
                        emptyIndex = i;
                        break;
                    }
                }

                if (emptyIndex != -1)
                {
                    permutations++;
                    bool canMoveLeft = false;
                    bool canMoveRight = false;
                    bool canMoveUp = false;
                    bool canMoveDown = false;

                    int sqrt = (int)Math.Sqrt(PuzzleSize);

                    if (emptyIndex % sqrt == 0)
                    {
                        canMoveRight = true;
                    }
                    else if (emptyIndex % sqrt == sqrt - 1)
                    {
                        canMoveLeft = true;
                    }
                    else //if(emptyIndex % Math.Sqrt(PuzzleSize) == 1)
                    {
                        canMoveRight = true;
                        canMoveLeft = true;
                    }

                    if (emptyIndex < sqrt)
                    {
                        canMoveDown = true;
                    }
                    else if (emptyIndex < sqrt * 2)
                    {
                        canMoveDown = true;
                        canMoveUp = true;
                    }
                    else
                    {
                        canMoveUp = true;
                    }


                    if (canMoveLeft)
                    {
                        string LeftString = "";
                        for (int i = 0; i < splitValues.Length; i++)
                        {
                            if (i == emptyIndex - 1)
                            {
                                LeftString += splitValues[i + 1];
                                LeftString += " ";
                                LeftString += splitValues[i];
                                LeftString += " ";
                                i++;
                            }
                            else
                            {
                                LeftString += splitValues[i];
                                LeftString += " ";
                            }
                        }

                        LeftString = LeftString.TrimEnd();

                        bool result = parentTable.TryGetValue(LeftString, out string val);

                        if (!result)
                        {
                            parentTable.Add(LeftString, currPuzzle);
                            paths.Enqueue(LeftString);
                        }
                    }
                    if (canMoveRight)
                    {
                        string RightString = "";
                        for (int i = 0; i < splitValues.Length; i++)
                        {
                            if (i == emptyIndex)
                            {
                                RightString += splitValues[i + 1];
                                RightString += " ";
                                RightString += splitValues[i];
                                RightString += " ";
                                i++;
                            }
                            else
                            {
                                RightString += splitValues[i];
                                RightString += " ";
                            }
                        }

                        RightString = RightString.TrimEnd();
                        bool result = parentTable.TryGetValue(RightString, out string val);

                        if (!result)
                        {
                            parentTable.Add(RightString, currPuzzle);
                            paths.Enqueue(RightString);
                        }
                    }
                    if (canMoveUp)
                    {
                        string UpString = "";
                        for (int i = 0; i < splitValues.Length; i++)
                        {
                            if (i == emptyIndex)
                            {
                                UpString += splitValues[i - sqrt];
                            }
                            else if (i == emptyIndex - sqrt)
                            {
                                UpString += splitValues[emptyIndex];
                            }
                            else
                            {
                                UpString += splitValues[i];
                            }

                            UpString += " ";
                        }

                        UpString = UpString.TrimEnd();
                        bool result = parentTable.TryGetValue(UpString, out string val);

                        if (!result)
                        {
                            parentTable.Add(UpString, currPuzzle);
                            paths.Enqueue(UpString);
                        }
                    }
                    if (canMoveDown)
                    {
                        string DownString = "";
                        for (int i = 0; i < splitValues.Length; i++)
                        {
                            if (i == emptyIndex)
                            {
                                DownString += splitValues[i + sqrt];
                            }
                            else if (i == emptyIndex + sqrt)
                            {
                                DownString += splitValues[emptyIndex];
                            }
                            else
                            {
                                DownString += splitValues[i];
                            }

                            DownString += " ";
                        }

                        DownString = DownString.TrimEnd();
                        bool result = parentTable.TryGetValue(DownString, out string val);

                        if (!result)
                        {
                            parentTable.Add(DownString, currPuzzle);
                            paths.Enqueue(DownString);
                        }
                    }

                }

            }
            List<string> path = new List<string>();
            path.Add(currPuzzle);

            while (currPuzzle != start)
            {
                path.Add(parentTable[currPuzzle]);
                currPuzzle = parentTable[currPuzzle];
            }

            path.Reverse();

            return path;
        }

        enum HeuristicType
        {
            Hamming,
            Manhattan,
            ManhattanConflict
        }

        static int Heuristic(string[] current, string[] end, HeuristicType heuristic)
        {
            int count = 0;
            if (heuristic == HeuristicType.Hamming)
            {
                for (int i = 0; i < current.Length; i++)
                {
                    if (current[i] != end[i])
                    {
                        count++;
                    }
                }
            }
            else if (heuristic == HeuristicType.Manhattan)
            {
                int sqrt = (int)Math.Sqrt(current.Length);
                Dictionary<string, int> indexDictionary = new Dictionary<string, int>();
                int indexCount = 0;
                foreach (string s in end)
                {
                    if (s != "e")
                    {
                        indexDictionary.Add(s, indexCount);
                    }
                    indexCount++;
                }

                for (int i = 0; i < current.Length; i++)
                {
                    if (current[i] == "e")
                    {
                        continue;
                    }
                    int endIndex = indexDictionary[current[i]];
                    int endMod = endIndex % sqrt;
                    int currMod = i % sqrt;
                    if (endMod != currMod)
                    {
                        count += Math.Abs(endMod - currMod);
                    }
                    int horizontalError = endMod - currMod;
                    int vertIndex = i + horizontalError;
                    int verticalError = 0;
                    while (vertIndex != endIndex)
                    {
                        if (vertIndex > endIndex)
                        {
                            vertIndex -= sqrt;
                        }
                        else if (vertIndex < endIndex)
                        {
                            vertIndex += sqrt;
                        }
                        verticalError++;
                    }
                    count += verticalError;
                }
            }
            else if (heuristic == HeuristicType.ManhattanConflict)
            {
                count += Heuristic(current, end, HeuristicType.Manhattan);
                count += LinearConflict(current, end);
            }
            return count;
        }


        static List<string> FindSolutionAStar(string start, string end, int PuzzleSize, HeuristicType heuristic)
        {
            Dictionary<string, string> parentTable = new Dictionary<string, string>();
            Dictionary<string, int> distanceTable = new Dictionary<string, int>();

            parentTable.Add(start, start);
            distanceTable.Add(start, 0);

            string[] splitEnd = end.Split(' ');

            PriorityQueue<int, string> paths = new PriorityQueue<int, string>();
            paths.Insert(Heuristic(start.Split(' '), splitEnd, heuristic), start);
            string currPuzzle = start;

            while (currPuzzle != end)
            {
                var temp = paths.Pop();
                currPuzzle = temp.value;
                string[] splitValues = currPuzzle.Split(' ');


                int emptyIndex = -1;
                for (int i = 0; i < splitValues.Length; i++)
                {
                    if (splitValues[i] == "e")
                    {
                        emptyIndex = i;
                        break;
                    }
                }

                if (emptyIndex != -1)
                {
                    permutations++;
                    bool canMoveLeft = false;
                    bool canMoveRight = false;
                    bool canMoveUp = false;
                    bool canMoveDown = false;

                    int sqrt = (int)Math.Sqrt(PuzzleSize);

                    if (emptyIndex % sqrt == 0)
                    {
                        canMoveRight = true;
                    }
                    else if (emptyIndex % sqrt == sqrt - 1)
                    {
                        canMoveLeft = true;
                    }
                    else //if(emptyIndex % Math.Sqrt(PuzzleSize) == 1)
                    {
                        canMoveRight = true;
                        canMoveLeft = true;
                    }

                    if (emptyIndex < sqrt)
                    {
                        canMoveDown = true;
                    }
                    else if (emptyIndex < sqrt * (sqrt - 1))
                    {
                        canMoveDown = true;
                        canMoveUp = true;
                    }
                    else
                    {
                        canMoveUp = true;
                    }


                    if (canMoveLeft)
                    {
                        string LeftString = "";
                        for (int i = 0; i < splitValues.Length; i++)
                        {
                            if (i == emptyIndex - 1)
                            {
                                LeftString += splitValues[i + 1];
                                LeftString += " ";
                                LeftString += splitValues[i];
                                LeftString += " ";
                                i++;
                            }
                            else
                            {
                                LeftString += splitValues[i];
                                LeftString += " ";
                            }
                        }

                        LeftString = LeftString.TrimEnd();

                        bool result = parentTable.TryGetValue(LeftString, out string val);

                        if (!result)
                        {
                            parentTable.Add(LeftString, currPuzzle);
                            int parentDistance = distanceTable[currPuzzle];
                            paths.Insert(Heuristic(LeftString.Split(' '), splitEnd, heuristic) + parentDistance + 1, LeftString);
                            distanceTable.Add(LeftString, parentDistance + 1);
                        }
                    }
                    if (canMoveRight)
                    {
                        string RightString = "";
                        for (int i = 0; i < splitValues.Length; i++)
                        {
                            if (i == emptyIndex)
                            {
                                RightString += splitValues[i + 1];
                                RightString += " ";
                                RightString += splitValues[i];
                                RightString += " ";
                                i++;
                            }
                            else
                            {
                                RightString += splitValues[i];
                                RightString += " ";
                            }
                        }

                        RightString = RightString.TrimEnd();
                        bool result = parentTable.TryGetValue(RightString, out string val);

                        if (!result)
                        {
                            parentTable.Add(RightString, currPuzzle);
                            int parentDistance = distanceTable[currPuzzle];
                            paths.Insert(Heuristic(RightString.Split(' '), splitEnd, heuristic) + parentDistance + 1, RightString);
                            distanceTable.Add(RightString, parentDistance + 1);
                        }
                    }
                    if (canMoveUp)
                    {
                        string UpString = "";
                        for (int i = 0; i < splitValues.Length; i++)
                        {
                            if (i == emptyIndex)
                            {
                                UpString += splitValues[i - sqrt];
                            }
                            else if (i == emptyIndex - sqrt)
                            {
                                UpString += splitValues[emptyIndex];
                            }
                            else
                            {
                                UpString += splitValues[i];
                            }

                            UpString += " ";
                        }

                        UpString = UpString.TrimEnd();
                        bool result = parentTable.TryGetValue(UpString, out string val);

                        if (!result)
                        {
                            parentTable.Add(UpString, currPuzzle);
                            int parentDistance = distanceTable[currPuzzle];
                            paths.Insert(Heuristic(UpString.Split(' '), splitEnd, heuristic) + parentDistance + 1, UpString);
                            distanceTable.Add(UpString, parentDistance + 1);
                        }
                    }
                    if (canMoveDown)
                    {
                        string DownString = "";
                        for (int i = 0; i < splitValues.Length; i++)
                        {
                            if (i == emptyIndex)
                            {
                                DownString += splitValues[i + sqrt];
                            }
                            else if (i == emptyIndex + sqrt)
                            {
                                DownString += splitValues[emptyIndex];
                            }
                            else
                            {
                                DownString += splitValues[i];
                            }

                            DownString += " ";
                        }

                        DownString = DownString.TrimEnd();
                        bool result = parentTable.TryGetValue(DownString, out string val);

                        if (!result)
                        {
                            parentTable.Add(DownString, currPuzzle);
                            int parentDistance = distanceTable[currPuzzle];
                            paths.Insert(Heuristic(DownString.Split(' '), splitEnd, heuristic) + parentDistance + 1, DownString);
                            distanceTable.Add(DownString, parentDistance + 1);
                        }
                    }

                }

            }
            List<string> path = new List<string>();
            path.Add(currPuzzle);

            while (currPuzzle != start)
            {
                path.Add(parentTable[currPuzzle]);
                currPuzzle = parentTable[currPuzzle];
            }

            path.Reverse();

            return path;
        }


        static int PuzzleSum(int PuzzleSize)
        {
            if (PuzzleSize == 0)
            {
                return 0;
            }
            return PuzzleSize + PuzzleSum(PuzzleSize - 1);
        }


        static List<string> FindSolutionIDAStar(string start, string end, int PuzzleSize, HeuristicType heuristic)
        {
            NodeVisited = new Dictionary<string, int>();
            var temp = IDAStar(start, end, end.Split(' '), PuzzleSize, heuristic, 0);
            NodeVisited = new Dictionary<string, int>();
            return temp;
        }

        static Dictionary<string, int> NodeVisited = new Dictionary<string, int>();

        static Dictionary<int, int> MaxPuzzle = new Dictionary<int, int>()
        {
            [9] = 31,
            [16] = 84,
            [25] = 215
        };


        static List<string> IDAStar(string current, string end, string[] splitEnd, int PuzzleSize, HeuristicType heuristic, int g)
        {
            if (current == end)
            {
                return new List<string>() { current };
            }

            string[] splitValues = current.Split(' ');


            bool isVisited = NodeVisited.TryGetValue(current, out int prevG);
            if (isVisited && prevG <= g)
            {
                return null;
            }
            else if (isVisited)
            {
                NodeVisited[current] = g;
            }
            else
            {
                NodeVisited.Add(current, g);
            }

            if (Heuristic(splitValues, splitEnd, heuristic) + g > MaxPuzzle[PuzzleSize])
            {
                return null;
            }

            permutations++;

            int emptyIndex = -1;
            for (int i = 0; i < splitValues.Length; i++)
            {
                if (splitValues[i] == "e")
                {
                    emptyIndex = i;
                    break;
                }
            }

            if (emptyIndex != -1)
            {
                bool canMoveLeft = false;
                bool canMoveRight = false;
                bool canMoveUp = false;
                bool canMoveDown = false;
                List<string> possibilities = new List<string>();

                int sqrt = (int)Math.Sqrt(PuzzleSize);

                if (emptyIndex % sqrt == 0)
                {
                    canMoveRight = true;
                }
                else if (emptyIndex % sqrt == sqrt - 1)
                {
                    canMoveLeft = true;
                }
                else //if(emptyIndex % Math.Sqrt(PuzzleSize) == 1)
                {
                    canMoveRight = true;
                    canMoveLeft = true;
                }

                if (emptyIndex < sqrt)
                {
                    canMoveDown = true;
                }
                else if (emptyIndex < sqrt * (sqrt - 1))
                {
                    canMoveDown = true;
                    canMoveUp = true;
                }
                else
                {
                    canMoveUp = true;
                }


                if (canMoveLeft)
                {
                    string LeftString = "";
                    for (int i = 0; i < splitValues.Length; i++)
                    {
                        if (i == emptyIndex - 1)
                        {
                            LeftString += splitValues[i + 1];
                            LeftString += " ";
                            LeftString += splitValues[i];
                            LeftString += " ";
                            i++;
                        }
                        else
                        {
                            LeftString += splitValues[i];
                            LeftString += " ";
                        }
                    }

                    LeftString = LeftString.TrimEnd();
                    possibilities.Add(LeftString);


                }
                if (canMoveRight)
                {
                    string RightString = "";
                    for (int i = 0; i < splitValues.Length; i++)
                    {
                        if (i == emptyIndex)
                        {
                            RightString += splitValues[i + 1];
                            RightString += " ";
                            RightString += splitValues[i];
                            RightString += " ";
                            i++;
                        }
                        else
                        {
                            RightString += splitValues[i];
                            RightString += " ";
                        }
                    }

                    RightString = RightString.TrimEnd();
                    possibilities.Add(RightString);

                }
                if (canMoveUp)
                {
                    string UpString = "";
                    for (int i = 0; i < splitValues.Length; i++)
                    {
                        if (i == emptyIndex)
                        {
                            UpString += splitValues[i - sqrt];
                        }
                        else if (i == emptyIndex - sqrt)
                        {
                            UpString += splitValues[emptyIndex];
                        }
                        else
                        {
                            UpString += splitValues[i];
                        }

                        UpString += " ";
                    }

                    UpString = UpString.TrimEnd();
                    possibilities.Add(UpString);
                }
                if (canMoveDown)
                {
                    string DownString = "";
                    for (int i = 0; i < splitValues.Length; i++)
                    {
                        if (i == emptyIndex)
                        {
                            DownString += splitValues[i + sqrt];
                        }
                        else if (i == emptyIndex + sqrt)
                        {
                            DownString += splitValues[emptyIndex];
                        }
                        else
                        {
                            DownString += splitValues[i];
                        }

                        DownString += " ";
                    }

                    DownString = DownString.TrimEnd();
                    possibilities.Add(DownString);
                }

                int possible = 0;
                List<int> HeuristicList = new List<int>();

                foreach (var c in possibilities)
                {
                    HeuristicList.Add(Heuristic(c.Split(), splitEnd, HeuristicType.Manhattan));
                }
                while (possible < possibilities.Count)
                {
                    int smallestIndex = 0;
                    for (int i = 0; i < possibilities.Count; i++)
                    {
                        if (HeuristicList[i] < HeuristicList[smallestIndex])
                        {
                            smallestIndex = i;
                        }
                    }
                    var result = IDAStar(possibilities[smallestIndex], end, splitEnd, PuzzleSize, heuristic, g + 1);
                    HeuristicList.RemoveAt(smallestIndex);
                    possibilities.RemoveAt(smallestIndex);
                    if (result != null)
                    {
                        result.Insert(0, current);
                        return result;
                    }
                }

            }
            return null;
        }


        static int LinearConflict(string[] current, string[] end)
        {
            int size = (int)Math.Sqrt(end.Length);
            int conflictCount = 0;
            for (int i = 0; i < size; i++)
            {
                string[] inCurrent = new string[size];
                string[] inEnd = new string[size];
                for (int j = 0; j < size; j++)
                {
                    inCurrent[j] = current[i + size * j];
                }
                for (int j = 0; j < size; j++)
                {
                    inEnd[j] = end[i + size * j];
                }
                int verticalCount = 0;
                for (int j = 0; j < size; j++)
                {
                    if (inEnd[j] != current[j] && Contains(inEnd, current[j]))
                    {
                        verticalCount++;
                    }
                }
                if (verticalCount >= 2)
                {
                    conflictCount += (2 * verticalCount);
                }


                for (int j = 0; j < size; j++)
                {
                    inCurrent[j] = current[i * size + j];
                }
                for (int j = 0; j < size; j++)
                {
                    inEnd[j] = end[i * size + j];
                }

                int horizontalCount = 0;
                for (int j = 0; j < size; j++)
                {
                    if (inEnd[j] != current[j] && Contains(inEnd, current[j]))
                    {
                        horizontalCount++;
                    }
                }
                if (horizontalCount >= 2)
                {
                    conflictCount += (2 * horizontalCount);
                }
            }
            return conflictCount;

        }

        static bool Contains(string[] array, string value)
        {
            foreach (var item in array)
            {
                if (item == value)
                {
                    return true;
                }
            }
            return false;
        }


        static List<string> FindSolutionRealIDAStar(string start, string end, int PuzzleSize, HeuristicType heuristic)
        {
            int threshold = Heuristic(start.Split(' '), end.Split(' '), heuristic);
            while (true)
            {
                NodeVisited = new Dictionary<string, int>();
                var temp = RealIDAStar(start, end, end.Split(' '), PuzzleSize, heuristic, 0, threshold);
                NodeVisited = new Dictionary<string, int>();
                if (temp != null)
                {
                    return temp;
                }
                threshold += 2;
            }
        }


        static List<string> RealIDAStar(string current, string end, string[] splitEnd, int PuzzleSize, HeuristicType heuristic, int gScore, int threshold)
        {
            if (current == end)
            {
                return new List<string>() { current };
            }

            string[] splitValues = current.Split(' ');


            bool isVisited = NodeVisited.TryGetValue(current, out int prevG);
            if (isVisited && prevG <= gScore)
            {
                return null;
            }
            else if (isVisited)
            {
                NodeVisited[current] = gScore;
            }
            else
            {
                NodeVisited.Add(current, gScore);
            }

            if (Heuristic(splitValues, splitEnd, heuristic) + 2 + gScore > threshold)
            {
                return null;
            }

            permutations++;

            int emptyIndex = -1;
            for (int i = 0; i < splitValues.Length; i++)
            {
                if (splitValues[i] == "e")
                {
                    emptyIndex = i;
                    break;
                }
            }

            if (emptyIndex != -1)
            {
                bool canMoveLeft = false;
                bool canMoveRight = false;
                bool canMoveUp = false;
                bool canMoveDown = false;
                List<string> possibilities = new List<string>();

                int sqrt = (int)Math.Sqrt(PuzzleSize);

                if (emptyIndex % sqrt == 0)
                {
                    canMoveRight = true;
                }
                else if (emptyIndex % sqrt == sqrt - 1)
                {
                    canMoveLeft = true;
                }
                else //if(emptyIndex % Math.Sqrt(PuzzleSize) == 1)
                {
                    canMoveRight = true;
                    canMoveLeft = true;
                }

                if (emptyIndex < sqrt)
                {
                    canMoveDown = true;
                }
                else if (emptyIndex < sqrt * (sqrt - 1))
                {
                    canMoveDown = true;
                    canMoveUp = true;
                }
                else
                {
                    canMoveUp = true;
                }


                if (canMoveLeft)
                {
                    string LeftString = "";
                    for (int i = 0; i < splitValues.Length; i++)
                    {
                        if (i == emptyIndex - 1)
                        {
                            LeftString += splitValues[i + 1];
                            LeftString += " ";
                            LeftString += splitValues[i];
                            LeftString += " ";
                            i++;
                        }
                        else
                        {
                            LeftString += splitValues[i];
                            LeftString += " ";
                        }
                    }

                    LeftString = LeftString.TrimEnd();
                    possibilities.Add(LeftString);


                }
                if (canMoveRight)
                {
                    string RightString = "";
                    for (int i = 0; i < splitValues.Length; i++)
                    {
                        if (i == emptyIndex)
                        {
                            RightString += splitValues[i + 1];
                            RightString += " ";
                            RightString += splitValues[i];
                            RightString += " ";
                            i++;
                        }
                        else
                        {
                            RightString += splitValues[i];
                            RightString += " ";
                        }
                    }

                    RightString = RightString.TrimEnd();
                    possibilities.Add(RightString);

                }
                if (canMoveUp)
                {
                    string UpString = "";
                    for (int i = 0; i < splitValues.Length; i++)
                    {
                        if (i == emptyIndex)
                        {
                            UpString += splitValues[i - sqrt];
                        }
                        else if (i == emptyIndex - sqrt)
                        {
                            UpString += splitValues[emptyIndex];
                        }
                        else
                        {
                            UpString += splitValues[i];
                        }

                        UpString += " ";
                    }

                    UpString = UpString.TrimEnd();
                    possibilities.Add(UpString);
                }
                if (canMoveDown)
                {
                    string DownString = "";
                    for (int i = 0; i < splitValues.Length; i++)
                    {
                        if (i == emptyIndex)
                        {
                            DownString += splitValues[i + sqrt];
                        }
                        else if (i == emptyIndex + sqrt)
                        {
                            DownString += splitValues[emptyIndex];
                        }
                        else
                        {
                            DownString += splitValues[i];
                        }

                        DownString += " ";
                    }

                    DownString = DownString.TrimEnd();
                    possibilities.Add(DownString);
                }

                int possible = 0;
                List<int> HeuristicList = new List<int>();

                foreach (var c in possibilities)
                {
                    HeuristicList.Add(Heuristic(c.Split(), splitEnd, HeuristicType.Manhattan));
                }
                while (possible < possibilities.Count)
                {
                    int smallestIndex = 0;
                    for (int i = 0; i < possibilities.Count; i++)
                    {
                        if (HeuristicList[i] < HeuristicList[smallestIndex])
                        {
                            smallestIndex = i;
                        }
                    }
                    var result = RealIDAStar(possibilities[smallestIndex], end, splitEnd, PuzzleSize, heuristic, gScore + 1, threshold);
                    HeuristicList.RemoveAt(smallestIndex);
                    possibilities.RemoveAt(smallestIndex);
                    if (result != null)
                    {
                        result.Insert(0, current);
                        return result;
                    }
                }

            }
            return null;

        }
    }
}
