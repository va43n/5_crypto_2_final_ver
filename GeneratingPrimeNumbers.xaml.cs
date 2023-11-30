using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.Storage;

namespace _5_crypto_2_final_ver
{

    public sealed partial class GeneratingPrimeNumbers : Page
    {
        //задание названия файлов и инициализация основного класса
        public string probs_file_name = "probabilities.txt";
        public string output_file_name = "output.txt";
        GeneratingPrimeNumbersClass pn = new GeneratingPrimeNumbersClass();

        public GeneratingPrimeNumbers()
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
            OutputPathTextBox.Text = temp + @"\" + output_file_name;

            //Если файлов не существует, создать их
            await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);
        }

        private async void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            //Функция, вызывающаяся при нажатии на кнопку "Сгенерировать простое число"
            string parameters, output;
            int result;

            //Получение доступа к нужной папке
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //Если файлов не существует, создать их
            StorageFile probs_file = await storageFolder.CreateFileAsync(probs_file_name, CreationCollisionOption.OpenIfExists);
            StorageFile output_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

            try
            {
                //Получение параметров из первого файла и выполнение основной функции класса
                parameters = await FileIO.ReadTextAsync(probs_file);
                pn.SetCharacteristicsAndGenerate(parameters);
                result = pn.PrimeNumber;

                //запись результата в файл и на экран
                output = "Полученное простое число: " + result + ". Результат был получен за время - " + "0" + ". Количество проверенных чисел, включая результирующее, равно " + pn.Iterations + Environment.NewLine + "Все числа, подвергшиеся проверке: ";
                foreach (int num in pn.AllNumbers)
                {
                    output += num + " ";
                }

                ResultTextBox.Text = output;
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
