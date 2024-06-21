using KOZELDOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

public class GameController
{
    public List<Player> players;
    public Board board;
    public Boneyard boneyard;
    public int currentPlayerIndex;

    public GameController(Player player1, Player player2)
    {
        players = new List<Player>();
        board = new Board();
        boneyard = new Boneyard();
        currentPlayerIndex = 0;

        players.Add(new Player(player1.name));
        players.Add(new Player(player2.name));
    }


    public static Boneyard Boneyard { get; } = new Boneyard();

    public void StartGame()
    {
        // Инициализация игроков и раздача начальных костей
    }

    public void EndGame()
    {
        // Определение победителя и объявление результатов
    }

    public bool DetermineWinner()
    {
        Player loser = null;
        int highestScore = 125;
        foreach (var player in players)
        {
            if (player.CalculateScore() > highestScore)
            {
                player.CalculateScore();
                loser = player;
                MessageBox.Show($"{loser.name} - козел!");
                return true;
            }
        }
        return false;
    }

    public Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }

    public void NextTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
    }

    public bool PlayTurn(Tile tile, bool addToLeft)
    {
        Player currentPlayer = GetCurrentPlayer();
        var rotatePicture = tile.isRotate;
        if (MakeMove(currentPlayer, tile, out rotatePicture, addToLeft))
        {
            tile.isRotate = rotatePicture;
            NextTurn();
            return true;
            
        }
        else
        {
            MessageBox.Show("Нельзя так ходить");
        }
        return false;
    }

    public void SetFirstPlayer()
    {
        var minTile = GetMinTile();
        
        Player playerFirst = (from p in players where p.hand.Contains(minTile) select p).FirstOrDefault();
        currentPlayerIndex = playerFirst == null ? new Random().Next(0, players.Count - 1) : players.IndexOf(playerFirst);
    }

    public void MakeFirstMove()
    {
        MakeMove(GetCurrentPlayer(), GetMinTile(), out bool rotatePicture);
        NextTurn();
    }

    public Tile GetMinTile()
    {
        Tile minTile = new Tile(7, 7, "OVO");
        
        foreach (var player in players)
        {
            foreach (var hand in player.hand)
            {
                if (hand.Value1 == hand.Value2)
                {
                    minTile = Tile.GetMin(minTile, hand);

                }
            }
        }
        
        return minTile;
    }

    public bool MakeMove(Player player, Tile tile, out bool rotatePicture, bool addToLeft = false)
    {
        // Check if the move is valid and add the tile to the board
        var validMove = board.CheckValidMove(tile, addToLeft);
        rotatePicture = validMove.Item2;
        if (validMove.Item1)
        {
            board.AddTileToBoard(tile, addToLeft);
            player.hand.Remove(tile);
            return true;
        }

        return false;
    }

    public bool EndRound(bool fish = false)
    {
        if (fish)
        {
            MessageBox.Show($"Рыба!");
            foreach (var pl in players)
            {
                pl.CalculateScore(true);
            }
            DetermineWinner();
            return true;
        }
        Player player = players[(currentPlayerIndex + 1) % players.Count];
        if (player.CheckRoundEnd())
        {
            MessageBox.Show($"{player.name} выиграл раунд!");
            foreach (var pl in players)
            {
                pl.CalculateScore(true);
            }
            DetermineWinner();
            return true;
        }
        return false;
    }

    public void TakeTileFromBoneyard()
    {
        if (!boneyard.IsEmpty())
        {
            Tile tile = boneyard.GetRandomTile();
            GetCurrentPlayer().hand.Add(tile);
            
        }
        else
        {
            MessageBox.Show("Базар пуст!");
        }
    }
}
