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
	public sealed partial class Hash : Page
	{
		public string input_file_name = "input.txt";
		public string output_file_name = "output.txt";

		public Hash()
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
			TextPathTextBox.Text = temp + @"\" + input_file_name;
			ResultPathTextBox.Text = temp + @"\" + output_file_name;

			//Если файлов не существует, создать их
			await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
			await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);
		}

		private async void GetResultButton_Click(object sender, RoutedEventArgs e)
		{
			string message, result;

			//Получение доступа к нужной папке
			StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

			//Если файла не существует, создать его
			StorageFile output_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

            //Получение сообщения
            message = ChangeTextTextBox.Text;

            try
			{
                MD5Class md5 = new MD5Class(message, -1);
                
				result = md5.CalculateHash();
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

		private async void ReadFileButton_Click(object sender, RoutedEventArgs e)
		{
			string message;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            //Если файла не существует, создать его
            StorageFile input_file = await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);

            //Получение сообщения
            message = await FileIO.ReadTextAsync(input_file);
            ChangeTextTextBox.Text = message;
        }

		private async void StartCheckButton_Click(object sender, RoutedEventArgs e)
		{
			string message, result;
			int position;

            //Получение сообщения
            message = ChangeTextTextBox.Text;
			try
			{
                position = Convert.ToInt32(StartCheckTextBox.Text);

                MD5Class md5 = new MD5Class(message, position);

                result = md5.CalculateHash();
                CheckResultTextTextBox.Text = result;
            }
			catch
			{
                //Если что-то пошло не так, на экран выводится сообщение с ошибкой
                MessageDialog md = new MessageDialog("Введено некорректное значение");
                await md.ShowAsync().AsTask();
            }
        }

    }
}
