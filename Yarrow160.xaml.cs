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
    public sealed partial class Yarrow160 : Page
    {
        public string input_file_name = "input.txt";
        public string output_file_name = "output.txt";

        public Yarrow160()
        {
            this.InitializeComponent();
            CheckFiles();
        }

        private async void CheckFiles()
        {
            string temp;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            temp = storageFolder.Path;

            //Вывод путей к файлам на экран
            GeneratorCoefficientsTextBox.Text = temp + @"\" + input_file_name;
            OutputPathTextBox.Text = temp + @"\" + output_file_name;

            //Если файлов не существует, создать их
            await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);
        }

        private async void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            string output, input;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //Если файлов не существует, создать их
            StorageFile input_file = await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile output_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

            //Получение параметров из файла
            input = await FileIO.ReadTextAsync(input_file);

            try
            {
                Yarrow160Class ya = new Yarrow160Class(input);

                ya.GenerateSequence();
                output = ya.output;

                output += string.Format("\n\nv:\n{0}\nВсего битов: {1}", ya.savedV, ya.totalNumberOfBytes);

                GenerateResultTextBox.Text = output;
                await FileIO.WriteTextAsync(output_file, output);
            }
            catch (Exception exc)
            {
                //Если что-то пошло не так, на экран выводится сообщение с ошибкой
                MessageDialog message = new MessageDialog(exc.Message);
                await message.ShowAsync().AsTask();
            }
        }

        private async void TestButton_Click(object sender, RoutedEventArgs e)
        {
            string input, output;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //Если файлов не существует, создать их
            StorageFile input_file = await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile output_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

            //Получение параметров из файла
            input = await FileIO.ReadTextAsync(input_file);

            try
            {
                Yarrow160Class ya = new Yarrow160Class(input);

                output = ya.TestGenerator();

                TestResultTextBox.Text = output;
                GenerateResultTextBox.Text = ya.output;

                await FileIO.WriteTextAsync(output_file, output);
            }
            catch (Exception exc)
            {
                //Если что-то пошло не так, на экран выводится сообщение с ошибкой
                MessageDialog message = new MessageDialog(exc.Message);
                await message.ShowAsync().AsTask();
            }
        }
    }
}
