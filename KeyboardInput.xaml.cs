using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Windows;
using Microsoft.Win32;
using Windows.Storage;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace _5_crypto_2_final_ver
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class KeyboardInput : Page
    {
        public KeyboardInput()
        {
            this.InitializeComponent();
        }
        private async void FirstChooseButton_Click(object sender, RoutedEventArgs e)
        {
            string output;
            try
            {
                StartParameters sp = new StartParameters(EnterProbsTextBox.Text);

                output = sp.CodeMessage(EnterInputTextBox.Text);
                ResultTextBox.Text = output;

                CodeWordsTextBox.Text = "";
                for (int i = 0; i < sp.N; i++)
                {
                    CodeWordsTextBox.Text += sp.names[i] + " -> " + sp.code_words[i] + Environment.NewLine;
                }

                CharacteristicsTextBox.Text = "Средняя длина кодового слова - " + sp.average_length + Environment.NewLine;
                CharacteristicsTextBox.Text += "Избыточность - " + sp.redundancy + Environment.NewLine;
                CharacteristicsTextBox.Text += "Неравенство Крафта - сумма равна " + sp.KraftInequality + " ";
                if (sp.KraftInequality < 1) { CharacteristicsTextBox.Text += "< 1, условие выполняется."; }
                else if (sp.KraftInequality == 1) { CharacteristicsTextBox.Text += "= 1, оптимальная кодировка."; }
                else { CharacteristicsTextBox.Text += "> 1, условие не выполняется."; }
            }
            catch (Exception exc)
            {
                MessageDialog message = new MessageDialog(exc.Message);
                message.ShowAsync().AsTask();
            }
        }

        private async void SecondChooseButton_Click(object sender, RoutedEventArgs e)
        {
            string output;
            try
            {
                StartParameters sp = new StartParameters(EnterProbsTextBox.Text);

                output = sp.DecodeMessage(EnterInputTextBox.Text);
                ResultTextBox.Text = output;

                CodeWordsTextBox.Text = "";
                for (int i = 0; i < sp.N; i++)
                {
                    CodeWordsTextBox.Text += sp.names[i] + " -> " + sp.code_words[i] + Environment.NewLine;
                }

                CharacteristicsTextBox.Text = "Средняя длина кодового слова - " + sp.average_length + Environment.NewLine;
                CharacteristicsTextBox.Text += "Избыточность - " + sp.redundancy + Environment.NewLine;
                CharacteristicsTextBox.Text += "Неравенство Крафта - сумма равна " + sp.KraftInequality + " ";
                if (sp.KraftInequality < 1) { CharacteristicsTextBox.Text += "< 1, условие выполняется."; }
                else if (sp.KraftInequality == 1) { CharacteristicsTextBox.Text += "= 1, оптимальная кодировка."; }
                else { CharacteristicsTextBox.Text += "> 1, условие не выполняется."; }
            }
            catch (Exception exc)
            {
                MessageDialog message = new MessageDialog(exc.Message);
                message.ShowAsync().AsTask();
            }
        }
    }
}
