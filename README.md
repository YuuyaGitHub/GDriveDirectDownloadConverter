# GDriveDirectDownloadConverter
This tool converts Google Drive shared URLs directly into download links.  
It's a console application; there's no GUI.

## System Requirements
* Windows, macOS, Linux
  * Binaries available for download from [Releases](https://github.com/YuuyaGitHub/GDriveDirectDownloadConverter/releases) are for Windows only. Compilation is required for other operating systems.
* .NET 8 or later

## Arguments
This program can accept the following arguments.
|   Argument name  | Description                               |
| ---------------- | ----------------------------------------- |
| `-u`<br>`--url`  | Specify the Google Drive sharing URL.     |
| `-c`<br>`--copy` | Copy the generated URL to the clipboard.  |

All arguments are optional.

## How to Use
1. Specify the URL using arguments or interactively.
2. The results will be displayed.
### Example
```bash
dotnet run -- -u "https://drive.google.com/file/d/1234567890abcdefg/view?usp=sharing" -c
```
```bash
GDriveDirectDownloadConverter.exe -u "https://drive.google.com/file/d/1IxFMcX0Z0Egi1BXuGCYWlfXEt2oNqZxd/view?usp=drive_link" -c
```

## License
MIT License
