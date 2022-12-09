namespace Gguc.Aoc.Y2022.Models;

using Newtonsoft.Json;

internal class FileDescription
{
    public Guid Id { get; } = Guid.NewGuid();

    public string Name { get; set; }

    [JsonIgnore]
    public FileDescription Parent { get; set; }

    public Guid? ParentId => Parent?.Id;

    public List<FileDescription> Children { get; set; } = new();

    public long Size { get; set; }

    public long TotalSize { get; set; }

    public long GetTotalSize()
    {
        if(TotalSize > 0) return TotalSize;

        var total = Size;
        Children.ForEach(x => total += x.GetTotalSize());

        TotalSize = total;
        return TotalSize;
    }
}