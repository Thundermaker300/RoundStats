# RoundStats
RoundStats is an SCP:SL Exiled plugin that shows various stats at the conclusion of a round. All stats can be turned on and off in the config, and can be set to display three at a time, one after another (example: round ends, three stats show, 5 seconds pass, another 3 stats show).

## Requirements
* Exiled: V2.8.0
* SCP:SL Server: V10.2.2

## Current Stats (as of V0.1.0)
* FirstEscape - Displays the name and role color of the first player that escapes.
* FirstKill - Displays the name, role, and role color of the player that makes the first kill.
* TotalDeaths - Displays the total amount of players that've died.
* TotalGrenadesThrown - Displays the total amount of grenades (SCP-018 not included) during the round.
* TotalMedicalItems - Displays the total amount of medical items (Medkit, Adrenaline, and SCP-500) used during the round.
* Total914Upgrades - Displays the amount of players and items that have been refined in SCP-914.
* TotalEscapes - Displays the total amount of escapes.
* TotalDoorsInteracted - Displays the total amount of door interactions (including opening and closing) during the round. Failures to open a door (locked, or no keycard on a keycard door) not included.