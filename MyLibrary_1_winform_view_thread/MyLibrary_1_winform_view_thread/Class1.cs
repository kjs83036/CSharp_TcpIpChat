using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyLibrary_1_winform_view_thread
{
    public class Winform_Thread_Display
    {

        // 현재는 listBox와 RichTextBox만 분기

        //사용가능 함수 2개

        public void DisplayText(string text, object obj)
        {
            ListBox listBox;
            RichTextBox richTextBox;
            TextBox textBox;


            if ((listBox = (obj as ListBox)) != null)
            {
                DisplayListBox(text, obj);
            }
            else if ((richTextBox = (obj as RichTextBox)) != null)
            {
                DisplayRichTextBox(text, obj);
            }
            else if ((textBox = (obj as TextBox)) != null)
            {
                DisplayTextBox(text, obj);
            }
        }

        public void DisplayClear(object obj)
        {
            ListBox listBox;
            RichTextBox richTextBox;
            TextBox textBox;


            if ((listBox = (obj as ListBox)) != null)
            {
                ClearListBox(listBox);
            }
            else if ((richTextBox = (obj as RichTextBox)) != null)
            {
                ClearRichTextBox(richTextBox);
            }
            else if ((textBox = (obj as TextBox)) != null)
            {
                ClearTextBox(textBox);
            }

        }

        //아래는 private

        private void ClearTextBox(object obj)
        {

            TextBox textBox = obj as TextBox;

            if (textBox.InvokeRequired)
            {
                //beginInvoke 는 비동기식
                textBox.BeginInvoke(new MethodInvoker(delegate
                {
                    textBox.Clear();
                }));
            }
            else
            {
                textBox.Clear();
            }

        }


        private void ClearRichTextBox(object obj)
        {
            RichTextBox richTextBox = obj as RichTextBox;

            if (richTextBox.InvokeRequired)
            {
                //beginInvoke 는 비동기식
                richTextBox.BeginInvoke(new MethodInvoker(delegate
                {
                    richTextBox.Clear();
                }));
            }
            else
            {
                richTextBox.Clear();
            }
        }

        private void ClearListBox(object obj)
        {
            ListBox listBox = obj as ListBox;

            if (listBox.InvokeRequired)
            {
                //beginInvoke 는 비동기식
                listBox.BeginInvoke(new MethodInvoker(delegate
                {
                    listBox.Items.Clear();
                }));
            }
            else
            {
                listBox.Items.Clear();
            }
        }

        private void DisplayRichTextBox(string text, object obj)
        {
            RichTextBox richTextBox = obj as RichTextBox;

            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new MethodInvoker(delegate
                {
                    richTextBox.AppendText(text + "\n");
                }));
            }
            else
            {
                richTextBox.AppendText(text + "\n");
            }
        }

        private void DisplayTextBox(string text, object obj)
        {
            TextBox textBox = obj as TextBox;

            if (textBox.InvokeRequired)
            {
                textBox.BeginInvoke(new MethodInvoker(delegate
                {
                    textBox.AppendText(text);
                }));
            }
            else
            {
                textBox.AppendText(text);
            }
        }


        private void DisplayListBox(string text, object obj)
        {
            ListBox listBox = obj as ListBox;

            if (listBox.InvokeRequired)
            {
                listBox.BeginInvoke(new MethodInvoker(delegate
                {
                    listBox.Items.Add(text);
                }));
            }
            else
            {
                listBox.Items.Add(text);
            }
        }

    }
}
