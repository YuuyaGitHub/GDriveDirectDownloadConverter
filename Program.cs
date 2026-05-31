using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

// Parsing command-line arguments
string? inputUrl = null;
bool shouldCopy = false;

var argsQueue = new Queue<string>(args);
while (argsQueue.Count > 0)
{
    var arg = argsQueue.Dequeue();
    switch (arg)
    {
        case "-u":
        case "--url":
            if (argsQueue.Count > 0) inputUrl = argsQueue.Dequeue();
            break;
        case "-c":
        case "--copy":
            shouldCopy = true;
            break;
    }
}

// If no arguments are specified, the user will be prompted for input
if (string.IsNullOrWhiteSpace(inputUrl))
{
    Console.Write("Please enter the Google Drive sharing URL: ");
    inputUrl = Console.ReadLine();
}

if (string.IsNullOrWhiteSpace(inputUrl))
{
    Console.WriteLine("Error: The URL has not been entered.");
    return;
}

// URL replacement process (extracting and replacing file_id using regular expressions)
// Correspondence patterns:
//   .../file/d/[FILE_ID]/view...
//   .../file/d/[FILE_ID]/edit...
string downloadUrl = inputUrl;
string pattern = @"/file/d/([a-zA-Z0-9_-]+)";
Match match = Regex.Match(inputUrl, pattern);

if (match.Success)
{
    string fileId = match.Groups[1].Value;
    // Replace "file/d/[id]" with "uc?export=download&id=[id]" and remove everything after "/view...".
    // The conversion will be performed while preserving the base URL (https://drive.google.com/).
    string baseUrl = inputUrl.Split(new[] { "/file/d/" }, StringSplitOptions.None)[0];
    downloadUrl = $"{baseUrl}/uc?export=download&id={fileId}";
}
else
{
    Console.WriteLine("Warning: This is not a standard Google Drive sharing URL format. It will be processed without conversion.");
}

// Show direct download link
Console.WriteLine("\nResult: " + downloadUrl +"\n");

// Copy to clipboard
if (shouldCopy)
{
    try
    {
        CopyToClipboard(downloadUrl);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Copying to the clipboard failed: {ex.Message}");
    }
}

// OS-independent clipboard copy helper method
static void CopyToClipboard(string text)
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        // For Windows, use PowerShell
        var escapedArgs = text.Replace("\"", "\\\"");
        Process.Start(new ProcessStartInfo
        {
            FileName = "powershell",
            Arguments = $"-NoProfile -Command \"Set-Clipboard -Value '{escapedArgs}'\"",
            CreateNoWindow = true,
            UseShellExecute = false
        })?.WaitForExit();
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
        // For macOS, use pbcopy
        using var process = Process.Start(new ProcessStartInfo
        {
            FileName = "pbcopy",
            RedirectStandardInput = true,
            UseShellExecute = false
        });
        process?.StandardInput.Write(text);
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        // For Linux, use xclip (It must be installed on your system)
        using var process = Process.Start(new ProcessStartInfo
        {
            FileName = "xclip",
            Arguments = "-selection clipboard",
            RedirectStandardInput = true,
            UseShellExecute = false
        });
        process?.StandardInput.Write(text);
    }
}