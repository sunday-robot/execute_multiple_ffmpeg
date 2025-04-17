using System.Diagnostics;

namespace libEmf
{
    public static class Emf
    {
        public static void Execute(string[] filePath, string encoderName, string[] options)
        {
            ForEachFile(
                filePath,
                // 動画ファイルかどうかの判定(拡張子のみで判定)
                (string filePath) =>
                {
                    var ext = Path.GetExtension(filePath);
                    switch (ext.ToUpper())
                    {
                        case ".AVI":
                        case ".M4V":
                        case ".MKV":
                        case ".MP4":
                        case ".MPEG":
                        case ".MPG":
                        case ".RMVB":
                        case ".TS":
                        case ".VOB":
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
                        options);
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
