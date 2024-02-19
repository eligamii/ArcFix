# ArcFix - [Direct download](https://github.com/eligamii/ArcFix/releases/download/v1/ArcFix.exe)
Very simple and lightweight program to fix Arc for Windows not being able to launch on certain devices.

## How to use
Open the ArcFix.exe (it will fix the issue until the session is closed)<br/>
If you want this program to launch automatically at startup instead at startup, copy the ArcFix.exe file you downloaded to the Startup Programs folder (enter `shell:startup` in the Run or File Explorer search bar to open the folder)
To kill the ArcFix.exe process, run `taskkill /f /im ArcFix.exe` in cmd
## What it does
This program will simply automatically delete all the files in the `%LocalAppData%\Packages\TheBrowserCompany.Arc_ttt1ap7aakyb4\LocalCache\Local\firestore\Arc\bcny-arc-server\main` folder when you close Arc. 

_This program is based on [this](https://www.reddit.com/r/ArcBrowser/comments/1ak6e59/comment/kqkmv78) fix found by jithu2004 on Reddit_
