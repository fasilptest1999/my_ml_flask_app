namespace ExplorerEliteWinUI.Models
{
    public class FileItem
    {
        public string Icon { get; set; }
        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool IsFolder { get; set; }
        public long Size { get; set; }
        public string SizeText { get; set; }
        public string Modified { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
