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
                         "-cq 38"
                     ]);
    }
}
