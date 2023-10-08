using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;

namespace _5_crypto_2_final_ver
{
    public sealed partial class KeyboardInput : Page
    {
        public KeyboardInput()
        {
            this.InitializeComponent();
        }
        private void FirstChooseButton_Click(object sender, RoutedEventArgs e)
        {
            //Кодирование информации
            string output;
            try
            {
                //Инициализация класса с использованием текстового поля с алфавитом
                StartParameters sp = new StartParameters(EnterProbsTextBox.Text);

                //Кодирование информации из текстового поля
                output = sp.CodeMessage(EnterInputTextBox.Text);
                //Вывод результата на экран
                ResultTextBox.Text = output;

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
                message.ShowAsync().AsTask();
            }
        }

        private void SecondChooseButton_Click(object sender, RoutedEventArgs e)
        {
            string output;
            try
            {
                //Инициализация класса с использованием текстового поля с алфавитом
                StartParameters sp = new StartParameters(EnterProbsTextBox.Text);

                //Декодирование информации из текстового поля
                output = sp.DecodeMessage(EnterInputTextBox.Text);
                //Вывод результата на экран
                ResultTextBox.Text = output;

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
                message.ShowAsync().AsTask();
            }
        }
    }
}
