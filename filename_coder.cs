using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace SmallProjects
{
  class FilenameCoder
  {
    private const string app_title = "FilenameCoder";

    public enum Platform
    {
      Windows,
      Unix
    }

    public static Platform runningPlatform()
    {
      switch (Environment.OSVersion.Platform)
      {
        case PlatformID.Unix:
        case PlatformID.MacOSX:
          return Platform.Unix;

        default:
          return Platform.Windows;
      }
    }

    private static DialogResult? msgBox(string msg,
                                        MessageBoxButtons buttons)
    {
      return MessageBox.Show(msg, app_title, buttons);
    }

    private static DialogResult? askDialog()
    {
      return msgBox("Encode (Yes)  /  Decode (No)",
                    MessageBoxButtons.YesNoCancel);
    }

    private static void exit()
    {
      msgBox("Please, do drag&drop some file onto me.",
             MessageBoxButtons.OK);
      Environment.Exit(0);
    }

    [STAThread]
    public static void Main(string[] args)
    {
      if (0 == args.Length) exit();

      DialogResult? btn_clicked = null;
      var marked_dirs = new List<string>();

      /* File Loop */
      FL:
      for (int i = 0; i < args.Length; ++i)
      {
        string file_path = Path.GetFullPath(args[i]);

        if (Directory.Exists(file_path))
        {
          marked_dirs.Add(file_path);
          continue;
        }

        // Sanity check
        if ((null == file_path) || (!File.Exists(file_path))) continue;

        string file_dir = Path.GetDirectoryName(file_path);

        string file_name = Path.GetFileNameWithoutExtension(file_path);
        string file_ext = Path.GetExtension(file_path);

        string file_name_coded;
        if (null == btn_clicked)
          btn_clicked = askDialog();
        if (DialogResult.No == btn_clicked)
          file_name_coded = Uri.UnescapeDataString(file_name) + file_ext;
        else if (DialogResult.Yes == btn_clicked)
          file_name_coded = Uri.EscapeDataString(file_name) + file_ext;
        else
          break;

        string file_path_coded = Path.Combine(file_dir, file_name_coded);

        bool paths_equal;
        if (Platform.Unix == runningPlatform())
          paths_equal = String.Equals(file_path, file_path_coded);
        else
          paths_equal = String.Equals(file_path, file_path_coded, StringComparison.OrdinalIgnoreCase);
        bool path_exist = File.Exists(file_path_coded);

        if (paths_equal)
        {
          /* msgBox(String.Format("File '{0}' is the same with '{1}'.", file_path, file_path_coded),
                 MessageBoxButtons.OK); */
          continue;
        }
        if (path_exist)
        {
          msgBox(String.Format("File '{0}' already exists.", file_path_coded),
                 MessageBoxButtons.OK);
          continue;
        }
        File.Move(file_path, file_path_coded);
      }
      /**/

      if (0 == marked_dirs.Count) return;

      /* Directory Loop */
      foreach (string dir_path in marked_dirs)
      {
        string[] files = Directory.GetFiles(dir_path);

        // HACK: and an ugly one
        args = new string[files.Length];
        args = files;
        marked_dirs.Remove(dir_path);
        goto FL;
      }
      /**/
    }
  }
}