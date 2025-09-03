using libForEachFileRecursively;

namespace libForEachFileRecursivelySample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ForEachFileRecursively.Do("c:\\Users\\sgx03", path => Console.Write("o"));
        }
    }
}
