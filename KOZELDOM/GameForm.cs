using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KOZELDOM
{
    public partial class GameForm : Form
    {


        // Массив PictureBox для костей игрока
        private PictureBox[] playerBonesPictureBoxes = new PictureBox[7];

        // Счетчик костей игрока
        private int boneCount = 0;

        public Player player1 = new Player("Вовка");
        public Player player2 = new Player("Петька");
        
        GameController game;
        List<PictureBox> PickHand1 = new List<PictureBox>() {};
        List<PictureBox> PickHand2 = new List<PictureBox>() {};

        List<PictureBox> LeftBoard = new List<PictureBox>() {};
        List<PictureBox> RightBoard = new List<PictureBox>() {};

        List<PictureBox> RightRotatePictureBox = new List<PictureBox> { };
        List<PictureBox> LeftRotatePictureBox = new List<PictureBox> { };
        List<PictureBox> VerticalRotatePictureBox = new List<PictureBox> { };

        int buttonPassClickedCount = 0;

        private PictureBox selectedPictureBox;


        public GameForm()
        {
            InitializeComponent();
            StartRound();
        }

        public void StartRound()
        {
            game = new GameController(player1, player2);
            int tileSum = 0;
            int score1, score2;
            if (int.TryParse(Score1.Text, out score1))
                game.players[0].score = score1;
            if (int.TryParse(Score2.Text, out score2))
                game.players[1].score = score2;


            StartHand();
            boneyardLabel.Text = game.boneyard.tiles.Count().ToString();
            GameForm_Load();
            ClearBoard();
            game.SetFirstPlayer();
            SetPictureBoxClick();

            if (game.GetMinTile().filepath != "OVO")
            {
                game.MakeFirstMove();
            }
            UpdateBoard();
        }

        private void GameForm_Load()
        {
            player1name.Text = player1.name;
            player2name.Text = player2.name;

            PickHand1 = new List<PictureBox>() { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox52, pictureBox53, pictureBox54, pictureBox58, pictureBox59, pictureBox60 };
            PickHand2 = new List<PictureBox>() { pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox55, pictureBox56, pictureBox57, pictureBox61, pictureBox62, pictureBox63 };

            LeftBoard = new List<PictureBox>() { pictureBox31, pictureBox30, pictureBox29, pictureBox28, pictureBox27, pictureBox26,
                    pictureBox25, pictureBox24, pictureBox23, pictureBox22, pictureBox21, pictureBox20, pictureBox19, pictureBox18, pictureBox17,
                    pictureBox16, pictureBox15};

            RightBoard = new List<PictureBox>() { pictureBox33, pictureBox34, pictureBox35, pictureBox36, pictureBox37, pictureBox38,
                    pictureBox39, pictureBox40, pictureBox41, pictureBox42, pictureBox43, pictureBox44, pictureBox45, pictureBox46, pictureBox47,
                    pictureBox48, pictureBox49, pictureBox50, pictureBox51};

            RightRotatePictureBox = new List<PictureBox> { pictureBox37, pictureBox38, pictureBox39, pictureBox40, pictureBox41, 
                pictureBox42, pictureBox43, pictureBox44, pictureBox45 };

            LeftRotatePictureBox = new List<PictureBox> { pictureBox26 };

            VerticalRotatePictureBox = new List<PictureBox> { pictureBox25, pictureBox24, pictureBox23, pictureBox22, 
                pictureBox21, pictureBox20, pictureBox19, pictureBox18, pictureBox17, pictureBox16};

            var player1Hand = game.players[0].hand;
            var player2Hand = game.players[1].hand;

            UpdateHand(PickHand1, player1Hand);
            UpdateHand(PickHand2, player2Hand);

            
        }
        

        private void UpdateBoard(bool addToLeft = false)
        {

            if (game.board.tiles.Count == 1)
            {
                pictureBox32.Image = Image.FromFile(game.board.tiles[0].filepath);
                pictureBox32.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox32.Tag = game.board.tiles[0];

                PictureBox pb1 = (from pb in PickHand1 where pb.Tag == pictureBox32.Tag select pb).FirstOrDefault();
                PictureBox pb2 = (from pb in PickHand2 where pb.Tag == pictureBox32.Tag select pb).FirstOrDefault();

                RemoveFromHand(PickHand1, pb1);
                RemoveFromHand(PickHand2, pb2);
            }
            else if (game.board.tiles.Count > 1)
            {

                if (selectedPictureBox != null)
                {
                    if (addToLeft)
                    {
                        foreach (PictureBox pb in LeftBoard)
                        {
                            if (pb.Image == null)
                            {
                                pb.Image = Image.FromFile(game.board.tiles.First().filepath);
                                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                                pb.Tag = game.board.tiles.First();
                                if (game.board.tiles.First().isRotate)
                                {
                                    pb.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                    pb.Invalidate();
                                }
                                break;
                            }
                        }
                    }
                    if (!addToLeft)
                    {
                        foreach (PictureBox pb in RightBoard)
                        {
                            if (pb.Image == null)
                            {
                                pb.Image = Image.FromFile(game.board.tiles.Last().filepath);
                                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                                pb.Tag = game.board.tiles.Last();
                                if (game.board.tiles.Last().isRotate)
                                {
                                    pb.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                    pb.Invalidate();
                                }
                                break;
                            }

                        }
                    }
                }
            }
            MakeInvisibleHand();
            RotateToRight();
            RotateToLeft();
            VerticalRotate();
            
        }

        private void MakeInvisibleHand()
        {

            if (game.GetCurrentPlayer() == game.players[0])
            {
                foreach (PictureBox enemyHand in PickHand2)
                {
                    enemyHand.Visible = false;
                }
                foreach (PictureBox playerHand in PickHand1)
                {
                    playerHand.Visible = true;
                }

            }
            if (game.GetCurrentPlayer() == game.players[1])
            {
                foreach (PictureBox enemyHand in PickHand1)
                {
                    enemyHand.Visible = false;
                }
                foreach (PictureBox playerHand in PickHand2)
                {
                    playerHand.Visible = true;
                }

            }

        }


        private void RemoveFromHand(List<PictureBox> PickHand, PictureBox removePictureBox)
        {
            foreach (PictureBox pb in PickHand)
            {
                if (removePictureBox != null)
                {
                    if (pb.Tag == removePictureBox.Tag)
                    {
                        pb.Image = null;
                    }
                }
            }
        }
        private void SetPictureBoxClick()
        {
            foreach (PictureBox pb in PickHand1)
            {
                pb.Click += PictureBox_Click;
            }
            foreach (PictureBox pb in PickHand2)
            {
                pb.Click += PictureBox_Click;
            }
        }
        
        public void UpdateHand(List<PictureBox> PickHand, List<Tile> PlayerHand)
        {
            for (int i = 0; i < PickHand.Count; i++)
            {
                if (i < PlayerHand.Count)
                {
                    PickHand[i].Image = Image.FromFile(PlayerHand[i].filepath);
                    PickHand[i].Tag = PlayerHand[i];
                    PickHand[i].SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    PickHand[i].Image = null;
                    PickHand[i].Tag = null;
                }
            }
        }

        private void ClearBoard()
        {
            foreach(PictureBox pb in RightBoard)
            {
                pb.Image = null;
                pb.Tag = null;
            }
            foreach (PictureBox pb in LeftBoard)
            {
                pb.Image = null;
                pb.Tag = null;
            }
            pictureBox32.Image = null;
            pictureBox32.Tag = null;
        }
        
        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;
            buttonPassClickedCount = 0;
            selectedPictureBox = clickedPictureBox; 
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Score2_Click(object sender, EventArgs e)
        {

        }
        private void StartHand()
        {
            // Раздаем кости игрокам
            for (int i = 0; i < 7; i++)
            {
                game.players[0].hand.Add(game.boneyard.GetRandomTile());
                game.players[1].hand.Add(game.boneyard.GetRandomTile());
            }
        }
        private void Leftgo_Click(object sender, EventArgs e)
        {
            /* MakeMove(domino, true)*/
            if (game.PlayTurn(selectedPictureBox.Tag as Tile, true))
            {
                UpdateBoard(true);
                selectedPictureBox.Image = null;
                selectedPictureBox = null;
                CheckEndRound();
            }
        }

        private void Rightgo_Click(object sender, EventArgs e)
        {
            // Проверка, разрешено ли текущему игроку нажимать на PictureBox
            if (game.PlayTurn(selectedPictureBox.Tag as Tile, false))
            {
                UpdateBoard(false);
                selectedPictureBox.Image = null;
                selectedPictureBox = null;
                CheckEndRound();
            }
        }

        private void BoneyardButton_Click(object sender, EventArgs e)
        {
            game.TakeTileFromBoneyard();
            boneyardLabel.Text = game.boneyard.tiles.Count.ToString();
            if (game.boneyard.tiles.Count == 0)
            {
                buttonPass.Visible = true;
            }
            var index = game.currentPlayerIndex;

            if (index == 0)
                UpdateHand(PickHand1, game.players[index].hand);
            else
                UpdateHand(PickHand2, game.players[index].hand);
        }

        public void CheckEndRound(bool fish = false)
        {
            if (game.EndRound(fish))
            {
                PickHand1 = null;
                PickHand2 = null;
                RightBoard = null;
                LeftBoard = null;
                RightRotatePictureBox = null;
                LeftRotatePictureBox = null;
                VerticalRotatePictureBox = null;
                MessageBox.Show($"у вовки {game.players[0].score} у петьки {game.players[1].score}");
                Score1.Text = game.players[0].score.ToString();
                Score2.Text = game.players[1].score.ToString();
                foreach (var player in game.players)
                    player.hand = null;
                game = null;

                StartRound();
            }
            
        }

        public void RotateToRight()
        {
            PictureBox removePB = null;
            foreach(var pb in RightRotatePictureBox)
            {
                if (pb.Image != null)
                {
                    removePB = pb;
                    pb.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    pb.Refresh();
                    break;
                }   
            }
            if (removePB != null)
                RightRotatePictureBox.Remove(removePB);

        }

        public void RotateToLeft()
        {
            PictureBox removePB = null;
            foreach (var pb in LeftRotatePictureBox)
            {
                if (pb.Image != null)
                {
                    removePB = pb;
                    pb.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    pb.Refresh();
                    break;
                }
            }
            if (removePB != null)
                LeftRotatePictureBox.Remove(removePB);
        }

        public void VerticalRotate()
        {
            PictureBox removePB = null;
            foreach (var pb in VerticalRotatePictureBox)
            {
                if (pb.Image != null)
                {
                    removePB = pb;
                    pb.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    pb.Refresh();
                    break;
                }
            }
            if (removePB != null)
                VerticalRotatePictureBox.Remove(removePB);
        }

        private void buttonPass_Click(object sender, EventArgs e)
        {
            buttonPassClickedCount++;
            game.NextTurn();
            if (buttonPassClickedCount == 2)
            {
                CheckEndRound(true);
                buttonPassClickedCount = 0;
                buttonPass.Visible = false;
            }
            UpdateBoard();

        }
    }
}
