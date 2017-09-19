using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omniscient.Controls
{
    class ResponsiveTreeView : TreeView
    {
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0203)
            {
                m.Msg = 0x0201;
            }
            base.WndProc(ref m);
        }
    }
}
