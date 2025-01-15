# CursorKeeper

CursorKeeper is a lightweight Windows system tray application that constrains your mouse cursor to the primary screen. It's perfect for multi-monitor setups where you want to prevent accidental cursor movement to secondary displays.

![CursorKeeper Tray Icon](screenshots/tray-icon.png)

## Features

- Keeps your cursor within the primary screen boundaries
- Lives quietly in your system tray
- Minimal resource usage using Windows hooks
- Easy to enable/disable with a single click
- Smooth cursor movement with edge padding
- Starts automatically in active mode

## Installation

1. Download the latest release from the [Releases](https://github.com/yourusername/cursorkeeper/releases) page
2. Extract the zip file to your desired location
3. Run `CursorKeeper.exe`
4. (Optional) Add to startup programs for automatic launch

### Building from Source

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/cursorkeeper.git
   ```

2. Open the solution in Visual Studio 2019 or later
3. Build the solution (Ctrl+Shift+B)
4. Find the executable in the `bin/Release` directory

## Usage

- Left or right-click the tray icon to open the menu
- Select "Disable CursorKeeper" to temporarily allow cursor movement to other screens
- Select "Enable CursorKeeper" to restore the cursor constraint
- Select "Exit" to close the application

## System Requirements

- Windows 7 or later
- .NET Framework 4.7.2 or later
- Administrator privileges (required for mouse hook functionality)

## Technical Details

CursorKeeper uses Windows Low Level Mouse Hook (WH_MOUSE_LL) to intercept cursor movement before it happens, ensuring smooth and reliable cursor containment. The application maintains a 2-pixel padding from screen edges for comfortable usage.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see below for details:

```
MIT License

Copyright (c) 2025 [Your Name]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

## Acknowledgments

- Thanks to the Windows API for providing low-level mouse hook functionality
- Inspired by the need for better multi-monitor cursor management

## Support

If you encounter any issues or have feature suggestions, please:
1. Check the [Issues](https://github.com/yourusername/cursorkeeper/issues) page
2. Create a new issue if your problem isn't already listed
