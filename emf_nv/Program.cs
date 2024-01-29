﻿// hevc_nvencは、NVidiaのGPUを使用して高速でエンコードするが、libx265に比べると、なぜかあまり画質が良くない/圧縮率が悪い。
// 同程度の画質にするために-cqオプションを変えて試してみたが、明らかにlibx265に比べると圧縮率が悪かった。

namespace EmfNv;

internal class Program
{
    static void Main(string[] args)
    {
        libEmf.Emf.Execute(args, "hevc_nvenc",
                     [
#if false
                        // Use B frames as references (from 0 to 2) (default disabled)
                        //     disabled        0            E..V....... B frames will not be used for reference
                        //     each            1            E..V....... Each B frame will be used for reference
                        //     middle          2            E..V....... Only(number of B frames) / 2 will be used for reference
                        "-b_ref_mode 0",
#endif
                         // Set target quality level (0 to 51, 0 means automatic) for constant quality mode in VBR rate control (from 0 to 51) (default 0)
                         //"cp 0"   // 0(自動)は画質は問題なさそうだが、H264の1.6倍のファイルサイズになってしまった。
                         //"-cq 38" // 38は低画質。ファイルサイズは30%。
                         //"-cq 35"   // 35は低画質。ファイルサイズは38%。
                         //"-cq 32"   // 32は低画質。ファイルサイズは38%。
                         "-cq 30"   // 30は画質は問題なさそう。ファイルサイズは63%。
                         //"-cq 28"   // 28は画質は問題なさそう。ファイルサイズは77%。
                         //"-cq 25"   // 25は画質は問題なさそう。ファイルサイズは108%。
                         //"-cq 19"   // 19は画質は問題なさそうだが、ファイルサイズは226%。
                     ]);
    }
}
