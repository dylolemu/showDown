using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace showDown
{
    public partial class Form1 : Form
    {
        SolidBrush ammoBrush = new SolidBrush(Color.Black);

        List<Ammo> ammunition = new List<Ammo>();
        List<Ammo> territory = new List<Ammo>();
        List<Ammo> player = new List<Ammo>();
        List<Ammo> area = new List<Ammo>();
        List<Ammo> area2 = new List<Ammo>();

        List<int> ammoDelete = new List<int>();
        List<int> bombDelete = new List<int>();
        List<int> areaDelete = new List<int>();
        List<int> areaDelete2 = new List<int>();

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            onStart();
        }

        int xLocation;
        int yLocation;
        int ammoSize = 3;
        int j, w;
        int down1, down2;
        bool g1, g2;
        bool multiply, multiply2, areaIncrease, areaIncrease2;
        double percent, percent2, playerSize, playerSize2;
        bool downk, upk, leftk, rightk;
        bool downk2, upk2, leftk2, rightk2, r1, r2;
        bool start, gameOver;

        public void onStart()
        {
            //resets all variables back to original
            for (int i = 0; i < area.Count(); i++) { area.RemoveAt(i); }
            for (int i = 0; i < area2.Count(); i++) {area2.RemoveAt(i); }
            area.Clear();
            area2.Clear();
            down1 = 0;
            down2 = 0;
            percent = 0;
            percent2 = 0;
            playerSize = 60;
            playerSize2 = 60;
            g2 = true;
            r2 = true;
            territory.Clear();
            ammunition.Clear();
            player.Clear();
            yLocation = 0;
            xLocation = 0;
            downk = false;
            downk2 = false;
            upk = false;
            upk2 = false;
            rightk = false;
            rightk2 = false;
            leftk = false;
            rightk2 = false;
            label2.Location = new Point(this.Width - 300, 19);
            
            //creates two player blocks 
            Ammo p = new Ammo(0, 0, Convert.ToInt32(playerSize), 0, 0);
            Ammo p2 = new Ammo(this.Width - Convert.ToInt32(playerSize2), this.Height - Convert.ToInt32(playerSize2), Convert.ToInt32(playerSize2), 0, 0);
            player.Add(p);
            player.Add(p2);
            j = 0;
            w = 0;
            start = false;
            timer1.Start();
        }

        Image p1 = Properties.Resources.P1_0;
        Image p2 = Properties.Resources.P02;
        Image b1 = Properties.Resources.Bomb;
        Image b2 = Properties.Resources.bombExplode;
        Image a1 = Properties.Resources.territory1;
        Image a2 = Properties.Resources.territory2;
        Image backGround = Properties.Resources.dotBlock;

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //draws all objects and changes their locations
            foreach (Ammo a in ammunition)
            {
                e.Graphics.FillEllipse(ammoBrush, a.x, a.y, a.size, a.size);
            }

            foreach (Ammo m in area)
            {
                Rectangle bRect = new Rectangle(m.x, m.y, m.size, m.size);
                e.Graphics.DrawImage(a1, bRect);
            }
            foreach (Ammo n in area2)
            {
                Rectangle bRect = new Rectangle(n.x, n.y, n.size, n.size);
                e.Graphics.DrawImage(a2, bRect);
            }
            foreach (Ammo c in territory)
            {
                Rectangle bRect = new Rectangle(c.x, c.y, c.size, c.size);
                if (c.time <= 0)
                {
                    e.Graphics.DrawImage(b2, bRect);
                }
                else
                {
                    e.Graphics.DrawImage(b1, bRect);
                }

            }

            if (start == false)
            {
                Rectangle backRect = new Rectangle(this.Width / 2 - 250, this.Height / 2 - 50, 500, 100);
                e.Graphics.DrawImage(backGround, backRect);
            }

            Rectangle p1Rect = new Rectangle(player[0].x, player[0].y, Convert.ToInt32(Math.Round(playerSize)), Convert.ToInt32(Math.Round(playerSize)));
            e.Graphics.DrawImage(p1, p1Rect);

            Rectangle p2Rect = new Rectangle(player[1].x, player[1].y, Convert.ToInt32(Math.Round(playerSize2)), Convert.ToInt32(Math.Round(playerSize2)));
            e.Graphics.DrawImage(p2, p2Rect);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if player does not place bomb or territory down over a period of time they will slowly decrease in size
            down1++;
            if (down1 > 300 && playerSize > 0) { playerSize -= 0.20; }
            down2++;
            if (down2 > 300 && playerSize2 > 0) { playerSize2 -= 0.20; }

            //dplaces initial dots everywhere on the screen
            if (yLocation <= this.Height)
            {
                for (int i = 0; i < this.Width; i += 15)
                {
                    xLocation = i;
                    Ammo a = new Ammo(xLocation, yLocation, ammoSize, 0, 0);
                    ammunition.Add(a);
                }

                yLocation += 15;
            }
            else { start = true; }

            //key press movements for both players
            if (downk == true && player[0].y < this.Height - playerSize) { player[0].y += 10; }
            if (rightk == true && player[0].x < this.Width - playerSize) { player[0].x += 10; }
            if (upk == true && player[0].y > 0) { player[0].y -= 10; }
            if (leftk == true && player[0].x > 0) { player[0].x -= 10; }
            if (downk2 == true && player[1].y < this.Height - playerSize2) { player[1].y += 10; }
            if (rightk2 == true && player[1].x < this.Width - playerSize2) { player[1].x += 10; }
            if (upk2 == true && player[1].y > 0) { player[1].y -= 10; }
            if (leftk2 == true && player[1].x > 0) { player[1].x -= 10; }

            explode();
            contact();
            areaGrow();
            Refresh();
        }

        public void areaGrow()
        {
            //if player gets 6 % of territory they win
            if((100 * percent) / (this.Width * this.Height) > 6)
            {
                label1.Text = "PLAYER 1 WINS!!";
                label2.Text = "PLAYER 2 LOSES";
                Refresh();
                Thread.Sleep(2500);
                gameOver = true;
                timer1.Stop();
            }
            if ((100 * percent2) / (this.Width * this.Height) > 6)
            {
                label2.Text = "PLAYER 2 WINS!!";
                label1.Text = "PLAYER 1 LOSES";
                Refresh();
                Thread.Sleep(2500);
                for (int i = 0; i < area2.Count(); i++) { area2.RemoveAt(i); }
                gameOver = true;
                timer1.Stop();
            }

            //calculates area covered and displays it
            if (j < area.Count())
            {
                percent += (area[j].size) * (area[j].size);
                j++;
            }
            label1.Text = "PLAYER 1 : " + ((100 * percent) / (this.Width * this.Height)).ToString("0.00") + " %";

            
            if (w < area2.Count())
            {
                percent2 += (area2[w].size) * (area2[w].size);
                w++;
            }
            label2.Text = "Player 2 :  " + ((100 * percent2) / (this.Width * this.Height)).ToString("0.00") + " %";

            //collision so player can't place territory on top of eachother
            foreach (Ammo m in area)
            {
                if (m.x + m.size > player[0].x && m.x < player[0].x + playerSize && m.y + m.size > player[0].y && m.y < player[0].y + playerSize)
                {
                    areaIncrease = false;
                }
            }
            foreach (Ammo n in area2)
            {
                if (n.x + n.size > player[1].x && n.x < player[1].x + playerSize2 && n.y + n.size > player[1].y && n.y < player[1].y + playerSize2)
                {
                    areaIncrease2 = false;
                }
            }

            //adds territory
            if (areaIncrease == true)
            {
                Ammo m = new Ammo(player[0].x, player[0].y, Convert.ToInt32(playerSize), 30, Convert.ToInt32(playerSize));
                area.Add(m);

                playerSize *= 1.0 / 2.0;
                player[0].x += Convert.ToInt32(playerSize) / 2;
                player[0].y += Convert.ToInt32(playerSize) / 2;

                areaIncrease = false;
            }

            if (areaIncrease2 == true)
            {
                Ammo n = new Ammo(player[1].x, player[1].y, Convert.ToInt32(playerSize2), 30, Convert.ToInt32(playerSize2));
                area2.Add(n);

                playerSize2 *= 1.0 / 2.0;
                player[1].x += Convert.ToInt32(playerSize2) / 2;
                player[1].y += Convert.ToInt32(playerSize2) / 2;

                areaIncrease2 = false;
            }
        }

        public void explode()
        {
            //places a bomb down
            if (multiply == true)
            {
                Ammo c = new Ammo(player[0].x, player[0].y, Convert.ToInt32(playerSize), 30, Convert.ToInt32(playerSize));
                territory.Add(c);

                playerSize *= 1.0 / 2.0;
                player[0].x += Convert.ToInt32(playerSize) / 2;
                player[0].y += Convert.ToInt32(playerSize) / 2;

                multiply = false;
            }

            if (multiply2 == true)
            {
                Ammo c = new Ammo(player[1].x, player[1].y, Convert.ToInt32(playerSize2), 30, Convert.ToInt32(playerSize2));
                territory.Add(c);

                playerSize2 *= 1.0 / 2.0;
                player[1].x += Convert.ToInt32(playerSize2) / 2;
                player[1].y += Convert.ToInt32(playerSize2) / 2;

                multiply2 = false;
            }

            //checks countdown of bomb to explode
            foreach (Ammo c in territory)
            {
                if (c.time > 0) { c.time--; }

                if (c.time == 0)
                {
                    c.x -= 10;
                    c.y -= 10;
                    c.size += 20;

                    if (c.size > c.ogSize * 3)
                    {
                        c.time = -1;
                        bombDelete.Add(territory.IndexOf(c));

                        //collision for destroying territory
                        foreach (Ammo m in area)
                        {
                            if (c.x + c.size > m.x && c.x < m.x + m.size && c.y + c.size > m.y && c.y < m.y + m.size)
                            {
                                areaDelete.Add(area.IndexOf(m));
                                percent = 0;
                                j = 0;
                            }
                        }
                        foreach (Ammo n in area2)
                        {
                            if (c.x + c.size > n.x && c.x < n.x + n.size && c.y + c.size > n.y && c.y < n.y + n.size)
                            {
                                areaDelete2.Add(area2.IndexOf(n));
                                percent2 = 0;
                                w = 0;
                            }
                        }
                    }
                    //collision for killing a player
                    if (c.x + c.size > player[0].x && c.x < player[0].x + playerSize && c.y + c.size > player[0].y && c.y < player[0].y + playerSize)
                    {
                        label2.Text = "PLAYER 2 WINS!!";
                        label1.Text = "PLAYER 1 LOSES";
                        Refresh();
                        Thread.Sleep(2500);
                        gameOver = true;
                        timer1.Stop();
                    }
                    if (c.x + c.size > player[1].x && c.x < player[1].x + playerSize2 && c.y + c.size > player[1].y && c.y < player[1].y + playerSize2)
                    {
                        label1.Text = "PLAYER 2 WINS!!";
                        label2.Text = "PLAYER 1 LOSES";
                        Refresh();
                        Thread.Sleep(2500);
                        gameOver = true;
                        timer1.Stop();
                    }
                }
            }
        }

        public void contact()
        {
            //collision for intial dots
            foreach (Ammo a in ammunition)
            {
                if (a.x + ammoSize > player[0].x && a.x < player[0].x + playerSize && a.y + ammoSize > player[0].y && a.y < player[0].y + playerSize)
                {

                    ammoDelete.Add(ammunition.IndexOf(a));
                    playerSize *= 1 + 1 / (playerSize * 9);

                }
                if (a.x + ammoSize > player[1].x && a.x < player[1].x + playerSize2 && a.y + ammoSize > player[1].y && a.y < player[1].y + playerSize2)
                {
                    ammoDelete.Add(ammunition.IndexOf(a));
                    playerSize2 *= 1 + 1 / (playerSize2 * 9);
                }
            }

            ammoDelete.Reverse();
            areaDelete.Reverse();
            areaDelete2.Reverse();

            //removes any necessary objects from specific list
            foreach (int i in ammoDelete)
            {
                ammunition.RemoveAt(i);
            }
            foreach (int b in bombDelete)
            {
                territory.RemoveAt(b);
            }
            foreach (int g in areaDelete)
            {
                area.RemoveAt(g);
            }
            foreach (int i in areaDelete2)
            {
                area2.RemoveAt(i);
            }
            ammoDelete.Clear();
            bombDelete.Clear();
            areaDelete.Clear();
            areaDelete2.Clear();

        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (gameOver == true)
            {
                onStart();
                gameOver = false;
            }
            if (start == true)
            {
                if (e.KeyCode == Keys.M)
                {
                    areaIncrease = true;
                    down1 = 0;
                }
                if (e.KeyCode == Keys.Down) { downk = true; }
                if (e.KeyCode == Keys.Up) { upk = true; }
                if (e.KeyCode == Keys.Left)
                {
                    leftk = true;
                    if (g2 == true)
                    {
                        g1 = true;
                        g2 = false;
                    }
                }
                if (e.KeyCode == Keys.Right)
                {
                    rightk = true;
                    if (g1 == true)
                    {
                        g2 = true;
                        playerSize += 4;
                        g1 = false;
                    }
                }
                if (e.KeyCode == Keys.Space)
                {
                    multiply = true;
                    down1 = 0;
                }
                if (e.KeyCode == Keys.Q)
                {
                    down2 = 0;
                    multiply2 = true;
                }
                if (e.KeyCode == Keys.S) { downk2 = true; }
                if (e.KeyCode == Keys.W) { upk2 = true; }
                if (e.KeyCode == Keys.E)
                {
                    down2 = 0;
                    areaIncrease2 = true;
                }
                if (e.KeyCode == Keys.A)
                {
                    leftk2 = true;
                    if (r2 == true)
                    {
                        r1 = true;
                        r2 = false;
                    }
                }
                if (e.KeyCode == Keys.D)
                {
                    rightk2 = true;
                    if (r1 == true)
                    {
                        r2 = true;
                        playerSize2 += 4;
                        r1 = false;
                    }
                }
                if (e.KeyCode == Keys.Escape) { this.Close(); }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (start == true)
            {
                if (e.KeyCode == Keys.Down) { downk = false; }
                if (e.KeyCode == Keys.Up) { upk = false; }
                if (e.KeyCode == Keys.Left) { leftk = false; }
                if (e.KeyCode == Keys.Right) { rightk = false; }
                if (e.KeyCode == Keys.S) { downk2 = false; }
                if (e.KeyCode == Keys.W) { upk2 = false; }
                if (e.KeyCode == Keys.A) { leftk2 = false; }
                if (e.KeyCode == Keys.D) { rightk2 = false; }
            }
        }
    }
}

