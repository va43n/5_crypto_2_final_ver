using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.Storage;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace _5_crypto_2_final_ver
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class VigenereCipher : Page
    {

        public string probs_file_name = "probabilities.txt";
        public string input_file_name = "input.txt";
        public string output_file_name = "output.txt";

        public VigenereCipher()
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
            ProbsPathTextBox.Text = temp + @"\" + probs_file_name;
            InputPathTextBox.Text = temp + @"\" + input_file_name;
            OutputPathTextBox.Text = temp + @"\" + output_file_name;

            //Если файлов не существует, создать их
            await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);
        }

        private async void EncodeFileButton_Click(object sender, RoutedEventArgs e)
        {
            string output, key, input;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //Если файлов не существует, создать их
            StorageFile probs_file = await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile input_file = await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile output_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

            //Получение ключа и сообщения из файлов
            key = await FileIO.ReadTextAsync(probs_file);
            input = await FileIO.ReadTextAsync(input_file);

            try
            {
                VigenereCipherClass vigenereCipher = new VigenereCipherClass();
                vigenereCipher.GetCharacteristics(key, input);
                vigenereCipher.EncodeDecodeMessage(true);

                output = vigenereCipher.Output;
                ResultTextBox.Text = output;
                await FileIO.WriteTextAsync(output_file, output);

                GetTheorem(vigenereCipher);
            }
            catch (Exception exc)
            {
                //Если что-то пошло не так, на экран выводится сообщение с ошибкой
                MessageDialog message = new MessageDialog(exc.Message);
                await message.ShowAsync().AsTask();
            }
        }

        private async void DecodeFileButton_Click(object sender, RoutedEventArgs e)
        {
            string output, key, input;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //Если файлов не существует, создать их
            StorageFile probs_file = await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile input_file = await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile output_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

            //Получение ключа и сообщения из файлов
            key = await FileIO.ReadTextAsync(probs_file);
            input = await FileIO.ReadTextAsync(input_file);

            try
            {
                VigenereCipherClass vigenereCipher = new VigenereCipherClass();
                vigenereCipher.GetCharacteristics(key, input);
                vigenereCipher.EncodeDecodeMessage(false);

                output = vigenereCipher.Output;
                ResultTextBox.Text = output;
                await FileIO.WriteTextAsync(output_file, output);

                GetTheorem(vigenereCipher);
            }
            catch (Exception exc)
            {
                //Если что-то пошло не так, на экран выводится сообщение с ошибкой
                MessageDialog message = new MessageDialog(exc.Message);
                await message.ShowAsync().AsTask();
            }
        }

        private void GetTheorem(VigenereCipherClass vigenereCipher)
        {
            TheoremTextBox.Text = "";
            TheoremTextBox.Text += "Необходимое и достаточное условие (P{Y|X} = P{Y}): " + Convert.ToString(1 / Math.Pow(vigenereCipher.Alphabet.Length, vigenereCipher.T)) + " == " + Convert.ToString(1 / Math.Pow(vigenereCipher.Alphabet.Length, vigenereCipher.NumInput.Length)) + " условие ";
            TheoremTextBox.Text += (1 / Math.Pow(vigenereCipher.Alphabet.Length, vigenereCipher.T) <= 1 / Math.Pow(vigenereCipher.Alphabet.Length, vigenereCipher.NumInput.Length)) ? "выполняется." : "не выполняется.";
            TheoremTextBox.Text += Environment.NewLine;
        }


    }
}
