using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace iaprojectform
{
    public partial class StarterForm : MetroFramework.Forms.MetroForm
    {
       
        private String INITIALIZER_FILE = "init.ini";
        private int LEFT_MARGIN = 20, TOP_MARGIN = 20, SIZE = 70, SQUARE_COUNT, GAP = 20;
        private MetroLabel[,] SQUARES;
        private String[,] ANSWERS_SQUARE;
        private int[] hor_start;  // Indicates the horizontal line's starting square. 


        private Color C_Sq_Avail = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(51)))), ((int)(((byte)(64)))));
        private Color C_Not_Avail = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(176)))), ((int)(((byte)(137)))));

        public StarterForm()
        {
            InitializeComponent();

            questionBox.Text += "\r\n";
            questionBox2.Text += "\r\n";

            PuzzleInitializer();
            questionBox.Location = new Point(SQUARE_COUNT*SIZE + LEFT_MARGIN*2, TOP_MARGIN + SIZE);
            //questionBox.MaximumSize = new Size(this.Width - (SQUARE_COUNT * SIZE + LEFT_MARGIN * 2) - LEFT_MARGIN, questionBox.Size.Height);
            questionBox.Size = new Size(this.Width - (SQUARE_COUNT * SIZE + LEFT_MARGIN * 2) - LEFT_MARGIN ,questionBox.Size.Height);

            questionBox2.Location = new Point(SQUARE_COUNT * SIZE + LEFT_MARGIN * 2, TOP_MARGIN + SIZE + questionBox.Height+GAP);
           // questionBox2.MaximumSize = new Size(this.Width - (SQUARE_COUNT * SIZE + LEFT_MARGIN * 2) - LEFT_MARGIN, questionBox.Size.Height);
            questionBox2.Size = new Size(this.Width - (SQUARE_COUNT * SIZE + LEFT_MARGIN * 2) - LEFT_MARGIN, questionBox.Size.Height);

            questionBox.ForeColor = Color.White;
            questionBox2.ForeColor = Color.White;
            questionBox.BackColor = C_Sq_Avail;
            questionBox2.BackColor = C_Sq_Avail;
        }


        private bool PuzzleInitializer()
        {
            // Read the input file. Create and assign squares accordingly.
            SQUARE_COUNT = 0;

            int counter = 1;
            string line;

            // Read the file and display it line by line.
            StreamReader file = new StreamReader(INITIALIZER_FILE);
            while ((line = file.ReadLine()) != null)
            {
                int sqrLine = 1;
                SQUARE_COUNT = Int32.Parse(line);
                Console.WriteLine("We have "+SQUARE_COUNT+"x"+SQUARE_COUNT+" squares!");
                // Create Label Array
                SQUARES = new MetroLabel[SQUARE_COUNT,SQUARE_COUNT];
                ANSWERS_SQUARE = new String[SQUARE_COUNT,SQUARE_COUNT];

                for (int a = 0; a < SQUARE_COUNT; a++) {
                    for (int j = 0; j < SQUARE_COUNT; j++){
                        ANSWERS_SQUARE[a,j] = null; } }

                line = file.ReadLine();  // Skip # char.
                for (int a = 0;  a < SQUARE_COUNT;  a++){
                    line = file.ReadLine(); // Now we have X X X X X like a string
                    string[] tokens = line.Split(' ');

                    // Lets start to create our labels.
                    for (int k = 0; k < tokens.Length; k++) {
                        Console.Write(tokens[k] + " ");
                        MetroLabel tmp = new MetroLabel();
                        tmp.Style = MetroFramework.MetroColorStyle.Red;
                        tmp.CustomBackground = true;
                        tmp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                        tmp.Location = new Point(k * SIZE + LEFT_MARGIN, sqrLine * SIZE + TOP_MARGIN);
                        tmp.Size = new Size(SIZE, SIZE);
                        if (Int32.Parse(tokens[k]) == 0) {  // Create a black square
                            tmp.BackColor = C_Not_Avail;
                           // Console.WriteLine("Row " + (sqrLine-1) + " Column " + k + " is BLACK!");
                            ANSWERS_SQUARE[sqrLine-1,k] = "*"; // Set its Answer to TAB?.
                        } else {
                            tmp.BackColor = C_Sq_Avail;
                        }
                        SQUARES[sqrLine-1,k] = tmp;
                        this.Controls.Add(tmp);
                    }
                    Console.WriteLine();
                    sqrLine++;
                }

                line = file.ReadLine();  // Skip # char.

                // Now lets read our questions and our answers.
                // First, horizontal questions.
                sqrLine = 1;
                for (int a = 0; a < SQUARE_COUNT; a++)
                {
                    line = file.ReadLine(); // Now we have XX|A B C
                    string[] tokens = line.Split('|');  // Now we have "XX" and "A B C"
                    // TODO Set question.
                    questionBox.Text += sqrLine + ": "+ tokens[0] + "\r\n";
                    string[] squareAnswers = tokens[1].Split(' ');
                    int ansCount = 0;
                    for(int k = 0;  k < SQUARE_COUNT; k++) {
                        // If its not BLACK
                        if (ANSWERS_SQUARE[sqrLine - 1, k] == null) {
                        //    Console.WriteLine("Added *" + squareAnswers[ansCount] + "* to row " + (sqrLine-1) + " and to column " + k );
                            ANSWERS_SQUARE[sqrLine - 1, k] = squareAnswers[ansCount];
                            ansCount++; 
                        }
                    }

                    sqrLine++;
                }

                line = file.ReadLine();  // Skip # char.
                // Now lets read our questions and our answers.
                // First, horizontal questions.
                sqrLine = 1;
                for (int a = 0; a < SQUARE_COUNT; a++)
                {
                    line = file.ReadLine(); // Now we have XX|A B C
                    string[] tokens = line.Split('|');  // Now we have "XX" and "A B C"
                    // TODO Set question.
                    questionBox2.Text += sqrLine + ": " + tokens[0] + "\r\n";
                    sqrLine++;
                }

                // Lets print answers 
                for (int a = 0; a < SQUARE_COUNT; a++) {
                    for (int j = 0; j < SQUARE_COUNT; j++){
                        Console.Write(ANSWERS_SQUARE[a, j]);
                    } 
                    Console.WriteLine();
                }

            }

            file.Close();

            // Suspend the screen.
            Console.ReadLine();


            /*
            this.sqr11.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.sqr11.CustomBackground = true;
            this.sqr11.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.sqr11.Location = new System.Drawing.Point(23, 117);
            this.sqr11.Name = "metroLabel1";
            this.sqr11.Size = new System.Drawing.Size(80, 80);
            this.sqr11.TabIndex = 0;
            this.sqr11.Text = "sqr11";
            */


            return true;
        }

    }
}
