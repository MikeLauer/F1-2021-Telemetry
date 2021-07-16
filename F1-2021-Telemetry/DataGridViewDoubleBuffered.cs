using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F1_2021_Telemetry
{
    public class DataGridViewDoubleBuffered : DataGridView
    {
        public DataGridViewDoubleBuffered()
        {
            DoubleBuffered = true;
        }
    }
}
