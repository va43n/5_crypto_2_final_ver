using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
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
	public sealed partial class Library : Page
	{
		private string input_file_name = "input.txt";
		private string output_file_name = "output.txt";
		private string output = "";

		private RSAParameters[] rsaParameters;
		private RSAParameters[] digitalSignatureParameters;

		public Library()
		{
			this.InitializeComponent();
			CheckFiles();

			Type1ComboBox.SelectedIndex = 0;
			Type2ComboBox.SelectedIndex = 0;
			Type3ComboBox.SelectedIndex = 0;
			Type4ComboBox.SelectedIndex = 0;

			ChooseComboBox.SelectedIndex = 0;

			rsaParameters = null;
			digitalSignatureParameters = null;
		}

		private async void CheckFiles()
		{
			string temp;

			//Получение доступа к нужной папке
			StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
			temp = storageFolder.Path;

			//Вывод путей к файлам на экран
			ParametersFileTextBox.Text = temp + @"\" + input_file_name;
			OutputFileTextBox.Text = temp + @"\" + output_file_name;

			//Если файлов не существует, создать их
			await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
			await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);
		}

		private async void Info1Button_Click(object sender, RoutedEventArgs e)
		{
			string text = "В программе используются два алгоритма сииметричного шифрования: 3DES и AES. Каждый из алгоритмов позволяет шифровать и дешифровать заданные входные данные.\n" +
				"1) Для шифрования алгоритмом 3DES необходимо во входной файл ввести данные следующим образом:\n" +
				"<Сообщение>\n" +
				"<Ключ><пробел><IV>\n" +
				"Сообщение задается набором символов кодировки UTF-8, ключ и IV также задаются символами кодировки UTF-8, причем ключ должен быть размером в 24 символа (24 байта), а IV должен быть размером в 8 символов (8 байт).\n" +
				"2) Для дешифрования алгоритмом 3DES необходимо во входной файл ввести данные следующим образом:\n" +
				"<Сообщение>\n" +
				"<Ключ><пробел><IV>\n" +
				"Сообщение задается набором байтов (набором чисел от 0 до 255), разделенных пробелами, ключ и IV задаются символами кодировки UTF-8, причем ключ должен быть размером в 24 символа (24 байта), а IV должен быть размером в 8 символов (8 байт).\n" +
				"3) Для шифрования алгоритмом AES необходимо во входной файл ввести данные следующим образом:\n" +
				"<Сообщение>\n" +
				"<Ключ><пробел><IV>\n" +
				"Сообщение задается набором символов кодировки UTF-8, ключ и IV также задаются символами кодировки UTF-8, причем ключ должен быть размером в 16 символов (16 байт), IV также должен быть размером в 16 символов (16 байт).\n" +
				"4) Для дешифрования алгоритмом AES необходимо во входной файл ввести данные следующим образом:\n" +
				"<Сообщение>\n" +
				"<Ключ><пробел><IV>\n" +
				"Сообщение задается набором байтов (набором чисел от 0 до 255), разделенных пробелами, ключ и IV задаются символами кодировки UTF-8, причем ключ должен быть размером в 16 символов (16 байт), IV также должен быть размером в 16 символов (16 байт).\n\n" +
				"Результатом алгоритмов шифрования станет набор байтов, результатом алгоритмов дешифрования станет набор символов кодировки UTF-8.";

			MessageDialog message = new MessageDialog(text);
			await message.ShowAsync().AsTask();
		}

		private async void Info2Button_Click(object sender, RoutedEventArgs e)
		{
			string text = "В программе используется один алгоритм асимметричного шифрования - RSA. Алгоритм позволяет шифровать и дешифровать заданные входные данные, причем открытый и закрытый ключи необходимые для алгоритма RSA генерируются в самом алгоритме.\n" +
				"1) Для шифрования алгоритмом RSA необходимо во входной файл ввести сообщение одной строкой в виде набора символов кодировки UTF-8.\n" +
				"В результате выведется зашифрованное сообщение в виде набора байтов (набора чисел от 0 до 255), разделенных пробелами, а также открытая часть ключа RSA (n, e).\n" +
				"2) Для дешифрования алгоритмом RSA необходимо сначала выполнить шифрование алгоритмом RSA для того чтобы сгенерировать открытый и закрытый ключи. После этого нужно во входной файл ввести сообщение одной строкой в виде набора байтов (набора чисел от 0 до 255), разделенных пробелами.\n" +
				"В результате выведется дешифрованное сообщение в виде набора символов кодировки UTF-8, а также закрытая часть ключа RSA (n, d).\n";

			MessageDialog message = new MessageDialog(text);
			await message.ShowAsync().AsTask();
		}

		private async void Info3Button_Click(object sender, RoutedEventArgs e)
		{
			string text = "В программе используются 4 алгоритма хеширования данных: MD5, SHA-1, SHA-256, SHA-512. Каждый из алгоритмов позволяет для входного сообщения найти значение хеша определенной длины.\n" +
				"Все алгоритмы имеют одинаковый принцип ввода входных значений. Существуют два варианта задания входных значений:\n" +
				"1) s<пробел><Сообщение> - сообщение задается набором символов кодировки UTF-8.\n" +
				"2) b<пробел><Сообщение> - сообщение задается набором байтов (набором чисел от 0 до 255), разделенных пробелами.\n\n" +
				"В результате выведется результат хеширования в виде набора байтов (чисел от 0 до 255), разделенных пробелами, а также в виде шестнадцатеричных символов.";

			MessageDialog message = new MessageDialog(text);
			await message.ShowAsync().AsTask();
		}

		private async void Info4Button_Click(object sender, RoutedEventArgs e)
		{
			string text = "В программе используется 1 алгоритм вычисления и проверки цифровой подписи, основанный на алгоритме RSA и использующий хеш-функцию SHA-256.\n" +
				"1) Для вычисления цифровой подписи необходимо во входной файл необходимо ввести сообщение в виде набора символов кодировки UTF-8. Результатом вычисления подписи будет сама подпись, представленная в виде набора байтов (набора чисел от 0 до 255), разделенных пробелами.\n" +
				"2) Для проверки цифровой подписи необходимо сначала вычислить цифровую подпись для того чтобы сгенерировать открытые ключи. Затем во входной файл нужно ввести сообщение и подпись следующим образом:\n" +
				"<Сообщение>\n" +
				"<Подпись>\n" +
				"причем сообщение должно быть представлено в виде набора символов кодировки UTF-8, а подпись должна задаваться в виде набора байтов (набора чисел от 0 до 255), разделенных пробелами.\n\n" +
				"Результатом проверки подписи будет строка, в которой будет написан результат проверки: \"Подпись верная\"/\"Подпись неверная\".";

			MessageDialog message = new MessageDialog(text);
			await message.ShowAsync().AsTask();
		}

		private async void ChooseButton_Click(object sender, RoutedEventArgs e)
		{
			string input;

			//Получение доступа к нужной папке
			StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
			//Если файлов не существует, создать их
			StorageFile input_file = await storageFolder.CreateFileAsync(input_file_name, CreationCollisionOption.OpenIfExists);
			StorageFile output_file = await storageFolder.CreateFileAsync(output_file_name, CreationCollisionOption.OpenIfExists);

			//Получение параметров из файла
			input = await FileIO.ReadTextAsync(input_file);

			try
			{
				if (input.Length == 0)
					throw new Exception("Вы не ввели параметры в файл.");

				if (ChooseComboBox.SelectedIndex == 0)
					SymmetricEncryption(input);
				else if (ChooseComboBox.SelectedIndex == 1)
					AsymmetricEncryption(input);
				else if (ChooseComboBox.SelectedIndex == 2)
					Hashing(input);
				else if (ChooseComboBox.SelectedIndex == 3)
					DigitalSignature(input);
			}
			catch (Exception exc)
			{
				//Если что-то пошло не так, на экран выводится сообщение с ошибкой
				MessageDialog message = new MessageDialog(exc.Message);
				await message.ShowAsync().AsTask();
			}

			ResultTextBox.Text = output;
			await FileIO.WriteTextAsync(output_file, output);
		}

		private void SymmetricEncryption(string input)
		{
			if (Type1ComboBox.SelectedIndex == 0)
				output = CryptographicFunctions.SymmetricEncryption3DESEncrypt(input);
			else if (Type1ComboBox.SelectedIndex == 1)
				output = CryptographicFunctions.SymmetricEncryption3DESDecrypt(input);
			else if (Type1ComboBox.SelectedIndex == 2)
				output = CryptographicFunctions.SymmetricEncryptionAES128Encrypt(input);
			else if (Type1ComboBox.SelectedIndex == 3)
				output = CryptographicFunctions.SymmetricEncryptionAES128Decrypt(input);
		}

		private void AsymmetricEncryption(string input)
		{
			if (Type2ComboBox.SelectedIndex == 0)
			{
				rsaParameters = CryptographicFunctions.AsymmetricEncryptionRSAGetParameters();
				output = CryptographicFunctions.AsymmetricEncryptionRSAEncrypt(input, rsaParameters[0]);
			}
			else if (Type2ComboBox.SelectedIndex == 1)
			{
				if (rsaParameters == null)
					throw new Exception("Вы не задали начальные значения для RSA алгоритма. Чтобы задать их, выполните RSA шифрование.");
				output = CryptographicFunctions.AsymmetricEncryptionRSADecrypt(input, rsaParameters[1]);
			}
		}

		private void Hashing(string input)
		{
			if (Type3ComboBox.SelectedIndex == 0)
			{
				output = CryptographicFunctions.Hashing(input, "MD5");
			}
			else if (Type3ComboBox.SelectedIndex == 1)
			{
				output = CryptographicFunctions.Hashing(input, "SHA1");
			}
			else if (Type3ComboBox.SelectedIndex == 2)
			{
				output = CryptographicFunctions.Hashing(input, "SHA256");
			}
			else if (Type3ComboBox.SelectedIndex == 3)
			{
				output = CryptographicFunctions.Hashing(input, "SHA512");
			}
		}

		private void DigitalSignature(string input)
		{
			if (Type4ComboBox.SelectedIndex == 0)
			{
				digitalSignatureParameters = CryptographicFunctions.AsymmetricEncryptionRSAGetParameters();
				output = CryptographicFunctions.DigitalSignatureCreateWithRSA(input, digitalSignatureParameters[1]);
			}
			else if (Type4ComboBox.SelectedIndex == 1)
			{
				if (digitalSignatureParameters == null)
					throw new Exception("Вы не задали начальные значения для проверки подписи с помощью RSA алгоритма. Чтобы задать их, выполните вычисление подписи с помощью RSA.");
				output = CryptographicFunctions.DigitalSignatureVerifyWithRSA(input, digitalSignatureParameters[0]);
			}
		}
	}
}
