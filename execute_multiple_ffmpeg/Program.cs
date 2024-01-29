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
#if true
            var encoderName = "hevc_nvenc";
#else
            var encoderName = "libx265";
#endif

            ForEachFile(
                args,

                // 動画ファイルかどうかの判定(ファイル名のみで判定)
                (string filePath) =>
                {
                    var ext = Path.GetExtension(filePath);
                    switch (ext.ToUpper())
                    {
                        case ".M4V":
                        case ".MKV":
                        case ".MP4":
                        case ".MPG":
                        case ".AVI":
                        case ".WMV":
                            return true;
                        default:
                            return false;
                    }
                },

                // ffmpegの実行
                (string filePath) =>
                {
                    ExecuteFfmpeg(
                        filePath,
                        encoderName,

                        // オプション
                        new string[] {
#if false
                        // Use B frames as references (from 0 to 2) (default disabled)
                        //     disabled        0            E..V....... B frames will not be used for reference
                        //     each            1            E..V....... Each B frame will be used for reference
                        //     middle          2            E..V....... Only(number of B frames) / 2 will be used for reference
                        "-b_ref_mode 0",
#endif
#if false
                        // Set target quality level (0 to 51, 0 means automatic) for constant quality mode in VBR rate control (from 0 to 51) (default 0)
                        "-cq 38"
#endif
                        });
                });
            Console.WriteLine("Finished.");
            Console.WriteLine("Hit enter key.");
            Console.ReadLine();
        }

        static void ForEachFile(string[] filePathList, Func<string, bool> isTarget, Action<string> processor)
        {
            foreach (var filePath in filePathList)
                if (Directory.Exists(filePath))
                    ProcessDirectory(filePath, isTarget, processor);
                else
                    ProcessFile(filePath, isTarget, processor);
        }

        static void ProcessDirectory(string parentDirectoryPath, Func<string, bool> isTarget, Action<string> processor)
        {
            foreach (var directoryPath in Directory.EnumerateDirectories(parentDirectoryPath))
                ProcessDirectory(directoryPath, isTarget, processor);
            foreach (var filePath in Directory.EnumerateFiles(parentDirectoryPath))
                ProcessFile(filePath, isTarget, processor);
        }

        static void ProcessFile(string filePath, Func<string, bool> isTarget, Action<string> processor)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"{filePath} does not exists.");
                return;
            }
            if (!isTarget(filePath))
                return;
            processor(filePath);
        }

        static void ExecuteFfmpeg(string inputFilePath, string encoderName, string[] options)
        {
            Console.WriteLine($"{inputFilePath}");

            var args = "-i";
            args += " \"" + inputFilePath + "\"";
            args += " -c:v " + encoderName;
            var outputFilePath = inputFilePath + "." + encoderName;
            foreach (var o in options)
                foreach (var e in o.Split(' '))
                {
                    args += " " + e;
                    outputFilePath += "_" + e;
                }
            outputFilePath += ".mp4";
            args += " \"" + outputFilePath + "\"";
#if true
            var proc = new Process();
            proc.StartInfo.FileName = @"C:\user\ffmpeg\bin\ffmpeg.exe";
            proc.StartInfo.Arguments = args;
            proc.Start();
            proc.WaitForExit();
            if (proc.ExitCode != 0)
            {
                Console.WriteLine($"  Error. ffmpeg terminated width exit code {proc.ExitCode}.");
                Console.WriteLine($"  arguments = \"{proc.StartInfo.Arguments}\"");
            }
#else
            Console.WriteLine($"{args}");
#endif
        }
    }
}
