using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dbExtract
{
    public partial class Form1 : Form
    {
        public static ArrayList codelist = new ArrayList();
        public static int progress = 0;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public static List<int> AllIndexesOf(string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }


        public static string findNumber(string line) {
            //string line = ;
            int frontIndex = line.IndexOf("- ");
            string cropfront = line.Substring(frontIndex+2);
            int cropback = cropfront.IndexOf("\n");
            string code = cropfront.Substring(0, cropback);
            return code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            int size = -1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(file);
                    size = text.Length;
                    List<int> indexes = AllIndexesOf(text, "http://osu.ppy.sh/b/");
                    
                    foreach (int i in indexes)
                    {
                        codelist.Add(findNumber(text.Substring(i)));
                    }
                    ArrayList a = codelist;
                    Console.WriteLine(size);
                    progressBar1.Maximum = codelist.Count;

                }
                catch (IOException)
                {
                }
            }
            System.Diagnostics.Debug.WriteLine(size); // <-- Shows file size in debugging mode.
            System.Diagnostics.Debug.WriteLine(result); // <-- For debugging use.
        }

        public static void DeepCopy(DirectoryInfo source, DirectoryInfo target)
        {

            // Recursively call the DeepCopy Method for each Directory
            foreach (DirectoryInfo dir in source.GetDirectories())
                DeepCopy(dir, target.CreateSubdirectory(dir.Name));

            // Go ahead and copy each file in "source" to the "target" directory
            foreach (FileInfo file in source.GetFiles()) {
                if (!System.IO.File.Exists(Path.Combine(target.FullName, file.Name)))
                {
                    file.CopyTo(Path.Combine(target.FullName, file.Name));
                }
            }
                

        }

        private void button2_Click(object sender, EventArgs e)
        {
            progress = 0;
            int size = -1;
            DialogResult result = folderBrowserDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = folderBrowserDialog1.SelectedPath;
                string targetPath = file + "\\MoonLight";
                
                try
                {
                    
                    string[] songs = Directory.GetDirectories(file+"\\Songs");
                    progressBar1.Maximum = songs.Length;
                    int max1 = songs.Length;
                    foreach (string sourcePath in songs)
                    {
                        progress++;
                        if (progress > progressBar1.Maximum) progress = progressBar1.Maximum;
                        progressBar1.Value = progress;
                        //label3.Text = progress + "/" + max1;
                        int percent1 = (int)(((double)progressBar1.Value / (double)progressBar1.Maximum) * 100);
                        progressBar1.CreateGraphics().DrawString(percent1.ToString() + "%", new Font("Arial", (float)8.25, FontStyle.Regular), Brushes.Black, new PointF(progressBar1.Width / 2 - 10, progressBar1.Height / 2 - 7));
                        progressBar2.Maximum = codelist.Count;
                        int max2 = codelist.Count;
                        int counter = 0;
                        foreach (string code in codelist) {
                            counter++;
                            progressBar2.Value = counter;
                            //label2.Text = counter+"/"+max2;

                            if ((counter == 246)&&(progress==11)) {
                                Console.WriteLine();
                            }
                            if (sourcePath.Contains(code)){
                                
                                string innertargetpath = targetPath + "\\" + sourcePath.Substring(sourcePath.LastIndexOf("\\") + 1);
                                if (!System.IO.Directory.Exists(innertargetpath)) ;
                                {
                                    System.IO.Directory.CreateDirectory(innertargetpath);
                                }
                                DirectoryInfo target = new DirectoryInfo(innertargetpath);
                                //if (System.IO.Directory.Exists(sourcePath))
                                //{
                                //    string[] files = System.IO.Directory.GetFiles(sourcePath);

                                //    // Copy the files and overwrite destination files if they already exist.
                                //    foreach (string s in files)
                                //    {
                                //        // Use static Path methods to extract only the file name from the path.
                                //        string fileName = System.IO.Path.GetFileName(s);
                                //        string destFile = System.IO.Path.Combine(targetPath, fileName);
                                //        System.IO.File.Copy(s, destFile, true);
                                //    }
                                //}
                                DirectoryInfo source = new DirectoryInfo(sourcePath);
                                DeepCopy(source, target);
                            }
                        }
                    }
                    label1.ForeColor = Color.Black;
                    
                    //ArrayList a = codelist;
                    //Console.WriteLine(size);

                }
                catch (IOException)
                {

                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
