namespace libEmf
{
    internal static class ForEachFileRecursively
    {
        internal static void Do(IEnumerable<string> paths, Action<string> processor)
        {
            foreach (var e in paths)
                Do(e, processor);
        }

        internal static void Do(string path, Action<string> processor)
        {
            if (Directory.Exists(path))
                ProcessDirectory(path, processor);
            else
                processor(path);
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

        static IEnumerable<string> SortByName(IEnumerable<string> paths)
        {
            return paths.OrderBy(e => Path.GetFileName(e), StringComparer.OrdinalIgnoreCase);
        }
    }
}
