using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
                var r = Run(arg);
                if (r != 0)
                {
                    Console.WriteLine($"{arg} : failed.({r})");
                }
            }
            Console.ReadLine();
        }

        static int Run(string filePath)
        {
            var proc = new Process();

            proc.StartInfo.FileName = @"C:\user\ffmpeg\bin\ffmpeg.exe";
            proc.StartInfo.Arguments = "-i " + filePath
                + " -c:v hevc_nvenc"
                + " -b_ref_mode 0"
                + " -cq 38"
                + " " + filePath + ".h265_nv_cq_38.mp4";
            proc.Start();
            proc.WaitForExit();
            return proc.ExitCode;
        }
    }
}
