using KOZELDOM;
using System.Collections.Generic;
using System.Windows.Forms;

public class Player
{
    public string name { get; set; }

    public int score;
    public List<Tile> hand { get; set; }

    public Player(string name)
    {
        this.name = name;
        this.score = 0;
        this.hand = new List<Tile>();
    }


    public int CalculateScore(bool addToScore = false)
    {
        // Calculate the total score based on the tiles in the hand
        int totalScore = score;
        foreach (var tile in hand)
        {
            totalScore += tile.Value1 + tile.Value2;
        }
        if (totalScore < 25) 
        { 
            totalScore = 0;
        }
        if (addToScore)
        {
            MessageBox.Show($"score = {score}, totalscore = {totalScore}");
            score = totalScore;
        }
        return totalScore;
    }

    public bool CheckRoundEnd()
    {
        return hand.Count == 0;
    }


}
