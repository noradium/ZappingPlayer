using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZappingPlayer
{
    public partial class ZappingPlayer : Form
    {
        private enum PlayerStatus {Play, Pause, Stop };
        private PlayerStatus status = PlayerStatus.Stop;

        WMPLib.WindowsMediaPlayer mediaPlayer = new WMPLib.WindowsMediaPlayer();

        private string[] fileArray = null;
        private SearchOption searchOption = SearchOption.AllDirectories;
        private Random cRandom = new Random();
        private int playerVolume;
        private double playtime = 14.0;
        private Timer timer = null;


        public ZappingPlayer()
        {
            InitializeComponent();
            mediaPlayer.settings.autoStart = false;
            playerVolume = this.trackBar1.Value;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.ShowDialog();
            this.richTextBox1.Text = this.folderBrowserDialog1.SelectedPath;

            
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked == true)
            {
                this.searchOption = SearchOption.AllDirectories;
            }
            else
            {
                this.searchOption = SearchOption.TopDirectoryOnly;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {   // 再生ボタン
            if (fileArray.Length > 0)
            {
                if (status == PlayerStatus.Stop)
                {
                    newStart();
                }
                else if (status == PlayerStatus.Pause)
                {

                }
            }
        }

        private void timer_tick_FadeIn(object sender, EventArgs e)
        {
            if (mediaPlayer.settings.volume < this.playerVolume)
            {
                mediaPlayer.settings.volume++;
            }
            else
            {
                timer.Dispose();
                timer = new Timer();
                timer.Tick += new EventHandler(startFadeOut);
                timer.Interval = (int)(playtime * 1000);
                timer.Start();
            }


        }

        private void timer_tick_FadeOut(object sender, EventArgs e)
        {
            if (mediaPlayer.settings.volume > 0)
            {
                mediaPlayer.settings.volume--;
            }
            else
            {
                timer.Stop();
                newStart();
            }


        }
        private void newStart()
        {
            if (timer != null)
            {
                timer.Dispose();
            }
            int fileIndex = cRandom.Next(fileArray.Length);
            Console.WriteLine("fileIndex = " + fileIndex);
            mediaPlayer.URL = fileArray[fileIndex];
            mediaPlayer.controls.play();

            double startPosition = cRandom.Next(120);//(double)cRandom.Next((int)(mediaPlayer.controls.currentItem.duration * 2.0 / 3.0));
            Console.WriteLine("duration = " + mediaPlayer.currentMedia.duration);
            Console.WriteLine("startPosition = " + startPosition);
            mediaPlayer.controls.currentPosition = startPosition;
            mediaPlayer.settings.volume = 0;
            
            timer = new Timer();
            timer.Tick += new EventHandler(timer_tick_FadeIn);
            timer.Interval = 80;
            timer.Enabled = true;

            
            status = PlayerStatus.Play;
            label3.Text = mediaPlayer.URL;

        }

        private void startFadeOut(object sender, EventArgs e)
        {
            timer.Dispose();
            timer = new Timer();
            timer.Tick += new EventHandler(timer_tick_FadeOut);
            timer.Interval = 80;
            timer.Start();

        }

        private void button3_Click(object sender, EventArgs e)
        {   //　一時停止ボタン
            if (status == PlayerStatus.Play)
            {
                timer.Stop();
                mediaPlayer.controls.pause();
                status = PlayerStatus.Pause;
            }
            else if(status == PlayerStatus.Pause)
            {
                timer.Start();
                mediaPlayer.controls.play();
                status = PlayerStatus.Play;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {   // 停止ボタン
            timer.Dispose();
            mediaPlayer.controls.stop();
            status = PlayerStatus.Stop;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            mediaPlayer.settings.volume = this.trackBar1.Value;
            this.playerVolume = this.trackBar1.Value;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            fileArray = Directory.GetFiles(this.richTextBox1.Text, "*.mp3", searchOption);

            label5.Text = fileArray.Length + " files are loaded.";
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            this.playtime = Double.Parse(richTextBox2.Text);
        }





    }
}
