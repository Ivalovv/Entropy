using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Entropy
{
    public partial class Form2 : Form
    {
        private string text;

        private int numChar = 0;
        private int charSelection = 0;

        private bool changeText = true;

        private string[] alphabet;

        private Dictionary<string, double> frequency1 = new Dictionary<string, double>();

        public Form2()
        {
            InitializeComponent();

            alphabet = new string[32] { "а", "б", "в", "г", "д", "е", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };

            for (int i = 0; i < alphabet.Length; i++)
            {
                frequency1.Add(alphabet[i], 0.0f);
            }

            dataGridView1.Columns.Add("Symbols", "Число символов");
            dataGridView1.Columns.Add("Entropy", "Энтропия");

            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void buttonEntropyCalculation_Click(object sender, EventArgs e)
        {
            ref Dictionary<string, double> frequency = ref frequency1;

            if (changeText)
            {
                text = textBoxText.Text.ToLower();
                changeText = false;
            }

            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows.Clear();
            }

            for (var i = 0; i < numericUpDownNumSelection.Value; i++)
            {
                dataGridView1.Rows.Add(charSelection, entropyCalculation(ref frequency, text.Substring(i * charSelection, charSelection)));
            }

        }

        private double entropyCalculation(ref Dictionary<string, double> frequency, string text)
        {
            double numberLetters = 0;
            double entropy = 0;

            int keyLen = frequency.Keys.First().Length;

            foreach (string key in frequency.Keys.ToList())
            {
                frequency[key] = 0.0f;
            }

            for (int i = 0; i < (text.Length - (keyLen - 1)); i++)
            {
                if (frequency.ContainsKey(text.Substring(i, keyLen)))
                {
                    frequency[text.Substring(i, keyLen)]++;
                    numberLetters++;
                }
                else if(text.Substring(i, keyLen) == "ё")
                {
                    frequency["е"]++;
                    numberLetters++;
                }
            }

            foreach (string key in frequency.Keys.ToList())
            {
                if (frequency[key] != 0)
                {
                    frequency[key] /= numberLetters;
                    entropy += frequency[key] * Math.Log(1.0f / frequency[key], 2);
                }
            }

            return Math.Round(entropy, 2);
        }

        private void textBoxText_TextChanged(object sender, EventArgs e)
        {
            changeText = true;

            numChar = textBoxText.Text.Length;
            textBoxNumChar.Text = $"{numChar}";

            charSelection = Convert.ToInt32(Math.Floor(numChar / numericUpDownNumSelection.Value));
            textBoxCharSelection.Text = $"{charSelection}";
        }

        private void numericUpDownNumSelection_ValueChanged(object sender, EventArgs e)
        {
            charSelection = Convert.ToInt32(Math.Floor(numChar / numericUpDownNumSelection.Value));
            textBoxCharSelection.Text = $"{charSelection}";
        }
    }
}
