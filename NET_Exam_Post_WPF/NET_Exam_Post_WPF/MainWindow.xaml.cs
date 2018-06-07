using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NET_Exam_Post_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<MailAddress> mailAddresses = new List<MailAddress>();
        private MailMessage m;

        public MainWindow()
        {
            InitializeComponent();

            string[] toArray = textBoxTo.Text.Split(',');

            //MailAddress to = new MailAddress(toArray[0]);

            m = new MailMessage();
        }

        private void BtnSendClick(object sender, RoutedEventArgs e)
        {
            try
            {
                MailAddress from = new MailAddress(textBoxFrom.Text);

                foreach (var address in textBoxTo.Text.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    m.To.Add(address);
                }

                m.From = from;
                m.Subject = "Тест";
                m.Body = textBoxText.Text;
                m.IsBodyHtml = false;

                foreach (var file in textBoxSelectFiles.Text.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    m.Attachments.Add(new Attachment(file));
                }

                //SmtpClient smtp = new SmtpClient("smtp.mail.ru", 465);
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

                smtp.Credentials = new NetworkCredential(textBoxFrom.Text, "5Sviuk:gma");
                smtp.EnableSsl = true;
                smtp.SendAsync(m, null);

                MessageBox.Show("Письмо отправлено");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSelectClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                FileInfo fileInfo = new FileInfo(filename);

                if ( fileInfo.Length <= 1024*1024*1024) //если файл больше 1 Гб
                {
                    textBoxSelectFiles.Text += filename + ",";
                }
                else
                {
                    MessageBox.Show("Файл слишком большой");
                }
            }
        }
    }
}
