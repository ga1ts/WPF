using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Нажатие кнопки Вычислить
		/// </summary>
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			textBlock2.Text = null;
			textBlock3.Text = null;

			if (string.IsNullOrEmpty(textBox1.Text)) //если строка пустая
			{
				MessageBox.Show("Не ввели число");
				return;
			}

			int n;
			if (!int.TryParse(textBox1.Text, out n)) //Если ошибка преобразования строки в число
			{
				MessageBox.Show("Неправильно ввели число");
				return;
			}

			if (n < 1 || n > 10)
			{
				MessageBox.Show("N должно быть от 1 до 10");
				return;
			}

			int[] mas = new int[n]; //создаём массив для чисел фибоначи

			int i = 0;
			mas[i] = 0; //1-е число Фибоначи

			if (n > 1) //если введено число n больше1, т.е. числел должно быть больше одного
			{
				i++;
				mas[i] = 1; //2-е число Фибоначи


				if (n > 2) //если чисел должно быть больше двух
				{
					i++;
					while (i < n)
					{
						mas[i] = mas[i - 1] + mas[i - 2]; // очередное число Фибоначи
						i++;
					}
				}
			}

			//Выводим числа и вычисляем сумму
			int sum = 0;

			for (int j = 0; j < n; j++)
			{
				textBlock2.Text += mas[j] + ","; //Выводим числа
				sum += mas[j]; //вычисляем сумму
			}
			textBlock3.Text = sum.ToString(); //Выводим сумму на экран
		}

		/// <summary>
		/// Нажата кнопка  Вычислить , Задание 2
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonZadanie2_Click(object sender, RoutedEventArgs e)
		{
			SystemSounds.Beep.Play(); //звук

			textResult.Text = "";
			string str = inputText.Text; //берём введённый текст
			StringBuilder stringBuilder = new();

			int i = 0;
			string word = ""; //очередное слово

			while (i < str.Length) //пока не дошли до конца строки
			{
				bool isLetter = false; //флаг, очередной символ - это буква

				//если очердной символ не знак препинания
				if (str[i] != ' ' && str[i] != ',' && str[i] != '.' && str[i] != '!' && str[i] != ':' && str[i] != '?' && str[i] != ';')
				{ //т.е. это буква, тогда прибавляем букву к слову
					word += str[i];
					isLetter = true;
				}

				//очередной символ - это знак препинания ИЛИ это уже конец строки
				if (!isLetter || i == str.Length - 1)
				{
					//рассматриваем слово
					if (word.Length > 5) //если длина слова >5
					{
						//выводим на экран и переходим на новую строку
						// textResult.Text += word + Environment.NewLine;
						// когда много операция сложения строк надо использовать StringBuilder
						stringBuilder.Append(word + Environment.NewLine);
					}
					word = "";//очистили слово
				}
				i++;
			}

			textResult.Text = stringBuilder.ToString();
		}

		/// <summary>
		/// Задание 3
		/// </summary>
		private void ButtonEmail_Click(object sender, RoutedEventArgs e)
		{
			SystemSounds.Beep.Play(); //звук

			//будем использовать регулярные выражения
	

			textResult.Text = "";
			string str = inputText.Text; //берём введённый текст

			//выражение взял отсюда https://www.cyberforum.ru/csharp-beginners/thread451073.html
			Regex regex = new Regex(@"\b[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}\b", RegexOptions.IgnoreCase);

			MatchCollection matches = regex.Matches(str);

			if (matches.Count > 0)
			{
				//будем записывать все найденные адреса в строку allAddresses
				//Сразу записывать в textResult.Text не стоит, т.к. textResult.Text работает очень медленно
				string allAddresses = "";

				foreach (Match match in matches)
				{
					if (allAddresses.Length > 0) allAddresses += "; ";
					allAddresses += match.Value;
				}

				textResult.Text = allAddresses;
			}
			else
			{
				MessageBox.Show("e-mail адресов не найдено");
			}
		}

		/// <summary>
		/// Задание 4
		/// </summary>
		private async void ButtonFile_Click(object sender, RoutedEventArgs e)
		{
			string filePath;
			string str = textResult.Text; //берём результат

			//статья Работа с диалогами  https://metanit.com/sharp/wpf/22.6.php
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.FileName = "Result"; // имя по умолчанию
			saveFileDialog.DefaultExt = ".text"; // расширение по умолчанию
			saveFileDialog.Filter = "Текст (.txt)|*.txt"; 

			if (saveFileDialog.ShowDialog() == true)
			{
				filePath = saveFileDialog.FileName;

				try
				{
					//чтение и запись в файл 
					// полная перезапись файла 
					using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.Unicode))
					{
						await writer.WriteLineAsync(str);
					}
					MessageBox.Show("Файл сохранён");
				}
				catch (Exception ex)
				{
					MessageBox.Show("При записи файла произошла ошибка: "+ex.Message);
				}
			}
		}

		/// <summary>
		/// Задание 5
		/// </summary>
		private async void ButtonFileAdd_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

			if (openFileDialog.ShowDialog() == true)
			{
				string filePath = openFileDialog.FileName;

				try
				{
					using (StreamReader reader = new StreamReader(filePath, Encoding.Unicode))
					{
						string text = await reader.ReadToEndAsync(); //читаем содержимое

						// т.к. в задании сказано о "добавлении текста",
						//то прочитанный текст добавляем к введённому тексту 
						inputText.Text += text;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("При чтении файла произошла ошибка: " + ex.Message);
				}
			}
		}
	}
}
