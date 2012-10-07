using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ChainQuiz
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitQuiz();
        }

        private List<FileInfo> fileList = new List<FileInfo>();
        private SHA256 SHAMachine = SHA256Managed.Create();
        private UTF8Encoding encoding = new UTF8Encoding();
        private Form2 f;

        private void InitQuiz()
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo("quiz\\");
                IEnumerable<FileInfo> fileListing = dir.EnumerateFiles("*.qz", SearchOption.AllDirectories);
                listBoxQuizName.BeginUpdate();

                foreach (FileInfo fi in fileListing)
                {
                    StreamReader quizInfo = new StreamReader("quiz\\" + fi.Name, encoding);
                    listBoxQuizName.Items.Add(quizInfo.ReadLine());
                    fileList.Add(fi);
                    quizInfo.Close();
                }

                listBoxQuizName.EndUpdate();
            }
            catch
            {
                // Do nothing
            }
        }

        public void listBoxQuizName_SelectedIndexChanged(object sender, EventArgs e)
        {
            StreamReader quizInfo = new StreamReader("quiz\\" + fileList[listBoxQuizName.SelectedIndex].Name, encoding);
            labelQuizName.Text = quizInfo.ReadLine();
            labelQuizDesc.Text = (quizInfo.ReadLine()).Replace("\\n", "\n");
            int totalCnt = Convert.ToInt32(quizInfo.ReadLine());
            string verifSalt = quizInfo.ReadLine();
            quizInfo.ReadLine();
            int ansCnt = 0;
            try
            {
                StreamReader quizAns = new StreamReader("quiz\\" + fileList[listBoxQuizName.SelectedIndex].Name + ".ans", encoding);
                while (!((quizInfo.EndOfStream) || (quizAns.EndOfStream)))
                {
                    string verifHashB64 = quizInfo.ReadLine();
                    string ans = quizAns.ReadLine();
                    if (verifHashB64 == Convert.ToBase64String(SHAMachine.ComputeHash(encoding.GetBytes(verifSalt + ans))))
                    {
                        ansCnt++;
                        quizInfo.ReadLine();
                        quizInfo.ReadLine();
                    }
                    else
                    {
                        break;
                    }
                }
                quizAns.Close();
            }
            catch
            {
                // Do nothing
            }
            quizInfo.Close();
            labelQuizDesc.Text += String.Format("\n\nProgress: {0}/{1}\n", ansCnt, totalCnt);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            f = new Form2(fileList[listBoxQuizName.SelectedIndex], this);
            f.Show();
        }
    }
}
