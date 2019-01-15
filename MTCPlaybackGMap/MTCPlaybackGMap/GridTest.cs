using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTCPlaybackGMap
{
    public partial class GridTest : Form
    {
        public GridTest(List<playBackRow> playbackData)
        {
            InitializeComponent();
            gridControl1.DataSource = playbackData;
        }
    }
}
