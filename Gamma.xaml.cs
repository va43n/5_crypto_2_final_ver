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
    public sealed partial class Gamma : Page
    {
        public string probs_file_name = "probabilities.txt";
        public string input_file_name = "input.txt";
        public string output_file_name = "output.txt";
        public Gamma()
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
            ResultPathTextBox.Text = temp + @"\" + input_file_name;
            KeyPathTextBox.Text = temp + @"\" + output_file_name;

            //Если файлов не существует, создать их
            await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);
        }

        private async void GetResultButton_Click(object sender, RoutedEventArgs e)
        {
            string message, key, result;
            KeyMode keyMode = KeyMode.Key2;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //Если файлов не существует, создать их
            StorageFile probs_file = await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile input_file = await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile output_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

            //Получение сообщения
            message = await FileIO.ReadTextAsync(probs_file);

            if (KeyModeComboBox.SelectedIndex == 0)
                keyMode = KeyMode.Key2;
            else if (KeyModeComboBox.SelectedIndex == 1)
                keyMode = KeyMode.Key16;
            else if (KeyModeComboBox.SelectedIndex == 2)
                keyMode = KeyMode.KeySymbol;

            key = GetKeyTextBox.Text;

            try
            {
                GammaClass gc = new GammaClass(message, key, keyMode);

                result = gc.EncodeMessage();

                ResultTextBox.Text = result;

                result = "Результат шифрования/дешифрования: " + result;
                await FileIO.WriteTextAsync(input_file, result);

                key = "Ключ, использованный для шифрования/дешифрования: " + key;
                await FileIO.WriteTextAsync(output_file, key);
            }
            catch (Exception exc)
            {
                //Если что-то пошло не так, на экран выводится сообщение с ошибкой
                MessageDialog md = new MessageDialog(exc.Message);
                await md.ShowAsync().AsTask();
            }
        }

        private async void GetKeyButton_Click(object sender, RoutedEventArgs e)
        {
            string key, message;
            KeyMode keyMode = KeyMode.Key2;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //Если файлов не существует, создать их
            StorageFile probs_file = await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            //Получение сообщения
            message = await FileIO.ReadTextAsync(probs_file);

            if (KeyModeComboBox.SelectedIndex == 0)
                keyMode = KeyMode.Key2;
            else if (KeyModeComboBox.SelectedIndex == 1)
                keyMode = KeyMode.Key16;
            else if (KeyModeComboBox.SelectedIndex == 2)
                keyMode = KeyMode.KeySymbol;

            try
            {
                key = GammaClass.GenerateKey(message.Length, keyMode);

                GetKeyTextBox.Text = key;
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
