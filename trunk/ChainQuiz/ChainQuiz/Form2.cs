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
    public partial class Form2 : Form
    {
        private FileInfo quiz;
        private int quizLen;
        private Question[] quizQuestions;
        private SHA256 SHAMachine = SHA256Managed.Create();
        private int curPos;
        private Form1 parentForm;
        private string quizName = "";

        public Form2(FileInfo q, Form1 f)
        {
            InitializeComponent();
            quiz = q;
            InitQuiz();
            parentForm = f;
        }

        private void InitQuiz()
        {
            StreamReader srq = new StreamReader("quiz\\" + quiz.Name, Encoding.UTF8);
            quizName = srq.ReadLine();
            Form2.ActiveForm.Text = "ChainQuiz - " + quizName;
            srq.ReadLine();
            quizLen = Convert.ToInt32(srq.ReadLine());
            string verifSalt = srq.ReadLine();
            string keySalt = srq.ReadLine();
            quizQuestions = new Question[quizLen+1];
            for (int i = 0; i < quizLen; i++)
            {
                string verifHashB64 = srq.ReadLine();
                string keyIVB64 = srq.ReadLine();
                string encryptedQ = srq.ReadLine();
                quizQuestions[i] = new Question(verifHashB64, keyIVB64, encryptedQ, verifSalt, keySalt);
                listBoxQuestions.Items.Add(String.Format("Question {0}", i + 1));
            }
            string keyIVB64last = srq.ReadLine();
            string endMsgEncrypted = srq.ReadLine();
            quizQuestions[quizLen] = new Question("", keyIVB64last, endMsgEncrypted, verifSalt, keySalt);
            listBoxQuestions.Items.Add("Goal Message");
            srq.Close();
            quizQuestions[0].DecryptQuestion(Convert.ToBase64String(SHAMachine.ComputeHash(Encoding.UTF8.GetBytes(quizName + keySalt))));
            int ii = 0;
            try
            {
                StreamReader sra = new StreamReader("quiz\\" + quiz.Name + ".ans", Encoding.UTF8);
                while ((ii < quizLen) && (!sra.EndOfStream))
                {
                    string curAns = sra.ReadLine();
                    if (quizQuestions[ii].CheckAnswer(curAns))
                    {
                        quizQuestions[ii + 1].DecryptQuestion(quizQuestions[ii].GetNextKey());
                        ii++;
                    }
                    else
                    {
                        break;
                    }
                }
                sra.Close();
            }
            catch
            {
                // MessageBox.Show("Error when decrypting. Quiz file is corrupted.", "ChainQuiz", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                listBoxQuestions.SelectedIndex = ii;
                curPos = ii;
            }
        }

        private void listBoxQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (quizQuestions[listBoxQuestions.SelectedIndex].isDecrypted())
            {
                textBoxQue.Text = quizQuestions[listBoxQuestions.SelectedIndex].GetDecryptedQuestion();
                if (quizQuestions[listBoxQuestions.SelectedIndex].isSolved())
                {
                    textBoxAns.Text = quizQuestions[listBoxQuestions.SelectedIndex].GetAnswer();
                    btnCheck.Enabled = false;
                }
                else if (listBoxQuestions.SelectedIndex != quizLen)
                {
                    textBoxAns.Text = "";
                    btnCheck.Enabled = true;
                }
                else
                {
                    textBoxAns.Text = "";
                    btnCheck.Enabled = false;
                }
            }
            else
            {
                listBoxQuestions.SelectedIndex = curPos;
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (quizQuestions[listBoxQuestions.SelectedIndex].CheckAnswer(textBoxAns.Text))
            {
                quizQuestions[listBoxQuestions.SelectedIndex + 1].DecryptQuestion(quizQuestions[listBoxQuestions.SelectedIndex].GetNextKey());
                curPos++;
                listBoxQuestions.SelectedIndex++;
                updateAnsFile(sender, e);
                if (listBoxQuestions.SelectedIndex == quizLen)
                {
                    MessageBox.Show(String.Format("Congratulations! You have solved the quiz {0}!", quizName), "ChainQuiz", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Wrong answer!", "ChainQuiz", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void updateAnsFile(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter("quiz\\" + quiz.Name + ".ans", false, Encoding.UTF8);
            int i = 0;
            while (quizQuestions[i].isSolved())
            {
                sw.WriteLine(quizQuestions[i].GetAnswer());
                i++;
            }
            sw.Close();
            parentForm.listBoxQuizName_SelectedIndexChanged(sender, e);
        }

        private void textBoxAns_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                btnCheck_Click(sender, e);
            }
        }
    }

    public class Question
    {
        private byte[] questionEncrypted;
        private string questionDecrypted;
        private string answer;
        private byte[] questionIV;
        private string answerVerifHashB64;
        private string verifSalt;
        private string keySalt;
        private SHA256 SHAMachine = SHA256Managed.Create();
        private bool solved = false;
        private bool decrypted = false;

        public Question(string ansHash, string qIV, string qEB64, string vS, string kS)
        {
            answerVerifHashB64 = ansHash;
            questionIV = Convert.FromBase64String(qIV);
            questionEncrypted = Convert.FromBase64String(qEB64);
            verifSalt = vS;
            keySalt = kS;
        }

        public bool CheckAnswer(string ans)
        {
            if (answerVerifHashB64 == Convert.ToBase64String(SHAMachine.ComputeHash(Encoding.UTF8.GetBytes(verifSalt + ans))))
            {
                answer = ans;
                solved = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DecryptQuestion(string hashKey)
        {
            AesCryptoServiceProvider AESMachine = new AesCryptoServiceProvider();
            AESMachine.Padding = PaddingMode.ISO10126;
            AESMachine.Mode = CipherMode.CBC;
            AESMachine.KeySize = 256;
            AESMachine.IV = questionIV;
            AESMachine.Key = Convert.FromBase64String(hashKey);
            ICryptoTransform AESDecrypt = AESMachine.CreateDecryptor();
            MemoryStream ms = new MemoryStream(questionEncrypted);
            CryptoStream cs = new CryptoStream(ms, AESDecrypt, CryptoStreamMode.Read);
            byte[] dc = new byte[questionEncrypted.Length];
            int actualLen = cs.Read(dc, 0, dc.Length);
            cs.Close();
            ms.Close();
            questionDecrypted = Encoding.UTF8.GetString(dc, 0, actualLen);
            decrypted = true;
        }

        public string GetNextKey()
        {
            if (!solved) throw new KeyNotFoundException();
            else return Convert.ToBase64String(SHAMachine.ComputeHash(Encoding.UTF8.GetBytes(answer + keySalt)));
        }

        public bool isDecrypted()
        {
            return decrypted;
        }

        public bool isSolved()
        { 
            return solved; 
        }

        public string GetDecryptedQuestion()
        {
            return questionDecrypted;
        }

        public string GetAnswer()
        {
            return answer;
        }
    }
}
