using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI.HtmlControls;

public partial class _Default : System.Web.UI.Page
{
    //todo case sensitivity
    private MySqlConnection connection;
    private static String summonerId;

    protected void Page_Load(object sender, EventArgs e)
    {
        summoner_id.Text = "";
        status.Text = "";
        status.Style.Add("display", "none");
        summoner_id.Style.Add("display", "none");
        runeTable.Style.Add("display", "none");

        champ_button.Attributes.Add("disabled", "disabled");
        matchup_panel.Style.Add("display", "none");

        //itemImg1.Style.Add("display", "none");
        matchupStats.Text = "";
        matchupPercent.Text = "";
        //matchupTopRunes.Text = "";
        //matchupTopMasteries.Text = "";
        ChampAsName.Text = "";
        ChampVsName.Text = "";
        winPercentAsLeft.Text = "";
        winPercentVsLeft.Text = "";
        winPercentAsRight.Text = "";
        winPercentVsRight.Text = "";
        matchupTopItemsPercent.Text = "";
        matchupTopRunesPercent.Text = "";
        matchupTopMasteriesPercent.Text = "";
        connection = connectToServer();
    }

    protected String parseRank (String rank)
    {
        String result = "";

        char div = rank[0];
        char num = rank[1];

        switch (div)
        {
            case 'B': result += "Bronze ";
                break;
            case 'S': result += "Silver ";
                break;
            case 'G': result += "Gold ";
                break;
            case 'P': result += "Platinum ";
                break;
            case 'D': result += "Diamond ";
                break;
            case 'M': result += "Master ";
                break;
            case 'C': result += "Challenger ";
                break;
            default: result = "Badly formatted rank";
                break;
        }

        switch (num)
        {
            case '1': result += "I";
                break;
            case '2': result += "II";
                break;
            case '3': result += "III";
                break;
            case '4': result += "IV";
                break;
            case '5': result += "V";
                break;
            default: result = "Badly formatted rank";
                break;
        }

        return result;
    }

    protected void submit(object sender, EventArgs e)
    {   
        MySqlDataReader reader;
        //search for summoner, Dynamic SQL
        MySqlCommand cmd = new MySqlCommand();
        cmd.CommandText = "SELECT player_champion_stat.champion_id," +
            "player_champion_stat.played_as_won," +
            "player_champion_stat.played_as_total," +
            "player_champion_stat.played_against_won," +
            "player_champion_stat.played_against_total," +
            "player_champion_stat.summoner_id," +
            "player.rank," +
            "player.level," +
            "player.last_update_match_id " +
            "FROM lolmatchups.player_champion_stat," +
                "lolmatchups.player where lolmatchups.player_champion_stat.summoner_id =" +
                " lolmatchups.player.summoner_id and lolmatchups.player.name = @name";
        cmd.Parameters.Add("@name", MySqlDbType.VarChar, 11);
        cmd.Parameters["@name"].Value = summoner_box.Text;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;

        //open connection
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }

        //execute
        try
        {
            reader = cmd.ExecuteReader();
            reader.Read();

            sum_name.InnerText = summoner_box.Text;
            sum_rank.InnerText = parseRank (reader.GetString(6));

            summonerId = reader.GetString(5);
            summoner_id.Text = "";
            summoner_id.Style.Add("display", "none");
        }
        catch (Exception notFound)
        {
            summoner_id.Text = "Summoner " + summoner_box.Text + " not found";
            summoner_id.Style.Add("display", "inline");
            champ_button.Attributes.Add("disabled", "disabled");

            connection.Close();
            return;
        }

        //close connection
        connection.Close();

        champ_button.Attributes.Remove("disabled");
    }

    protected void getMatchups (object sender, EventArgs e)
    {
        string championAsName;
        string championVsName;
        string offenceMastery;
        string defenceMastery;
        string utilityMastery;

        MySqlDataReader reader;
        //search for summoner, Dynamic SQL
        MySqlCommand cmd = new MySqlCommand();
        cmd.CommandText =
            "SELECT player.name," +
            "opponent.name," +
            "player_matchup.won," +
            "player_matchup.played," +
            "items.item_id1," +
            "items.item_id2," +
            "items.item_id3," +
            "items.item_id4," +
            "items.item_id5," +
            "items.item_id6," +
            "items.item_id7," + //10
            "items.won," +
            "items.used," +
            "masteries.offense_values," +
            "masteries.defense_values," +
            "masteries.utility_values," +
            "masteries.won," +
            "masteries.used," + //17
            "runes.rune_id1," +
            "runes.rune_id2," +
            "runes.rune_id3," +
            "runes.rune_id4," +
            "runes.rune_id5," +
            "runes.rune_id6," +
            "runes.rune_id7," +
            "runes.rune_id8," +
            "runes.rune_id9," +
            "runes.rune_id10," +
            "runes.rune_id11," +
            "runes.rune_id12," +
            "runes.rune_id13," +
            "runes.rune_id14," +
            "runes.rune_id15," +
            "runes.rune_id16," +
            "runes.rune_id17," +
            "runes.rune_id18," +
            "runes.rune_id19," +
            "runes.rune_id20," +
            "runes.rune_id21," +
            "runes.rune_id22," +
            "runes.rune_id23," +
            "runes.rune_id24," +
            "runes.rune_id25," +
            "runes.rune_id26," +
            "runes.rune_id27," +
            "runes.rune_id28," +
            "runes.rune_id29," +
            "runes.rune_id30," + //47
            "runes.won," +
            "runes.used" +
            " FROM lolmatchups.player_matchup " +
                "join lolmatchups.champion player on player_matchup.player_champion_id = player.champion_id " +
                "join lolmatchups.champion opponent on player_matchup.opponent_champion_id = opponent.champion_id " +
                "join lolmatchups.player_items items on player_matchup.matchup_id = items.matchup_id " +
                "join lolmatchups.best_matchup_items bestItems on player_matchup.matchup_id = bestItems.matchup_id " +
                "join lolmatchups.player_mastery masteries on player_matchup.matchup_id = masteries.matchup_id " +
                "join lolmatchups.best_matchup_masteries bestMasteries on player_matchup.matchup_id = bestMasteries.matchup_id " +
                "join lolmatchups.player_rune_set runes on player_matchup.matchup_id = runes.matchup_id " +
                "join lolmatchups.best_matchup_rune_set bestRunes on player_matchup.matchup_id = bestRunes.matchup_id " +
                " where player.name = @as" +
                " and opponent.name = @vs" +
                " and lolmatchups.player_matchup.summoner_id = @id";
        cmd.Parameters.Add("@as", MySqlDbType.VarChar, 32);
        cmd.Parameters["@as"].Value = champ_as.Text;
        cmd.Parameters.Add("@vs", MySqlDbType.VarChar, 32);
        cmd.Parameters["@vs"].Value = champ_vs.Text;
        cmd.Parameters.Add("@id", MySqlDbType.VarChar, 8);
        cmd.Parameters["@id"].Value = summonerId;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;


        //open connection
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }

        //execute
        try
        {
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {

                reader.Read();
                //calculate win % and diplay
                float totalWinPercent = reader.GetFloat(2) / reader.GetFloat(3) * 100;
                float itemsWinPercent = reader.GetFloat(11) / reader.GetFloat(12) * 100;
                float runesWinPercent = reader.GetFloat(48) / reader.GetFloat(49) * 100;
                float masteriesWinPercent = reader.GetFloat(16) / reader.GetFloat(17) * 100;

                matchupPercent.Text = totalWinPercent + "%";
                matchupTopItemsPercent.Text = itemsWinPercent + "%";
                matchupTopRunesPercent.Text = runesWinPercent + "%";
                matchupTopMasteriesPercent.Text = masteriesWinPercent + "%";

                championAsName = reader.GetString(0);
                championVsName = reader.GetString(1);

                //fill in data for labels
                ChampAsName.Text = championAsName;
                ChampVsName.Text = championVsName;


                //fill champion Images
                championAsPath.Value = "ChampionImages\\" + championAsName + "_Square_0.png";
                championVsPath.Value = "ChampionImages\\" + championVsName + "_Square_0.png";

                //fill item images
                item1Path.Value = "ItemImages\\" + reader.GetString(4) + ".png";
                item2Path.Value = "ItemImages\\" + reader.GetString(5) + ".png";
                item3Path.Value = "ItemImages\\" + reader.GetString(6) + ".png";
                item4Path.Value = "ItemImages\\" + reader.GetString(7) + ".png";
                item5Path.Value = "ItemImages\\" + reader.GetString(8) + ".png";
                item6Path.Value = "ItemImages\\" + reader.GetString(9) + ".png";
                item7Path.Value = "ItemImages\\" + reader.GetString(10) + ".png";
                
                //get masteries
                offenceMastery = reader.GetString(13);
                defenceMastery = reader.GetString(14);
                utilityMastery = reader.GetString(15);
               

                string[] runeList = new string[30];
                for (int i = 18; i < 48; i++)
                {
                    string aRune = reader.GetString(i);
                    if (aRune == null) runeList[i - 18] = "0";
                    else { runeList[i - 18] = aRune; }
                    //matchupTopRunesPercent.Text += runeList[i - 18] + ",";
                }
 

                connection.Close();

                //fill chapion stats
                fillChampStats(championAsName, championVsName);

                //fill masteries
                fillMasteries(offenceMastery, defenceMastery, utilityMastery);

                //fill the runes pages
                processRunes(runeList);

                rune_panel.Attributes.Add("class", "fullwidth panel panel-lol noshadow");
                mastery_panel.Attributes.Add("class", "fullwidth panel panel-lol noshadow");

                matchup_panel.Style.Add("display", "block");
            }
            else
            {
                matchupStats.Text = "No data found";
                connection.Close();
            }

        }
        catch (Exception notFound)
        {
            matchupStats.Text = notFound.ToString();
            connection.Close();
        }
    }


    protected MySqlConnection connectToServer()
    {
        
        //connect to server
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["lolmatchupsDB"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);

        return connection;
    }


    protected void fillMasteries(String offence, String defence, String utility)
    {
        MySqlDataReader reader;
        //search for mastery details
        MySqlCommand cmd = new MySqlCommand();

        //mastery disctionaries
        Dictionary<int, string[]> offenceDict = new Dictionary<int, string[]>();
        Dictionary<int, string[]> defenceDict = new Dictionary<int, string[]>();
        Dictionary<int, string[]> utilityDict = new Dictionary<int, string[]>();
        int totalOffence = 0; ;
        int totalDefence = 0; ;
        int totalUtility = 0; ;

        cmd = new MySqlCommand();
        cmd.CommandText = "select * from lolmatchups.mastery ORDER BY tree, `index`";
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;

        //parse offence masteries
        String parseOffence = offence.Substring(1, offence.Length - 2);
        String parseDefence = defence.Substring(1, defence.Length - 2);
        String parseUtility = utility.Substring(1, utility.Length - 2);
        string[] splitOffence = parseOffence.Split(new char[] { ',' });
        for (int i = 0; i < splitOffence.Length; i++)
        {
            splitOffence[i] = splitOffence[i].TrimStart();
            totalOffence += int.Parse(splitOffence[i]);
        }
        string[] splitDefence = parseDefence.Split(new char[] { ',' });
        for (int i = 0; i < splitDefence.Length; i++)
        {
            splitDefence[i] = splitDefence[i].TrimStart();
            totalDefence += int.Parse(splitDefence[i]);
        }
        string[] splitUtility = parseUtility.Split(new char[] { ',' });
        for (int i = 0; i < splitUtility.Length; i++)
        {
            splitUtility[i] = splitUtility[i].TrimStart();
            totalUtility += int.Parse(splitUtility[i]);
        }


        //open connection
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }

        //execute
        try
        {
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //get defence
                for (int i = 0; i < 19;i++ )
                {
                    reader.Read();
                    string[] masteryRanks = new string[6];

                    int totalRanks = 1;
                    masteryRanks[1] = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                    {
                        masteryRanks[2] = reader.GetString(3);
                        totalRanks++;
                    }
                    else { masteryRanks[2] = "This rune level does not exist"; }
                    if (!reader.IsDBNull(4))
                    {
                        masteryRanks[3] = reader.GetString(4);
                        totalRanks++;
                    }
                    else { masteryRanks[3] = "This rune level does not exist"; }
                    if (!reader.IsDBNull(5))
                    {
                        masteryRanks[4] = reader.GetString(5);
                        totalRanks++;
                    }
                    else { masteryRanks[4] = "This rune level does not exist"; }
                    masteryRanks[5] = totalRanks + "";
                    masteryRanks[0] = reader.GetString(1);
                    defenceDict.Add(i, masteryRanks);
                }
                //get offence
                for (int i = 0; i < 20; i++)
                {
                    reader.Read();
                    string[] masteryRanks = new string[6];

                    int totalRanks = 1;
                    masteryRanks[1] = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                    {
                        masteryRanks[2] = reader.GetString(3);
                        totalRanks++;
                    }
                    else { masteryRanks[2] = "This rune level does not exist"; }
                    if (!reader.IsDBNull(4))
                    {
                        masteryRanks[3] = reader.GetString(4);
                        totalRanks++;
                    }
                    else { masteryRanks[3] = "This rune level does not exist"; }
                    if (!reader.IsDBNull(5))
                    {
                        masteryRanks[4] = reader.GetString(5);
                        totalRanks++;
                    }
                    else { masteryRanks[4] = "This rune level does not exist"; }
                    masteryRanks[5] = totalRanks + "";
                    masteryRanks[0] = reader.GetString(1);
                    offenceDict.Add(i, masteryRanks);
                }
                //get utility
                for (int i = 0; i < 18; i++)
                {
                    reader.Read();
                    string[] masteryRanks = new string[6];

                    int totalRanks = 1;
                    masteryRanks[1] = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                    {
                        masteryRanks[2] = reader.GetString(3);
                        totalRanks++;
                    }
                    else { masteryRanks[2] = "This rune level does not exist"; }
                    if (!reader.IsDBNull(4))
                    {
                        masteryRanks[3] = reader.GetString(4);
                        totalRanks++;
                    }
                    else { masteryRanks[3] = "This rune level does not exist"; }
                    if (!reader.IsDBNull(5))
                    {
                        masteryRanks[4] = reader.GetString(5);
                        totalRanks++;
                    }
                    else { masteryRanks[4] = "This rune level does not exist"; }
                    masteryRanks[5] = totalRanks + "";
                    masteryRanks[0] = reader.GetString(1);
                    utilityDict.Add(i, masteryRanks);
                }


            }
            else
            {
                matchupStats.Text = "No data found";
                connection.Close();
            }
        }
        catch (Exception notFound)
        {
            matchupStats.Text = notFound.ToString();
            connection.Close();
        }
        connection.Close();
        //fill in labels
        //defence
        defence1.Text = splitDefence[0] + "/" + defenceDict[0][5];
        defenceDesc1.Text = defenceDict[0][int.Parse(splitDefence[0])];

        defence2.Text = splitDefence[1] + "/" + defenceDict[1][5];
        defenceDesc2.Text = defenceDict[1][int.Parse(splitDefence[1])];

        defence3.Text = splitDefence[2] + "/" + defenceDict[2][5];
        defenceDesc3.Text = defenceDict[2][int.Parse(splitDefence[2])];

        defence4.Text = splitDefence[3] + "/" + defenceDict[3][5];
        defenceDesc4.Text = defenceDict[3][int.Parse(splitDefence[3])];

        defence5.Text = splitDefence[4] + "/" + defenceDict[4][5];
        defenceDesc5.Text = defenceDict[4][int.Parse(splitDefence[4])];

        defence6.Text = splitDefence[5] + "/" + defenceDict[5][5];
        defenceDesc6.Text = defenceDict[5][int.Parse(splitDefence[5])];

        defence7.Text = splitDefence[6] + "/" + defenceDict[6][5];
        defenceDesc7.Text = defenceDict[6][int.Parse(splitDefence[6])];

        defence8.Text = splitDefence[7] + "/" + defenceDict[7][5];
        defenceDesc8.Text = defenceDict[7][int.Parse(splitDefence[7])];

        defence9.Text = splitDefence[8] + "/" + defenceDict[8][5];
        defenceDesc9.Text = defenceDict[8][int.Parse(splitDefence[8])];

        defence10.Text = splitDefence[9] + "/" + defenceDict[9][5];
        defenceDesc10.Text = defenceDict[9][int.Parse(splitDefence[9])];

        defence11.Text = splitDefence[10] + "/" + defenceDict[10][5];
        defenceDesc11.Text = defenceDict[10][int.Parse(splitDefence[10])];

        defence12.Text = splitDefence[11] + "/" + defenceDict[11][5];
        defenceDesc12.Text = defenceDict[11][int.Parse(splitDefence[11])];

        defence13.Text = splitDefence[12] + "/" + defenceDict[12][5];
        defenceDesc13.Text = defenceDict[12][int.Parse(splitDefence[12])];

        defence14.Text = splitDefence[13] + "/" + defenceDict[13][5];
        defenceDesc14.Text = defenceDict[13][int.Parse(splitDefence[13])];

        defence15.Text = splitDefence[14] + "/" + defenceDict[14][5];
        defenceDesc15.Text = defenceDict[14][int.Parse(splitDefence[14])];

        defence16.Text = splitDefence[15] + "/" + defenceDict[15][5];
        defenceDesc16.Text = defenceDict[15][int.Parse(splitDefence[15])];

        defence17.Text = splitDefence[16] + "/" + defenceDict[16][5];
        defenceDesc17.Text = defenceDict[16][int.Parse(splitDefence[16])];

        defence18.Text = splitDefence[17] + "/" + defenceDict[17][5];
        defenceDesc18.Text = defenceDict[17][int.Parse(splitDefence[17])];

        defence19.Text = splitDefence[18] + "/" + defenceDict[18][5];
        defenceDesc19.Text = defenceDict[18][int.Parse(splitDefence[18])];

        //offence
        offence1.Text = splitOffence[0] + "/" + offenceDict[0][5];
        offenceDesc1.Text = offenceDict[0][int.Parse(splitOffence[0])];

        offence2.Text = splitOffence[1] + "/" + offenceDict[1][5];
        offenceDesc2.Text = offenceDict[1][int.Parse(splitOffence[1])];

        offence3.Text = splitOffence[2] + "/" + offenceDict[2][5];
        offenceDesc3.Text = offenceDict[2][int.Parse(splitOffence[2])];

        offence4.Text = splitOffence[3] + "/" + offenceDict[3][5];
        offenceDesc4.Text = offenceDict[3][int.Parse(splitOffence[3])];

        offence5.Text = splitOffence[4] + "/" + offenceDict[4][5];
        offenceDesc5.Text = offenceDict[4][int.Parse(splitOffence[4])];

        offence6.Text = splitOffence[5] + "/" + offenceDict[5][5];
        offenceDesc6.Text = offenceDict[5][int.Parse(splitOffence[5])];

        offence7.Text = splitOffence[6] + "/" + offenceDict[6][5];
        offenceDesc7.Text = offenceDict[6][int.Parse(splitOffence[6])];

        offence8.Text = splitOffence[7] + "/" + offenceDict[7][5];
        offenceDesc8.Text = offenceDict[7][int.Parse(splitOffence[7])];

        offence9.Text = splitOffence[8] + "/" + offenceDict[8][5];
        offenceDesc9.Text = offenceDict[8][int.Parse(splitOffence[8])];

        offence10.Text = splitOffence[9] + "/" + offenceDict[9][5];
        offenceDesc10.Text = offenceDict[9][int.Parse(splitOffence[9])];

        offence11.Text = splitOffence[10] + "/" + offenceDict[10][5];
        offenceDesc11.Text = offenceDict[10][int.Parse(splitOffence[10])];

        offence12.Text = splitOffence[11] + "/" + offenceDict[11][5];
        offenceDesc12.Text = offenceDict[11][int.Parse(splitOffence[11])];

        offence13.Text = splitOffence[12] + "/" + offenceDict[12][5];
        offenceDesc13.Text = offenceDict[12][int.Parse(splitOffence[12])];

        offence14.Text = splitOffence[13] + "/" + offenceDict[13][5];
        offenceDesc14.Text = offenceDict[13][int.Parse(splitOffence[13])];

        offence15.Text = splitOffence[14] + "/" + offenceDict[14][5];
        offenceDesc15.Text = offenceDict[14][int.Parse(splitOffence[14])];

        offence16.Text = splitOffence[15] + "/" + offenceDict[15][5];
        offenceDesc16.Text = offenceDict[15][int.Parse(splitOffence[15])];

        offence17.Text = splitOffence[16] + "/" + offenceDict[16][5];
        offenceDesc17.Text = offenceDict[16][int.Parse(splitOffence[16])];

        offence18.Text = splitOffence[17] + "/" + offenceDict[17][5];
        offenceDesc18.Text = offenceDict[17][int.Parse(splitOffence[17])];

        offence19.Text = splitOffence[18] + "/" + offenceDict[18][5];
        offenceDesc19.Text = offenceDict[18][int.Parse(splitOffence[18])];

        offence20.Text = splitOffence[19] + "/" + offenceDict[19][5];
        offenceDesc20.Text = offenceDict[19][int.Parse(splitOffence[19])];

        //utility
        utility1.Text = splitUtility[0] + "/" + utilityDict[0][5];
        utilityDesc1.Text = utilityDict[0][int.Parse(splitUtility[0])];

        utility2.Text = splitUtility[1] + "/" + utilityDict[1][5];
        utilityDesc2.Text = utilityDict[1][int.Parse(splitUtility[1])];

        utility3.Text = splitUtility[2] + "/" + utilityDict[2][5];
        utilityDesc3.Text = utilityDict[2][int.Parse(splitUtility[2])];

        utility4.Text = splitUtility[3] + "/" + utilityDict[3][5];
        utilityDesc4.Text = utilityDict[3][int.Parse(splitUtility[3])];

        utility5.Text = splitUtility[4] + "/" + utilityDict[4][5];
        utilityDesc5.Text = utilityDict[4][int.Parse(splitUtility[4])];

        utility6.Text = splitUtility[5] + "/" + utilityDict[5][5];
        utilityDesc6.Text = utilityDict[5][int.Parse(splitUtility[5])];

        utility7.Text = splitUtility[6] + "/" + utilityDict[6][5];
        utilityDesc7.Text = utilityDict[6][int.Parse(splitUtility[6])];

        utility8.Text = splitUtility[7] + "/" + utilityDict[7][5];
        utilityDesc8.Text = utilityDict[7][int.Parse(splitUtility[7])];

        utility9.Text = splitUtility[8] + "/" + utilityDict[8][5];
        utilityDesc9.Text = utilityDict[8][int.Parse(splitUtility[8])];

        utility10.Text = splitUtility[9] + "/" + utilityDict[9][5];
        utilityDesc10.Text = utilityDict[9][int.Parse(splitUtility[9])];

        utility11.Text = splitUtility[10] + "/" + utilityDict[10][5];
        utilityDesc11.Text = utilityDict[10][int.Parse(splitUtility[10])];

        utility12.Text = splitUtility[11] + "/" + utilityDict[11][5];
        utilityDesc12.Text = utilityDict[11][int.Parse(splitUtility[11])];

        utility13.Text = splitUtility[12] + "/" + utilityDict[12][5];
        utilityDesc13.Text = utilityDict[12][int.Parse(splitUtility[12])];

        utility14.Text = splitUtility[13] + "/" + utilityDict[13][5];
        utilityDesc14.Text = utilityDict[13][int.Parse(splitUtility[13])];

        utility15.Text = splitUtility[14] + "/" + utilityDict[14][5];
        utilityDesc15.Text = utilityDict[14][int.Parse(splitUtility[14])];

        utility16.Text = splitUtility[15] + "/" + utilityDict[15][5];
        utilityDesc16.Text = utilityDict[15][int.Parse(splitUtility[15])];

        utility17.Text = splitUtility[16] + "/" + utilityDict[16][5];
        utilityDesc17.Text = utilityDict[16][int.Parse(splitUtility[16])];

        utility18.Text = splitUtility[17] + "/" + utilityDict[17][5];
        utilityDesc18.Text = utilityDict[17][int.Parse(splitUtility[17])];



        sampleMasteriesAggregate.Text = totalOffence + "-" + totalDefence + "-" + totalUtility;
    }


    protected void fillChampStats(String nameAs, String nameVs)
    {
        MySqlDataReader reader;
        //search for summoner, Dynamic SQL
        MySqlCommand cmd = new MySqlCommand();

        //fill percent total % champions
        cmd = new MySqlCommand();
        cmd.CommandText = "select champs.name, " +
                            "stats.played_as_won, " +
                            "stats.played_as_total, " +
                            "stats.played_against_won, " +
                            "stats.played_against_total " +
            "from lolmatchups.player_champion_stat stats " +
            "join lolmatchups.champion champs on stats.champion_id = champs.champion_id " +
            "where stats.summoner_id = @id and champs.name in (@as, @vs)";
        cmd.Parameters.Add("@as", MySqlDbType.VarChar, 32);
        cmd.Parameters["@as"].Value = nameAs;
        cmd.Parameters.Add("@vs", MySqlDbType.VarChar, 32);
        cmd.Parameters["@vs"].Value = nameVs;
        cmd.Parameters.Add("@id", MySqlDbType.VarChar, 8);
        cmd.Parameters["@id"].Value = summonerId;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;
        

        //open connection
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }

        //execute
        try
        {
            reader = cmd.ExecuteReader();

            float winPercentAschamp;
            float winPercentVschamp;

            if (reader.HasRows)
            {
                reader.Read();
                if(reader.GetString(0).Equals(nameAs))
                {
                    winPercentAschamp = reader.GetFloat(1) / reader.GetFloat(2) * 100;
                    winPercentVschamp = reader.GetFloat(3) / reader.GetFloat(4) * 100;
                    winPercentAsLeft.Text = winPercentAschamp + "%";
                    winPercentVsLeft.Text = winPercentVschamp + "%";
                
                    reader.Read();
                    winPercentAschamp = reader.GetFloat(1) / reader.GetFloat(2) * 100;
                    winPercentVschamp = reader.GetFloat(3) / reader.GetFloat(4) * 100;
                    winPercentAsRight.Text = winPercentAschamp + "%";
                    winPercentVsRight.Text = winPercentVschamp + "%";
                }
                else
                {
                    winPercentVschamp = reader.GetFloat(1) / reader.GetFloat(2) * 100;
                    winPercentAschamp = reader.GetFloat(3) / reader.GetFloat(4) * 100;
                    winPercentAsLeft.Text = winPercentAschamp + "%";
                    winPercentVsLeft.Text = winPercentVschamp + "%";
                
                    reader.Read();
                    winPercentVschamp = reader.GetFloat(1) / reader.GetFloat(2) * 100;
                    winPercentAschamp = reader.GetFloat(3) / reader.GetFloat(4) * 100;
                    winPercentAsRight.Text = winPercentAschamp + "%";
                    winPercentVsRight.Text = winPercentVschamp + "%";
                }

                //cleanup in case no matches were played
                if (winPercentAsLeft.Text.Equals("NaN%")) winPercentAsLeft.Text = "None played";
                if (winPercentVsLeft.Text.Equals("NaN%")) winPercentVsLeft.Text = "None played";
                if (winPercentAsRight.Text.Equals("NaN%")) winPercentAsRight.Text = "None played";
                if (winPercentVsRight.Text.Equals("NaN%")) winPercentVsRight.Text = "None played";

            }
            else
            {
                matchupStats.Text = "No data found";
            }

            connection.Close();
        }
        catch (Exception notFound)
        {
            matchupStats.Text = notFound.ToString();
            connection.Close();
        }

    }



    protected void processRunes(String[] runesIds)
    {
        Dictionary<string, double> runeDict = new Dictionary<string, double>();

        MySqlDataReader reader;
        //search for summoner, Dynamic SQL
        MySqlCommand cmd = new MySqlCommand();
        cmd.CommandText = "select * from lolmatchups.rune where rune.rune_id in (@r1,@r2,@r3,@r3,@r5,@r6,@r7,@r8,@r9,@r10,@r11,@r12,@r13,@r14,@r15,@r16,@r17,@r18,@r19,@r20,@r21,@r22,@r23,@r24,@r25,@r26,@r27,@r28,@r29,@r30)";
        for (int i = 0; i < 30; i++)
        {
            cmd.Parameters.Add("@r" + (i+1), MySqlDbType.VarChar, 8);
            cmd.Parameters["@r" + (i + 1)].Value = runesIds[i];
        }

        cmd.CommandType = CommandType.Text;
        cmd.Connection = connection;

        //open connection
        try { connection.Open(); }
        catch (Exception conException) { status.Text = "Did not connect to the Database Server."; }

        //execute
        try
        {
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    string runeid = reader.GetString(0);
                    string runeEffect = reader.GetString(4);

                    string[] split = runeEffect.Split(new char[] {' '}, 2);
                    
                    string effectType = split[1].TrimStart();
                    string effectAmount = split[0].Substring(1, split[0].Length-1);

                    if (effectAmount.EndsWith("%")) effectAmount = effectAmount.Substring(0, split[0].Length - 2);

                    //remove additional parentheses
                    string[] refineSplit = effectType.Split(new char[] { '(' }, 2);
                    effectType = refineSplit[0].TrimEnd();

                    //capitalize first letter
                    effectType = char.ToUpper(effectType[0]) + effectType.Substring(1);

                    int runeCount = 0;

                    foreach (string aRune in runesIds)
                    {
                        if (aRune.Equals(runeid)) runeCount++;
                    }
                    runeDict.Add(effectType, runeCount * Double.Parse(effectAmount));


                    
                }

                HtmlTableRow runeRow = new HtmlTableRow();
                HtmlTableCell runeCell = new HtmlTableCell();
                

                foreach (String runeKey in runeDict.Keys)
                {
                    runeRow = new HtmlTableRow();

                    //rune name
                    runeCell = new HtmlTableCell();
                    runeCell.InnerText = runeKey;
                    runeRow.Cells.Add(runeCell);


                    //total effect
                    runeCell = new HtmlTableCell();
                    runeCell.InnerText = runeDict[runeKey] + "";
                    runeRow.Cells.Add(runeCell);

                    runeTable.Rows.Add(runeRow);
                  
                }


                runeTable.Style.Remove("display");
            }
            else
            {
                matchupStats.Text = "No data found";
            }

        }
        catch (Exception notFound)
        {
            matchupStats.Text = notFound.ToString();
            connection.Close();
        }
        connection.Close();
    }
}