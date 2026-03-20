using System.Diagnostics;

namespace ExplorerEliteWinUI.Services
{
    public class CompressionService
    {
        public string SevenZipPath { get; set; } = @""C:\Program Files\7-Zip\7z.exe"";
        public string RarPath { get; set; } = @""C:\Program Files\WinRAR\rar.exe"";

        public async Task<bool> CompressAsync(string archivePath, string[] files, string format)
        {
            string args = format.ToLower() switch
            {
                "".zip"" => $""a -tzip \""{archivePath}\"" {string.Join("" "", files.Select(f => $""\""{f}\""""))}"",
                "".7z""  => $""a \""{archivePath}\"" {string.Join("" "", files.Select(f => $""\""{f}\""""))}"",
                "".rar"" => $""a -o+ -y \""{archivePath}\"" {string.Join("" "", files.Select(f => $""\""{f}\""""))}"",
                _ => throw new NotSupportedException(""Unsupported format"")
            };

            string exe = format.ToLower() switch
            {
                "".zip"" => ""powershell"",
                "".7z""  => SevenZipPath,
                "".rar"" => RarPath,
                _ => null
            };

            if (exe == null) return false;

            var psi = new ProcessStartInfo(exe, format==""".zip""? $""-Command Compress-Archive -Path {string.Join("",", files.Select(f=> $"""{f}"""))} -DestinationPath \""{archivePath}\"" -Force"" : args)
            {
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var proc = Process.Start(psi);
            await proc.WaitForExitAsync();
            return File.Exists(archivePath);
        }
    }
}
