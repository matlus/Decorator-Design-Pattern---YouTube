using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DecoratorDesignPattern
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add(MimeType.Audio);
            comboBox1.Items.Add(MimeType.Video);
            comboBox1.Items.Add(MimeType.Image);
        }

        private void MimeTypeSelectionChanged(object sender, EventArgs e)
        {
            ThumbnailerBase thumbnailer = ThumbnailerFactory.CreateInstance((MimeType)comboBox1.SelectedItem, checkBox1.Checked);
            pictureBox1.Image = thumbnailer.GenerateThumbnail();
        }
    }
}
