using System.Diagnostics;

namespace libEmf
{
    public static class Emf
    {
        public static void Execute(IEnumerable<string> paths, string encoderName, string[] options)
        {
            ForEachFileRecursively.Do(
                paths,
                filePath =>
                {
                    if (!HasMovieFileExtension(filePath))
                        return;

                    ExecuteFfmpeg(filePath, encoderName, options);
                });
            Console.WriteLine("Finished.");
            Console.WriteLine("Hit enter key.");
            Console.ReadLine();
        }

        static readonly HashSet<string> movieExtensions = [
            ".ASF",
            ".AVI",
            ".M4V",
            ".MKV",
            ".MP4",
            ".MPEG",
            ".MPG",
            ".RM",
            ".RMVB",
            ".TS",
            ".VOB",
            ".WMV"
        ];

        /// <summary>
        /// 動画ファイルかどうかを返す。拡張子で判断するだけのもの。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        static bool HasMovieFileExtension(string filePath)
        {
            var ext = Path.GetExtension(filePath);
            ext = ext.ToUpper();
            return movieExtensions.Contains(ext);
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
