using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using Windows.Web.Syndication;
using Windows.Media.Ocr;
using System.Linq;

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

		private void GoToLenstraMethod(object sender, RoutedEventArgs e)
		{
			frame.Content = new LenstraMethod();
		}

		private void GoToGamma(object sender, RoutedEventArgs e)
		{
			frame.Content = new Gamma();
		}

		private void GoToScrambler(object sender, RoutedEventArgs e)
		{
			frame.Content = new Scrambler();
		}
        
        private void GoToHash(object sender, RoutedEventArgs e)
        {
            frame.Content = new Hash();
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
		public int k { get; set; }
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
				catch (FormatException) { throw new Exception("Значение " + characteristics[i] + " не является числовым."); }
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

		public bool CheckPrimeNumber(int number)
		{
			if (number == 2 || number == 1)
				return true;

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
				if (Functions.GCD(a, number) != 1) { return false; }

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
	}

	public class LenstraMethod_Class
	{
		private GeneratingPrimeNumbersClass generatingPrimeNumbers;
		public List<int>[] dividers;
		public int num;
		public int allIterations;
		//public List<int> all_r;

		public LenstraMethod_Class()
		{
			generatingPrimeNumbers = new GeneratingPrimeNumbersClass();
			generatingPrimeNumbers.k = 10;

			dividers = new List<int>[2];
			dividers[0] = new List<int>();
			dividers[1] = new List<int>();

			allIterations = 0;

			//all_r = new List<int>();
		}

		public void CheckIfPrimeNumber(string input)
		{
			int number;

			if (input.Length == 0)
				throw new Exception("Вы не ввели значение n");

			try
			{
				number = Convert.ToInt32(input);
			}
			catch
			{
				throw new Exception("Значение " + input + " не является числовым");
			}

			if (number <= 3)
			{
				throw new Exception("Число должно быть больше 3");
			}
			else if (generatingPrimeNumbers.CheckPrimeNumber(number))
			{
				throw new Exception("Число " + input + " простое");
			}

			num = number;
		}

		public void FindDividers(int n)
		{
			List<int[]> numbers = new List<int[]>();
			List<int> c = new List<int>();
			int S, r_star, r_;
			int s = 0;
			int q;
			int iterations;
			int numberOfResults, c_temp, possibleValue;
			double[] x = new double[2], y = new double[2];
			double temp;
			bool isFindSomething;
			double eps = Math.Pow(10, -10);


			S = Convert.ToInt32(Math.Floor(Math.Pow(n, 1.0 / 3.0)) + 1);

			for (int i = n / 2 + 1; i < n; i++)
			{
				if (generatingPrimeNumbers.CheckPrimeNumber(i))
				{
					s = i;
					break;
				}
			}

			for (int r = s - 1; r >= 1; r--)
			{
				isFindSomething = false;
				iterations = 0;

				r_star = Functions.EuclideanAlgorithm(r, 1, s);
				while (r_star < 0)
					r_star += s;

				r_ = (r_star * n) % s;

				numbers.Add(new int[3] { s, 0, 0 });

				iterations++;
				numbers.Add(new int[3]);

				numbers[1][0] = (r_ * r_star) % s;
				if (numbers[1][0] == 0)
					numbers[1][0] = s;
				numbers[1][1] = 1;

				numbers[1][2] = ((n - r * r_) / s * r_star) % s;

				do
				{
					iterations++;
					numbers.Add(new int[3]);

					q = numbers[0][0] / numbers[1][0];

					numbers[2][0] = numbers[0][0] - q * numbers[1][0];
					numbers[2][1] = numbers[0][1] - q * numbers[1][1];
					numbers[2][2] = numbers[0][2] - q * numbers[1][2];
					while (numbers[2][2] < 0)
						numbers[2][2] += s;
					if (numbers[2][2] > 0)
						numbers[2][2] = numbers[2][2] % s;

					if (iterations % 2 == 0)
					{
						c.Add(numbers[2][2] % s);
						c.Add(numbers[2][2] % s);
						if (numbers[2][2] > 0)
						{
							while (c[1] > 0)
								c[1] -= s;
						}
						else
						{
							while (c[1] < 0)
								c[1] += s;
						}
					}
					else
					{
						for (int i = 2 * numbers[2][0] * numbers[2][1]; i <= n / (s * s) + numbers[2][0] * numbers[2][1]; i++)
						{
							c_temp = i;
							if (c_temp > 0)
							{
								while (c_temp >= s)
									c_temp -= s;
							}
							else
							{
								while (c_temp < 0)
									c_temp += s;
							}
							if (c_temp == numbers[2][2])
								c.Add(i);
							else if (c_temp - s == numbers[2][2])
								c.Add(i);
						}
					}

					numbers.RemoveAt(0);

					for (int i = 0; i < c.Count; i++)
					{
						if (numbers[1][0] == 0)
						{
							numberOfResults = 1;
							y[0] = Convert.ToDouble(c[i]) / numbers[1][1];
							x[0] = Convert.ToDouble(n - r * y[0] * s - r * r_) / (s * s * y[0] + s * r_);
						}
						else
						{
							numberOfResults = 2;

							y[0] = -(s * s * c[i] - s * r_ * numbers[1][1] + s * r * numbers[1][0]);
							temp = Math.Sqrt(Math.Pow(s * s * c[i] - s * r_ * numbers[1][1] + s * r * numbers[1][0], 2) + 4 * (s * s * numbers[1][1]) * (s * c[i] * r_ + r * r_ * numbers[1][0] - n * numbers[1][0]));
							y[1] = (y[0] - temp) / (-2 * numbers[1][1] * s * s);
							y[0] = (y[0] + temp) / (-2 * numbers[1][1] * s * s);

							x[0] = (c[i] - y[0] * numbers[1][1]) / numbers[1][0];
							x[1] = (c[i] - y[1] * numbers[1][1]) / numbers[1][0];
						}

						for (int j = 0; j < numberOfResults; j++)
						{
							if (x[j] >= 0 && y[j] >= 0 && Math.Abs(x[j] - Math.Round(x[j])) < eps && Math.Abs(y[j] - Math.Round(y[j])) < eps)
							{
								possibleValue = Convert.ToInt32(Math.Floor(x[j]) * s + r);
								if (possibleValue != 1 && generatingPrimeNumbers.CheckPrimeNumber(possibleValue) && possibleValue < n)
								{
									c_temp = dividers[0].IndexOf(possibleValue);
									if (c_temp == -1)
									{
										isFindSomething = true;
										//all_r.Add(r);
										dividers[0].Add(possibleValue);
										dividers[1].Add(1);
									}
									else
									{
										if (n % Convert.ToInt32(Math.Pow(dividers[0][c_temp], dividers[1][c_temp] + 1)) == 0)
										{
											isFindSomething = true;
											//all_r.Add(r);
											dividers[1][c_temp] += 1;
										}
									}

									possibleValue = 1;
									for (int k = 0; k < dividers[0].Count; k++)
										possibleValue *= Convert.ToInt32(Math.Pow(dividers[0][k], dividers[1][k]));
									if (possibleValue == n)
									{
										allIterations += iterations;
										return;
									}
								}
							}
						}
					}
					c.Clear();
				}
				while (numbers[1][0] != 0);

				numbers.Clear();

				if (isFindSomething)
					allIterations += iterations;
			}
		}
	}


	public enum KeyMode { Key2, Key16, KeySymbol };


	public class GammaClass
	{
		public static string alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя .,!@#$%^&*()-_=+?/\\|[]{}\"':;`~<>¶☺☻♥♦♣♠•◘○◙♂♀♪♫☼►◄↕‼¶§▬↨↑↓→←∟↔▲▼░▒▓█ЄєЇїЎў°∙√¤■©½჻ÅÐØΔΘΛΞΣΦΨΩαβγδεζηθιλμξπρςστψωƢƔǢϾϠѨ҂҉֎֏ʬჱ⁞⃝꙰";
		private string keyIn2;
		private string messageIn2;

		public GammaClass(string message, string key, KeyMode keyMode)
		{
			messageIn2 = "";
			keyIn2 = "";
			CheckMessage(message);
			CheckKey(key, message.Length, keyMode);
		}

		private void CheckKey(string inputKey, int messageLength, KeyMode keyMode)
		{
			string symbol;
			int pos;

			if (keyMode == KeyMode.KeySymbol)
			{
				if (inputKey.Length != messageLength)
					throw new Exception("Длина ключа в символьном представлении должна быть равна длине входного сообщения.");

				for (int i = 0; i < inputKey.Length; i++)
				{
					pos = alphabet.IndexOf(inputKey[i]);
					if (pos == -1)
						throw new Exception("В символьном ключе присутствует символ " + inputKey[i] + ", который отсутствует в алфавите.");

					symbol = Functions.ConvertTo2(pos);
					while (symbol.Length < 8)
						symbol = "0" + symbol;

					keyIn2 += symbol;
				}
			}

			else if (keyMode == KeyMode.Key2)
			{
				if (inputKey.Length / 8 != messageLength)
					throw new Exception("Длина ключа в двоичном представлении должна быть в 8 раз больше длины входного сообщения.");

				for (int i = 0; i < inputKey.Length; i++)
				{
					pos = alphabet.IndexOf(inputKey[i], 0, 2);
					if (pos == -1)
						throw new Exception("В двоичном ключе присутствует символ " + inputKey[i] + ", который отсутствует в двоичной системе счисления.");
				}

				keyIn2 = inputKey;
			}

			else if (keyMode == KeyMode.Key16)
			{
				if (inputKey.Length / 2 != messageLength)
					throw new Exception("Длина ключа в шестнадцатеричном представлении должна быть в 2 раза больше длины входного сообщения.");

				for (int i = 0; i < inputKey.Length; i++)
				{
					pos = alphabet.IndexOf(inputKey[i], 0, 16);
					if (pos == -1)
						throw new Exception("В шестнадцатеричном ключе присутствует символ " + inputKey[i] + ", который отсутствует в шестнадцатеричной системе счисления.");

					symbol = Functions.ConvertTo2(pos);
					while (symbol.Length < 4)
						symbol = "0" + symbol;

					keyIn2 += symbol;
				}
			}
		}

		private void CheckMessage(string message)
		{
			int pos;
			string symbolIn2;

			if (message.Length == 0)
				throw new Exception("Вы не ввели сообщение для шифрования/дешифрования");

			for (int i = 0; i < message.Length; i++)
			{
				pos = alphabet.IndexOf(message[i]);
				if (pos == -1)
					throw new Exception("Во входном сообщении присутствует символ " + message[i] + ", который отсутствует в алфавите.");

				symbolIn2 = Functions.ConvertTo2(pos);
				while (symbolIn2.Length < 8)
					symbolIn2 = "0" + symbolIn2;

				messageIn2 += symbolIn2;
			}
		}

		public string EncodeMessage()
		{
			string result = "", resultIn2 = "";

			for (int i = 0; i < messageIn2.Length; i++)
				resultIn2 += (Math.Abs((messageIn2[i] - '0') + (keyIn2[i] - '0')) % 2).ToString();

			for (int i = 0; i < resultIn2.Length / 8; i++)
				result += alphabet[Convert.ToInt32(Functions.ConvertTo10(resultIn2.Substring(i * 8, 8), 2, "01"))];

			return result;
		}

		public static string GenerateKey(int messageLength, KeyMode keyMode)
		{
			if (messageLength == 0)
				throw new Exception("Вы не ввели сообщение для шифрования/дешифрования");

			string key = "";
			Random random = new Random();

			if (keyMode == KeyMode.Key2)
			{
				for (int i = 0; i < messageLength * 8; i++)
					key += random.Next(2).ToString();
			}
			else if (keyMode == KeyMode.Key16)
			{
				for (int i = 0; i < messageLength * 2; i++)
					key += alphabet[random.Next(16)];
			}
			else if (keyMode == KeyMode.KeySymbol)
			{
				for (int i = 0; i < messageLength; i++)
					key += alphabet[random.Next(alphabet.Length)];
			}

			return key;
		}
	}


	public class ScramblerClass
	{
		private string memoryCells;
		private string symbolMessage;

		private int[] scramblerCoefficients;

		private KeyMode keyMode;

		public ScramblerClass(string message, string input, KeyMode km)
		{
			keyMode = km;
			CheckInput(input);
			CheckMessage(message);
		}

		private void CheckInput(string input)
		{
			string[] parameters;

			if (input.Length == 0)
				throw new Exception("Вы не ввели коэффициенты и начальные значения скремблера в файл.");

			if (input.IndexOf("\n") != -1)
				throw new Exception("В файле с коэффициентами и начальными значениями скремблера не должно быть переносов на новую строку.");

			//1 0 0 1 1 10110
			parameters = input.Split(' ');
			if (parameters.Length < 2)
				throw new Exception("В файле с коэффициентами и начальными значениями скремблера должен быть хотя бы один коэффициент и одно значение скремблера.");

			memoryCells = parameters[parameters.Length - 1];

			try
			{
				scramblerCoefficients = new int[parameters.Length - 1];
				for (int i = 0; i < parameters.Length - 1; i++)
					scramblerCoefficients[i] = Convert.ToInt32(parameters[i]);
			}
			catch
			{
				throw new Exception("В файле с коэффициентами и начальными значениями скремблера находится недопустимый коэффициент.");
			}

			CheckCoefficients();
			CheckMemoryCells();
		}

		private void CheckCoefficients()
		{
			for (int i = 0; i < scramblerCoefficients.Length; i++)
			{
				if (scramblerCoefficients[i] != 0 && scramblerCoefficients[i] != 1)
					throw new Exception("Недопустимый коэффициент скремблера: " + scramblerCoefficients[i] + ", коэффициенты скремблера должны принимать значения 0 либо 1.");
			}
		}

		private void CheckMemoryCells()
		{
			int number;

			if (keyMode == KeyMode.Key2)
			{
				if (memoryCells.Length != scramblerCoefficients.Length)
					throw new Exception("Количество коэффициентов скремблера должно быть равно размерности начального значения скремблера в двоичном представлении.");

				for (int i = 0; i < memoryCells.Length; i++)
				{
					if (GammaClass.alphabet.IndexOf(memoryCells[i], 0, 2) == -1)
						throw new Exception("Недопустимое начальное значение скремблера: " + memoryCells[i] + ", начальные значения скремблера в двоичном представлении должны принимать значения 0 либо 1.");
				}
			}
			else if (keyMode == KeyMode.Key16)
			{
				for (int i = 0; i < memoryCells.Length; i++)
				{
					number = GammaClass.alphabet.IndexOf(memoryCells[i], 0, 16);
					if (number == -1)
						throw new Exception("Недопустимое начальное значение скремблера: " + memoryCells[i] + ", начальные значения скремблера в шестнадцатеричном представлении должны принимать значения от 0 до F.");
				}

				number = Convert.ToInt32(Functions.ConvertTo10(memoryCells, 16, GammaClass.alphabet.Substring(0, 16)));
				if (number >= Math.Pow(2, scramblerCoefficients.Length))
					throw new Exception("Начальное значение скремблера, введенное в шестнадцатеричном представлении, должно быть меньше числа 2^<количество коэффициентов скремблера>.");

				memoryCells = Functions.ConvertTo2(number);
				while (memoryCells.Length < scramblerCoefficients.Length)
					memoryCells = "0" + memoryCells;
			}
			else if (keyMode == KeyMode.KeySymbol)
			{
				for (int i = 0; i < memoryCells.Length; i++)
				{
					number = GammaClass.alphabet.IndexOf(memoryCells[i]);
					if (number == -1)
						throw new Exception("Недопустимое начальное значение скремблера: " + memoryCells[i] + ", начальные значения скремблера в символьном представлении должны принимать значения от 0 до F.");
				}

				number = Convert.ToInt32(Functions.ConvertTo10(memoryCells, 256, GammaClass.alphabet));
				if (number >= Math.Pow(2, scramblerCoefficients.Length))
					throw new Exception("Начальное значение скремблера, введенное в символьном представлении, должно быть меньше числа 2^<количество коэффициентов скремблера>.");

				memoryCells = Functions.ConvertTo2(number);
				while (memoryCells.Length < scramblerCoefficients.Length)
					memoryCells = "0" + memoryCells;
			}
		}

		private void CheckMessage(string message)
		{
			int pos;

			if (message.Length == 0)
				throw new Exception("Вы не ввели сообщение для шифрования/дешифрования");

			for (int i = 0; i < message.Length; i++)
			{
				pos = GammaClass.alphabet.IndexOf(message[i]);
				if (pos == -1)
					throw new Exception("Во входном сообщении присутствует символ " + message[i] + ", который отсутствует в алфавите.");
			}

			symbolMessage = message;
		}

		public string GenerateSequence(int messageLength)
		{
			string result = "";
			int newByte;

			while (result.Length < messageLength)
			{
				newByte = 0;

				result += memoryCells[0];

				for (int i = 0; i < memoryCells.Length; i++)
					newByte = Math.Abs((newByte - '0') + (memoryCells[i] - '0') * (scramblerCoefficients[i] - '0')) % 2;

				memoryCells = memoryCells.Substring(1);
				memoryCells += newByte.ToString();
			}

			return result;
		}

		public string EncodeMessage(string sequence)
		{
			GammaClass gm = new GammaClass(symbolMessage, sequence, KeyMode.Key2);
			return gm.EncodeMessage();
		}

		public string GetProperties()
		{
			string result = "", period;
			double s;

			period = GenerateSequence(Convert.ToInt32(Math.Pow(2, scramblerCoefficients.Length)) * 2 + 2);
			period = FindPeriod(period);
			result += string.Format("Период последовательности скремблера: размер периода - {0}, период - {1}\n", period.Length, period);

			result += FindBalance(period);

			result += CheckCycle(period);

			result += FindCorrelation(period);

			s = CheckUniform(period);
			if (s == -1)
				result += "Критерий x^2: Последовательность неравномерная.";
			else
			{
				result += string.Format("Критерий x^2: {0} < {1}, ", s, 3.84);
				result += s < 3.84 ? "последовательность равномерная." : "Последовательность неравномерная.";
			}

			return result;
		}

		private string FindPeriod(string sequence)
		{
			string period = sequence;
			bool equal;
			List<string> parts = new List<string>();

			if (sequence.Length % 2 == 1)
				sequence = sequence.Substring(0, sequence.Length - 1);

			for (int i = 1; i <= sequence.Length / 2; i++)
			{
				equal = true;

				for (int j = 0; j < sequence.Length / i; j++)
					parts.Add(sequence.Substring(j * i, i));

				for (int j = 1; j < parts.Count; j++)
				{
					if (parts[j] != parts[j - 1])
					{
						equal = false;
						break;
					}
				}
				if (!equal)
				{
					parts.Clear();
					continue;
				}

				period = parts[0];
				break;
			}

			return period;
		}

		private string FindBalance(string period)
		{
			double[] count = new double[2];

			for (int i = 0; i < period.Length; i++)
				count[period[i] - '0']++;

			count[0] = count[0] / period.Length * 100;

			return string.Format("- Сбалансированность: количество 0 - {0}%, количество 1 - {1}%.\n", count[0], 100 - count[0]);
		}

		private string CheckCycle(string period)
		{
			string result;
			List<int> stripes = new List<int>();
			char currentSymbol;
			int counter;
			double summary = 0;

			currentSymbol = period[0];
			counter = 1;

			for (int i = 1; i < period.Length; i++)
			{
				if (currentSymbol != period[i])
				{
					while (stripes.Count < counter)
						stripes.Add(0);
					stripes[counter - 1]++;

					summary++;

					counter = 1;
					currentSymbol = period[i];
				}
				else
					counter++;
			}

			result = string.Format("- Цикличность: Всего полос - {0}.\n", summary);
			for (int i = 0; i < stripes.Count; i++)
			{
				result += string.Format("\tПолос размера {0}: {1} ({2}%);\n", i + 1, stripes[i], stripes[i] / summary * 100);
			}

			return result;
		}

		private string FindCorrelation(string period)
		{
			string firstHalf, secondHalf;
			double equals = 0;

			if (period.Length == 1)
				return "- Корреляция: количество совпадений - 100%, количество несовпадений - 0%.\n";

			if (period.Length % 2 == 1)
				period = period.Substring(1);

			firstHalf = period.Substring(0, period.Length / 2);
			secondHalf = period.Substring(period.Length / 2);

			for (int i = 0; i < firstHalf.Length; i++)
				if (firstHalf[i] == secondHalf[i])
					equals++;

			equals = equals / firstHalf.Length * 100;

			return string.Format("- Корреляция: количество совпадений - {0}%, количество несовпадений - {1}%.\n", equals, 100 - equals);
		}

		private double CheckUniform(string period)
		{
			double pi = 1.0 / 2.0;
			double[] k = new double[2];

			if (period.Length == 1)
				return -1;

			for (int i = 0; i < period.Length; i++)
				k[period[i] - '0']++;

			k[0] = Math.Pow(k[0] / period.Length - pi, 2) / pi;
			k[1] = Math.Pow(k[1] / period.Length - pi, 2) / pi;

			return period.Length * (k[0] + k[1]);
		}

	}


	public class MD5Class
	{
		private static string alphabet = "0123456789ABCDEF";
        private static int byteLength = 8;
        private static int wordLength = 32;
		private static int oneDigitLength;
        private static double mod = Math.Pow(2, 32);

		private string inputIn2;
		private string secondInputIn2;

		private int position;

		public MD5Class(string input, int position)
		{
			int pos;
			string oneDigit;

			secondInputIn2 = "";
			oneDigitLength = (int)Math.Log(alphabet.Length, 2);

			inputIn2 = "";
			for (int i = 0; i < input.Length; i++)
			{
				pos = alphabet.IndexOf(input[i]);
                if (pos == -1)
                    throw new Exception(string.Format("Во входной последовательности находится символ {0} (позиция {1}), которого нет в алфавите", input[i], i + 1));

				oneDigit = Functions.ConvertTo2(pos);
				while (oneDigit.Length < oneDigitLength)
					oneDigit = "0" + oneDigit;

				inputIn2 += oneDigit;
            }

			if (position != -1)
			{
				if (position > input.Length || position < 1)
					throw new Exception(string.Format("Позиции с номером {0} нет в начальном числе.", position));
				position -= 1;

				secondInputIn2 = "";
				secondInputIn2 += input.Substring(0, position);
				secondInputIn2 += alphabet[(alphabet.IndexOf(input[position]) + 1) % alphabet.Length].ToString();
				if (position + 1 < input.Length)
					secondInputIn2 += input.Substring(position + 1);

				input = secondInputIn2;
				secondInputIn2 = "";
                for (int i = 0; i < input.Length; i++)
                {
                    pos = alphabet.IndexOf(input[i]);

                    oneDigit = Functions.ConvertTo2(pos);
                    while (oneDigit.Length < oneDigitLength)
                        oneDigit = "0" + oneDigit;

                    secondInputIn2 += oneDigit;
                }

				this.position = position;
            }
			else
                this.position = -1;
        }

		public string CalculateHash()
		{
			string oneWord, oneWord2 = "", startLengthIn2, oneDigit, result, result2 = "", resultOfCheck = "";
			int numberOfWords, N;
			int startLength = inputIn2.Length;
			//char[] charArray;
			int[] differencies = new int[4];
			string[] buffer = new string[4], buffer2 = new string[4];
			string[] currentValueOfBuffer = new string[buffer.Length], currentValueOfBuffer2 = new string[buffer2.Length];
			string[] startValues = { "67452301", "EFCDAB89", "98BADCFE", "10325476" };
			double[] T = new double[64];

			result = inputIn2;

            //Шаг 1
            result += "1";

			while (result.Length % 512 != 448)
                result += "0";

			numberOfWords = result.Length / wordLength;
            for (int i = 0; i < numberOfWords; i++)
			{
				oneWord = result.Substring(i * wordLength, wordLength);

				for (int j = 0; j < oneWord.Length / byteLength; j++)
				{
					result = result.Remove(i * wordLength + j * byteLength, byteLength);
					result = result.Insert(i * wordLength + j * byteLength, oneWord.Substring(oneWord.Length - (j + 1) * byteLength, byteLength));
				}
			}

			if (position != -1)
			{
				result2 = secondInputIn2;
                result2 += "1";

                while (result2.Length % 512 != 448)
                    result2 += "0";

                numberOfWords = result2.Length / wordLength;
                for (int i = 0; i < numberOfWords; i++)
                {
                    oneWord = result2.Substring(i * wordLength, wordLength);

                    for (int j = 0; j < oneWord.Length / byteLength; j++)
                    {
                        result2 = result2.Remove(i * wordLength + j * byteLength, byteLength);
                        result2 = result2.Insert(i * wordLength + j * byteLength, oneWord.Substring(oneWord.Length - (j + 1) * byteLength, byteLength));
                    }
                }
            }
			
			//Шаг 2
			startLengthIn2 = Functions.ConvertTo2(startLength);
			while (startLengthIn2.Length < 2 * wordLength)
				startLengthIn2 = "0" + startLengthIn2;

            result += startLengthIn2.Substring(wordLength, wordLength);
            result += startLengthIn2.Substring(0, wordLength);

			if (position != -1)
			{
                result2 += startLengthIn2.Substring(wordLength, wordLength);
                result2 += startLengthIn2.Substring(0, wordLength);
            }

			N = result.Length / wordLength;

			//Шаг 3
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < startValues[i].Length; j++)
				{
					oneDigit = Functions.ConvertTo2(alphabet.IndexOf(startValues[i][j]));
					while (oneDigit.Length < oneDigitLength)
						oneDigit = "0" + oneDigit;

					buffer[i] += oneDigit;
				}
			}

			if (position != -1)
			{
				buffer2[0] = buffer[0];
				buffer2[1] = buffer[1];
                buffer2[2] = buffer[2];
                buffer2[3] = buffer[3];
            }

			for (int i = 0; i < T.Length; i++)
				T[i] = Math.Floor(mod * Math.Abs(Math.Sin(i + 1)));

			//Шаг 4. Основной цикл
			for (int i = 0; i < N / 16; i++)
			{
				oneWord = result.Substring(i * 16 * wordLength, 16 * wordLength);

                for (int j = 0; j < buffer.Length; j++)
					currentValueOfBuffer[j] = buffer[j];

                if (position != -1)
				{
                    oneWord2 = result2.Substring(i * N * wordLength, N * wordLength);
                    for (int j = 0; j < buffer2.Length; j++)
                        currentValueOfBuffer2[j] = buffer2[j];
                }

				//Раунд 1
				buffer = CalculateRound1(buffer, oneWord, T);

                if (position != -1)
				{
                    buffer2 = CalculateRound1(buffer2, oneWord2, T);
					differencies[0] = FindDifferences(buffer[0] + buffer[1] + buffer[2] + buffer[3], buffer2[0] + buffer2[1] + buffer2[2] + buffer2[3]);
                    resultOfCheck += string.Format("Количество изменившихся бит в {0} блоке в {1} раунде равно {2}\n", (i + 1), 1, differencies[0]);
                }


				//Раунд 2
				buffer = CalculateRound2(buffer, oneWord, T);

                if (position != -1)
				{
                    buffer2 = CalculateRound2(buffer2, oneWord2, T);
					differencies[1] = FindDifferences(buffer[0] + buffer[1] + buffer[2] + buffer[3], buffer2[0] + buffer2[1] + buffer2[2] + buffer2[3]);
                    resultOfCheck += string.Format("Количество изменившихся бит в {0} блоке в {1} раунде равно {2}\n", (i + 1), 2, differencies[1]);
                }


				//Раунд 3
				buffer = CalculateRound3(buffer, oneWord, T);

                if (position != -1)
				{
                    buffer2 = CalculateRound3(buffer2, oneWord2, T);
					differencies[2] = FindDifferences(buffer[0] + buffer[1] + buffer[2] + buffer[3], buffer2[0] + buffer2[1] + buffer2[2] + buffer2[3]);
                    resultOfCheck += string.Format("Количество изменившихся бит в {0} блоке в {1} раунде равно {2}\n", (i + 1), 3, differencies[2]);
                }


				//Раунд 4
				buffer = CalculateRound4(buffer, oneWord, T);

                if (position != -1)
				{
                    buffer2 = CalculateRound4(buffer2, oneWord2, T);
					differencies[3] = FindDifferences(buffer[0] + buffer[1] + buffer[2] + buffer[3], buffer2[0] + buffer2[1] + buffer2[2] + buffer2[3]);
                    resultOfCheck += string.Format("Количество изменившихся бит в {0} блоке в {1} раунде равно {2}\n", (i + 1), 4, differencies[3]);

					resultOfCheck += "Номера раундов:\n";
					for (int j = 0; j < 4; j++)
						resultOfCheck += string.Format("{0}\n", (j + 1));
                    resultOfCheck += "\nЧисло различий:\n";
                    for (int j = 0; j < 4; j++)
                        resultOfCheck += string.Format("{0}\n", differencies[j]);

                    return resultOfCheck;
				}


				//Конец раундов
				buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(currentValueOfBuffer[0], 2, "01")) % mod);
                while (buffer[0].Length < wordLength)
                    buffer[0] = "0" + buffer[0];

                buffer[1] = Functions.ConvertDoubleTo2(Functions.ConvertTo10(buffer[1], 2, "01") + (Functions.ConvertTo10(currentValueOfBuffer[1], 2, "01")) % mod);
                while (buffer[1].Length < wordLength)
                    buffer[1] = "0" + buffer[1];

                buffer[2] = Functions.ConvertDoubleTo2(Functions.ConvertTo10(buffer[2], 2, "01") + (Functions.ConvertTo10(currentValueOfBuffer[2], 2, "01")) % mod);
                while (buffer[2].Length < wordLength)
                    buffer[2] = "0" + buffer[2];

                buffer[3] = Functions.ConvertDoubleTo2(Functions.ConvertTo10(buffer[3], 2, "01") + (Functions.ConvertTo10(currentValueOfBuffer[3], 2, "01")) % mod);
                while (buffer[3].Length < wordLength)
                    buffer[3] = "0" + buffer[3];
            }

			result = "";
			for (int i = 0; i < buffer.Length; i++)
			{
				for (int j = 0; j < buffer[i].Length / byteLength; j++)
                    result += buffer[i].Substring(buffer[i].Length - (j + 1) * byteLength, byteLength);
			}

			oneWord = result;
			result = "";

			for (int i = 0; i < oneWord.Length / oneDigitLength; i++)
			{
                result += alphabet[(int)Functions.ConvertTo10(oneWord.Substring(i * oneDigitLength, oneDigitLength), 2, "01")];
                if ((i + 1) % byteLength == 0)
                    result += " ";
            }

			return result;
        }

		private string[] CalculateRound1(string[] buffer, string oneWord, double[] T)
		{
            //Строка 1
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(F(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(0 * wordLength, wordLength), 2, "01") + T[1 - 1]) % mod), 7)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(F(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(1 * wordLength, wordLength), 2, "01") + T[2 - 1]) % mod), 12)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(F(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(2 * wordLength, wordLength), 2, "01") + T[3 - 1]) % mod), 17)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(F(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(3 * wordLength, wordLength), 2, "01") + T[4 - 1]) % mod), 22)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            //Строка 2
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(F(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(4 * wordLength, wordLength), 2, "01") + T[5 - 1]) % mod), 7)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(F(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(5 * wordLength, wordLength), 2, "01") + T[6 - 1]) % mod), 12)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(F(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(6 * wordLength, wordLength), 2, "01") + T[7 - 1]) % mod), 17)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(F(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(7 * wordLength, wordLength), 2, "01") + T[8 - 1]) % mod), 22)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            //Строка 3
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(F(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(8 * wordLength, wordLength), 2, "01") + T[9 - 1]) % mod), 7)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(F(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(9 * wordLength, wordLength), 2, "01") + T[10 - 1]) % mod), 12)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(F(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(10 * wordLength, wordLength), 2, "01") + T[11 - 1]) % mod), 17)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(F(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(11 * wordLength, wordLength), 2, "01") + T[12 - 1]) % mod), 22)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            //Строка 4
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(F(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(12 * wordLength, wordLength), 2, "01") + T[13 - 1]) % mod), 7)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(F(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(13 * wordLength, wordLength), 2, "01") + T[14 - 1]) % mod), 12)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(F(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(14 * wordLength, wordLength), 2, "01") + T[15 - 1]) % mod), 17)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(F(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(15 * wordLength, wordLength), 2, "01") + T[16 - 1]) % mod), 22)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            return buffer;
        }

		private string[] CalculateRound2(string[] buffer, string oneWord, double[] T)
		{
            //Раунд 2
            //Строка 1
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(G(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(1 * wordLength, wordLength), 2, "01") + T[17 - 1]) % mod), 5)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(G(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(6 * wordLength, wordLength), 2, "01") + T[18 - 1]) % mod), 9)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(G(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(11 * wordLength, wordLength), 2, "01") + T[19 - 1]) % mod), 14)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(G(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(0 * wordLength, wordLength), 2, "01") + T[20 - 1]) % mod), 20)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            //Строка 2
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(G(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(5 * wordLength, wordLength), 2, "01") + T[21 - 1]) % mod), 5)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(G(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(10 * wordLength, wordLength), 2, "01") + T[22 - 1]) % mod), 9)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(G(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(15 * wordLength, wordLength), 2, "01") + T[23 - 1]) % mod), 14)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(G(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(4 * wordLength, wordLength), 2, "01") + T[24 - 1]) % mod), 20)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            //Строка 3
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(G(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(9 * wordLength, wordLength), 2, "01") + T[25 - 1]) % mod), 5)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(G(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(14 * wordLength, wordLength), 2, "01") + T[26 - 1]) % mod), 9)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(G(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(3 * wordLength, wordLength), 2, "01") + T[27 - 1]) % mod), 14)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(G(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(8 * wordLength, wordLength), 2, "01") + T[28 - 1]) % mod), 20)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            //Строка 4
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(G(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(13 * wordLength, wordLength), 2, "01") + T[29 - 1]) % mod), 5)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(G(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(2 * wordLength, wordLength), 2, "01") + T[30 - 1]) % mod), 9)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(G(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(7 * wordLength, wordLength), 2, "01") + T[31 - 1]) % mod), 14)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(G(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(12 * wordLength, wordLength), 2, "01") + T[32 - 1]) % mod), 20)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            return buffer;
        }

		private string[] CalculateRound3(string[] buffer, string oneWord, double[] T)
		{
            //Строка 1
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(H(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(5 * wordLength, wordLength), 2, "01") + T[33 - 1]) % mod), 4)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(H(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(8 * wordLength, wordLength), 2, "01") + T[34 - 1]) % mod), 11)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(H(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(11 * wordLength, wordLength), 2, "01") + T[35 - 1]) % mod), 16)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(H(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(14 * wordLength, wordLength), 2, "01") + T[36 - 1]) % mod), 23)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            //Строка 2
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(H(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(1 * wordLength, wordLength), 2, "01") + T[37 - 1]) % mod), 4)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(H(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(4 * wordLength, wordLength), 2, "01") + T[38 - 1]) % mod), 11)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(H(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(7 * wordLength, wordLength), 2, "01") + T[39 - 1]) % mod), 16)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(H(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(10 * wordLength, wordLength), 2, "01") + T[40 - 1]) % mod), 23)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            //Строка 3
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(H(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(13 * wordLength, wordLength), 2, "01") + T[41 - 1]) % mod), 4)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(H(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(0 * wordLength, wordLength), 2, "01") + T[42 - 1]) % mod), 11)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(H(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(3 * wordLength, wordLength), 2, "01") + T[43 - 1]) % mod), 16)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(H(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(6 * wordLength, wordLength), 2, "01") + T[44 - 1]) % mod), 23)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            //Строка 4
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(H(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(9 * wordLength, wordLength), 2, "01") + T[45 - 1]) % mod), 4)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(H(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(12 * wordLength, wordLength), 2, "01") + T[46 - 1]) % mod), 11)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(H(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(15 * wordLength, wordLength), 2, "01") + T[47 - 1]) % mod), 16)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(H(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(2 * wordLength, wordLength), 2, "01") + T[48 - 1]) % mod), 23)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            return buffer;
        }

		private string[] CalculateRound4(string[] buffer, string oneWord, double[] T)
		{
            //Строка 1
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(I(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(0 * wordLength, wordLength), 2, "01") + T[49 - 1]) % mod), 6)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(I(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(7 * wordLength, wordLength), 2, "01") + T[50 - 1]) % mod), 10)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(I(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(14 * wordLength, wordLength), 2, "01") + T[51 - 1]) % mod), 15)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(I(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(5 * wordLength, wordLength), 2, "01") + T[52 - 1]) % mod), 21)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            //Строка 2
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(I(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(12 * wordLength, wordLength), 2, "01") + T[53 - 1]) % mod), 6)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(I(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(3 * wordLength, wordLength), 2, "01") + T[54 - 1]) % mod), 10)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(I(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(10 * wordLength, wordLength), 2, "01") + T[55 - 1]) % mod), 15)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(I(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(1 * wordLength, wordLength), 2, "01") + T[56 - 1]) % mod), 21)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            //Строка 3
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(I(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(8 * wordLength, wordLength), 2, "01") + T[57 - 1]) % mod), 6)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(I(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(15 * wordLength, wordLength), 2, "01") + T[58 - 1]) % mod), 10)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(I(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(6 * wordLength, wordLength), 2, "01") + T[59 - 1]) % mod), 15)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(I(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(13 * wordLength, wordLength), 2, "01") + T[60 - 1]) % mod), 21)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            //Строка 4
            buffer[0] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[1], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[0], 2, "01") + Functions.ConvertTo10(I(buffer[1], buffer[2], buffer[3]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(4 * wordLength, wordLength), 2, "01") + T[61 - 1]) % mod), 6)) % mod) % mod);
            while (buffer[0].Length < wordLength)
                buffer[0] = "0" + buffer[0];

            buffer[3] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[0], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[3], 2, "01") + Functions.ConvertTo10(I(buffer[0], buffer[1], buffer[2]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(11 * wordLength, wordLength), 2, "01") + T[62 - 1]) % mod), 10)) % mod) % mod);
            while (buffer[3].Length < wordLength)
                buffer[3] = "0" + buffer[3];

            buffer[2] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[3], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[2], 2, "01") + Functions.ConvertTo10(I(buffer[3], buffer[0], buffer[1]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(2 * wordLength, wordLength), 2, "01") + T[63 - 1]) % mod), 15)) % mod) % mod);
            while (buffer[2].Length < wordLength)
                buffer[2] = "0" + buffer[2];

            buffer[1] = Functions.ConvertDoubleTo2((Functions.ConvertTo10(buffer[2], 2, "01") + Convert.ToDouble(CyclicShift2(Convert.ToUInt32((Functions.ConvertTo10(buffer[1], 2, "01") + Functions.ConvertTo10(I(buffer[2], buffer[3], buffer[0]), 2, "01") + Functions.ConvertTo10(oneWord.Substring(9 * wordLength, wordLength), 2, "01") + T[64 - 1]) % mod), 21)) % mod) % mod);
            while (buffer[1].Length < wordLength)
                buffer[1] = "0" + buffer[1];

            return buffer;
        }

		private uint CyclicShift2(uint x, int s)
		{
			x &= 0xFFFFFFFF;
			return ((x << s) | (x >> (wordLength - s))) & 0xFFFFFFFF;
		}

        private string F(string x, string y, string z)
		{
			//(x ^ y) v (-x ^ z)
			return Functions.Disjunction(Functions.Conjunction(x, y), Functions.Conjunction(Functions.Negation(x), z));
		}

        private string G(string x, string y, string z)
        {
            //(x ^ z) v (-z ^ y)
            return Functions.Disjunction(Functions.Conjunction(x, z), Functions.Conjunction(Functions.Negation(z), y));
        }

        private string H(string x, string y, string z)
        {
            //x + y + z
            return Functions.XOR(Functions.XOR(x, y), z);
        }

        private string I(string x, string y, string z)
        {
            //y + (-z v x)
            return Functions.XOR(y, Functions.Disjunction(Functions.Negation(z), x));
        }

		private int FindDifferences(string x, string y)
		{
			int number = 0;

			if (x.Length != y.Length)
				throw new Exception("???");

			for (int i = 0; i < x.Length; i++)
				if (x[i] - '0' != y[i] - '0')
					number++;

			return number;
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
			if (number <= 1) { return number.ToString(); }
			return ConvertTo2(number / 2) + (number % 2);
		}

        public static string ConvertDoubleTo2(double number)
        {
            if (number <= 1) { return number.ToString(); }
			if (number % 2 == 1)
				return ConvertDoubleTo2((number - 1) / 2) + (number % 2);
			else
				return ConvertDoubleTo2(number / 2) + (number % 2);
        }

        public static double ConvertTo10(string value, int pi, string alphabet)
		{
            double result = 0;

			for (int i = 0; i < value.Length; i++)
				result += alphabet.IndexOf(value[i]) * Math.Pow(pi, value.Length - i - 1);

			return result;
		}

		public static int GCD(int n1, int n2)
		{
			//нахождение НОД по методу Евклида
			while (n1 != n2)
			{
				if (n1 > n2) { n1 -= n2; }
				else { n2 -= n1; }
			}
			return n1;
		}

		public static int EuclideanAlgorithm(int a, int b, int n)
		{
			int[]   y = new int[3] { 1, 0, n }, 
					z = new int[3] { 0, 1, a }, 
					t = new int[3] { 0, 0, 0 };
			int q;

			while (z[2] != 0)
			{
				q = (y[2] - y[2] % z[2]) / z[2];
				for (int i = 0; i < 3; i++) { t[i] = z[i]; }
				for (int i = 0; i < 3; i++) { z[i] = y[i] - q * z[i]; }
				for (int i = 0; i < 3; i++) { y[i] = t[i]; }
			}

			return (b * y[1]) % n;
		}

		public static string Disjunction(string x, string y)
		{
			string result = "";

			while (x.Length < y.Length)
				x = "0" + x;
			while (y.Length < x.Length)
				y = "0" + y;

			for (int i = 0; i < x.Length; i++)
				result += (x[i] - '0') + (y[i] - '0') >= 1 ? "1" : "0";

			return result;
		}

        public static string Conjunction(string x, string y)
        {
            string result = "";

            while (x.Length < y.Length)
                x = "0" + x;
            while (y.Length < x.Length)
                y = "0" + y;

            for (int i = 0; i < x.Length; i++)
                result += ((x[i] - '0') * (y[i] - '0')).ToString();

			return result;
        }

        public static string Negation(string x)
        {
            string result = "";

            for (int i = 0; i < x.Length; i++)
                result += (((x[i] - '0') + 1) % 2).ToString();

            return result;
        }

        public static string XOR(string x, string y)
        {
            string result = "";

            while (x.Length < y.Length)
                x = "0" + x;
            while (y.Length < x.Length)
                y = "0" + y;

            for (int i = 0; i < x.Length; i++)
                result += (((x[i] - '0') + (y[i] - '0')) % 2).ToString();

            return result;
        }
    }
}
