using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace REGEX_STRINGS_PARSER
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("yo nick ");
        }




        //public Form1()
        //{
        //InitializeComponent();
        //label1.Text = @"Files is in c:\Temp\test.txt";

        //}


        StringBuilder sb = new StringBuilder();


        string ms_saveString = "";


        string[] lines;
        private string WholeFile;


        void readInFile()
        {


            // How to: Write to a Text File. You can change the path and


            // file name to substitute text files of your own.


            // Example #1


            // Read the file as one string.


            string text = System.IO.File.ReadAllText(@"C:\Temp\test.txt");


            // Display the file contents to the console. Variable text is a string.


            // MessageBox.Show("Contents of WriteText.txt = " + text);


            // Example #2


            // Read each line of the file into a string array. Each element


            // of the array is one line of the file.
            lines = System.IO.File.ReadAllLines(@"C:\Temp\test.txt");

            System.IO.StreamReader currReader = new System.IO.StreamReader(@"C:\Temp\text.txt");
            WholeFile = currReader.ReadLine();
            string ls_oneLine = "";
            lines = WholeFile.Split(' ');
            string[] ls_firstWord;
            string[] ls_SecondWord;
            string[] ls_try;
            int li_location = 0;
            string ls_newString = " ";
            foreach (string line in lines)
            {
                ls_oneLine = WholeFile;
            }
            while (currReader.EndOfStream != true)
            {
                ls_oneLine = currReader.ReadLine();
                ls_oneLine = ls_oneLine.TrimStart();
                ls_try = ls_oneLine.Split(' ');

                ls_newString = ls_try[1] + ls_try[0];
                //            if (ls_oneLine.Trim() == '\t')
                //{

                //}
                //           li_location = ls_oneLine.IndexOf()
            }

        }


        private void CreateRowOfNumbers()
        {


            StringBuilder builder = new StringBuilder();


            // Append to StringBuilder.


            for (int i = 0; i < 10; i++)
            {
                builder.Append(i).Append(

                " ");
            }


            MessageBox.Show("STring builder Fields " + builder);

        }


        private void CreateDelimitedString(string ps_beforeDelim, string ps_string, string ps_delim)
        {



            // ms_saveString += sb;


            //MessageBox.Show("STring builder length " + sb.Length);
            sb.Append(ps_beforeDelim + ps_string + ps_delim);


            // MessageBox.Show("after append " + sb.ToString());
        }


        private void CreateDelimitedStringVertical(string ps_beforeDelim, string ps_string, string ps_delim)
        {


            // ms_saveString += sb;


            //MessageBox.Show("STring builder length " + sb.Length);
            sb.Append(ps_beforeDelim + ps_string + ps_delim +

            Environment.NewLine);


            // MessageBox.Show("after append " + sb.ToString());
        }


        //private void button1_Click(object sender, EventArgs e)
        //{
        //    sb.Clear();
        //    readInFile();
        //    // WriteOutFile();
        //}


        private void button2_Click(object sender, EventArgs e)
        {
            WriteOutFile();
        }


        private void WriteOutFile()
        {



            // MessageBox.Show("Writing File");



            string mydocpath =


            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            // StringBuilder sb = new StringBuilder();


            //foreach (string txtName in Directory.EnumerateFiles(mydocpath,"*.txt"))


            //{


            // using (StreamReader sr = new StreamReader(txtName))


            // {


            // sb.AppendLine(txtName.ToString());


            // sb.AppendLine("= = = = = =");


            // sb.Append(sr.ReadToEnd());


            // sb.AppendLine();


            // sb.AppendLine();


            // }


            //}



            using (StreamWriter outfile =


            new StreamWriter(@"C:\Temp\TestOut.txt"))


            // new StreamWriter(mydocpath + @"C:\Temp\out.txt"))
            {


                outfile.Write(sb.ToString());


                // outfile.Write("nick was here");
            }


            // }


            // }



            // System.IO.File.WriteAllLines(@"C:\Temp\out.txt",sb.ToString());
        }


        private void Replace(StringBuilder ps_sb, string ps_replaceFrom, string ps_replaceTo)
        {



            // const string message =


            //"Would you like to Replace another character ?";


            // const string caption = "Replacing ";


            // var result = MessageBox.Show(message, caption,


            // MessageBoxButtons.YesNo,


            // MessageBoxIcon.Question);


            // If the no button was pressed ...


            // if (result == DialogResult.Yes)


            // {


            // textBox3.Text

            int i = 0;
            foreach (var line in lines)
            {
                //       i =  Convert.ToInt32(line);

                line.ToUpper();

                //if (checkBox7.Checked == true)
                //{
                //    lines[i] = lines[i].ToUpper();
                //}

                //if (checkBox8.Checked == true)
                //{
                //    lines[i] = lines[i].ToLower();
                //}
                ps_sb.Replace(ps_replaceFrom, ps_replaceTo);


            }
            label1.Text =

            "Character " + ps_replaceFrom + " Has been replaced with " + ps_replaceTo + " ";


            // }


        }


        private void button3_Click(object sender, EventArgs e)
        {


            // CreateRowOfNumbers();
            Replace(sb, textBox2.Text, textBox3.Text);
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled =

            true;
            textBox3.Enabled =

            true;
        }


        private void button4_Click(object sender, EventArgs e)
        {



            if (checkBox2.Checked == true)
            {


                foreach (string line in lines)
                {


                    // Use a tab to indent each line of the file.


                    // MessageBox.Show("Each on separate line \t" + line);
                    CreateDelimitedString(textBox4.Text, line, textBox1.Text);
                    CheckReplaceBoxes();

                }
            }


            else
            {


                if (checkBox3.Checked == true)
                {


                    foreach (string line in lines)
                    {

                        // Use a tab to indent each line of the file.


                        // MessageBox.Show("Each on separate line \t" + line);
                        CreateDelimitedStringVertical(textBox4.Text, line, textBox1.Text);
                        CheckReplaceBoxes();

                    }
                }
            }

        }

        private void CheckReplaceBoxes()
        {

            if (checkBox1.Checked == true)
            {
                Replace(sb, textBox2.Text, textBox3.Text);
            }
            if (checkBox4.Checked == true)
            {
                Replace(sb, textBox5.Text, textBox6.Text);
            }
            if (checkBox5.Checked == true)
            {
                Replace(sb, textBox7.Text, textBox8.Text);
            }
            if (checkBox6.Checked == true)
            {
                Replace(sb, textBox9.Text, textBox10.Text);
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox3.Checked =

            false;
        }


        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked =

            false;
        }


        private void checkBox2_Click(object sender, EventArgs e)
        {
            checkBox2.Checked =

            true;
        }


        private void checkBox3_Click(object sender, EventArgs e)
        {
            checkBox3.Checked =

            true;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                checkBox8.Checked = false;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true)
            {
                checkBox7.Checked = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (string line in lines)
            {

                line.Replace(line, line.ToUpper());

                // Use a tab to indent each line of the file.

            }
        }


    }
}
}
