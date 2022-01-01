namespace Gguc.Aoc.Core.Services;


public class FileReader : IFileReader
{
    private static readonly string ClassId = nameof(FileReader);

    private readonly ILog _log;

    public FileReader(ILog log)
    {
        _log = log;
    }

    public FileType FileType { get; set; }

    public string FileName { get; set; }

    public string FileEntry { get; set; }

    public bool ValidateFile()
    {
        var isValid = File.Exists(FileName);

        if (!isValid)
        {
            _log.WarnLog(ClassId, $"File {FileName} does not exists!");
        }

        return isValid;
    }

    public IEnumerable<string> ReadFile()
    {
        if (FileType == FileType.Default)
        {
            return File.ReadAllLines(FileName);
        }

        return ReadZipFile(FileName, FileEntry);
    }

    private IEnumerable<string> ReadZipFile(string zipPath, string entry)
    {
        using ZipArchive archive = ZipFile.OpenRead(zipPath);
        var sample = archive.GetEntry(entry);
        if (sample == null) yield break;

        using var zipEntryStream = sample.Open();
        TextReader tr = new StreamReader(zipEntryStream);
        while (true)
        {
            var line = tr.ReadLine();

            if (line == null) break;

            yield return line;
        }
        tr.Close();
    }
}
