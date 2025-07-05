using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WifiWatchdogApp
{
    public class DebugLog
    {
        private readonly string logFilePath;
        private readonly object lockObj = new object();
        private readonly TimeSpan retention = TimeSpan.FromHours(24);

        public DebugLog(string filePath)
        {
            logFilePath = filePath;
            PruneOldEntries();
        }

        public void Log(string message)
        {
            var entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            lock (lockObj)
            {
                File.AppendAllLines(logFilePath, new[] { entry });
            }
        }

        public List<string> GetEntries()
        {
            lock (lockObj)
            {
                if (!File.Exists(logFilePath)) return new List<string>();
                var lines = File.ReadAllLines(logFilePath).ToList();
                return PruneOldEntries(lines);
            }
        }

        public void Clear()
        {
            lock (lockObj)
            {
                File.WriteAllText(logFilePath, "");
            }
        }

        public void Export(string exportPath)
        {
            lock (lockObj)
            {
                File.Copy(logFilePath, exportPath, true);
            }
        }

        private void PruneOldEntries()
        {
            if (!File.Exists(logFilePath)) return;
            var lines = File.ReadAllLines(logFilePath).ToList();
            var pruned = PruneOldEntries(lines);
            if (pruned.Count != lines.Count)
                File.WriteAllLines(logFilePath, pruned);
        }

        private List<string> PruneOldEntries(List<string> lines)
        {
            var cutoff = DateTime.Now - retention;
            return lines.Where(line =>
            {
                if (line.Length < 21) return false;
                if (DateTime.TryParseExact(line.Substring(1, 19), "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out var dt))
                    return dt >= cutoff;
                return true;
            }).ToList();
        }
    }
}
