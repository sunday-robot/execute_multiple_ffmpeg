namespace emfTo4k;

internal class Program
{
    static void Main(string[] args)
    {
        libEmf.Emf.Do(args, "libx265", [],
            [
            "-map 0:v", // 全ての映像ストリームを出力する
            "-map 0:a?", // 全ての音声ストリームを出力する
            "-map 0:s?", // 全ての字幕ストリームを出力する
            "-map 0:t?", // 全てのタグストリームを出力する
            "-map_chapters 0", // 全てのチャプターストリームをコピーする
            "-map_metadata 0", // 全てのストリームのメタデータをコピーする
            "-c:a copy",   // 音声は変換しない
            "-c:s copy",   // 字幕は変換しない
            "-vf scale=4096:-2:flags=lanczos", // 映像の横幅を4096にスケーリングする(縦方向は自動計算し、かつ偶数にする)"
        ]);
    }
}
