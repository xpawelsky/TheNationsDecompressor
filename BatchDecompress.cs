class BatchDecompress
{
    public static void decompressAll(string inputPath, string outputPath, string extension)
    {
        string[] files = ListFiles(inputPath, extension);

        foreach (string v in files)
        {
            ImageDecompressor.UncompressFile(inputPath, Path.GetFileName(v), outputPath);
        }

        
    }

    static string[] ListFiles(string path, string extension)
    {
        string[] files = Directory.GetFiles(path, "*" + extension);

        foreach (string file in files)
        {
            Console.WriteLine(file);
        }
        Console.WriteLine(files.Length + " *" + extension + " files.");

        return files;
    }
}