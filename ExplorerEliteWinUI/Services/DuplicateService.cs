using System.Security.Cryptography;

namespace ExplorerEliteWinUI.Services
{
    public class DuplicateService
    {
        public async Task<List<List<string>>> FindDuplicatesAsync(IEnumerable<string> paths)
        {
            var allFiles = paths.SelectMany(p => Directory.EnumerateFiles(p, ""*"", SearchOption.AllDirectories)).ToList();
            var metaGroups = allFiles.GroupBy(f =>
            {
                var info = new FileInfo(f);
                return $""{info.Name}|{info.Length}|{info.CreationTime:yyyy-MM-dd HH:mm:ss}"";
            }).Where(g => g.Count() > 1);

            var duplicates = new List<List<string>>();
            foreach (var group in metaGroups)
            {
                var hashTable = new Dictionary<string,List<string>>();
                foreach (var file in group)
                {
                    try
                    {
                        using var sha = SHA256.Create();
                        using var stream = File.OpenRead(file);
                        var hash = Convert.ToHexString(sha.ComputeHash(stream));
                        if (!hashTable.ContainsKey(hash)) hashTable[hash] = new List<string>();
                        hashTable[hash].Add(file);
                    }
                    catch { }
                }

                duplicates.AddRange(hashTable.Values.Where(v => v.Count > 1));
            }
            return duplicates;
        }
    }
}
