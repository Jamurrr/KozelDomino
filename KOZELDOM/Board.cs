using KOZELDOM;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public class Board
{
    public List<Tile> tiles;

    public Board()
    {
        tiles = new List<Tile>();
    }

    //Первый бул (можно ли ходить), второй - нужно ли переворачивать картинку
    public (bool, bool) CheckValidMove(Tile tile, bool addToLeft = false)
    {
        if (tiles.Count == 0)
        {
            return (true, false); // Любая кость может быть добавлена, если доска пустая.
        }

        Tile leftEnd = tiles.First();
        Tile rightEnd = tiles.Last();
        int rememberTileValue = 0;

        
        if (tiles.Count == 1)
        {
            if (addToLeft)
            {
                if (leftEnd.Value1 == tile.Value1)
                {
                    rememberTileValue = tile.Value1;
                    tile.Value1 = tile.Value2;
                    tile.Value2 = rememberTileValue;

                    return (true, true);
                }
                if (leftEnd.Value1 == tile.Value2) 
                {
                    return (true, false);
                } 
                
            }
            else
            {
                if (leftEnd.Value2 == tile.Value1) 
                    return (true, false);

                if (leftEnd.Value2 == tile.Value2)
                {
                    rememberTileValue = tile.Value1;
                    tile.Value1 = tile.Value2;
                    tile.Value2 = rememberTileValue;

                    return (true, true);
                } 
                
            }
        }
        else
        {
            if (addToLeft)
            {
                if (leftEnd.Value1 == tile.Value1) 
                {
                    rememberTileValue = tile.Value1;
                    tile.Value1 = tile.Value2;
                    tile.Value2 = rememberTileValue;

                    return (true, true);
                } 
                if (leftEnd.Value1 == tile.Value2)
                {
                    return (true, false);
                }
                
            }
            else
            {
                if (rightEnd.Value2 == tile.Value1) 
                    return (true, false);

                if (rightEnd.Value2 == tile.Value2)
                {
                    rememberTileValue = tile.Value1;
                    tile.Value1 = tile.Value2;
                    tile.Value2 = rememberTileValue;
                    return (true, true);
                }
                
            }
        }
        return (false, false);
    }

    public void AddTileToBoard(Tile tile, bool addToLeft = false)
    {
        if (tiles.Count == 0)
        {
            tiles.Add(tile); // Первая кость может быть добавлена напрямую.
        }
        else if (addToLeft)
        {
            tiles.Insert(0, tile); // Добавляем кость влево.
        }
        else
        {
            tiles.Add(tile); // Добавляем кость вправо.
        }
    }

    public List<Tile> GetTiles()
    {
        return tiles;
    }
}
