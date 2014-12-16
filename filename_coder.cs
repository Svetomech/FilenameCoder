using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace SmallProjects
{
    class FilenameCoder
    {
        private const string app_title = "FilenameCoder";

        private static void exit()
        {
            MessageBox.Show("No passed argument or an incorrect one.",
                            app_title,
                            MessageBoxButtons.OK);
            Environment.Exit(0);
        }

        private static DialogResult? askDialog()
        {
            return MessageBox.Show("Decode (Yes)  /  Encode (No)",
                                   app_title,
                                   MessageBoxButtons.YesNoCancel);
        }

        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length == 0) exit();

            DialogResult? btn_clicked = null;
            for (int i = 0; i < args.Length; ++i)
            {
                string file_path = Path.GetFullPath(args[i]);
                if (!File.Exists(file_path)) exit();

                string file_dir = Path.GetDirectoryName(file_path);

                string file_name = Path.GetFileNameWithoutExtension(file_path);
                string file_ext = Path.GetExtension(file_path);

                string file_name_coded;
                if (btn_clicked == null)
                    btn_clicked = askDialog();
                if (btn_clicked == DialogResult.Yes)
                    file_name_coded = Uri.UnescapeDataString(file_name) + file_ext;
                else if (btn_clicked == DialogResult.No)
                    file_name_coded = Uri.EscapeDataString(file_name) + file_ext;
                else
                    return;

                string file_path_coded = Path.Combine(file_dir, file_name_coded);

                File.Move(file_path, file_path_coded);
            }
        }
    }
}
