# Wi-Fi Watchdog Tray App

This is a C# WinForms application that runs in the system tray, monitors your Wi-Fi connection every 10 seconds, and attempts to reconnect to a specified SSID if disconnected. It shows notifications, plays a sound on pass/fail, and requests admin rights if needed. Use the tray icon's context menu to exit the app.

## Features
- System tray icon with context menu
- Monitors Wi-Fi connection to SSID: goldenwifi
- Attempts to reconnect by disabling/enabling the adapter and reconnecting
- Shows notifications for status changes
- Plays a beep sound on pass/fail
- Requests admin rights for Wi-Fi control

## How to Build
1. Open the solution in Visual Studio.
2. Build the solution (Ctrl+Shift+B).
3. Run the app (F5 or double-click the EXE in the output folder).

## How to Use
- The app will minimize to the tray and monitor your Wi-Fi.
- Double-click the tray icon to show status.
- Right-click the tray icon to exit.

## Requirements
- Windows 10/11
- .NET 6.0 or later
- Admin rights for Wi-Fi control

## Customization
- Change the `ssid` and `adapter` variables in `TrayAppContext.cs` to match your network.
- Replace the tray icon by setting the `Icon` property in `TrayAppContext`.

<!-- TEMP: test-branch visibility marker -->
