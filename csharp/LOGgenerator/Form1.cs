using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LOGgenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<String> res = new List<string>();

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();


            
            int files = (int)numericUpDown2.Value;
            while (files-- > 0)
            {
                int times = (int)numericUpDown1.Value;
                string add = "";
                while (times-- > 0)
                    add += LogConsts.makeRandomLog()+"\r\n";
                textBox1.Text = add;
                res.Add(add);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WriterToFile.writedown(res);
        }
    }
}
