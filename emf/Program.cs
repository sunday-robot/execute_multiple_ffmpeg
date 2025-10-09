namespace Emf;

internal class Program
{
    static void Main(string[] args)
    {
        libEmf.Emf.Do(args, "libx265", [
            //"-c:a copy",   // 音声は変換しない。これは使えない?
        ]);
    }
}
