using System;
using System.Diagnostics;
/*using System.Windows.Forms;*/
using System.IO;

class FilenameFixer
{
	/*[STAThread]*/
	static public void Main(string[] args)
	{
		if (args.Length == 0) return;

		string file_path = Path.GetFullPath(args[0]);
		string file_dir = Path.GetDirectoryName(file_path);

		string file_name = Path.GetFileNameWithoutExtension(file_path);
		string file_ext = Path.GetExtension(file_path);
		string file_name_decoded = Uri.UnescapeDataString(file_name) + file_ext;

		string file_path_decoded = String.Format("{0}\\{1}", file_dir, file_name_decoded);

		File.Move(file_path, file_path_decoded);
    }
}