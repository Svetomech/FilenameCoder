using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace SmallProjects
{
    class FilenameCoder
    {
        private const string app_title = "FilenameCoder";

        private static DialogResult? MsgBox(string msg,
                                            MessageBoxButtons buttons)
        {
            return MessageBox.Show(msg, app_title, buttons);
        }

        private static DialogResult? askDialog()
        {
            return MsgBox("Encode (Yes)  /  Decode (No)",
                          MessageBoxButtons.YesNoCancel);
        }

        private static void exit(bool err = false)
        {
            if (err) MsgBox("Please, do drag&drop some file onto me.",
                            MessageBoxButtons.OK);
            Environment.Exit(0);
        }

        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length == 0) exit(true);

            DialogResult? btn_clicked = null;
            for (int i = 0; i < args.Length; ++i)
            {
                bool isDirectory = false;
                string file_path = Path.GetFullPath(args[i]);
                if (Directory.Exists(file_path)) isDirectory = true;

                if (isDirectory)
                {
                    string[] files = Directory.GetFiles(file_path);

                    string[] args_new = new string[args.Length + files.Length];

                    for (int j = 0; j < args.Length; ++j)
                    {
                        if (!Directory.Exists(args[j])) args_new[j] = args[j];
                    }
                    for (int k = args.Length; k < args.Length + files.Length; ++k)
                    {
                        args_new[k] = files[k - args.Length];
                    }

                    args = args_new;

                    continue;
                }

                if (!File.Exists(file_path)) continue;

                string file_dir = Path.GetDirectoryName(file_path);

                string file_name = Path.GetFileNameWithoutExtension(file_path);
                string file_ext = Path.GetExtension(file_path);

                string file_name_coded;
                if (btn_clicked == null)
                    btn_clicked = askDialog();
                if (btn_clicked == DialogResult.No)
                    file_name_coded = Uri.UnescapeDataString(file_name) + file_ext;
                else if (btn_clicked == DialogResult.Yes)
                    file_name_coded = Uri.EscapeDataString(file_name) + file_ext;
                else
                    break;

                string file_path_coded = Path.Combine(file_dir, file_name_coded);

                bool paths_equal = String.Equals(file_path, file_path_coded);
                bool path_exist = File.Exists(file_path_coded);
                if (paths_equal)
                {
                    /* MsgBox(String.Format("File '{0}' is the same with '{1}'.", file_path, file_path_coded),
                           MessageBoxButtons.OK); */
                    continue;
                }
                if (path_exist)
                {
                    MsgBox(String.Format("File '{0}' already exists.", file_path_coded),
                           MessageBoxButtons.OK);
                    continue;
                }
                File.Move(file_path, file_path_coded);
            }
        }
    }
}