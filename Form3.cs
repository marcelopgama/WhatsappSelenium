using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhatsappSelenium
{
    public partial class Form3 : Form
    {
        bool arrastar = false;
        int posiçãoX,posiçãoY;
        public int Enviados,NãoEnviados;        

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label1.Text = "Enviados: " + Enviados;
            label3.Text = "Não enviados: " + NãoEnviados;

            foreach(String s in Settings.ContatosNãoEnviados)
            {
                listBox1.Items.Add(s);
            }
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

        private void button_MouseLeave(object sender, EventArgs e)
        {
            Button botão = sender as Button;
            botão.Height = botão.Height - 6;
            botão.Width = botão.Width - 6;
            botão.Location = new Point(botão.Location.X + 3, botão.Location.Y + 3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
