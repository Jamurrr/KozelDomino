using KOZELDOM;
using System;
using System.Collections.Generic;
using System.Linq;

public class Boneyard
{
    public List<Tile> tiles;

    public Boneyard()
    {
        tiles = new List<Tile>();
        // Инициализация костяшек со значениями от 0-0 до 6-6
        for (int i = 0; i <= 6; i++)
        {
            for (int j = i; j <= 6; j++)
            {
                tiles.Add(new Tile(i, j, $"E:\\c#\\KOZELDOM\\KOZELDOM\\picture\\{i} {j}.png"));
             
            }
        }
    }

    public Tile GetRandomTile()
    {
        // Get a random tile from the boneyard
        // Remove the tile from the list
        if (tiles.Count > 0)
        {
            Random random = new Random();
            int index = random.Next(tiles.Count);
            Tile tile = tiles[index];
            tiles.RemoveAt(index);
            return tile;
        }
        return null; // Placeholder logic
    }

    public bool IsEmpty()
    {
        // Check if the boneyard is empty
        return tiles.Count == 0;
    }
}