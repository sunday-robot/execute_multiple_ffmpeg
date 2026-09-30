using libForEachFileRecursively;
using System.Diagnostics;

namespace libEmf
{
    public static class Emf
    {
        public static void Do(Decoder decoder, IEnumerable<string> paths, string encoderName, string[] options, string[] silentOptions)
        {
            ForEachFileRecursively.Do(
                paths,
                filePath =>
                {
                    var ext = Path.GetExtension(filePath);
                    if (!videoFileExtensions.Contains(ext))
                        return;

                    ExecuteFfmpeg(decoder, filePath, encoderName, options, silentOptions);
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
            ".DIVX",
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

        static void ExecuteFfmpeg(Decoder decoder, string inputFilePath, string encoderName, string[] options, string[] silentOptions)
        {
            Console.WriteLine($"{inputFilePath}");

            string args = "";
            switch (decoder)
            {
                case Decoder.None:
                    break;
                case Decoder.Cuda:
                    args += "-hwaccel cuda";
                    break;
                case Decoder.D3d11va:
                    args += "-hwaccel d3d11va";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(decoder), decoder, null);
            }
            args += " -i";
            args += " \"" + inputFilePath + "\"";
            args += " -c:v " + encoderName;
            var outputFilePath = inputFilePath + "." + encoderName;
            foreach (var o in options)
                foreach (var e in o.Split(' '))
                {
                    args += " " + e;
                    outputFilePath += "_" + EncodeArgument(e);
                }
            foreach (var o in silentOptions)
                foreach (var e in o.Split(' '))
                {
                    args += " " + e;
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

        static string EncodeArgument(string arg)
        {
            return arg.Replace(':', '_');
        }
    }
}
