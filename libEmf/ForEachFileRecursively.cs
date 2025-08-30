namespace libEmf
{
    internal static class ForEachFileRecursively
    {
        internal static void Do(IEnumerable<string> paths, Action<string> processor)
        {
            foreach (var e in paths)
                if (Directory.Exists(e))
                    ProcessDirectory(e, processor);
                else
                    processor(e);
        }

        static IEnumerable<string> SortByName(IEnumerable<string> paths)
        {
            return paths.OrderBy(e => Path.GetFileName(e), StringComparer.OrdinalIgnoreCase);
        }

        static void ProcessDirectory(string directoryPath, Action<string> processor)
        {
            var sortedSubDirectoryPaths = SortByName(Directory.EnumerateDirectories(directoryPath));
            foreach (var e in sortedSubDirectoryPaths)
                ProcessDirectory(e, processor);

            var sortedFilePaths = SortByName(Directory.EnumerateFiles(directoryPath));
            foreach (var e in sortedFilePaths)
                processor(e);
        }
    }
}
