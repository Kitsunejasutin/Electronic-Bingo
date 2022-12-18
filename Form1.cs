using Electronic_Bingo.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Electronic_Bingo
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public String[] randomized_letter = { };
        public String[] randomized_number = { };
        public Thread randomizer_letter;
        public Thread randomizer_number;
        public bool letter_status = false;
        public bool number_status = false;
        private EventWaitHandle wh = new AutoResetEvent(false);
        private EventWaitHandle xh = new AutoResetEvent(false);
        public int combination_click = 0;
        public List<string> exceptions = new List<string>();
        public List<string> exceptions_list = new List<string>();
        public int counter = 1;
        public bool cleared = false;
        public Form1()
        {
            InitializeComponent();
            lblLetter.Text = "";
            lblNumber.Text = "";
            randomizer_letter = new Thread(randomize_letter);
            randomizer_letter.IsBackground = true;
            randomizer_number = new Thread(randomize_number);
            randomizer_number.IsBackground = true;
            metroButtonPrevious.Enabled = false;
            label4.Text = "Pattern " + counter;
        }

        private void metroButtonCallLett_Click(object sender, EventArgs e)
        {
            if (combination_click == 3)
            {
                combination_click = 0;
            }
            combination_click++;
            if (combination_click % 2 == 0)
            {
                try
                {
                    letter_status = true;
                    randomizer_number.Start();
                }
                catch(ThreadStateException)
                {

                    if (letter_status == true && number_status == false)
                    {
                        number_status = true;
                    }
                    else if (letter_status == true && number_status == true)
                    {
                        // will executed for fourth click
                        lblLetter.Text = "";
                        lblNumber.Text = "";
                        letter_status = false;
                        wh.Set();
                    }
                }
            }
            else
            {
                if (lblLetter.Text == "" && lblNumber.Text == "")
                {
                    if (!cleared){
                        // This is being executed in the first click
                        try
                        {
                            randomizer_letter.Start();
                        }
                        catch (ThreadStateException)
                        {
                            MessageBox.Show("All combinations has been selected, Please try again", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    else
                    {
                        cleared = false;
                        letter_status = false;
                        number_status = true;
                        wh.Set();
                    }

                }
                else if (letter_status == true && number_status == false)
                {
                    // being executed every third click
                    letter_status = true;
                    number_status = true;
                    metroGrid1.Rows.Add(lblLetter.Text + " " + lblNumber.Text);
                    metroGrid1.FirstDisplayedScrollingRowIndex = metroGrid1.RowCount - 1;
                    exceptions.Add(lblNumber.Text);
                }
                else if (letter_status == false && number_status == true)
                {
                    letter_status = true;
                    number_status = false;
                    xh.Set();
                }
                else if (letter_status == true && number_status == true)
                {
                    // will executed for fourth click
                    lblLetter.Text = "";
                    lblNumber.Text = "";
                    letter_status = false;
                }
            }
        }

        private void randomizer()
        {
            string[] bingo_letter = { "B", "I", "N", "G", "O"};
            bingo_letter = bingo_letter.Except(exceptions_list).ToArray();
            if (bingo_letter.Length == 0)
            {
                bingo_letter = new[] { "" };
            }
            Random random = new Random();
            bingo_letter = bingo_letter.OrderBy(x => random.Next()).ToArray();
            foreach (var i in bingo_letter)
            {
                randomized_letter = bingo_letter.ToArray();
            }
        }

        private void numbers()
        {
            String[] number = new[] { "" };
            
            if (lblLetter.Text == "B")
            {
                number = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
            }
            if (lblLetter.Text == "I")
            {
                number = new[] { "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30" };
            }
            if (lblLetter.Text == "N")
            {
                number = new[] { "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45" };
            }
            if (lblLetter.Text == "G")
            {
                number = new[] { "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "60" };
            }
            if (lblLetter.Text == "O")
            {
                number = new[] { "61", "62", "63", "64", "65", "66", "67", "68", "69", "70", "71", "72", "73", "74", "75" };
            }
            number = number.Except(exceptions).ToArray();
            if (number.Length == 0)
            {
                exceptions_list.Add(lblLetter.Text);
                number = new[] { "" }; 
            }
            Random random = new Random();
            number = number.OrderBy(x => random.Next()).ToArray();
            foreach (var i in number)
            {
                randomized_number = number.ToArray();
            }
        }

        private void randomize_letter()
        {
            int i = 0;
            while (true)
            {
                if (letter_status == true)
                {
                    wh.WaitOne();
                }
                if (i == randomized_letter.Length)
                {
                    i = 0;
                    randomizer();
                }
                try
                {
                    lblLetter.Invoke((MethodInvoker)(() => lblLetter.Text = randomized_letter[i]));
                    lblLetter.Invoke((MethodInvoker)(() => lblLetter.Update()));
                    i++;
                    Thread.Sleep(50);

                }

                catch (ThreadInterruptedException)
                {
                    break;
                }
            }
        }

        private void randomize_number()
        {
            int i = 0;
            while (true)
            {
                if (number_status == true)
                {
                    xh.WaitOne();
                }
                if (i == randomized_number.Length)
                {
                    i = 0;
                    numbers();
                }
                try
                {
                    lblNumber.Invoke((MethodInvoker)(() => lblNumber.Text = randomized_number[i]));
                    lblNumber.Invoke((MethodInvoker)(() => lblNumber.Update()));
                    i++;
                    Thread.Sleep(50);
                }

                catch (ThreadInterruptedException)
                {
                    break;
                }
            }
        }

        private void metroButtonClear_Click(object sender, EventArgs e)
        {
            metroGrid1.Rows.Clear();
            metroGrid1.Update();
            exceptions.Clear();
            exceptions_list.Clear();
            lblLetter.Text = "";
            lblNumber.Text = "";
            cleared = true;
        }

        private void metroButtonNext_Click(object sender, EventArgs e)
        {
            counter++;
            label4.Text = "Pattern " + counter;
            if (counter == 25)
            {
                metroButtonNext.Enabled = false;
            }
            metroButtonPrevious.Enabled = true;
            pictureBox2.Image = Image.FromFile(@"(path of file)" + counter + ".png");
        }

        private void metroButtonPrevious_Click(object sender, EventArgs e)
        {
            counter--;
            label4.Text = "Pattern " + counter;
            if (counter == 1)
            {
                metroButtonPrevious.Enabled = false;
            }
            metroButtonNext.Enabled = true;
            pictureBox2.Image = Image.FromFile(@"(path of file)" + counter + ".png");
        }
    }
}
