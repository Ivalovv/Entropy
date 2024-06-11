using System;
using System.Windows.Forms;


namespace Entropy
{
    public partial class Entropy : Form
    {
        public Entropy()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}
