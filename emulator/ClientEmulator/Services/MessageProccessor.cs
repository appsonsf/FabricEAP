using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientEmulator.Services
{
    public class MessageProccessor
    {
        private readonly RichTextBox _textBox;

        public MessageProccessor(RichTextBox textBox)
        {
            _textBox = textBox;
        }

        public void AddMessage(string message)
        {
            var text = this._textBox.Text;
            text += "Send:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + message + Environment.NewLine;
            this._textBox.Text = text;
        }

        public void RecivedMessage(string message)
        {
            var text = this._textBox.Text;
            text += "Recived:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + message + Environment.NewLine;
            this._textBox.Text = text;
        }


    }
}
