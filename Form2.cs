using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Entropy
{
    public partial class Form2 : Form
    {
        // Переменные класса для хранения текста, настроек и результатов
        private string text;

        private int accuracy = 6; // Точность вычислений энтропии (количество знаков после запятой)
        private int numSelection = 1; // Количество выборок (групп символов)
        private int numCharText = 0; // Общее количество символов в тексте
        private int charSelection = 0; // Количество символов в каждой выборке

        private double avgEntropy = 0; // Средняя энтропия текста

        private bool fInput = false;

        // Массив символов алфавита
        private string[] alphabet;

        // Словарь для хранения частот символов
        private Dictionary<string, double> frequency1 = new Dictionary<string, double>();

        // Конструктор формы
        public Form2()
        {
            InitializeComponent();

            // Инициализация алфавита (русские буквы)
            alphabet = new string[32] { "а", "б", "в", "г", "д", "е", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };

            // Заполнение словаря частот символов нулевыми значениями
            for (int i = 0; i < alphabet.Length; i++)
            {
                frequency1.Add(alphabet[i], 0.0d);
            }

            // Настройка столбцов DataGridView для отображения результатов
            dataGridView1.Columns.Add("Symbols", "Число символов"); // Столбец для числа символов в выборке
            dataGridView1.Columns.Add("Entropy", "Энтропия"); // Столбец для значения энтропии

            // Выравнивание заголовков и ячеек по центру
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Автоматическое изменение размера столбцов
        }

        // Обработчик нажатия кнопки для расчета энтропии
        private void buttonEntropyCalculation_Click(object sender, EventArgs e)
        {
            // Ссылка на словарь частот символов
            ref Dictionary<string, double> frequency = ref frequency1;
            avgEntropy = 0; // Сброс средней энтропии

            // Очистка DataGridView, если он не пуст
            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows.Clear();
            }

            if (fInput)
            {
                // Проверка, помещаются ли все символы в выборки
                if (charSelection * numericUpDownNumSelection.Value <= numCharText)
                {
                    // Цикл для создания выборок и расчета энтропии
                    for (var i = 0; i < numericUpDownNumSelection.Value; i++)
                    {
                        // Добавление строки в DataGridView с числом символов и значением энтропии
                        dataGridView1.Rows.Add(charSelection, entropyCalculation(ref frequency, text.Substring(i * charSelection, charSelection)));
                    }
                }
                else if (charSelection * numericUpDownNumSelection.Value > numCharText)
                {
                    // Если количество символов больше длины текста, обрабатываем случай зацикливания
                    for (int j = 0, i = 0; i < numericUpDownNumSelection.Value; i++, j += charSelection)
                    {
                        while (j + charSelection > numCharText)
                        {
                            // Переносим начало подстроки на начало текста, если вышли за его пределы
                            j = (j + charSelection) - numCharText;

                            if (j == numCharText)
                            {
                                j -= numCharText; // Если дошли до конца текста, начинаем с начала
                            }
                        }

                        // Добавление строки в DataGridView с числом символов и значением энтропии
                        dataGridView1.Rows.Add(charSelection, entropyCalculation(ref frequency, text.Substring(j, charSelection)));
                    }
                }
            }
            else
            {
                // Вычисляем остаток символов
                int j = numCharText - Convert.ToInt32(charSelection * numericUpDownNumSelection.Value);

                // Цикл для создания выборок и расчета энтропии
                for (var i = 0; i < numericUpDownNumSelection.Value; i++)
                {
                    // Добавление строки в DataGridView с числом символов и значением энтропии
                    dataGridView1.Rows.Add(charSelection + j, entropyCalculation(ref frequency, text.Substring(i * charSelection, charSelection + j)));
                }
            }

            // Добавление строки со средней энтропией в DataGridView
            dataGridView1.Rows.Add("Средняя энтропия", Math.Round(avgEntropy / Convert.ToDouble(numSelection), accuracy));
        }

        // Метод для расчета энтропии
        private double entropyCalculation(ref Dictionary<string, double> frequency, string text)
        {
            double numberLetters = 0; // Общее количество символов
            double entropy = 0; // Энтропия

            int keyLen = frequency.Keys.First().Length; // Длина ключей в словаре частот

            // Сброс частот символов перед началом подсчета
            foreach (string key in frequency.Keys.ToList())
            {
                frequency[key] = 0.0d;
            }

            // Подсчет частот символов в тексте
            for (int i = 0; i < (text.Length - (keyLen - 1)); i++)
            {
                if (frequency.ContainsKey(text.Substring(i, keyLen)))
                {
                    // Увеличение счетчика для символа, если он есть в алфавите
                    frequency[text.Substring(i, keyLen)]++;
                    numberLetters++;
                }
                else if (text.Substring(i, keyLen) == "ё")
                {
                    // Специальная обработка буквы "ё" как "е"
                    frequency["е"]++;
                    numberLetters++;
                }
            }

            // Вычисление энтропии на основе частот символов
            foreach (string key in frequency.Keys.ToList())
            {
                if (frequency[key] != 0)
                {
                    // Нормализация частоты символа и добавление вклада в энтропию
                    frequency[key] /= numberLetters;
                    entropy += frequency[key] * Math.Log(1.0d / frequency[key], 2);
                }
            }

            avgEntropy += entropy; // Добавление текущей энтропии к средней энтропии
            return Math.Round(entropy, accuracy); // Возвращение округленного значения энтропии
        }

        // Обработчик изменения текста в текстовом поле
        private void textBoxText_TextChanged(object sender, EventArgs e)
        {
            // Фильтрация текста для удаления всех символов кроме русских букв
            text = Regex.Replace(textBoxText.Text.ToLower(), "[^а-яё]", "");

            // Обновление количества символов в тексте
            numCharText = text.Length;
            textBoxNumChar.Text = $"{numCharText}";

            // Вычисление количества символов в каждой выборке
            charSelection = numCharText / numSelection;
            textBoxCharSelection.Text = $"{charSelection + (numCharText - Convert.ToInt32(charSelection * numericUpDownNumSelection.Value))}";
        }

        // Обработчик изменения значения в numericUpDown для количества выборок
        private void numericUpDownNumSelection_ValueChanged(object sender, EventArgs e)
        {
            // Обновление количества выборок
            numSelection = Convert.ToInt32(numericUpDownNumSelection.Value);
            charSelection = numCharText / numSelection; // Обновление количества символов в каждой выборке
            textBoxCharSelection.Text = $"{charSelection + (numCharText - Convert.ToInt32(charSelection * numericUpDownNumSelection.Value))}";
        }

        // Обработчик изменения текста в текстовом поле для количества символов в выборке
        private void textBoxCharSelection_TextChanged(object sender, EventArgs e)
        {
            if (fInput)
            {
                // Проверка, является ли введенное значение числом
                if (int.TryParse(textBoxCharSelection.Text, out int x))
                {
                    buttonEntropyCalculation.Enabled = true; // Включение кнопки, если значение корректное
                    charSelection = Convert.ToInt32(textBoxCharSelection.Text);
                }
                else
                {
                    buttonEntropyCalculation.Enabled = false; // Отключение кнопки, если значение некорректное
                    MessageBox.Show("Число пожалуйста", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Проверка, не превышает ли количество символов в выборке количество символов в тексте
                if (charSelection > numCharText)
                {
                    buttonEntropyCalculation.Enabled = false; // Отключение кнопки, если символов слишком много
                    MessageBox.Show("Слишком большое число символов в выборке", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    buttonEntropyCalculation.Enabled = true; // Включение кнопки, если значение допустимое
                }
            }
        }

        // Обработчик двойного клика на текстовом поле для автоматического обновления количества символов в выборке
        private void textBoxCharSelection_DoubleClick(object sender, EventArgs e)
        {
            // Автоматический расчет и обновление количества символов в выборке при двойном клике
            charSelection = numCharText / numSelection;
            textBoxCharSelection.Text = $"{charSelection + (numCharText - Convert.ToInt32(charSelection * numericUpDownNumSelection.Value))}";
        }

        // Обработчик изменения значения в numericUpDown для точности вычислений
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // Обновление точности вычислений энтропии
            accuracy = Convert.ToInt32(numericUpDownAccuracy.Value);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            fInput = !fInput;

            if (!fInput)
            {
                charSelection = numCharText / numSelection;
                textBoxCharSelection.Text = $"{charSelection + (numCharText - Convert.ToInt32(charSelection * numericUpDownNumSelection.Value))}";

                textBoxCharSelection.ReadOnly = true;
            }
            else
            {
                textBoxCharSelection.ReadOnly = false;
            }
        }
    }
}
