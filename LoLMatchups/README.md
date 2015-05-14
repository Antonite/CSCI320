Anton Pashyk
Tomas Bravo
Kevin Dombrosky

# CSCI320
Group Project using Riot Games API to create a web application using a SQL back end

# Installation
Our application does not have its own server; therefore, one must be provided. For all testing, workbench pointing to localhost has been used.
1)Execute the necessary database creation scripts
    -- create_db_and_tables.sql, champs_insert.sql, item_insert.sql, mastery_insert.sql, rune_insert.sql
	No need to run the sample scripts unless desired. They provide additional data that can be used to view matchups without the need for API.

2)Load the website though visual studio
3)Set the server configurations in web.config
4)Run splash.aspx
5)Search for a summoner by summoner_name
	"Sir Geoffers", "H4R1ND3R", "Dyrus", "Kowz Rule"  - these are active players that will most likely have data
6)View database champion_matchup table to find matchups of the respective player, query the champion table for the champions using champion_id and use the names for the matchups
	This step is clearly only there for testing and grading since real users can use the application by simply looking at the champions in game.

Good example to search for:
summoner - turtle
Leblanc vs Zed


Current bug we did not have time to fix: rune win % is above 100 in some cases