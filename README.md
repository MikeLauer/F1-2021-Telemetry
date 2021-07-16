# Leaderboard for F1 2021
Leaderboard with additional information based on the telemetry information provided by the game. Great to use as a commentator or driver on the second screen.

Most data is provided by the game. Some, however, are calculated by the program itself:
- The "Estimated position after pitstop" in the driver circle. It is tenth a second accurate but it's precision heavily depends on the pitstop delta that can be set.
- The gaps between the cars. If a driver hasn't passed a measure point yet, no gap time will be shown.

Please note:
The UI is designed for 1080p screens. Higher resolutions are no problem and the driver circle adjusts it's size respectively. Lower resolutions will not work properly. It is currently designed for 20 players, so multiplayer sessions with 22 drivers or the my team mode will probably cause problems!

### Screenshot Qualifying
![Screenshot Qualifying](https://github.com/MikeLauer/F1-2021-Telemetry/blob/master/Screenshots/screenshot.JPG)

### Features
- Leaderboard table
  - Team colour
  - Position
  - Driver name
  - Current Lap
  - Best lap time
  - Delta to best lap time
  - Last lap time
  - Delta to last lap time of driver in front
  - Gap to driver in front
  - Gap to leader
  - Pit stop count / DNF / DSQ
  - Status (OutLap / InLap / FlyingLap)
  - Best Sector times (1,2,3)
  - Last Sector times (1,2,3)
  - Tyre and the age in laps
  - Sum of penalties in seconds
- Marshal zones (green/yellow/...)
- Safety car status (VSC/SC/None)
- Time left in current session
- Session info
  - Current Track
  - Air temperature
  - Track temperature
  - Theoredical best lap based on all best sectors
- Gaps: Again show gap to front/back/leader (useful when driving to quickly see gap information)
- Pitstop
  - Ideal lap for pit stop (current strategy)
  - Latest lap for pit stop (current strategy)
  - Position after pit stop (this is from game, not computed by the program)
- Weather forecast showing weather information for now, 5min, 10min, 15min, 30min
- Driver circle
  - Shows every driver on the track. Number is the position in the leaderboard
  - Pit stop line is estimated position after pit stop (precision heavily depends on the pit stop delta that can be set)

### Settings
- Columns can be hidden/shown individualy depending on your preference.
- Live timing shows live sector times of each driver.
- Human driver highlighting colours the players name and some other cells in the (bit darker) team colour.
- Performance mode disabled colours in the table and therby reduces CPU load by a few percent (not really necessary)


### Enable UDP output
To make this program work, you need to enable UDP output in the telemetry options of the game. Chosen 127.0.0.1 (default) for IP and 20777 (default) for port.


### Credits
Deserializer for F1 2020 UDP Data from Tim Hanewich: https://github.com/TimHanewich/Codemasters.F1_2020


### Code Quality Disclaimer
Yes, it's not a pretty baby I have there. It was a short time project and I will improve code quality over time.
I'm open for feedback and suggestions.
