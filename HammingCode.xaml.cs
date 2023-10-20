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
    public sealed partial class HammingCode : Page
    {
        public string input_file_name = "input.txt";
        public string output_file_name = "output.txt";
        public HammingCode()
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
            InputPathTextBox.Text = temp + @"\" + input_file_name;
            OutputPathTextBox.Text = temp + @"\" + output_file_name;

            //Если файлов не существует, создать их
            await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);
        }

        private async void EncodeFileButton_Click(object sender, RoutedEventArgs e)
        {
            string output, input;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //Если файлов не существует, создать их
            StorageFile input_file = await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile output_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

            //Получение алфавита и сообщения из файлов
            input = await FileIO.ReadTextAsync(input_file);

            try
            {
                //Инициализация класса с использованием текста из файла probabilities.txt
                StartParameters sp = new StartParameters("", 2);

                //Кодирование информации из файла input.txt
                output = sp.EncodeHammingMessage(input);
                ResultTextBox.Text = output;

                //Запись результата в файл
                await FileIO.WriteTextAsync(output_file, output);

                //Вывод кодовых слов на экран
                CodeWordsTextBox.Text = "";
                for (int i = 0; i < sp.N; i++)
                {
                    CodeWordsTextBox.Text += sp.names[i] + " -> " + sp.code_words[i] + Environment.NewLine;
                }

                //Вывод характеристик на экран
                sp.GetNewCharacteristics();

                CharacteristicsTextBox.Text = "Кодовое расстояние d0 - " + sp.codeDistance + Environment.NewLine;
                CharacteristicsTextBox.Text += "Граница Хэмминга: " + sp.r + " >= " + sp.HammingBound;
                if (sp.HammingBound <= sp.r) { CharacteristicsTextBox.Text += ", условие выполняется." + Environment.NewLine; }
                else { CharacteristicsTextBox.Text += ", условие не выполняется." + Environment.NewLine; }
                CharacteristicsTextBox.Text += "Граница Плоткина: " + sp.codeDistance + " <= " + sp.PlotkinBound;
                if (sp.codeDistance <= sp.PlotkinBound) { CharacteristicsTextBox.Text += ", условие выполняется." + Environment.NewLine; }
                else { CharacteristicsTextBox.Text += ", условие не выполняется." + Environment.NewLine; }
                CharacteristicsTextBox.Text += "Граница Варшамова-Гильберта: " + Math.Pow(2, 1) + " > " + sp.Gilbert_VarshamovBound;
                if (Math.Pow(2, 1) > sp.Gilbert_VarshamovBound) { CharacteristicsTextBox.Text += ", условие выполняется." + Environment.NewLine; }
                else { CharacteristicsTextBox.Text += ", условие не выполняется." + Environment.NewLine; }
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
            string input;
            string[] output;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //Если файлов не существует, создать их
            StorageFile input_file = await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile output_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

            //Получение алфавита и сообщения из файлов
            input = await FileIO.ReadTextAsync(input_file);

            try
            {
                //Инициализация класса с использованием текста из файла probabilities.txt
                StartParameters sp = new StartParameters("", 2);

                //Декодирование информации из файла input.txt
                output = sp.DecodeHammingMessage(input);
                ResultTextBox.Text = output[0];
                MistakesTextBox.Text = output[1];

                //Запись результата в файл
                await FileIO.WriteTextAsync(output_file, output[0]);

                //Вывод кодовых слов на экран
                CodeWordsTextBox.Text = "";
                for (int i = 0; i < sp.N; i++)
                {
                    CodeWordsTextBox.Text += sp.names[i] + " -> " + sp.code_words[i] + Environment.NewLine;
                }

                //Вывод характеристик на экран
                sp.GetNewCharacteristics();

                CharacteristicsTextBox.Text = "Кодовое расстояние d0 - " + sp.codeDistance + Environment.NewLine;
                CharacteristicsTextBox.Text += "Граница Хэмминга: " + sp.r + " >= " + sp.HammingBound;
                if (sp.HammingBound <= sp.r) { CharacteristicsTextBox.Text += ", условие выполняется." + Environment.NewLine; }
                else { CharacteristicsTextBox.Text += ", условие не выполняется." + Environment.NewLine; }
                CharacteristicsTextBox.Text += "Граница Плоткина: " + sp.codeDistance + " <= " + sp.PlotkinBound;
                if (sp.codeDistance <= sp.PlotkinBound) { CharacteristicsTextBox.Text += ", условие выполняется." + Environment.NewLine; }
                else { CharacteristicsTextBox.Text += ", условие не выполняется." + Environment.NewLine; }
                CharacteristicsTextBox.Text += "Граница Варшамова-Гильберта: " + Math.Pow(2, 1) + " > " + sp.Gilbert_VarshamovBound;
                if (Math.Pow(2, 1) > sp.Gilbert_VarshamovBound) { CharacteristicsTextBox.Text += ", условие выполняется." + Environment.NewLine; }
                else { CharacteristicsTextBox.Text += ", условие не выполняется." + Environment.NewLine; }
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
