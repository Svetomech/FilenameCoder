using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

class FilenameCoder
{
	private const string app_title = "FilenameCoder";

	private static void exit()
	{
		MessageBox.Show("No passed argument or is not a file.",
		                app_title,
						MessageBoxButtons.OK);
		Environment.Exit(0);
	}

	[STAThread]
	public static void Main(string[] args)
	{
		if (args.Length == 0) exit();
		string file_path = Path.GetFullPath(args[0]);
		if (!File.Exists(file_path)) exit();

		string file_dir = Path.GetDirectoryName(file_path);

		string file_name = Path.GetFileNameWithoutExtension(file_path);
		string file_ext = Path.GetExtension(file_path);

		string file_name_coded;
		var btn_clicked = MessageBox.Show("Do you want to decode the file (Yes) or encode it (No)?",
		                                  app_title,
										  MessageBoxButtons.YesNoCancel);
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