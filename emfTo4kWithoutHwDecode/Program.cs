using libEmf;

namespace emfTo4kWithoutHwDecode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Emf.Do(false, args, "libx265", [],
                [
                "-map 0:v", // 全ての映像ストリームを出力する
            "-map 0:a?", // 全ての音声ストリームを出力する
            "-map 0:s?", // 全ての字幕ストリームを出力する
            "-map 0:t?", // 全てのタグストリームを出力する
            "-map_chapters 0", // 全てのチャプターストリームをコピーする
            "-map_metadata 0", // 全てのストリームのメタデータをコピーする
            "-c:a aac",   // 音声はAAC
            "-b:a 192k",   // 音声ビットレートを192kbpsに設定
            "-c:s copy",   // 字幕は変換しない
            "-vf scale=4096:-2:flags=lanczos", // 映像の横幅を4096にスケーリングする(縦方向は自動計算し、かつ偶数にする)"
        ]);
        }
    }

}
