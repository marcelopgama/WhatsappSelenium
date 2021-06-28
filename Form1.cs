using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhatsappSelenium
{
    public partial class Form1 : Form
    {
        IWebDriver driver;
        bool arrastar = false;
        int posiçãoX, posiçãoY;

        public Form1()
        {
            InitializeComponent();
            Settings.Mensagens = new List<MessageClass>();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        void NovaConversa()
        {
            try
            {
                driver.FindElement(By.XPath("//div[@title='Nova conversa']")).Click();
            }
            catch 
            {
                Task.Delay(1000);
                NovaConversa(); 
            }
        }

        //Enviar mensagens
        private void button1_Click(object sender, EventArgs e)
        {
            int enviadas = 0;

            Settings.ContatosNãoEnviados = new List<string>();

            string contato = "";

            progressBar1.Value = 0;
            int maxValue = dataGridView1.RowCount;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    contato = row.Cells[0].Value.ToString();
                }

                try
                {
                    try
                    {
                        driver.FindElement(By.XPath("//div[@title='Nova conversa']")).Click();
                    }
                    catch
                    {
                        driver.Navigate().Refresh();

                        bool visivel = false;
                        //arrumar erro (Não clicável)
                        do
                        {
                            try
                            {
                                driver.FindElement(By.XPath("//div[@title='Nova conversa']")).Click();
                                visivel = true;
                            }
                            catch
                            {
                                Task.Delay(1000);
                            }
                                                   
                        } while (visivel == false);                        
                    }

                    if (row.Cells[0].Value != null)
                    {
                   
                    contato = row.Cells[0].Value.ToString();

                    if (contato != String.Empty && contato != "")
                    {
                        driver.FindElement(By.XPath("//div[@data-tab='3']")).Clear();
                        driver.FindElement(By.XPath("//div[@data-tab='3']")).SendKeys(contato);

                        Thread.Sleep(1500);

                        try
                        {
                            List<IWebElement> elements = driver.FindElements(By.XPath("//span[@class='_3ko75 _5h6Y_ _3Whw5']")).ToList().Where(x => x.Text.IndexOf(contato, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

                            if (elements.Count > 0)
                            {
                                elements[0].Click();

                                foreach (MessageClass message in Settings.Mensagens)
                                {
                                    //Verificar quantidade de mensagens enviadas
                                    if (enviadas == 15)
                                    {
                                        Thread.Sleep(2000);
                                        enviadas = 0;
                                    }

                                    //Enviar texto
                                    if (message.ImageMessage == false)
                                    {
                                        string texto = message.Text;
                                        driver.FindElement(By.XPath("//div[@data-tab='1']")).Clear();
                                        driver.FindElement(By.XPath("//div[@data-tab='1']")).SendKeys(texto);
                                        driver.FindElement(By.XPath("//div[@data-tab='1']")).SendKeys(OpenQA.Selenium.Keys.Enter);
                                    }
                                    //Enviar imagem
                                    else
                                    {

                                        Clipboard.SetImage(message.Imagem);

                                        driver.FindElement(By.XPath("//div[@data-tab='1']")).SendKeys(OpenQA.Selenium.Keys.Control + "v");

                                        bool visivel = false;

                                        while (visivel == false)
                                        {
                                            try
                                            {

                                                driver.FindElement(By.XPath("//span[@data-testid='send']")).Click();
                                                Thread.Sleep(500);
                                                visivel = true;
                                            }
                                            catch { visivel = false; }
                                        }
                                        enviadas++;

                                    }

                                }
                            }
                            else
                            {
                                if (Settings.ContatosNãoEnviados.Contains(contato) == false)
                                {
                                    Settings.ContatosNãoEnviados.Add(contato);
                                }

                            }


                        }
                        catch { }


                    }
                }
                    }
                    catch
                    {
                        Settings.ContatosNãoEnviados.Add(contato);

                    }

                    Thread.Sleep(1000);

                    Invoke(new Action(() =>
                    {
                        progressBar1.Value = Convert.ToInt32(dataGridView1.Rows.IndexOf(row) / maxValue);
                    }));

              
            }

            //Abrir relatório
            if (Settings.ContatosNãoEnviados.Count > 0)
            {
                Form3 form = new Form3();
                form.NãoEnviados = Settings.ContatosNãoEnviados.Count();
                form.Enviados = dataGridView1.Rows.Count - form.NãoEnviados;
                form.ShowDialog();
            }


        }
                
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dataGrid = sender as DataGridView;

            if (dataGrid.Focused == true)
            {

                if (e.Control == true && e.KeyCode == System.Windows.Forms.Keys.V)
                {
                    string text = Clipboard.GetText();
                    string[] separadores = new string[1] { "\r\n" };
                    Console.WriteLine(text);
                    List<string> textoSeparado = text.Split(separadores, StringSplitOptions.RemoveEmptyEntries).ToList();

                    int i = 0;

                    dataGridView1.Rows.Add(textoSeparado.Count);

                    for (int j = 0; j < textoSeparado.Count; j++)
                    {
                        Console.WriteLine("J:" + j + " I:" + i);
                        dataGridView1.Rows[i].Cells[0].Value = textoSeparado[j];
                        i++;
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.FormClosing += Form2_FormClosing;
            form2.ShowDialog();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {

            listBox1.Items.Clear();

            foreach (MessageClass msg in Settings.Mensagens)
            {
                if (msg.ImageMessage == false)
                {
                    listBox1.Items.Add(msg.Text);
                }
                else
                {
                    listBox1.Items.Add(msg.ImageName);
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = listBox1.SelectedIndex;

                Settings.Mensagens.RemoveAt(selectedIndex);
                listBox1.Items.RemoveAt(selectedIndex);
            }
            catch { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                driver = new FirefoxDriver();

                driver.Navigate().GoToUrl("https://web.whatsapp.com/");
                driver.Manage().Window.Maximize();
            }
            catch { MessageBox.Show("Erro ao tentar abrir o Firefox", "Erro"); }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                driver = new ChromeDriver();

                driver.Navigate().GoToUrl("https://web.whatsapp.com/");
                driver.Manage().Window.Maximize();
            }
            catch { MessageBox.Show("Erro ao tentar abiri o Chrome", "Erro"); }

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

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                driver.Close();
                driver.Quit();
            }
            catch { }
            Application.Exit();
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
    }
}
