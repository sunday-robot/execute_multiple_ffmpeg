using System.Diagnostics;

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
                + " -b_ref_mode 0"  // Use B frames as references (from 0 to 2) (default disabled)
                                    //     disabled        0            E..V....... B frames will not be used for reference
                                    //     each            1            E..V....... Each B frame will be used for reference
                                    //     middle          2            E..V....... Only(number of B frames) / 2 will be used for reference
                + " -cq 38" // Set target quality level (0 to 51, 0 means automatic) for constant quality mode in VBR rate control (from 0 to 51) (default 0)
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
