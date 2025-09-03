namespace libForEachFileRecursively
{
    public static class ForEachFileRecursively
    {
        static readonly NaturalComparer _naturalComparer = new();

        public static void Do(IEnumerable<string> paths, Action<string> action)
        {
            foreach (var e in paths)
                Do(e, action);
        }

        public static void Do(string path, Action<string> action)
        {
            if (Directory.Exists(path))
                DoForDirectory(path, action);
            else
                action(path);
        }

        static void DoForDirectory(string directoryPath, Action<string> action)
        {
            try
            {
                var sortedSubDirectoryPaths = SortByName(Directory.EnumerateDirectories(directoryPath));
                foreach (var e in sortedSubDirectoryPaths)
                    DoForDirectory(e, action);

                var sortedFilePaths = SortByName(Directory.EnumerateFiles(directoryPath));
                foreach (var e in sortedFilePaths)
                    action(e);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"UnauthorizedAccessException: {directoryPath}");
            }
            // 上記以外の例外についてはあえて何もしない。
        }

        static IEnumerable<string> SortByName(IEnumerable<string> paths)
        {
            return paths.OrderBy(e => Path.GetFileName(e), _naturalComparer);
        }
    }
}
