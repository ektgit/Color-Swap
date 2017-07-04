using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Color_Swap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);

            if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
            {
                drgevent.Effect = DragDropEffects.All;
            }
        }

        void SwapChannels(Bitmap copy)
        {
            for (var row = 0; row < copy.Height; row++)
            {
                for (var col = 0; col < copy.Width; col++)
                {
                    var pixel = copy.GetPixel(col, row);
                    copy.SetPixel(col, row, Color.FromArgb(pixel.A, pixel.B, pixel.G, pixel.R));
                }
            }
        }

        void SwapChannels(string file)
        {
            Bitmap copy;
            using (var temp = new Bitmap(file))
            {
                copy = new Bitmap(temp);
            }

            SwapChannels(copy);

            copy.Save(file);
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);

            Cursor.Current = Cursors.WaitCursor;

            var files = (string[])drgevent.Data.GetData(DataFormats.FileDrop, false);
            foreach(var file in files)
            {
                try
                {
                    SwapChannels(file);
                }
                catch(ArgumentException)
                {
                    if(DialogResult.OK != MessageBox.Show("Invalid image","Error",MessageBoxButtons.OKCancel))
                    {
                        return;
                    }
                }
            }
        }
    }
}
