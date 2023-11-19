using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.Storage;


namespace _5_crypto_2_final_ver
{
    public sealed partial class Generating : Page
    {
        //задание названия файлов и инициализация основного класса
        public string probs_file_name = "probabilities.txt";
        public string input_file_name = "input.txt";
        public string output_file_name = "output.txt";
        public GeneratingClass g = new GeneratingClass();

        public Generating()
        {
            this.InitializeComponent();
            CheckFiles();
        }

        private async void CheckFiles()
        {
            //Проверка файлов на существование
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

        private async void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            //Функция, вызывающаяся при нажатии на кнопку "Сгенерировать последовательность"
            string parameters, output;
            double check;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //Если файлов не существует, создать их
            StorageFile probs_file = await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile output1_file = await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile output2_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

            try
            {
                //Получение параметров из первого файла и выполнение основной функции класса
                parameters = await FileIO.ReadTextAsync(probs_file);
                g.SetParametersAndGenerateSubsequence(parameters);
                output = g.Subsequence;

                //запись последовательности в файл и на экран
                ResultTextBox.Text = output;
                await FileIO.WriteTextAsync(output1_file, output);

                //проверка гипотезы, вывод на экран и в файл
                output = "Проверка качества сгенерированной последовательности: при степени свободы, равной 9, и уровне значимости, равном 0.05, оценка S = ";
                TheoremTextBox.Text = "Проверка качества сгенерированной последовательности: при степени свободы, равной 9, и уровне значимости, равной 0.05, оценка S = ";
                check = g.CheckHypothesis();
                output += check;
                TheoremTextBox.Text += check;
                if (check < 16.91898)
                {
                    output += ", гипотеза не отвергается.";
                    TheoremTextBox.Text += ", гипотеза не отвергается.";
                }
                else
                {
                    output += ", гипотеза отвергается.";
                    TheoremTextBox.Text += ", гипотеза отвергается.";
                }
                output += Environment.NewLine;
                TheoremTextBox.Text += Environment.NewLine;

                //поиск периода, вывод на экран и в файл
                check = g.FindPeriod();
                output += "Период последовательности T = ";
                TheoremTextBox.Text += "Период последовательности T = ";
                output += check;
                TheoremTextBox.Text += check;

                await FileIO.WriteTextAsync(output2_file, output);
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
