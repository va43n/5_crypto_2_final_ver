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
    public sealed partial class FileInput : Page
    {
        public string probs_file_name = "probabilities.txt";
        public string input_file_name = "input.txt";
        public string output_file_name = "output.txt";
        public FileInput()
        {
            this.InitializeComponent();
            CheckFiles();
        }

        private async void CheckFiles()
        {
            string temp;

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            temp = storageFolder.Path;

            ProbsPathTextBox.Text = temp + @"\" + probs_file_name;
            InputPathTextBox.Text = temp + @"\" + input_file_name;
            OutputPathTextBox.Text = temp + @"\" + output_file_name;

            await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);
        }

        private async void EncodeFileButton_Click(object sender, RoutedEventArgs e)
        {
            string output, probs, input;

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile probs_file = await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile input_file = await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile output_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

            probs = await FileIO.ReadTextAsync(probs_file);
            input = await FileIO.ReadTextAsync(input_file);

            try
            {
                StartParameters sp = new StartParameters(probs);

                output = sp.CodeMessage(input);
                ResultTextBox.Text = output;

                await FileIO.WriteTextAsync(output_file, output);

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
                await message.ShowAsync().AsTask();
            }
        }

        private async void CodeFileButton_Click(object sender, RoutedEventArgs e)
        {
            string output, probs, input;

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile probs_file = await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile input_file = await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile output_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

            probs = await FileIO.ReadTextAsync(probs_file);
            input = await FileIO.ReadTextAsync(input_file);

            try
            {
                StartParameters sp = new StartParameters(probs);

                output = sp.DecodeMessage(input);
                ResultTextBox.Text = output;

                await FileIO.WriteTextAsync(output_file, output);

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
                await message.ShowAsync().AsTask();
            }
        }
    }
}