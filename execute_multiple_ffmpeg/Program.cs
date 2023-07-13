using System;
using System.Diagnostics;
using System.IO;

namespace execute_multiple_ffmpeg
{
    /// <summary>
    /// コマンドライン引数に渡された各動画ファイルに対し、ffmepgを実行させる。
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                if (Directory.Exists(arg))
                {
                    RunDirectory(arg);
                }
                else
                {
                    if (!File.Exists(arg))
                    {
                        Console.WriteLine($"{arg} does not exists.");
                        continue;
                    }
                    if (!IsMovieFile(arg))
                    {
                        continue;
                    }
                    RunFile(arg);
                }
            }
            Console.ReadLine();
        }

        static bool IsMovieFile(string filePath)
        {
            var ext = Path.GetExtension(filePath);
            switch (ext.ToUpper())
            {
                case ".M4V":
                case ".MKV":
                case ".MP4":
                case ".AVI":
                case ".WMV":
                    return true;
                default:
                    return false;
            }
        }

        static void RunDirectory(string directoryPath)
        {
            foreach (var directory in Directory.EnumerateDirectories(directoryPath))
            {
                RunDirectory(directory);
            }
            foreach (var file in Directory.EnumerateFiles(directoryPath))
            {
                if (!IsMovieFile(file))
                {
                    Console.WriteLine($"{file} is not movie file.");
                    continue;
                }
                RunFile(file);
            }
        }

        static void RunFile(string filePath)
        {
            Console.WriteLine($"{filePath}");
            var proc = new Process();

            proc.StartInfo.FileName = @"C:\user\ffmpeg\bin\ffmpeg.exe";
            proc.StartInfo.Arguments = "-i \"" + filePath + "\""
                + " -c:v hevc_nvenc"
                + " -b_ref_mode 0"
                + " -cq 38"
                + " \"" + filePath + ".h265_nv_cq_38.mp4\"";
#if true
            proc.Start();
            proc.WaitForExit();
            if (proc.ExitCode != 0)
            {
                Console.WriteLine($"  Error. ffmpeg terminated width exit code {proc.ExitCode}.");
                Console.WriteLine($"  arguments = \"{proc.StartInfo.Arguments}\"");
            }
#else
            Console.WriteLine($"{proc.StartInfo.Arguments}");
#endif
        }
    }
}
