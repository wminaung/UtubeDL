using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace UtubeDL
{
    public partial class FmUtubeDL : Form
    {   

        private bool isDownloaded { get; set; } 
        
       

        public FmUtubeDL()
        {
            InitializeComponent();
        
        }

        private void UtubeDL_Load(object sender, EventArgs e)
        {
            
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
          isDownloaded = false;
          string URL = textBoxURL.Text;

             if (URL.Trim().Length == 0 || URL.Length < 4)
             {
                 MessageBox.Show("You need to add URL from youtube");
                 return;
             };
           
            if (sbd.ShowDialog() == DialogResult.OK)
            {
               string selectedPath = sbd.SelectedPath;
               btnDownload.Enabled = false;         
               Console.WriteLine("select: " + selectedPath);
               downloadFromYTDl(URL,selectedPath);
                
                if(isDownloaded)
                {
                    MessageBox.Show("Successfully downloaded");
                    textBoxURL.Text = "";
                    isDownloaded = false;
                }
                else
                {
                    MessageBox.Show("SomethingWrong! Please enter a valid url");

                }
                btnDownload.Enabled = true;

            }


        }


        private  void downloadFromYTDl(string videoUrl ,string selectedPath)
        {
            try
            {
             
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string ytdlExePath = baseDir + "yt-dlp.exe";
                Console.WriteLine(ytdlExePath);
                string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                 
                string outputDir = $"{selectedPath}\\%(title)s.%(ext)s";
                Console.WriteLine($"outputDir: {outputDir}");
                // Check if yt-dlp executable exists
                if (System.IO.File.Exists(ytdlExePath))
                {
                    // Start yt-dlp process with the desired command
                    using (Process process = new Process())
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = ytdlExePath,
                            Arguments = $"-o {outputDir}  {videoUrl}",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                        };

                        process.StartInfo = startInfo;
                        process.Start();

                        // Read the output and error streams
                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();

                        process.WaitForExit();
                        Console.WriteLine("Output: " + output);
                        Console.WriteLine("Error: " + error);
                        if(error != "")
                        {
                            isDownloaded = false;
                        }
                        else
                        {
                            isDownloaded = true;
                        }

                    }
                }
                else
                {
                    Console.WriteLine("yt-dlp not found. You need to download yt-dlp yourself. but don't worry I don't help you ");
                }
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error: {ex.Message}"); 
                MessageBox.Show("download too long");
            }
          
        }

       

    

        private void textBoxURL_TextChanged(object sender, EventArgs e)
        {
            
        }
    }// class
}
