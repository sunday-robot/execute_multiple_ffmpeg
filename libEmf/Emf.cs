using libForEachFileRecursively;
using System.Diagnostics;

namespace libEmf
{
    public static class Emf
    {
        public static void Do(IEnumerable<string> paths, string encoderName, string[] options)
        {
            ForEachFileRecursively.Do(
                paths,
                filePath =>
                {
                    var ext = Path.GetExtension(filePath);
                    if (!videoFileExtensions.Contains(ext))
                        return;

                    ExecuteFfmpeg(filePath, encoderName, options);
                });
            Console.WriteLine("Finished.");
            Console.WriteLine("Hit enter key.");
            Console.ReadLine();
        }

        /// <summary>
        /// 動画ファイルの拡張子のセット(OrdinalIgnoreCaseは、大文字小文字を区別しないようにするためのもの)
        /// </summary>
        static readonly HashSet<string> videoFileExtensions = new(StringComparer.OrdinalIgnoreCase) {
            ".ASF",
            ".AVI",
            ".FLV",
            ".M2TS",
            ".M4V",
            ".MKV",
            ".MOV",
            ".MP4",
            ".MPEG",
            ".MPG",
            ".RM",
            ".RMVB",
            ".TS",
            ".VOB",
            ".WEBM",
            ".WMV"
        };

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
