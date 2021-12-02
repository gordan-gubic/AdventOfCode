namespace Gguc.Aoc.Core.Services;

public interface IFileReader
{
    FileType FileType { get; set; }
    
    string FileName { get; set; }
    
    string FileEntry { get; set; }
    
    bool ValidateFile();

    IEnumerable<string> ReadFile();
}