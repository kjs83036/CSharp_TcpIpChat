using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tcp_Chat_WF_NToN
{
    public partial class Form1 : Form
    {
        List<RichTextBox> rtb_list = new List<RichTextBox>();
        int r, c;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for(int i =0;i<6;i++)
            {
                RichTextBox rtb = new RichTextBox();
                Label rtb_label = new Label();
                rtb_label.Text = "123";
                rtb_list.Add(rtb);
                if ((i % 3 == 0 && i != 0))
                {
                    r = 0;
                    c += 200;
                }
                rtb.Location = new Point(350 + (r)*200, c + (c/200*20) + 20);
                r++;
                rtb_label.Location = new Point(350 + (r++) * 200, c + (c / 200 * 20));
                rtb.Size = new Size(200, 200);
                this.Controls.Add(rtb);
                this.Controls.Add(rtb_label);
            }
        }
    }
}
