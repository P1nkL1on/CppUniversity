using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonCircles
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private Random rnd = new Random(DateTime.Now.Millisecond);
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(PolygonDrawer.execute((rnd.Next(100, 600)/10f), true), 10f, 10f);
            g.Dispose();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
        }
    }
}
