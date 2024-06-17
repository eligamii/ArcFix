# ArcFix - [Direct download](https://github.com/eligamii/ArcFix/releases/download/v2/ArcFix.exe)

Very simple and lightweight program to fix Arc for Windows not being able to start on certain devices more than one time.

## How to use

Open the ArcFix.exe (it will fix the issue until the session is closed)<br/><br/>
If you want this program to launch automatically at startup instead of having to open it at every user session, open the Command Prompt at the ArcFix location (type `cmd` in the File Explorer address bar), then run the command __`ArcFix.exe startup on`__ on it . It will automatically copy ArcFix to another location (`%AppData%\ArcFix`)<br/><br/>
To kill the ArcFix.exe process, run `taskkill /f /im ArcFix.exe` in cmd

## What it does

This program will simply automatically delete the `%LocalAppData%\Packages\TheBrowserCompany.Arc_ttt1ap7aakyb4\LocalCache\Local\firestore\Arc\` folder when you close Arc. 

_This program is based on [this](https://www.reddit.com/r/ArcBrowser/comments/1ak6e59/comment/kqkmv78) fix found by jithu2004 on Reddit_  


