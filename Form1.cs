using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Entropy
{
    public partial class Form1 : Form
    {
        private string text;

        private double entropy;

        private char[] alphabet;

        private double[] letterFrequency = new double[34];

        public Form1()
        {
            InitializeComponent();

            alphabet = new char[34] { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я', ' ' };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double numberLetters = 0;

            for (int i = 0; i < letterFrequency.Length; i++) letterFrequency[i] = 0.0d;

            textBoxEn.Text = string.Empty;
            text = textBox1.Text;

            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (text[i] == alphabet[j])
                    {
                        letterFrequency[j] += 1;
                        break;
                    }
                }
            }

            for (int i = 0; i < letterFrequency.Length; i++)
            {
                numberLetters += letterFrequency[i];
            }

            for (int i = 0; i < letterFrequency.Length; i++)
            {
                letterFrequency[i] = letterFrequency[i] / numberLetters;
            }

            for (int i = 0; i < letterFrequency.Length; i++)
            {
                entropy = letterFrequency[i] * Math.Log(1 / letterFrequency[i], 2);
            }

            textBoxEn.Text = entropy.ToString();
        }
    }
}