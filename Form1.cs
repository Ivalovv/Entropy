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

        private string[] textArr;
        private string[] separatingStrings;
        private string[] alphabet;
        
        private Dictionary<string, double> frequency1 = new Dictionary<string, double>();
        private Dictionary<string, double> frequency2 = new Dictionary<string, double>();
        private Dictionary<string, double> frequency3 = new Dictionary<string, double>();

        public Form1()
        {
            InitializeComponent();

            separatingStrings = new string[] { "<<", "...", " ", ",", ".", ":", "\t", "\n" };

            alphabet = new string[34] { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я", " " };

            for (int i = 0; i < alphabet.Length; i++)
            {
                frequency1.Add(alphabet[i], 0.0f);
            }

            for (int i = 0; i < alphabet.Length - 1; i++)
            {
                for (int j = 0; j < alphabet.Length - 1; j++)
                {
                    frequency2.Add(alphabet[i] + alphabet[j], 0.0f);
                }
            }

            for (int i = 0; i < alphabet.Length - 1; i++)
            {
                for (int j = 0; j < alphabet.Length - 1; j++)
                {
                    for (int k = 0; k < alphabet.Length - 1; k++)
                    {
                        frequency3.Add(alphabet[i] + alphabet[j] + alphabet[k], 0.0f);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            text = textBox1.Text.ToLower();

            //textArr = text.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

            textBoxEn1.Text = string.Empty;
            textBoxEn1.Text = entropyCalculation(ref frequency1, ref text).ToString();

            textBoxEn2.Text = string.Empty;
            textBoxEn2.Text = entropyCalculation(ref frequency2, ref text).ToString();

            textBoxEn3.Text = string.Empty;
            textBoxEn3.Text = entropyCalculation(ref frequency3, ref text).ToString();
        }

        private double entropyCalculation(ref Dictionary<string, double> frequency, ref string text)
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
            }

            foreach (string key in frequency.Keys.ToList())
            {
                if (frequency[key] != 0)
                {
                    frequency[key] /= numberLetters;
                    entropy += frequency[key] * Math.Log(1.0f / frequency[key], 2);
                }
            }

            return entropy;
        }
    }
}
