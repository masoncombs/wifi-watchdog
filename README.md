# Wi-Fi Watchdog

Wi-Fi Watchdog is a modern, robust C# WinForms tray application for automatic Wi-Fi monitoring, reconnection, and reliability on Windows. Designed for both end-users and IT professionals, it ensures your system stays connected to your preferred wireless network and provides advanced configuration, logging, and startup options—all with a polished, user-friendly interface.

## Features

- **System Tray Integration**
  - Runs silently in the Windows system tray with a modern, dark-mode-aware context menu.
  - Custom icon and notifications for status, errors, and actions.

- **Automatic Wi-Fi Monitoring & Reconnect**
  - Monitors your Wi-Fi connection in real time.
  - Automatically attempts to reconnect to your chosen SSID if disconnected.
  - Supports custom Wi-Fi password entry and live SSID refresh.

- **Ping & Connectivity Checks**
  - Configurable ping targets (gateway and internet) to verify true connectivity.
  - User-tunable timing, retry, and fail count settings for robust detection.

- **Startup & Privilege Management**
  - Flexible auto-run options: None, Registry (user), or Scheduled Task (admin, no UAC prompt).
  - Launcher handles privilege escalation and ensures the main app always runs with the required rights.
  - Automated desktop shortcut creation and one-click uninstall (removes all settings, registry, and tasks).

- **Persistent User Settings**
  - All settings (SSID, password, dark mode, notifications, timing, ping targets, startup options) are saved and applied instantly.
  - Settings sync between the launcher and main app.

- **Modern UI & Dark Mode**
  - VS Code-style dark mode throughout all forms, dialogs, and tray menus.
  - Custom borderless title bars and consistent theming.
  - Dark mode is enabled by default on first run if the system is in dark mode.

- **Notifications & Sounds**
  - Customizable notifications for reconnect, success, and failure events.
  - Optional popup sound when the launcher runs at login.

- **Debug Logging**
  - Detailed debug log of all key events, with 24-hour retention.
  - View, export, or clear logs from the tray menu.
  - Auto-refresh and auto-scroll features for live monitoring.

- **Uninstall & Cleanup**
  - One-click uninstall from the tray menu, with confirmation.
  - Cleans up all settings, registry entries, scheduled tasks, and desktop shortcuts.

- **Reliability & Professionalism**
  - All privilege and startup logic is robust and user-friendly.
  - Designed for reliability, security, and ease of use.

## Getting Started

### Option 1: Use the Prebuilt Launcher and Main App (Recommended)

**No build required!**

1. **Download the latest release**
   - Go to the [Releases](https://github.com/masoncombs/wifi-watchdog/releases) page.
   - Download the provided ZIP file containing `WifiWatchdogLauncher.exe` and `WifiWatchdogApp.exe`.
   - Extract both files to the same folder (e.g., your Desktop or `C:\Program Files\WiFiWatchdog`).

2. **Run the Launcher**
   - Double-click `WifiWatchdogLauncher.exe` to start the app.
   - On first run, you’ll be prompted to enable auto-run and select your preferred startup method (Registry or Scheduled Task).
   - The launcher will handle all privilege elevation and setup for you.

3. **Access the Tray Menu**
   - Right-click the tray icon to open the context menu.
   - From here, you can access Settings, Debug Log, and Uninstall options.

4. **Configure Your Preferences**
   - Set your Wi-Fi SSID, password, notification preferences, timing, ping targets, and dark mode.
   - All changes are saved instantly and applied live.

5. **Uninstalling**
   - Use the tray menu’s Uninstall option for a complete cleanup (removes all settings, registry entries, scheduled tasks, and shortcuts).

---

### Option 2: Build from Source

1. **Clone the Repository**
   ```sh
   git clone https://github.com/masoncombs/wifi-watchdog.git
   cd wifi-watchdog
   ```

2. **Build the Solution**
   - Open the solution in Visual Studio and build, or run:
     ```sh
     dotnet build
     ```

3. **Run the Launcher**
   - Navigate to the output folder (e.g., `bin/Debug/net6.0-windows/` or `BuildOutput/Debug/net6.0-windows/`).
   - Run `WifiWatchdogLauncher.exe` (not the main app directly).

4. **Follow the same steps as above to configure and use the app.**

---

## Requirements
- Windows 10/11
- .NET 6.0 or later
- Administrator rights for scheduled task auto-run and Wi-Fi control

## License
This project is open source and available under the MIT License.

---

For more information, feature requests, or contributions, please visit the [GitHub repository](https://github.com/masoncombs/wifi-watchdog).
