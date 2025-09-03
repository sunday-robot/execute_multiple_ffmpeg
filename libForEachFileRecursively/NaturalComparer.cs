using System.Runtime.InteropServices;

namespace libForEachFileRecursively
{
    /// <summary>
    /// Windowsのエクスプローラーと同じ自然順序で文字列を比較するためのクラス
    /// </summary>
    public class NaturalComparer : IComparer<string>
    {
        internal class NativeMethodsX
        {
            // ↓以下の馬鹿メッセージを抑止するためのもの。(LibraryImportAttributeは事実上使い物にならない。）
            // コンパイル時に P/Invoke マーシャリング コードを生成するには、'DllImportAttribute' の代わりに 'LibraryImportAttribute' を持つメソッド 'MessageBox' をマークします
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "SYSLIB1054")] 
            [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
            internal static extern int StrCmpLogicalW(string x, string y);
        }

        public int Compare(string? x, string? y)    // ICompareで、Compare(T x, T y)ではなく、Compare(T? x, T? y)になっているため、おかしなことになっている。
        {
            return NativeMethodsX.StrCmpLogicalW(x!, y!);   // 実際にはxもyもnullではないので、!でコンパイラ警告を抑止する。
        }
    }
}
