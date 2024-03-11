using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace _5_crypto_2_final_ver
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Scrambler : Page
    {
        public string probs_file_name = "probabilities.txt";
        public string input_file_name = "input.txt";
        public string output_file_name = "output.txt";
        public Scrambler()
        {
            this.InitializeComponent();

            KeyModeComboBox.SelectedIndex = 0;

            CheckFiles();
        }

        private async void CheckFiles()
        {
            string temp;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            temp = storageFolder.Path;

            //Вывод путей к файлам на экран
            TextPathTextBox.Text = temp + @"\" + probs_file_name;
            ResultPathTextBox.Text = temp + @"\" + output_file_name;
            KeyPathTextBox.Text = temp + @"\" + input_file_name;

            //Если файлов не существует, создать их
            await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);
        }

        private async void GetResultButton_Click(object sender, RoutedEventArgs e)
        {
            string message, input, sequence, result, properties;
            KeyMode keyMode = KeyMode.Key2;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //Если файлов не существует, создать их
            StorageFile probs_file = await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile input_file = await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile output_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

            //Получение сообщения
            message = await FileIO.ReadTextAsync(probs_file);
            input = await FileIO.ReadTextAsync(input_file);

            if (KeyModeComboBox.SelectedIndex == 0)
                keyMode = KeyMode.Key2;
            else if (KeyModeComboBox.SelectedIndex == 1)
                keyMode = KeyMode.Key16;
            else if (KeyModeComboBox.SelectedIndex == 2)
                keyMode = KeyMode.KeySymbol;

            try
            {
                ScramblerClass scrambler = new ScramblerClass(message, input, keyMode);

                sequence = scrambler.GenerateSequence(message.Length * 8);
                GetKeyTextBox.Text = sequence;

                result = scrambler.EncodeMessage(sequence);
                properties = scrambler.GetProperties();

                result = string.Format("Результат шифрования/дешифрования: {0}\nСвойства:\n{1}", result, properties);

                ResultTextBox.Text = result;
                await FileIO.WriteTextAsync(output_file, result);
            }
            catch (Exception exc)
            {
                //Если что-то пошло не так, на экран выводится сообщение с ошибкой
                MessageDialog md = new MessageDialog(exc.Message);
                await md.ShowAsync().AsTask();
            }
        }
    }
}
