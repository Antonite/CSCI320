Anton Pashyk
Tomas Bravo
Kevin Dombrosky

# CSCI320
Group Project using Riot Games API to create a web application using a SQL back end

# Installation
Our application does not have its own server; therefore, one must be provided. For all testing, workbench pointing to localhost has been used.
1)Execute the necessary database creation scripts
    - create_db_and_tables.sql, sample_player.sql, sample_player_champ.sql
Other scripts can be used to populate the table more completely, but are not required currently
2)Load the website though visual studio
3)Set the server configurations in web.config
4)Run main.aspx
5)Search for a summoner by summoner_name
    - We have added about 10’000 rows with summoner data, which can be located in the “player” table. Example tested summoner_names (case insensitive): Prideful, who does that, Lemos, Perdo, c4lm
6)The web page should display a table with all champions the player has played with or against, as well as information regarding won/total games played.

# Potential issues
Server might fail to start due to MySQL assembly missing/version mismatch. In that case:
    - Remove the assembly reference to Mysql.Data in web.config
    - Manually add it by right clicking on web project (LoLMatchup) -> add reference -> search for MySQL.data -> checkmark it and click ok
    - Verify the version referenced in config matches that installed on local machine

Server fail due to various MySQL.web namespace/assembly issues
    - If this issue persists for some reason, resolve by following same steps as MySQL.data.

Web page opens on root directory
    - Simply add “/main.aspx” to url path

No summoner found message after populating the database
    - Make sure to search for “summoner_name” not “summoner_id”
    - Spaces and capitalization should not be the issue
