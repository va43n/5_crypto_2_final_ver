using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace _5_crypto_2_final_ver
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            frame.Content = new MainMenu();
        }

        private void GoToMainMenu(object sender, RoutedEventArgs e)
        {
            frame.Content = new MainMenu();
        }

        private void GoToKeyboardInput(object sender, RoutedEventArgs e)
        {
            frame.Content = new KeyboardInput();
        }

        private void GoToFileInput(object sender, RoutedEventArgs e)
        {
            frame.Content = new FileInput();
        }

        private void GoToNewCode(object sender, RoutedEventArgs e)
        {
            frame.Content = new NewCode();
        }
    }

    class StartParameters
    {
        public readonly int N;
        public readonly double[] probabilities;
        public readonly string[] names;
        public string[] code_words;
        private readonly string[] indices;
        public readonly int max_code_word_length;
        public readonly double average_length;
        public readonly double redundancy;
        public readonly double KraftInequality;

        public StartParameters(string probs, int mode = 0)
        {
            string[] parameters;
            int counter = 0;
            double temp = 0;
            int min1, min2;
            bool[] used;
            string text = probs;

            //Проверка валидности текста, задающего алфавит
            if (text == null || !text.Contains(" ")) { throw new Exception("Неправильная запись алфавита. Каждый символ должна сопровождаться вероятностью её появления, между символами и вероятностями должны стоять пробелы"); }

            if (mode == 0)
            {
                //Массив элементов алфавита
                parameters = text.Split(' ');
                //Если элементов в алфавите нечетное число, значит пропущен какой-либо элемент
                N = parameters.Length;
                if (N % 2 != 0) { throw new Exception("Элементов должно быть четное число, нечетные элементы - буквы алфавита, четные элементы - соответствующие буквам вероятности. Скорее всего элементов не хватает."); }

                //Необходимо убедиться, что символов в алфавите хотя бы 2
                if (N < 4) { throw new Exception("Символов введено слишком мало. Введите хотя бы два символа и повторите попытку."); }
                N /= 2;

                names = new string[N];
                code_words = new string[N];
                probabilities = new double[N + N];
                probabilities[N + N - 1] = 2;

                foreach (string p in parameters)
                {
                    //Проверка символов алфавита
                    if (counter % 2 == 0)
                    {
                        if (Array.Exists(names, element => element == p)) { throw new Exception("Алфавит должен состоять из неповторяющихся элементов. Элемент \"" + p + "\" повторяется."); }
                        names[counter / 2] = p;
                    }
                    //Проверка вероятностей алфавита
                    else
                    {
                        try { probabilities[(counter - 1) / 2] = Convert.ToDouble(p); }
                        catch { throw new Exception("Вероятность \"" + p + "\" не является числом. Возможно следует поменять \".\" на \",\"."); }

                        if (probabilities[(counter - 1) / 2] < 0) { throw new Exception("Вероятность \"" + p + "\" меньше 0, а должна быть больше или равна 0."); }

                        temp += probabilities[(counter - 1) / 2];
                    }

                    counter++;
                }

                //Проверка равенства суммы вероятностей и единицы
                if (!(temp + Math.Pow(10, -10) >= 1 && temp - Math.Pow(10, -10) <= 1)) { throw new Exception("Сумма всех вероятностей должна быть равна 1. Сейчас она равна " + temp + "."); }

                indices = new string[N + N - 1];
            }
            else if (mode == 1)
            {
                names = text.Split(' ');
                N = names.Length;
                probabilities = new double[N + N];
                code_words = new string[N];
                indices = new string[N + N - 1];

                probabilities[N + N - 1] = 2;
                temp = 1.0 / N;
                for (int i = 0; i < N; i++)
                {
                    probabilities[i] = temp;
                }
            }

            used = new bool[N + N - 1];

            for (int i = 0; i < N; i++) { indices[i] = Convert.ToString(i); }

            //Создание бинарного дерева из имеющегося алфавита
            for (int i = N; i < 2 * N - 1; i++)
            {
                min1 = N + N - 1;
                min2 = N + N - 1;
                //Поиск двух самых малых вероятностей
                for (int j = 0; j < i; j++)
                {
                    if (probabilities[j] < probabilities[min1] && j != min1 && !used[j])
                    {
                        min2 = min1;
                        min1 = j;
                    }
                    else if (probabilities[j] < probabilities[min2] && j != min1 && !used[j]) { min2 = j; }
                }
                //Объединение самых малых вероятностей в новую вероятность
                probabilities[i] = probabilities[min1] + probabilities[min2];
                if (min1 < min2) { indices[i] = Convert.ToString(min2) + ":" + Convert.ToString(min1); }
                else { indices[i] = Convert.ToString(min1) + ":" + Convert.ToString(min2); }

                //Найденные вероятности больше не учитываются при поиске следующих самых малых вероятностей
                used[min1] = true;
                used[min2] = true;
            }

            //Обход бинарного дерева с целью получения кодовых слов
            FindCodeWords(N + N - 2, "");

            //Подсчет характеристик
            max_code_word_length = 0;
            KraftInequality = 0;
            temp = 0;
            //Получение средней длины и суммы в правой части неравенства Крафта
            for (int i = 0; i < N; i++)
            {
                temp += code_words[i].Length * probabilities[i];
                KraftInequality += Math.Pow(2, -code_words[i].Length);
                if (code_words[i].Length > max_code_word_length) { max_code_word_length = code_words[i].Length; }
            }
            average_length = temp;

            //Поиск избыточности
            redundancy = 0;
            for (int i = 0; i < N; i++)
            {
                if (probabilities[i] == 0) { continue; }
                redundancy += probabilities[i] * Math.Log10(probabilities[i]) / Math.Log10(2);
            }
            redundancy /= Math.Log10(N) / Math.Log10(2);
            redundancy += 1;
            if (redundancy < Math.Pow(10, -10)) { redundancy = 0; }

        }

        private void FindCodeWords(int i, string current_code_word)
        {
            string[] halfs;

            halfs = indices[i].Split(':');
            //Если левый элемент является "листом" дерева, то к последовательности добавляется еще один "0" и таким образом получается кодовое слово, по индексу совпадающее с индексом "листа"
            if (Convert.ToInt32(halfs[0]) < N) { code_words[Convert.ToInt32(halfs[0])] = current_code_word + "0"; }
            //Иначе к последовательности добавляется "0"
            else { FindCodeWords(Convert.ToInt32(halfs[0]), current_code_word + "0"); }
            //Если правый элемент является "листом" дерева, то к последовательности добавляется еще одна "1" и таким образом получается кодовое слово, по индексу совпадающее с индексом "листа"
            if (Convert.ToInt32(halfs[1]) < N) { code_words[Convert.ToInt32(halfs[1])] = current_code_word + "1"; }
            //Иначе к последовательности добавляется "1"
            else { FindCodeWords(Convert.ToInt32(halfs[1]), current_code_word + "1"); }
        }

        public string CodeMessage(string input)
        {
            //Кодирование сообщения
            string text, output = "";
            bool isElementConsists;

            text = input;

            //Для каждого символа сообщения проверяется, существует ли он в заданном алфавите, если да, то в выходное сообщение записывается соответсвующее кодовое слово, иначе вызывается исключение
            foreach (char letter in text)
            {
                isElementConsists = false;
                for (int i = 0; i < N; i++)
                {
                    if (names[i] == Convert.ToString(letter))
                    {
                        isElementConsists = true;
                        output += code_words[i];
                        break;
                    }
                }
                if (!isElementConsists) { throw new Exception("Элемент " + Convert.ToString(letter) + " не существует в алфавите."); }
            }

            return output;
        }

        public string DecodeMessage(string input)
        {
            //Декодирование сообщения
            string text, code_word = "", output = "";

            text = input;

            foreach (char letter in text)
            {
                code_word += Convert.ToString(letter);
                //Поэлементно добавляем к некоторой строке значения из сообщения, если существует такое кодовое слово, то соответствующий символ алфавита записывается в выходную строку
                for (int i = 0; i < N; i++)
                {
                    if (code_word == code_words[i])
                    {
                        output += names[i];
                        code_word = "";
                        break;
                    }
                }
                //Если некоторая строка превышает максимальную длину кодового слова, значит сообщение для декодирования заисано неверно
                if (code_word.Length == max_code_word_length) { throw new Exception("Текст невозможно декодировать, проверьте правильность ввода и повторите попытку."); }
            }
            //Если что-то осталось после попытки декодирования, значит сообщение для декодирования записано неверно
            if (code_word != "") { throw new Exception("Текст невозможно декодировать, проверьте правильность ввода и повторите попытку."); }

            return output;
        }

        public void AddEvenBit()
        {
            int temp;

            for (int i = 0; i < N; i++)
            {
                temp = 0;
                foreach(char letter in code_words[i])
                {
                    temp += letter - '0';
                    temp %= 2;
                }
                code_words[i] += Convert.ToString(temp);
            }
        }

        public string[] DecodeWithMistakes(string input)
        {
            string[] output;
            string decode = "", mistakes = "", code_word;
            int numberOfCodeWords, bit;

            if (input.Length % (max_code_word_length + 1) != 0) { throw new Exception("Текст невозможно декодировать, проверьте правильность ввода и повторите попытку."); }

            numberOfCodeWords = input.Length / (max_code_word_length + 1);
            for (int i = 0; i < numberOfCodeWords; i++)
            {
                code_word = "";
                for (int j = i * (max_code_word_length + 1); j < i * (max_code_word_length + 1) + max_code_word_length + 1; j++)
                {
                    code_word += input[j];
                }

                bit = 0;
                foreach (char letter in code_word)
                {
                    bit += letter - '0';
                    bit %= 2;
                }

                if (bit != 0) { mistakes += Convert.ToString(i + 1) + ": " + code_word + ". "; }
                else
                {
                    for (int k = 0; k < N; k++)
                    {
                        if (code_word == code_words[k])
                        {
                            decode += names[k];
                            break;
                        }
                    }
                }
            }

            output = new string[2];
            output[0] = decode;
            output[1] = mistakes;
            return output;
        }
    }
}
