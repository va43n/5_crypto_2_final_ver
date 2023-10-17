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
    public sealed partial class NewCode : Page
    {
        public string probs_file_name = "probabilities.txt";
        public string input_file_name = "input.txt";
        public string output_file_name = "output.txt";
        public NewCode()
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
            string output, probs, input;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //Если файлов не существует, создать их
            StorageFile probs_file = await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile input_file = await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile output_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

            //Получение алфавита и сообщения из файлов
            probs = await FileIO.ReadTextAsync(probs_file);
            input = await FileIO.ReadTextAsync(input_file);

            try
            {
                //Инициализация класса с использованием текста из файла probabilities.txt
                StartParameters sp = new StartParameters(probs);
                sp.AddEvenBit();

                //Кодирование информации из файла input.txt
                output = sp.CodeMessage(input);
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
                CharacteristicsTextBox.Text = "Средняя длина кодового слова - " + sp.average_length + Environment.NewLine;
                CharacteristicsTextBox.Text += "Избыточность - " + sp.redundancy + Environment.NewLine;
                CharacteristicsTextBox.Text += "Неравенство Крафта - сумма равна " + sp.KraftInequality + " ";
                if (sp.KraftInequality < 1) { CharacteristicsTextBox.Text += "< 1, условие выполняется."; }
                else if (sp.KraftInequality == 1) { CharacteristicsTextBox.Text += "= 1, оптимальная кодировка."; }
                else { CharacteristicsTextBox.Text += "> 1, условие не выполняется."; }
            }
            catch (Exception exc)
            {
                //Если что-то пошло не так, на экран выводится сообщение с ошибкой
                MessageDialog message = new MessageDialog(exc.Message);
                await message.ShowAsync().AsTask();
            }
        }

        private async void CodeFileButton_Click(object sender, RoutedEventArgs e)
        {
            string probs, input;
            string[] output;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //Если файлов не существует, создать их
            StorageFile probs_file = await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile input_file = await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile output_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

            //Получение алфавита и сообщения из файлов
            probs = await FileIO.ReadTextAsync(probs_file);
            input = await FileIO.ReadTextAsync(input_file);

            try
            {
                //Инициализация класса с использованием текста из файла probabilities.txt
                StartParameters sp = new StartParameters(probs);
                sp.AddEvenBit();

                //Кодирование информации из файла input.txt
                output = sp.DecodeWithMistakes(input);
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
                CharacteristicsTextBox.Text = "Средняя длина кодового слова - " + sp.average_length + Environment.NewLine;
                CharacteristicsTextBox.Text += "Избыточность - " + sp.redundancy + Environment.NewLine;
                CharacteristicsTextBox.Text += "Неравенство Крафта - сумма равна " + sp.KraftInequality + " ";
                if (sp.KraftInequality < 1) { CharacteristicsTextBox.Text += "< 1, условие выполняется."; }
                else if (sp.KraftInequality == 1) { CharacteristicsTextBox.Text += "= 1, оптимальная кодировка."; }
                else { CharacteristicsTextBox.Text += "> 1, условие не выполняется."; }
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
