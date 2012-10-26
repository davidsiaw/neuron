using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace BlueBlocksLib.UITools
{
    public static class CueTools
    {
        enum WindowMessage
        {
            ECM_FIRST = 0x1500,
            EM_SETCUEBANNER,
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, WindowMessage Msg, uint wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        
        public static void SetWatermark(TextBox textBox, string watermarkText)
        {
            SendMessage(textBox.Handle, WindowMessage.EM_SETCUEBANNER, 0, watermarkText);
        }
    }
}
