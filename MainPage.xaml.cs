﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;

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

        private void GoToHammingCode(object sender, RoutedEventArgs e)
        {
            frame.Content = new HammingCode();
        }

        private void GoToVigenereCipher(object sender, RoutedEventArgs e)
        {
            frame.Content = new VigenereCipher();
        }

        private void GoToGenerating(object sender, RoutedEventArgs e)
        {
            frame.Content = new Generating();
        }

        private void GoToGeneratingPrimeNumbers(object sender, RoutedEventArgs e)
        {
            frame.Content = new GeneratingPrimeNumbers();
        }
    }

    class StartParameters
    {
        public readonly int N;
        public readonly double[] probabilities;
        public readonly string[] names;
        public string[] code_words;
        private readonly string[] indices;
        public readonly int k;
        public readonly int r;
        public readonly int n;
        public readonly double average_length;
        public readonly double redundancy;
        public readonly double KraftInequality;
        public int codeDistance;
        public double HammingBound;
        public double PlotkinBound;
        public int Gilbert_VarshamovBound;
        public int[] H;
        public int[] G;


        public StartParameters(string probs, int mode = 0)
        {
            string[] parameters;
            int counter = 0;
            double temp = 0;
            int min1, min2;
            bool[] used;
            string text = probs;


            if (mode == 0)
            {
                //Проверка валидности текста, задающего алфавит
                if (text == null || !text.Contains(" ")) { throw new Exception("Неправильная запись алфавита. Каждый символ должна сопровождаться вероятностью её появления, между символами и вероятностями должны стоять пробелы"); }
                
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
                //Проверка валидности текста, задающего алфавит
                if (text == null || !text.Contains(" ")) { throw new Exception("Неправильная запись алфавита. Между символами должны быть пробелы, алфавит должен состоять хотя бы из двух элементов"); }

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

                r = 1;
            }
            else if (mode == 2)
            {
                N = 32;
                k = 5;
                n = 9;
                r = n - k;

                names = new string[] { "А", "Б", "В", "Г", "Д", "Е", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" };
                code_words = new string[] { "00000", "00001", "00010", "00011", "00100", "00101", "00110", "00111", "01000", "01001", "01010", "01011", "01100", "01101", "01110", "01111", "10000", "10001", "10010", "10011", "10100", "10101", "10110", "10111", "11000", "11001", "11010", "11011", "11100", "11101", "11110", "11111" };
                H = new int[] { 1, 0, 0, 0, 0, 1, 0, 0, 0,
                                0, 1, 0, 1, 1, 0, 1, 0, 0,
                                0, 1, 1, 1, 0, 0, 0, 1, 0,
                                1, 0, 1, 1, 1, 0, 0, 0, 1 };
                G = new int[] { 1, 0, 0, 0, 0, 1, 0, 0, 1,
                                0, 1, 0, 0, 0, 0, 1, 1, 0,
                                0, 0, 1, 0, 0, 0, 0, 1, 1,
                                0, 0, 0, 1, 0, 0, 1, 1, 1,
                                0, 0, 0, 0, 1, 0, 1, 0, 1 };

                string code_word;
                int t;
                for (int i = 0; i < N; i++)
                {
                    code_word = "";
                    for (int j = 0; j < n; j++)
                    {
                        t = 0;
                        for (int K = 0; K < k; K++)
                        {
                            t = (t + ((code_words[i][K] - '0') * G[K * n + j])) % 2;
                        }
                        code_word += t;
                    }
                    code_words[i] = code_word;
                }
            }

            if (mode < 2)
            {
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
                k = 0;
                KraftInequality = 0;
                temp = 0;
                //Получение средней длины и суммы в правой части неравенства Крафта
                for (int i = 0; i < N; i++)
                {
                    temp += code_words[i].Length * probabilities[i];
                    KraftInequality += Math.Pow(2, -code_words[i].Length);
                    if (code_words[i].Length > k) { k = code_words[i].Length; }
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


                n = k + r;
            }
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

        public string EncodeHammingMessage(string input)
        {
            //Кодирование с использованием кодов Хэмминга
            string text, output = "";
            bool isElementConsists;

            text = input;

            //Каждый символ нужно найти в алфавите
            foreach(char letter in text)
            {
                isElementConsists = false;
                for (int i = 0; i < N; i++)
                {
                    if (Convert.ToString(letter) == names[i])
                    {
                        //Если такой символ есть, записываем соответствующее кодовое слово
                        isElementConsists = true;
                        output += code_words[i];
                        break;
                    }
                }
                //Если такого символа нет, сообщаем об ошибке
                if (!isElementConsists) { throw new Exception("Элемента " + letter + " не существует в алфавите."); }
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
                if (code_word.Length == k) { throw new Exception("Текст невозможно декодировать, проверьте правильность ввода и повторите попытку."); }
            }
            //Если что-то осталось после попытки декодирования, значит сообщение для декодирования записано неверно
            if (code_word != "") { throw new Exception("Текст невозможно декодировать, проверьте правильность ввода и повторите попытку."); }

            return output;
        }

        public string[] DecodeHammingMessage(string input)
        {
            //Декодирование с использованием кодов Хэмминга
            string[] output = new string[2];
            string decode = "", mistakes = "", code_word, str_syndrome;
            int numberOfCodeWords, temp;
            int[] syndrome = new int[r];
            bool isConsistMistakes, isDecodable;
            string[] keysWords;
            int[] values;
            keysWords = new string[Convert.ToInt32(Math.Pow(2, r))];
            values = new int[] { -1, 8, 7, 2, 6, 4, 1, 3, 5, 0, -2, -2, -2, -2, -2, -2 };


            //Проверяем, есть ли лишние символы в последовательности
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] - '0' != 0 && input[i] - '0' != 1) { throw new Exception("Сообщение для декодирования содержит какие-то другие символы кроме 0 и 1."); }
            }

            //Проверяем, делится ли последовательность на длину кодового слова
            if (input.Length % n != 0) { throw new Exception("Текст невозможно декодировать, проверьте правильность ввода и повторите попытку."); }
            numberOfCodeWords = input.Length / n;

            for (int i = 0; i < numberOfCodeWords; i++)
            {
                //Берем i-е кодовое слово
                code_word = "";
                for (int j = i * n; j < i * n + n; j++)
                {
                    code_word += input[j];
                }

                //Вычисляем синдромы для него
                for (int R = 0; R < r; R++)
                {
                    temp = 0;
                    for (int j = 0; j < n; j++)
                    {
                        temp = (temp + ((code_word[j] - '0') * H[R * n + j])) % 2;
                    }
                    syndrome[R] = temp;
                }

                //Проверяем, есть ли ошибки
                isConsistMistakes = false;
                str_syndrome = "";
                for (int s = 0; s < r; s++)
                {
                    str_syndrome += syndrome[s];
                    if (syndrome[s] == 1)
                    {
                        isConsistMistakes = true;
                    }
                }
                //Если да то пытаемся либо исправить ошибку, либо просто сообщить об ошибке
                if (isConsistMistakes)
                {
                    for (int j = 0; j < Convert.ToInt32(Math.Pow(2, r)); j++)
                    {
                        keysWords[j] = Convert.ToString(j, 2);
                        while (keysWords[j].Length < r)
                        {
                            keysWords[j] = keysWords[j].Insert(0, "0");
                        }
                    }

                    mistakes += (i + 1) + ": mistakes in word " + code_word + ", syndrome = ";
                    for (int j = 0; j < r; j++)
                    {
                        mistakes += syndrome[j];
                    }
                    mistakes += ", number of mistakes ";

                    for (int j = 0; j < Convert.ToInt32(Math.Pow(2, r)); j++)
                    {
                        if (str_syndrome == keysWords[j])
                        {
                            if (values[j] >= 0)
                            {
                                mistakes += "1, on position " + (values[j] + 1) + ".\n";
                                if (code_word[values[j]] == '0') { code_word = code_word.Remove(values[j], 1).Insert(values[j], "1"); }
                                else { code_word = code_word.Remove(values[j], 1).Insert(values[j], "0"); }
                            }
                            else if (values[j] == -2)
                            {
                                mistakes += "2.\n";
                            }
                            break;
                        }
                    }


                }
                //Если удалось исправить ошибку либо ее вовсе не было, то запишем соответствующую букву, иначе сообщим об ошибке
                isDecodable = false;
                for (int j = 0; j < N; j++)
                {
                    if (code_words[j] == code_word)
                    {
                        decode += names[j];
                        isDecodable = true;
                        break;
                    }
                }
                if(!isDecodable) { decode += "{" + (i + 1) + "}"; }
            }

            output[0] = decode;
            output[1] = mistakes;

            return output;
        }

        public void AddEvenBit()
        {
            //Добавление бита четности
            int temp;

            for (int i = 0; i < N; i++)
            {
                //Производим операцию сумма по модулю два для каждого кодового слова и добавляем нужный бит четности
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
            //Декодирование сообщения с битом четности
            string[] output;
            string decode = "", mistakes = "", code_word;
            int numberOfCodeWords, bit;
            bool isDecodable;

            //Проверяем, есть ли лишние символы в последовательности
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] - '0' != 0 && input[i] - '0' != 1) { throw new Exception("Сообщение для декодирования содержит какие-то другие символы кроме 0 и 1."); }
            }

            //Проверяем, делится ли последовательность на длину кодового слова
            if (input.Length % (k + 1) != 0) { throw new Exception("Текст невозможно декодировать, проверьте правильность ввода и повторите попытку."); }
            numberOfCodeWords = input.Length / (k + 1);

            for (int i = 0; i < numberOfCodeWords; i++)
            {
                //Берем i-e кодовое слово
                code_word = "";
                for (int j = i * (k + 1); j < i * (k + 1) + k + 1; j++)
                {
                    code_word += input[j];
                }

                //Вычисляем бит четности
                bit = 0;
                foreach (char letter in code_word)
                {
                    bit += letter - '0';
                    bit %= 2;
                }

                //Если кодовое слово нечетное значит есть ошибка
                if (bit != 0)
                {
                    mistakes += Convert.ToString(i + 1) + ": " + code_word + ".\n";
                    decode += "{" + (i + 1) + "}";
                }
                //Иначе ошибки нет, добавляем соответствующий символ в последовательность
                else
                {
                    isDecodable = false;
                    for (int j = 0; j < N; j++)
                    {
                        if (code_words[j] == code_word)
                        {
                            decode += names[j];
                            isDecodable = true;
                            break;
                        }
                    }
                    if (!isDecodable) { decode += "{" + (i + 1) + "}"; }
                }
            }

            output = new string[2];
            output[0] = decode;
            output[1] = mistakes;
            return output;
        }

        public void GetNewCharacteristics()
        {
            //Вычисление новых характеристик
            int temp;

            //Кодовое расстояние кода
            codeDistance = -1;
            temp = -1;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N && i != j; j++)
                {
                    temp = 0;
                    for (int K = 0; K < n; K++)
                    {
                        if (code_words[i][K] != code_words[j][K]) { temp += 1; }
                    }
                }
                if ((codeDistance == -1 || codeDistance > temp) && temp != -1) { codeDistance = temp; }
            }

            //граница Хэмминга
            HammingBound = 0;
            for (int i = 0; i <= Math.Floor((codeDistance - 1) / 2.0); i++)
            {
                HammingBound += Functions.Factorial(n) / Functions.Factorial(i) / Functions.Factorial(n - i);
            }
            HammingBound = Math.Log(HammingBound) / Math.Log(2);

            //граница Плоткина
            PlotkinBound = n * (Math.Pow(2, k - 1)) / (Math.Pow(2, k) - 1);

            //граница Варшамова-Гильберта
            Gilbert_VarshamovBound = 0;
            for (int i = 0; i <= codeDistance - 2; i++) { Gilbert_VarshamovBound += Functions.Factorial(n - 1) / Functions.Factorial(i) / Functions.Factorial(n -  1 - i); }
        }



    }

    public class VigenereCipherClass
    {
        public int T { get; set; }
        public int[] NumTkey;
        public int[] NumInput;
        public string Output { get; set; }
        public string Alphabet { get; }
        public int[] NumAlphabet { get; }

        public VigenereCipherClass()
        {
            Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ_";
            NumAlphabet = new int[Alphabet.Length];
            for (int i = 0; i < Alphabet.Length; i++) { NumAlphabet[i] = i + 1; }
        }

        public void GetCharacteristics(string Tkey0, string Input0)
        {
            bool isExists;
            //Проверка размера ключа
            if (Tkey0.Length == 0) { throw new Exception("Размер ключа должен быть больше 0"); }

            //Проверка букв ключа
            Array.Resize(ref NumTkey, Tkey0.Length);
            for (int i = 0; i < Tkey0.Length; i++)
            {
                isExists = false;
                for (int j = 0; j < Alphabet.Length; j++)
                {
                    if (Tkey0[i] == Alphabet[j])
                    {
                        isExists = true;
                        NumTkey[i] = NumAlphabet[j];
                        break;
                    }
                }
                if (!isExists) { throw new Exception("Ключ состоит из букв, не существующих в алфавите"); }
            }

            //Проверка букв входной последовательности
            Array.Resize(ref NumInput, Input0.Length);
            for (int i = 0; i < Input0.Length; i++)
            {
                isExists = false;
                for (int j = 0; j < Alphabet.Length; j++)
                {
                    if (Input0[i] == Alphabet[j])
                    {
                        isExists = true;
                        NumInput[i] = NumAlphabet[j];
                        break;
                    }
                }
                if (!isExists) { throw new Exception("Входная последовательность состоит из букв, не существующих в алфавите"); }
            }
            //Получение размера ключа
            T = Tkey0.Length;
        }

        public void EncodeDecodeMessage(bool encode)
        {
            int[] keyInput = new int[NumInput.Length];
            int counter = 0, keyCounter = 0;

            //Тиражирование ключа
            while (counter < keyInput.Length)
            {
                keyInput[counter] = NumTkey[keyCounter];
                keyCounter++;
                counter++;
                if (keyCounter == NumTkey.Length) { keyCounter = 0; }
            }

            Output = "";
            //Если кодируем
            if (encode)
            {
                for (int i = 0; i < NumInput.Length; i++)
                {
                    counter = (NumInput[i] + keyInput[i]) % (Alphabet.Length) - 1;
                    if (counter < 0) { counter += Alphabet.Length; }
                    Output += Alphabet[counter];
                }
            }
            //Если декодируем
            else
            {
                for (int i = 0; i < NumInput.Length; i++)
                {
                    counter = (NumInput[i] - keyInput[i] + Alphabet.Length) % (Alphabet.Length) - 1;
                    if (counter < 0) { counter += Alphabet.Length; }
                    Output += Alphabet[counter];
                }
            }
        }



    }

    public class GeneratingClass
    {
        public int N { get; }
        public int K { get; }
        public int L { get; set; }
        public int[] a { get; set; }
        public int[] c { get; set; }
        public int[] x0 { get; set; }
        public string Alphabet { get; }
        public string Subsequence { get; set; }
        public int[] NumbersSubsequence { get; set; }

        public GeneratingClass()
        {
            //Задание основных переменных
            Alphabet = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYy";
            N = 50;
            K = 20;
            a = new int[2];
            c = new int[2];
            x0 = new int[2];
        }

        public void SetParametersAndGenerateSubsequence(string parameters)
        {
            //Получение параметров из первого файла и их последовательная обработка
            string[] temp = parameters.Split(" ");
            int[] numbers;
            if (temp.Length != 7) { throw new Exception("В файле с параметрами должно быть 7 элементов: L, a1, a2, c1, c2, x01, x02."); }
            numbers = new int[5];

            //Проверяем, являются ли первые пять параметров числами
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    numbers[i] = Convert.ToInt32(temp[i]);
                    if (numbers[i] <= 0) { throw new Exception("Параметры L, a, c должны быть положительными."); }
                }
                catch { throw new Exception("Значение \"" + temp[i] + "\" " + (i + 1) + "-го параметра не является числом, однако первые пять параметров должны быть числовые."); }
            }

            L = numbers[0];
            NumbersSubsequence = new int[L];

            a[0] = numbers[1]; a[1] = numbers[2];
            c[0] = numbers[3]; c[1] = numbers[4];

            //Пытаемся найти символы из файла в алфавите, если их там нет, выдаем ошибку
            x0[0] = Alphabet.IndexOf(temp[5]);
            x0[1] = Alphabet.IndexOf(temp[6]);
            if (x0[0] == -1 || x0[1] == -1) { throw new Exception("Элементы x01 и x02 должны являться элементами алфавита."); }
            if (x0[1] >= K) { throw new Exception("Элемент x02 должен входить в алфавит, состоящий из первых K букв начального алфавита."); }

            //запускаем генерацию последовательности
            GenerateSubsequence();
        }

        private void GenerateSubsequence()
        {
            //генерация последовательности
            int[] firstSubsequence, secondSubsequence;
            int[] table = new int[K];

            //находим две псевдослучайные последовательности с помощью ЛКГ
            firstSubsequence = GenerateLCG(L + K, N, 0);
            secondSubsequence = GenerateLCG(L, K, 1);

            //заполняем таблицу первыми К элементами первой последовательности
            for (int k = 0; k < K; k++) { table[k] = firstSubsequence[k]; }

            //выполняем основную логику
            Subsequence = "";
            for (int k = 0; k < L; k++)
            {
                Subsequence += Alphabet[table[secondSubsequence[k]]];
                NumbersSubsequence[k] = table[secondSubsequence[k]];
                table[secondSubsequence[k]] = firstSubsequence[K + k];
            }

        }

        private int[] GenerateLCG(int length, int maxLetter, int number)
        {
            //генерация последовательности с помощью ЛКГ
            int[] subsequence = new int[length];

            //Используем рекуррентное соотношение
            subsequence[0] = x0[number];
            for (int i = 1; i < length; i++) { subsequence[i] = (a[number] * subsequence[i - 1] + c[number]) % maxLetter; }

            return subsequence;
        }

        public double CheckHypothesis()
        {
            //проверка гипотезы
            double S = 0, temp;
            int k = 10;

            //считаем количество каждой буквы в последоватеьности
            int[] countLetters = new int[N];
            for (int i = 0; i < L; i++) { countLetters[NumbersSubsequence[i]] += 1; }

            //Находим оценку по формуле
            for (int i = 0; i < k; i++)
            {
                temp = 0;
                for (int j = 0; j < N / k; j++) { temp += countLetters[i * N / k + j]; }
                S += Math.Pow(temp / L - 1.0 / k, 2) * k;
            }
            S *= L;

            return S;
        }

        public int FindPeriod()
        {
            //поиск периода
            int T = 1;
            bool isEqual;

            //сначала период равен 1, в цикле мы его последовательно увеличиваем
            while (T < Math.Min(L / 2, N))
            {
                isEqual = true;
                for (int i = L / 2 - T; i < L / 2; i++)
                {
                    //Если хоть в одном элементе последовательность не совадает, то последовательности не равны
                    if (NumbersSubsequence[i] != NumbersSubsequence[i + T])
                    {
                        isEqual = false;
                        break;
                    }
                }
                //Если все элементы были равны, возвращаем период
                if (isEqual) { return T; }
                T += 1;
            }

            return T;
        }

    }


    public class GeneratingPrimeNumbersClass
    {
        public int PrimeNumber { get; set; }
        public List<int> AllNumbers { get; set; }
        private int BitDepth { get; set; }
        private int k { get; set; }
        public int Iterations { get; set; }

        public GeneratingPrimeNumbersClass()
        {
            AllNumbers = new List<int>();
        }

        public void SetCharacteristicsAndGenerate(string input)
        {
            //основная функция, обрабатывающая ввод Пользвателя и вызывающая генерацию чисел и тесты
            AllNumbers.Clear();
            Iterations = 0;
            PrimeNumber = -1;

            string[] characteristics = input.Split(" ");
            int[] numbers = new int[2];
            int temp = -2;
            bool isPrime = false;

            if (characteristics.Length != 2) { throw new Exception("Во входном файле должно быть два параметра: разрядность числа и количество повторений теста"); }

            //Проверка чисел на натуральность
            for (int i = 0; i < characteristics.Length; i++)
            {
                try
                {
                    numbers[i] = Convert.ToInt32(characteristics[i]);
                    if (numbers[i] <= 0) { throw new Exception("Оба параметра должны быть натуральными числами."); }
                }
                catch (FormatException e) { throw new Exception("Значение " + characteristics[i] + " не является числовым."); }
            }

            //задание основных параметров
            BitDepth = numbers[0];
            k = numbers[1];

            //запуск основного цикла
            while (!isPrime)
            {
                //генерация числа, его запись в список, проверка на простоту
                temp = GenerateNumber();
                AllNumbers.Add(temp);
                isPrime = CheckPrimeNumber(temp);
            }
            PrimeNumber = temp;
            Iterations = AllNumbers.Count;
        }

        private int GenerateNumber()
        {
            //генерация чисел
            Random rand = new Random();
            int number = rand.Next(Convert.ToInt32(Math.Pow(10, BitDepth - 1)), Convert.ToInt32(Math.Pow(10, BitDepth)));
            //получение нечетного числа
            return (number % 2 == 1) ? number : number + 1;
        }

        private bool CheckPrimeNumber(int number)
        {
            //тест Рабина-Миллера
            Random rand = new Random();

            int s = 0, t, z;
            int a = number - 1;
            bool isEqual;

            //получение S и t - шаг 0
            while (a % 2 == 0)
            {
                a /= 2;
                s += 1;
            }
            t = (number - 1) / Convert.ToInt32(Math.Pow(2, s));

            //остальные шаги алгоритма
            for (int i = 0; i < k; i++)
            {
                a = rand.Next(2, number);
                if (GCD(a, number) != 1) { return false; }

                z = MOD(a, t, number);
                if (z == 1 || z == number - 1) { continue; }

                isEqual = false;
                for (int S = 1; S < s; S++)
                {
                    z = MOD(a, Convert.ToInt32(Math.Pow(2, S) * t), number);
                    if (z == number - 1)
                    {
                        isEqual = true;
                        break;
                    }
                }
                if (isEqual) { continue; }
                return false;
            }

            return true;
        }

        private int MOD(int a, int power, int n)
        {
            //Получение остатка для большого числа
            int c = 1;

            for (int p = 1; p < power + 1; p++) { c = (c * a) % n; }

            return c;
        }

        private int GCD(int n1, int n2)
        {
            //нахождение НОД по методу Евклида
            while(n1 != n2)
            {
                if (n1 > n2) { n1 -= n2; }
                else { n2 -= n1; }
            }
            return n1;
        }
    }


    public static class Functions
    {
        public static int Factorial(int n)
        {
            if (n == 0) { return 1; }
            return n * Factorial(n - 1);
        }

        public static string ConvertTo2(int number)
        {
            if (number == 1) { return "1"; }
            return ConvertTo2(number / 2) + (number % 2);
        }
    }
}
