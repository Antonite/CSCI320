import json, random

def main():
    root_file_name = "matches"
    player_file_name = "sample_player.sql"
    player_champion_file_name = "sample_player_champ.sql"
    player_matchup_file_name = "sample_player_matchup.sql"
    player_mastery_file_name = "sample_player_mastery.sql"
    player_rune_file_name = "sample_player_runes.sql"
    player_ss_file_name = "sample_player_ss.sql"
    player_items_file_name = "sample_player_items.sql"

    players = {}

    rank_prefixes = [ 'B', 'S', 'G', 'P', 'D' ]
    rank_suffixes = [ '1', '2', '3', '4', '5' ]
    
    for i in range( 1, 11 ):
        print( i )
        with open( "../" + root_file_name + str( i ) + ".json" ) as match_file:
            file_contents = match_file.read()
            match_data = json.loads( file_contents )
            for match in match_data["matches"]:
                for participant in match["participants"]:
                    if "runes" not in participant.keys() or "masteries" not in participant.keys():
                        continue
                    participant_id = participant["participantId"]
                    champion_id = participant["championId"]
                    match_id = match["matchId"]
                    team_id = participant["teamId"]
                    summoner_id = None
                    summoner_name = None
                    kills = participant["stats"]["kills"]
                    deaths = participant["stats"]["deaths"]
                    assists = participant["stats"]["assists"]
                    creep_score = participant["stats"]["minionsKilled"]
                    masteries = { "masteries" : getMasteryMappings( participant["masteries"] ) }
                    runes = { "runes" : getRuneDict( participant["runes"] ) }
                    items = { "items" : getItemsDict( participant["stats"] ) }
                    ss1 = participant["spell1Id"]
                    ss2 = participant["spell2Id"]
                    
                    
                    if match["participantIdentities"][participant_id - 1]["participantId"] == participant_id:
                        summoner_id = match["participantIdentities"][participant_id - 1]["player"]["summonerId"]
                        summoner_name = match["participantIdentities"][participant_id - 1]["player"]["summonerName"]
                        
                    else:
                        #just in case participants isnt ordered correctly
                        for participant_entry in match["participantIdentities"]:
                            if participant_entry["participantId"] == participant_id:
                                summoner_id = participant_entry["player"]["summonerId"]
                                summoner_name = participant_entry["player"]["summonerName"]
                                kills = participant_entry["stats"]["kills"]
                                deaths = participant_entry["stats"]["deaths"]
                                assists = participant_entry["stats"]["assists"]
                                creep_score = participant_entry["stats"]["minionsKilled"]
                                break

                    matchup = findMatchOpponent( match["participants"], summoner_id, participant_id )
                    if matchup == None:
                        continue
                    
                    matchup["won"] = str( match["teams"][int(str(team_id)[0])-1]["winner"] )

                    played_as_won = 0
                    their_champ_against_won = 0
                    if matchup["won"] == "True":
                        played_as_won = 1
                        their_champ_against_won = 1
                    ss_dict = { "ss1" : ss1, "ss2" : ss2, "won" : played_as_won, "played" : 1 }
                    masteries["won"] = played_as_won
                    runes["won"] = played_as_won
                    items["won"] = played_as_won
                    masteries["played"] = 1
                    runes["played"] = 1
                    items["played"] = 1
                    
                    if summoner_id not in players:
                        players[summoner_id] = { "summoner_name" : summoner_name, "last_match_id" : match_id, "champion_info" : {}, "matchups" : {} }
                        
                        players[summoner_id]["champion_info"][matchup["my_champ"]] = { "played_as_won" : played_as_won, "played_as_total" : 1, "played_against_won" : 0, "played_against_total" : 0 }
                        players[summoner_id]["champion_info"][matchup["their_champ"]] = { "played_as_won" : 0, "played_as_total" : 0, "played_against_won" : their_champ_against_won, "played_against_total" : 1 }

                        players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])] = { "won" : played_as_won, "played" : 1, "kills" : kills, "deaths" : deaths, "assists" : assists, "creep_score" : creep_score, "masteries" : [masteries], "runes" : [runes], "ss" : [ss_dict], "items" : [items] }
                    else:
                        
                        if matchup["my_champ"] in players[summoner_id]["champion_info"]:
                            players[summoner_id]["champion_info"][matchup["my_champ"]]["played_as_won"] += played_as_won
                            players[summoner_id]["champion_info"][matchup["my_champ"]]["played_as_total"] += 1
                        else:
                            players[summoner_id]["champion_info"][matchup["my_champ"]] = { "played_as_won" : played_as_won, "played_as_total" : 1, "played_against_won" : 0, "played_against_total" : 0 }

                        if matchup["their_champ"] in players[summoner_id]:
                            players[summoner_id]["champion_info"][matchup["their_champ"]]["played_against_won"] += their_champ_against_won
                            players[summoner_id]["champion_info"][matchup["their_champ"]]["played_against_total"] += 1
                        else:
                            players[summoner_id]["champion_info"][matchup["their_champ"]] = { "played_as_won" : 0, "played_as_total" : 0, "played_against_won" : their_champ_against_won, "played_against_total" : 1 }

                        if str(matchup["my_champ"]) + "-" + str(matchup["their_champ"]) in players[summoner_id]["matchups"]:
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["won"] += played_as_won
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["played"] += 1
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["kills"] += kills
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["deaths"] += deaths
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["assists"] += assists
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["creep_score"] += creep_score
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["masteries"].append( masteries )
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["runes"].append( runes )
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["ss"].append( ss_dict )
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])]["items"].append( items )
                        else:
                            players[summoner_id]["matchups"][str(matchup["my_champ"]) + "-" + str(matchup["their_champ"])] = { "won" : played_as_won, "played" : 1, "kills" : kills, "deaths" : deaths, "assists" : assists, "creep_score" : creep_score, "masteries" : [masteries], "runes" : [runes], "ss" : [ss_dict], "items" : [items] }
                            

                        if match_id > players[summoner_id]["last_match_id"]:
                            players[summoner_id]["last_match_id"] = match_id
    
    player_file = open( player_file_name, 'w' )
    player_champ_file = open( player_champion_file_name, 'w' )
    player_matchup_file = open( player_matchup_file_name, 'w' )
    player_mastery_file = open( player_mastery_file_name, 'w' )
    player_rune_file = open( player_rune_file_name, 'w' )
    player_ss_file = open( player_ss_file_name, 'w' )
    player_items_file = open( player_items_file_name, 'w' )

    player_output = "USE lolmatchups;\nINSERT INTO player VALUES "
    player_champ_output = "USE lolmatchups;\nINSERT INTO player_champion_stat VALUES "
    player_matchup_output = "USE lolmatchups;\nINSERT INTO player_matchup VALUES "
    player_mastery_output = "USE lolmatchups;\nINSERT INTO player_mastery(matchup_id, offense_values, defense_values, utility_values, won, used) VALUES "
    player_rune_output = "USE lolmatchups;\nINSERT INTO player_rune_set(matchup_id,rune_id1,rune_id2,rune_id3,rune_id4,rune_id5,rune_id6,rune_id7,rune_id8,rune_id9,rune_id10,rune_id11,rune_id12,rune_id13,rune_id14,rune_id15,rune_id16,rune_id17,rune_id18,rune_id19,rune_id20,rune_id21,rune_id22,rune_id23,rune_id24,rune_id25,rune_id26,rune_id27,rune_id28,rune_id29,rune_id30,won,used) VALUES "
    player_ss_output = "USE lolmatchups;\nINSERT INTO player_summoner_spells(matchup_id,ss_id1,ss_id2,won,used) VALUES "
    player_items_output = "USE lolmatchups;\nINSERT INTO player_items(matchup_id, item_id1, item_id2, item_id3, item_id4, item_id5, item_id6, item_id7, won, used) VALUES "

    matchup_count = 1
    for ( summoner_id, player_info ) in players.items():
        random_rank = random.choice( rank_prefixes ) + random.choice( rank_suffixes )
        player_output += "(" + str( summoner_id ) + ",\"" + player_info["summoner_name"] + "\",\"" + random_rank + "\"," + "30" + "," + str( player_info["last_match_id"] ) + "),\n"
        for ( champion_id, champion_record ) in player_info["champion_info"].items():
            player_champ_output += "(" + str( summoner_id ) + "," + str( champion_id ) + "," + str( champion_record["played_as_won"] ) + "," + str( champion_record["played_as_total"] ) + "," + str( champion_record["played_against_won"] ) + "," + str( champion_record["played_against_total"] ) + "),\n"
        for ( matchup_pair, matchup_info ) in player_info["matchups"].items():
            player_champ_id = matchup_pair[:matchup_pair.index('-')]
            opp_champ_id = matchup_pair[matchup_pair.index('-')+1:]
            mastery_list = matchup_info["masteries"]
            rune_list = matchup_info["runes"]
            ss_list = matchup_info["ss"]
            item_list = matchup_info["items"]
            player_matchup_output += "(" + str( matchup_count ) + "," + str( summoner_id ) + "," + player_champ_id + "," + opp_champ_id + "," + str( matchup_info["won"] ) + "," + str( matchup_info["played"] ) + "," + str( matchup_info["kills"] ) + "," + str( matchup_info["deaths"] ) + "," + str( matchup_info["assists"] ) + "," + str( matchup_info["creep_score"] ) + "),\n"
            for ss in ss_list:
                player_ss_output += "(" + str( matchup_count ) + "," + getSSOutput( ss ) + "),\n"
            for runes in rune_list:
                player_rune_output += "(" + str( matchup_count ) + "," + getRuneOutput( runes ) + "),\n"
            for mastery in mastery_list:
                player_mastery_output += "(" + str( matchup_count ) + "," + getMasteryOutput( mastery ) + "),\n"
            for items in item_list:
                player_items_output += "(" + str( matchup_count ) + "," + getItemsOutput( items ) + "),\n"
            matchup_count += 1

    player_output = player_output[:-2] + ";"
    player_champ_output = player_champ_output[:-2] + ";"
    player_matchup_output = player_matchup_output[:-2] + ";"
    player_mastery_output = player_mastery_output[:-2] + ";"
    player_rune_output = player_rune_output[:-2] + ";"
    player_ss_output = player_ss_output[:-2] + ";"
    player_items_output = player_items_output[:-2] + ";"

    #player_file.write( player_output )
    #player_champ_file.write( player_champ_output )
    player_matchup_file.write( player_matchup_output )
    player_mastery_file.write( player_mastery_output )
    player_rune_file.write( player_rune_output )
    player_ss_file.write( player_ss_output )
    player_items_file.write( player_items_output )




def findMatchOpponent( participants, summoner_id, participant_id ):
    my_participant = None
    for participant in participants:
        if participant_id == participant["participantId"]:
            my_participant = participant

    #print( "Target: " + my_participant["timeline"]["lane"] + " " + my_participant["timeline"]["role"] + " " + str( my_participant["teamId"] ) )

    their_participant = None
    for participant in participants:
        #print( "Candidate: " + participant["timeline"]["lane"] + " " + participant["timeline"]["role"] + " " + str( participant["teamId"] ) )
        if participant["timeline"]["lane"] == my_participant["timeline"]["lane"] and participant["timeline"]["role"] == my_participant["timeline"]["role"] and my_participant["teamId"] != participant["teamId"]:
            their_participant = participant

    if their_participant == None:
        return None

    return_dict = { "my_champ" : my_participant["championId"], "their_champ" : their_participant["championId"] }
    return return_dict

def getMasteryMappings( masteries ):
    mastery_mapping = {\
        "4211" : 0,\
        "4212" : 1,\
        "4213" : 2,\
        "4214" : 3,\
        "4221" : 4,\
        "4222" : 5,\
        "4224" : 6,\
        "4231" : 7,\
        "4232" : 8,\
        "4233" : 9,\
        "4234" : 10,\
        "4241" : 11,\
        "4242" : 12,\
        "4243" : 13,\
        "4244" : 14,\
        "4251" : 15,\
        "4252" : 16,\
        "4253" : 17,\
        "4262" : 18,\
        "4111" : 0,\
        "4112" : 1,\
        "4113" : 2,\
        "4114" : 3,\
        "4121" : 4,\
        "4122" : 5,\
        "4123" : 6,\
        "4124" : 7,\
        "4131" : 8,\
        "4132" : 9,\
        "4133" : 10,\
        "4134" : 11,\
        "4141" : 12,\
        "4142" : 13,\
        "4143" : 14,\
        "4144" : 15,\
        "4151" : 16,\
        "4152" : 17,\
        "4154" : 18,\
        "4162" : 19,\
        "4311" : 0,\
        "4312" : 1,\
        "4313" : 2,\
        "4314" : 3,\
        "4322" : 4,\
        "4323" : 5,\
        "4324" : 6,\
        "4331" : 7,\
        "4332" : 8,\
        "4333" : 9,\
        "4334" : 10,\
        "4341" : 11,\
        "4342" : 12,\
        "4343" : 13,\
        "4344" : 14,\
        "4352" : 15,\
        "4353" : 16,\
        "4362" : 17}
    offense = [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
    defense = [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
    utility = [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
    for mastery_obj in masteries:
        mastery_id = mastery_obj["masteryId"]
        if str( mastery_id )[1] == '1':
            offense[mastery_mapping[str(mastery_id)]] = mastery_obj["rank"]
        elif str( mastery_id )[1] == '2':
            defense[mastery_mapping[str(mastery_id)]] = mastery_obj["rank"]
        elif str( mastery_id )[1] == '3':
            utility[mastery_mapping[str(mastery_id)]] = mastery_obj["rank"]
    return ( offense, defense, utility )

def getRuneDict( rune_list ):
    new_rune_dict = {}
    for rune_dict in rune_list:
        new_rune_dict[rune_dict["runeId"]] = rune_dict["rank"]
    return new_rune_dict

def getItemsDict( stats ):
    items_dict = { "item0" : stats["item0"], "item1" : stats["item1"], "item2" : stats["item2"], "item3" : stats["item3"], "item4" : stats["item4"], "item5" : stats["item5"], "item6" : stats["item6"] }
    return items_dict

def getSSOutput( ss ):
    ss1 = ss["ss1"]
    ss2 = ss["ss2"]
    if ss1 > ss2:
        return str( ss2 ) + "," + str( ss1 ) + "," + str( ss["won"] ) + ',' + str( ss["played"] )
    else:
        return str( ss1 ) + "," + str( ss2 ) + "," + str( ss["won"] ) + ',' + str( ss["played"] )

def getRuneOutput( runes ):
    rune_keys = sorted( runes["runes"].keys() )
    output_str = ""
    total_runes = 0
    for key in rune_keys:
        if not key == "won" and not key == "played":
            quantity = runes["runes"][key]
            total_runes += quantity
            for i in range( quantity ):
                output_str += str( key ) + ","
    if total_runes < 30:
        remaining_runes = 30 - total_runes
        for i in range( remaining_runes ):
            output_str += "NULL,"

    return output_str + str( runes["won"] ) + "," + str( runes["played"] )

def getMasteryOutput( mastery ):
    return "\"" + str( mastery["masteries"][0] ) + "\",\"" + str( mastery["masteries"][1] ) + "\",\"" + str( mastery["masteries"][2] ) + "\"," + str( mastery["won"] ) + "," + str( mastery["played"] )

def getItemsOutput( items ):
    item_ids = [items["items"]["item0"],items["items"]["item1"],items["items"]["item2"],items["items"]["item3"],items["items"]["item4"],items["items"]["item5"],items["items"]["item6"]]
    item_ids = sorted( item_ids, reverse=True )
    output_str = ""
    for item_id in item_ids:
        if item_id == 0:
            output_str += "NULL,"
        else:
            output_str += str( item_id ) + ","

    return output_str + str( items["won"] ) + "," + str( items["played"] )
            
    
    
    
main()

