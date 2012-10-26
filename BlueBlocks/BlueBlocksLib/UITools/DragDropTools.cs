using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BlueBlocksLib.UITools
{
    public static class DragDropTools
    {

        public static void EnableFileDrop(Control c, Action<string[]> onFilesDropped)
        {

            c.AllowDrop = true;
            c.DragEnter += new DragEventHandler((o, e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            });

            c.DragDrop += new DragEventHandler((o, e) =>
            {
                string[] a = (string[])e.Data.GetData(DataFormats.FileDrop);
                onFilesDropped(a);
            });

            c.DragOver += new DragEventHandler((o, e) =>
            {
                e.Effect = DragDropEffects.Copy;
            });

        }
    }

}
