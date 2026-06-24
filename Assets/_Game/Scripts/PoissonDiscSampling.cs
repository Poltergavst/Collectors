using System.Collections.Generic;
using UnityEngine;

public static class PoissonDiscSampling
{
    public static List<Vector2> GeneratePoints(float radius, Vector2 regionSize, int triesBeforeRejection = 30)
    {
        float cellSize = radius / Mathf.Sqrt(2);
        
        int[,] grid = new int[Mathf.CeilToInt(regionSize.x / cellSize), Mathf.CeilToInt(regionSize.y / cellSize)];  
        
        List<Vector2> validPoints = new();

        List<Vector2> spawnPoints = new()
        {
            regionSize * MathConstants.Half
        };

        while (spawnPoints.Count > 0)
        {
            bool candidateAccepted = false;
            int spawnIndex = Random.Range(0, spawnPoints.Count);

            Vector2 spawnCentre = spawnPoints[spawnIndex];

            for (int i = 0; i < triesBeforeRejection; i++)
            {
                float angle = Random.value * Mathf.PI * 2;

                Vector2 direction = new(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate = spawnCentre + direction * Random.Range(radius, 2 * radius);

                int cellX = (int)(candidate.x / cellSize);
                int cellY = (int)(candidate.y / cellSize);

                if (IsValid(candidate, regionSize, cellX, cellY, radius, validPoints, grid))
                {
                    validPoints.Add(candidate);
                    spawnPoints.Add(candidate);

                    grid[cellX, cellY] = validPoints.Count;

                    candidateAccepted = true;
                    break;
                }
            }

            if (candidateAccepted == false)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        return validPoints;
    }

    private static bool IsValid(Vector2 candidate, Vector2 regionSize, int cellX, int cellY, float radius, List<Vector2> points, int[,] grid)
    {
        if (IsWithinRegion(candidate, regionSize))
        {
            int cellsFromCenter = 2;

            int searchStartX = Mathf.Max(0, cellX - cellsFromCenter);
            int searchEndX = Mathf.Min(grid.GetLength(0) - 1, cellX + cellsFromCenter);
            int searchStartY = Mathf.Max(0, cellY - cellsFromCenter);
            int searchEndY = Mathf.Min(grid.GetLength(1) - 1, cellY + cellsFromCenter);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;

                    bool isCellOccupied = pointIndex != -1;

                    if (isCellOccupied)
                    {
                        float distance = (candidate - points[pointIndex]).sqrMagnitude;

                        if (distance < radius * radius)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        return false;
    }

    private static bool IsWithinRegion(Vector2 candidate, Vector2 regionSize)
    {
        return new Rect(Vector2.zero, regionSize).Contains(candidate);
    }
}
