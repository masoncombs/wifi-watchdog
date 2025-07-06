using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WifiWatchdogApp
{
    public static class WifiUtils
    {
        public static string[] GetAvailableSSIDs()
        {
            var ssids = new List<string>();
            try
            {
                var psi = new ProcessStartInfo("netsh", "wlan show networks mode=Bssid")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (var proc = Process.Start(psi))
                {
                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();
                    var matches = Regex.Matches(output, @"SSID [0-9]+ : (.+)");
                    foreach (Match match in matches)
                    {
                        string ssid = match.Groups[1].Value.Trim();
                        if (!string.IsNullOrEmpty(ssid) && !ssids.Contains(ssid))
                            ssids.Add(ssid);
                    }
                }
            }
            catch { }
            return ssids.ToArray();
        }
    }
}
