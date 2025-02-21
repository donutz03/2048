namespace game2048cs.model;

public class GameAI
{
    private const int DepthLimit = 4;  
    private const double EmptyCellWeight = 2.7;  
    private const double MonotonicityWeight = 1.0;  
    private const double SmoothnessWeight = 0.1;  
    
    public enum Direction { Up, Down, Left, Right }

    public Direction GetBestMove(int[,] grid)
    {
        var (bestMove, _) = ExpectimaxRoot(grid, DepthLimit);
        return bestMove;
    }

    private (Direction bestMove, double score) ExpectimaxRoot(int[,] grid, int depth)
    {
        double bestScore = double.MinValue;
        Direction bestMove = Direction.Up;

        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            var newGrid = SimulateMove(grid, dir);
            if (GridsEqual(grid, newGrid)) continue;  

            double score = Expectimax(newGrid, depth - 1, false);
            if (score > bestScore)
            {
                bestScore = score;
                bestMove = dir;
            }
        }

        return (bestMove, bestScore);
    }

    private double Expectimax(int[,] grid, int depth, bool isPlayerTurn)
    {
        if (depth == 0) return EvaluateGrid(grid);

        if (isPlayerTurn)
        {
            double maxScore = double.MinValue;
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                var newGrid = SimulateMove(grid, dir);
                if (GridsEqual(grid, newGrid)) continue;

                double score = Expectimax(newGrid, depth - 1, false);
                maxScore = Math.Max(maxScore, score);
            }
            return maxScore == double.MinValue ? EvaluateGrid(grid) : maxScore;
        }
        else
        {
            double totalScore = 0;
            int emptyCells = 0;
            
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (grid[i,j] == 0)
                    {
                        emptyCells++;
                        var newGrid = (int[,])grid.Clone();
                        newGrid[i,j] = 2;
                        totalScore += 0.9 * Expectimax(newGrid, depth - 1, true);

                        newGrid = (int[,])grid.Clone();
                        newGrid[i,j] = 4;
                        totalScore += 0.1 * Expectimax(newGrid, depth - 1, true);
                    }
                }
            }

            return emptyCells == 0 ? EvaluateGrid(grid) : totalScore / (emptyCells * 2);
        }
    }

    private double EvaluateGrid(int[,] grid)
    {
        double score = 0;

        score += CountEmptyCells(grid) * EmptyCellWeight;

        score += CalculateMonotonicity(grid) * MonotonicityWeight;

        score += CalculateSmoothness(grid) * SmoothnessWeight;

        score += EvaluateCorners(grid);

        return score;
    }

    private int CountEmptyCells(int[,] grid)
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                if (grid[i,j] == 0) count++;
        return count;
    }

    private double CalculateMonotonicity(int[,] grid)
    {
        double[] scores = new double[4]; 

        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                if (grid[i,j-1] != 0 && grid[i,j] != 0)
                {
                    double current = Math.Log(grid[i,j], 2);
                    double previous = Math.Log(grid[i,j-1], 2);
                    if (current >= previous)
                        scores[0] += current - previous;
                    else
                        scores[1] += previous - current;
                }
            }
        }

        for (int j = 0; j < 4; j++)
        {
            for (int i = 1; i < 4; i++)
            {
                if (grid[i-1,j] != 0 && grid[i,j] != 0)
                {
                    double current = Math.Log(grid[i,j], 2);
                    double previous = Math.Log(grid[i-1,j], 2);
                    if (current >= previous)
                        scores[2] += current - previous;
                    else
                        scores[3] += previous - current;
                }
            }
        }

        return Math.Max(scores[0], scores[1]) + Math.Max(scores[2], scores[3]);
    }

    private double CalculateSmoothness(int[,] grid)
    {
        double smoothness = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (grid[i,j] != 0)
                {
                    double value = Math.Log(grid[i,j], 2);
                    if (j < 3 && grid[i,j+1] != 0)
                        smoothness -= Math.Abs(value - Math.Log(grid[i,j+1], 2));
                    if (i < 3 && grid[i+1,j] != 0)
                        smoothness -= Math.Abs(value - Math.Log(grid[i+1,j], 2));
                }
            }
        }
        return smoothness;
    }

    private double EvaluateCorners(int[,] grid)
    {
        double score = 0;
        int maxValue = 0;
        int maxI = 0, maxJ = 0;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (grid[i,j] > maxValue)
                {
                    maxValue = grid[i,j];
                    maxI = i;
                    maxJ = j;
                }
            }
        }

        if ((maxI == 0 || maxI == 3) && (maxJ == 0 || maxJ == 3))
            score += Math.Log(maxValue, 2) * 2;

        return score;
    }

    private int[,] SimulateMove(int[,] grid, Direction dir)
    {
        var newGrid = (int[,])grid.Clone();
        bool merged = false;

        switch (dir)
        {
            case Direction.Left:
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (newGrid[i,j] != 0)
                        {
                            for (int k = j + 1; k < 4; k++)
                            {
                                if (newGrid[i,k] != 0)
                                {
                                    if (newGrid[i,j] == newGrid[i,k])
                                    {
                                        newGrid[i,j] *= 2;
                                        newGrid[i,k] = 0;
                                        merged = true;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    for (int j = 0; j < 3; j++)
                    {
                        if (newGrid[i,j] == 0)
                        {
                            for (int k = j + 1; k < 4; k++)
                            {
                                if (newGrid[i,k] != 0)
                                {
                                    newGrid[i,j] = newGrid[i,k];
                                    newGrid[i,k] = 0;
                                    merged = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                break;

            // TODO: Similar implementations for RIGHT, UP, and DOWN...
        }

        return merged ? newGrid : grid;
    }

    private bool GridsEqual(int[,] grid1, int[,] grid2)
    {
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                if (grid1[i,j] != grid2[i,j]) return false;
        return true;
    }
}
