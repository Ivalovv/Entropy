using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Entropy
{
    public partial class Form2 : Form
    {
        private string text;

        private int accuracy = 6;
        private int numSelection = 1;
        private int numChar = 0;
        private int charSelection = 0;

        private double avgEntropy = 0;

        private string[] alphabet;

        private Dictionary<string, double> frequency1 = new Dictionary<string, double>();

        public Form2()
        {
            InitializeComponent();

            alphabet = new string[32] { "а", "б", "в", "г", "д", "е", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };

            for (int i = 0; i < alphabet.Length; i++)
            {
                frequency1.Add(alphabet[i], 0.0d);
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
            avgEntropy = 0;

            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows.Clear();
            }

            if (charSelection * numericUpDownNumSelection.Value <= numChar)
            {
                int j = numChar - Convert.ToInt32(charSelection * numericUpDownNumSelection.Value);

                for (var i = 0; i < numericUpDownNumSelection.Value; i++)
                {
                    dataGridView1.Rows.Add(charSelection + j, entropyCalculation(ref frequency, text.Substring(i * charSelection, charSelection + j)));
                }
            }
            dataGridView1.Rows.Add("Средняя энтропия", Math.Round(avgEntropy / Convert.ToDouble(numSelection), accuracy));
        }

        private double entropyCalculation(ref Dictionary<string, double> frequency, string text)
        {
            double numberLetters = 0;
            double entropy = 0;

            int keyLen = frequency.Keys.First().Length;

            foreach (string key in frequency.Keys.ToList())
            {
                frequency[key] = 0.0d;
            }

            for (int i = 0; i < (text.Length - (keyLen - 1)); i++)
            {
                if (frequency.ContainsKey(text.Substring(i, keyLen)))
                {
                    frequency[text.Substring(i, keyLen)]++;
                    numberLetters++;
                }
                else if (text.Substring(i, keyLen) == "ё")
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
                    entropy += frequency[key] * Math.Log(1.0d / frequency[key], 2);
                }
            }

            avgEntropy += entropy;
            return Math.Round(entropy, accuracy);
        }

        private void textBoxText_TextChanged(object sender, EventArgs e)
        {
            text = Regex.Replace(textBoxText.Text.ToLower(), "[^а-яё]", "");

            numChar = text.Length;
            textBoxNumChar.Text = $"{numChar}";

            charSelection = numChar / numSelection;
            textBoxCharSelection.Text = $"{charSelection}";
        }

        private void numericUpDownNumSelection_ValueChanged(object sender, EventArgs e)
        {
            numSelection = Convert.ToInt32(numericUpDownNumSelection.Value);
            charSelection = numChar / numSelection;
            textBoxCharSelection.Text = $"{charSelection}";
        }

        private void textBoxCharSelection_TextChanged(object sender, EventArgs e)
        {

            if (int.TryParse(textBoxCharSelection.Text, out int x))
            {
                buttonEntropyCalculation.Enabled = true;
                charSelection = Convert.ToInt32(textBoxCharSelection.Text);
            }
            else
            {
                buttonEntropyCalculation.Enabled = false;
                MessageBox.Show("Число пожалуйста", "ew", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxCharSelection_DoubleClick(object sender, EventArgs e)
        {
            charSelection = numChar / numSelection;
            textBoxCharSelection.Text = $"{charSelection}";
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            accuracy = Convert.ToInt32(numericUpDownAccuracy.Value);
        }
    }
}
