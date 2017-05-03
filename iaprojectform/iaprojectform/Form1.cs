using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Net.Http;

namespace iaprojectform
{
    public partial class StarterForm : MetroFramework.Forms.MetroForm
    {
        private bool fromNet = true;
          
        private String INITIALIZER_FILE = "init.ini";
        private int LEFT_MARGIN = 20, TOP_MARGIN = 20, SIZE = 70, SQUARE_COUNT, GAP = 20;
        private MetroLabel[,] SQUARES;
        private String[,] ANSWERS_SQUARE;
        private int[] hor_start;  // Indicates the horizontal line's starting square. 
        private char[,] ANSWERSFROMAI;
        private List<string> columnQuestions, rowQuestions;
        private char[,] squareLocations;
        private Color C_Sq_Avail = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(51)))), ((int)(((byte)(64)))));
        private Color C_Not_Avail = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(176)))), ((int)(((byte)(137)))));

        public StarterForm()
        {
            InitializeComponent();
            if (fromNet) ParsePuzzleFromNYT();
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

            Parser parserObj = new Parser();
            parserObj.GetSynonym("xa213123");
            parserObj.GetDictionary("assort12312312312ment");

            Dictionary<string,int> dic =  parserObj.GetFromGoogle("One of 100 in trump's presidency");

          //  foreach (KeyValuePair<string, int> a in dic) { Console.WriteLine("Word " + a.Key + " used {0} times", a.Value); }
         

            
            // We have our puzzle correctly.
            // Lets test
            Console.WriteLine("Square Locations:");
            for (int k = 0; k < SQUARE_COUNT; k++) { for (int l = 0; l < SQUARE_COUNT; l++) { Console.Write(squareLocations[k,l] + " "); } Console.WriteLine(); };
            Console.WriteLine("Row questions:");
            foreach (string question in rowQuestions ) { Console.WriteLine(question); };
            Console.WriteLine("Column questions:");
            foreach (string question in columnQuestions) { Console.WriteLine(question); };

            Solver solverObj = new Solver();
            ANSWERSFROMAI = solverObj.Solve(squareLocations,rowQuestions,columnQuestions);

            PrintToPuzzle();
            
        }

        private void PrintToPuzzle()
        {
            for (int c = 0; c < SQUARE_COUNT; c++) {
                for (int r = 0;  r < SQUARE_COUNT; r++ ) {
                    if (squareLocations[r, c] != '1') { //Not black..
                        if (ANSWERSFROMAI[r,c] != '*')
                            SQUARES[r, c].Text += "\r\n      " +  ANSWERSFROMAI[r, c];
                    }
                }
            }
        }


        private void ParsePuzzleFromNYT()
        {
            string url = "http://www.nytimes.com/crosswords/game/mini";
            string HTML_FILE = "";
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = content.ReadAsStringAsync().Result;
                        HTML_FILE += result;
                    }
                }
            }


            HtmlAgilityPack.HtmlDocument document2 = new HtmlAgilityPack.HtmlDocument();
            document2.LoadHtml(HTML_FILE);

            HtmlNode[] rows = document2.DocumentNode.SelectNodes("//div[@class='flex-table animated']//div[@class='flex-row']").ToArray();
            string FINAL_INIT = "";
            int ROW_COUNT = rows.Length;
            int[,] SQUARES = new int[ROW_COUNT,ROW_COUNT];
            int[] SQUARE_CELL_COUNT = new int[ROW_COUNT];
            FINAL_INIT = ROW_COUNT + "\n#\n";

            for ( int k = 0;  k < rows.Length; k++) {
                HtmlNode row = rows[k];
                HtmlNode[] squares = row.ChildNodes.ToArray();
                int sqrCount = 0;

                
                for (int i = 1; i < squares.Length-1; i++) {
                  //  Console.Write(squares[i].GetAttributeValue("class","")+ ",");
                    if (squares[i].GetAttributeValue("class", "").Equals("flex-cell ")){
                        SQUARES[k,i-1] = 1;
                        FINAL_INIT = FINAL_INIT + 1 + " ";
                        sqrCount++;
                    }else if (squares[i].GetAttributeValue("class", "").Equals("flex-cell black")) {
                        SQUARES[k, i-1] = 0;
                        FINAL_INIT = FINAL_INIT + 0 + " ";
                    }
                }


                if (squares[squares.Length - 1].GetAttributeValue("class", "").Equals("flex-cell "))
                {
                    SQUARES[k, squares.Length - 2] = 1;
                    FINAL_INIT = FINAL_INIT + 1 + "\n";
                    sqrCount++;
                }
                else if (squares[squares.Length - 1].GetAttributeValue("class", "").Equals("flex-cell black"))
                {
                    SQUARES[k, squares.Length - 2] = 0;
                    FINAL_INIT = FINAL_INIT + 0 + "\n";
                }

                SQUARE_CELL_COUNT[k] = sqrCount;
            }

            FINAL_INIT = FINAL_INIT  + "#\n";

            HtmlNode[] clues = document2.DocumentNode.SelectNodes("//ol[@class='clue-list']//li").ToArray();

            for(int o = 0; o < clues.Length; o++) {
                FINAL_INIT = FINAL_INIT + clues[o].GetAttributeValue("value","") + ": " + clues[o].InnerHtml + "|";
                for (int f = 0; f < SQUARE_CELL_COUNT[o%ROW_COUNT]-1; f++) FINAL_INIT += "A ";
                FINAL_INIT += "A\n";
                if (o+1 == ROW_COUNT)
                {
                    FINAL_INIT += "#\n";
                }
            }

            FINAL_INIT = FINAL_INIT.Substring(0, FINAL_INIT.Length - 1);
           // Console.WriteLine(FINAL_INIT);

            System.IO.StreamWriter file = new System.IO.StreamWriter("init.ini");
            file.WriteLine(FINAL_INIT);

            file.Close();
        }


        private bool PuzzleInitializer() {
            // Read the input file. Create and assign squares accordingly.
            SQUARE_COUNT = 0;

            rowQuestions = new List<string>();
            columnQuestions = new List<string>();

            int counter = 1;
            string line;

            // Read the file and display it line by line.
            StreamReader file = new StreamReader(INITIALIZER_FILE);
            while ((line = file.ReadLine()) != null) {
                int sqrLine = 1;
                SQUARE_COUNT = Int32.Parse(line);
                Console.WriteLine("We have "+SQUARE_COUNT+"x"+SQUARE_COUNT+" squares!");
                // Create Label Array
                SQUARES = new MetroLabel[SQUARE_COUNT,SQUARE_COUNT];
                ANSWERS_SQUARE = new String[SQUARE_COUNT,SQUARE_COUNT];
                squareLocations = new char[SQUARE_COUNT, SQUARE_COUNT];

                for (int a = 0; a < SQUARE_COUNT; a++) {
                    for (int j = 0; j < SQUARE_COUNT; j++){
                        ANSWERS_SQUARE[a,j] = null; } }

                bool weAllDone = false;
                int startIndexForRow = -1;
                bool firstRow = true;
                line = file.ReadLine();  // Skip # char.
                for (int a = 0;  a < SQUARE_COUNT;  a++){
                    line = file.ReadLine(); // Now we have X X X X X like a string
                    string[] tokens = line.Split(' ');
                    bool innerChecker = true;

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
                            squareLocations[sqrLine - 1, k] = '1';
                        } else {
                            tmp.BackColor = C_Sq_Avail;  //  Create available square
                            squareLocations[sqrLine - 1, k] = '0';
                            if (firstRow)  {
                                tmp.Text = counter + "";
                                counter++;
                                if (counter >= 6) {
                                    weAllDone = true;
                                } else {
                                    startIndexForRow = k+1;
                                }
                            } else  {
                                if (innerChecker) {
                                    tmp.Text = counter + "";
                                    counter++;
                                    innerChecker = false;
                                }

                                if (!weAllDone) {
                                    if (k == startIndexForRow)
                                    {
                                        tmp.Text = counter + "";
                                        counter++;
                                        startIndexForRow = k+1;
                                    }
                                }
                            }


                            /*
                             * 
                             * tmp.BackColor = C_Sq_Avail;  //  Create available square
                            squareLocations[sqrLine - 1, k] = '0';
                            if (firstRow)  {
                                tmp.Text = counter + "";
                                counter++;
                                if (counter >= 5) {
                                    weAreNotDone = true;
                                }
                            }
                            else  {
                                if (innerChecker) {
                                    tmp.Text = counter + "";
                                    counter++;
                                    innerChecker = false;
                                }
                            }
                             */
                        }

                        SQUARES[sqrLine-1,k] = tmp;
                        this.Controls.Add(tmp);
                    }
                    Console.WriteLine();
                    sqrLine++;
                    firstRow = false;
                }

                line = file.ReadLine();  // Skip # char.

                int index_counter = 1;
                // Now lets read our questions and our answers.
                // First, horizontal questions.
                sqrLine = 1;
                for (int a = 0; a < SQUARE_COUNT; a++) {
                    line = file.ReadLine(); // Now we have XX|A B C
                    string[] tokens = line.Split('|');  // Now we have "XX" and "A B C"
                    // TODO Set question.
                    questionBox.Text += tokens[0] + "\r\n";//sqrLine + ": "+ tokens[0] + "\r\n";
                    rowQuestions.Add(tokens[0].Substring(3));
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
                
                // Vertical
                sqrLine = 1;
                for (int a = 0; a < SQUARE_COUNT; a++) {
                    line = file.ReadLine(); // Now we have XX|A B C
                    string[] tokens = line.Split('|');  // Now we have "XX" and "A B C"
                    // TODO Set question.
                    questionBox2.Text += tokens[0] + "\r\n";//sqrLine + ": " + tokens[0] + "\r\n";
                    columnQuestions.Add(tokens[0].Substring(3));
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

            return true;
        }

    }
}
