# Leaderboard for F1 2021
Leaderboard with additional information based on the telemetry information provided by the game. Great to use as a commentator or driver on the second screen.

### Screenshot Qualifying
![Screenshot Qualifying](https://github.com/MikeLauer/F1-2021-Telemetry/blob/master/Screenshots/screenshot_20210827.JPG)

![Screenshot Settings](https://github.com/MikeLauer/F1-2021-Telemetry/blob/master/Screenshots/screenshot_settings_20210827.JPG)

### Features
- Leaderboard table
  - Team colour
  - Position
  - Driver name
  - Current Lap
  - Best lap time
  - Delta to best lap time
  - Delta to best lap time of driver in front
  - Last lap time
  - Delta to last lap time of driver in front
  - Gap to driver in front
  - Gap to leader
  - Pit stop count / DNF / DSQ
  - Status (OutLap / InLap / FlyingLap / ...)
  - Best Sector times (1,2,3)
  - Last Sector times (1,2,3)
  - Tyre and the age in laps
  - Position difference from start grid (e.g. +3)
  - Sum of penalties in seconds
- Time left in current session
- Marshal zones (green/yellow/blue/...)
- Safety car status (VSC/SC/None)
- Warning if driver behind (400m) is on hotlap
- Session info
  - Session type (Race / Qualifying / ...)
  - Current Track
  - Air temperature
  - Track temperature
  - Theoredical best lap based on all best sectors
  - Front wing damage (left / right) in percent because the different types of green in the game are not very informative
- Gaps (for a quick overview compared to the infomation in the table)
  - To leader
  - To driver in front
  - To driver behind
- Pitstop
  - Ideal lap for pit stop (current strategy)
  - Latest lap for pit stop (current strategy)
  - Position after pit stop (this is from game, not computed by the program)
- Weather forecast showing weather information for now, 5min, 10min, 15min, 30min
- Graph showing the last 10/15 lap times of your self, driver ahead and behind
- Driver circle
  - Shows every driver on the track. Number is the position in the leaderboard
  - Pit stop line is estimated position after pit stop (precision heavily depends on the pit stop delta that can be set manually)

### Settings
- Resources
  - Performance mode: Disables colours in the leaderboard and therby decreases CPU load
  - Update frequency of the tablelll
- Columns can be hidden/shown individualy depending on your preference.
- Information
  - Live timing shows live sector times of each driver.
  - Weather can be hidden in case a bigger graph is prefered
- Human driver highlighting colours the players name and some other cells in the (bit darker) team colour for quick spotting in the table
  - Can be set to off, only yourself, or every human player

### Notes
Most data is provided by the game. Some, however, are calculated by the program itself:
- The "Estimated position after pitstop" in the driver circle. It is tenth a second accurate but it's precision heavily depends on the pitstop delta that can be set.
- The gaps between the cars. If a driver hasn't passed a measure point yet, no gap time will be shown.

Please be also aware of:
The UI is designed for 1080p screens. Higher resolutions are no problem and the driver circle and the graph adjust their size respectively. Lower resolutions will not work properly. It is currently designed for 20 players, so multiplayer sessions with 22 drivers or the *My Team* mode will probably not work correctly!

### Enable UDP output
To make this program work, you need to enable UDP output in the telemetry options of the game. Choose 127.0.0.1 (default) for IP and 20777 (default) for port.

### Credits
Deserializer for F1 2020 UDP Data from [Tim Hanewich](https://github.com/TimHanewich/Codemasters.F1_2020)
