using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhatsappSelenium
{
    public partial class Form2 : Form
    {
        MessageClass novaMensagem = new MessageClass();
        bool arrastar = false;
        int posiçãoX, posiçãoY;

        public Form2()
        {
            InitializeComponent();

            checkBox1.Checked = true;
            novaMensagem.ImageMessage = false;

            tabControl1.Location = new Point(-5, 43);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(novaMensagem.ImageMessage==false)
            {
                if (richTextBox1.Text != String.Empty)
                {
                    novaMensagem.Text = richTextBox1.Text;
                }
                else
                {
                    MessageBox.Show("A mensagem não pode ser nula.", "Erro");
                    return;
                }
                
            }
            else
            {
                if (novaMensagem.Imagem == null) 
                {
                    MessageBox.Show("Selecione uma imagem.", "Erro");
                    return;
                }
            }
           
            Settings.Mensagens.Add(novaMensagem);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {               
                checkBox2.Checked = false;
                pictureBox1.Enabled = false;
                richTextBox1.Enabled = true;
                novaMensagem.ImageMessage = false;
                tabControl1.SelectedTab = tabControl1.TabPages[0];
            }
            else
            {                
                checkBox2.Checked = true;
                pictureBox1.Enabled = true;
                richTextBox1.Enabled = false;
                novaMensagem.ImageMessage = true;
                tabControl1.SelectedTab = tabControl1.TabPages[1];
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {               
                checkBox1.Checked = false;
                pictureBox1.Enabled = true;
                richTextBox1.Enabled = false;
                novaMensagem.ImageMessage = true;
            }
            else
            {
                checkBox1.Checked = true;
                pictureBox1.Enabled = false;
                richTextBox1.Enabled = true;
                novaMensagem.ImageMessage = false;
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void selecionarImagemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
           
            if(dialog.ShowDialog()==DialogResult.OK)
            {
                Image img = Image.FromStream(dialog.OpenFile());
                pictureBox1.BackgroundImage = img;
                novaMensagem.ImageName = dialog.SafeFileName;
                novaMensagem.ImageURL = dialog.FileName;
                novaMensagem.Imagem = img;
                
            }
        }

        private void selecionarArquivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileStream img = new FileStream(dialog.FileName, FileMode.Open);
                pictureBox1.BackgroundImage = null;
                novaMensagem.FileURL = dialog.FileName;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        //Clicar e arrastar
        private void Arrastar_MouseDown(object sender, MouseEventArgs e)
        {
            arrastar = true;
            posiçãoX = e.X;
            posiçãoY = e.Y;
        }
        private void Arrastar_MouseUp(object sender, MouseEventArgs e)
        {
            arrastar = false;
        }
        private void Arrastar_MouseMove(object sender, MouseEventArgs e)
        {
            if (arrastar == true)
            {
                Location = new Point(MousePosition.X - posiçãoX, MousePosition.Y - posiçãoY);
            }
        }

        //Modificar Tamanho do botão
        private void button_MouseEnter(object sender, EventArgs e)
        {
            Button botão = sender as Button;
            botão.Height = botão.Height + 6;
            botão.Width = botão.Width + 6;
            botão.Location = new Point(botão.Location.X - 3, botão.Location.Y - 3);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            Button botão = sender as Button;
            botão.Height = botão.Height - 6;
            botão.Width = botão.Width - 6;
            botão.Location = new Point(botão.Location.X + 3, botão.Location.Y + 3);
        }

    }
}
