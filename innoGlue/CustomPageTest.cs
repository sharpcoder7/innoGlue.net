using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using innoGlue.innoGlue;

namespace innoGlue
{
    public partial class CustomPageTest : BaseWizardPage
    {
        public CustomPageTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello from winforms!");
        }

        private void textBox1_TextChanged(object sender, EventArgs args)
        {
            try
            {
                var result = InnoGlue.Context.ExpandConstant(textBox1.Text);
                textBox2.Text = result;
            }
            catch (Exception e)
            {
                textBox2.Text = "Exception: " + e.Message;
            }
            
        }
    }
}
