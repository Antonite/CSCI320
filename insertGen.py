import json, os, collections
import urllib.request

API_KEY = "e171bba5-29fa-41e8-a1af-2c82b601b947"
TARGET_DIR = "D:/College Year 3/Data Management/CSCI320"
API_ROOT = "https://global.api.pvp.net/api/lol/static-data/na/v1.2/"
SQL_PREFIX = "USE lolmatchups;\n"

def main():
    generateRuneInsert()
    generateSummonerSpellInsert()
    generateItemInsert()
    generateChampionInsert()
    generateAbilityInsert()
    generateMasteryInsert()
    return

def generateRuneInsert():
    output = SQL_PREFIX + "INSERT INTO rune VALUES \n"
    
    received_byte_data_runes = urllib.request.urlopen( API_ROOT + "rune?api_key=" + API_KEY )
    rune_json = json.loads( received_byte_data_runes.read().decode( "utf-8" ) )["data"]

    output_values = ""
    for ( rune_id, rune_info ) in rune_json.items():
        if len( output_values ) != 0:
            output_values += ",\n"
        output_values += "(" + rune_id + ",\"" + rune_info["name"] + "\",\"" + getRuneType( rune_info["rune"]["type"] ) + "\"," + rune_info["rune"]["tier"] + ",\"" + rune_info["description"] + "\")"

    f = open( TARGET_DIR + "/rune_insert.sql", 'w' )
    f.write( output + output_values + ";" )
    return

def getRuneType( color):
    mapping = { "red" : "mark", "yellow" : "seal", "blue" : "glyph", "black" : "quintessence" }
    return mapping[color]

def generateSummonerSpellInsert():
    output = SQL_PREFIX + "INSERT INTO summoner_spell VALUES \n"

    received_byte_data_summoner_spells = urllib.request.urlopen( API_ROOT + "summoner-spell?api_key=" + API_KEY )
    ss_json = json.loads( received_byte_data_summoner_spells.read().decode( "utf-8" ) )["data"]

    output_values = ""
    for ( key, ss_data ) in ss_json.items():
        if len( output_values ) != 0:
            output_values += ",\n"
        output_values += "(" + str( ss_data["id"] ) +",\"" + ss_data["name"] + "\",\"" + ss_data["description"] + "\"," + str( ss_data["summonerLevel"] ) + ")"

    f = open( TARGET_DIR + "/ss_insert.sql", 'w' )
    f.write( output + output_values + ";" )
    return

def generateItemInsert():
    output = SQL_PREFIX + "INSERT INTO item VALUES \n"
    
    received_byte_data_items = urllib.request.urlopen( API_ROOT + "item?itemListData=from,image&api_key=" + API_KEY )
    items_json = json.loads( received_byte_data_items.read().decode( "utf-8" ) )["data"]

    output_values = ""
    for( item_id, item_data ) in items_json.items():
        if len( output_values )!= 0:
            output_values += ",\n"
        output_values += "(" + item_id + ",\"" + item_data["name"] + "\",\"" + item_data["description"] + "\","

        
        item_image_url = "http://ddragon.leagueoflegends.com/cdn/5.2.1/img/item/" + item_data["image"]["full"]
        
        if ( item_id != "3751" and item_id != "3286" and item_id != "3285" ):
            f = open( "LoLMatchups/ItemImages/" + item_data["image"]["full"],'wb')
            f.write(urllib.request.urlopen( item_image_url ).read())
            f.close()
        
        if "from" in item_data.keys():
            for i in range( 4 ):
                try:
                    child_id = item_data["from"][i]
                    output_values += child_id
                except IndexError:
                    output_values += "NULL"
                    
                if i != 3:
                    output_values += ","
        else:
            output_values += "NULL,NULL,NULL,NULL"

        output_values += ",\"" + "ItemImages/" + item_data["image"]["full"] + "\")"
            

    f = open( TARGET_DIR + "/item_insert.sql", 'w' )
    f.write( output + output_values + ";" )
    return

def generateChampionInsert():
    output = SQL_PREFIX + "INSERT INTO champion VALUES \n"

    received_byte_data_champs = urllib.request.urlopen( API_ROOT + "champion?api_key=" + API_KEY )
    champs_json = json.loads( received_byte_data_champs.read().decode( "utf-8" ) )["data"]

    output_values = ""
    for ( champ_name, champ_data ) in champs_json.items():
        if len( output_values ) != 0:
            output_values += ",\n"
        champ_img_file = "ChampionImages/" + champ_name + "_Square_0.png"
        output_values += "(" + str( champ_data["id"] ) + ",\"" + champ_name + "\",\"" + champ_data["title"] + "\",\"" + champ_img_file + "\")"

    f = open( TARGET_DIR + "/champs_insert.sql", 'w' )
    f.write( output + output_values + ";" )
    return

def generateAbilityInsert():
    output = SQL_PREFIX + "INSERT INTO champion_ability VALUES \n"

    received_byte_data_ability = urllib.request.urlopen( API_ROOT + "champion?champData=spells&api_key=" + API_KEY )
    ability_json = json.loads( received_byte_data_ability.read().decode( "utf-8" ) )["data"]

    for ( champ_name, champ_data ) in ability_json.items():
        print( champ_data )
    return

def generateMasteryInsert():
    output = SQL_PREFIX + "INSERT INTO mastery VALUES \n"

    received_byte_data_mastery = urllib.request.urlopen( API_ROOT + "mastery?masteryListData=tree&api_key=" + API_KEY )
    received_json = json.loads( received_byte_data_mastery.read().decode( "utf-8" ) )
    mastery_json = received_json["data"]
    tree_json = received_json["tree"]

    tree_values = { "Offense" : [], "Defense" : [], "Utility" : [] }
    for offenseRow in tree_json["Offense"]:
        for masteryEntry in offenseRow["masteryTreeItems"]:
            if masteryEntry is not None:
                tree_values["Offense"].append( masteryEntry["masteryId"] )
    for defenseRow in tree_json["Defense"]:
        for masteryEntry in defenseRow["masteryTreeItems"]:
            if masteryEntry is not None:
                tree_values["Defense"].append( masteryEntry["masteryId"] )
    for utilityRow in tree_json["Utility"]:
        for masteryEntry in utilityRow["masteryTreeItems"]:
            if masteryEntry is not None:
                tree_values["Utility"].append( masteryEntry["masteryId"] )
    
    output_values = ""
    for ( mastery_id, mastery_data ) in mastery_json.items():
        mastery_image_url = "http://ddragon.leagueoflegends.com/cdn/5.2.1/img/mastery/" + str( mastery_id ) + ".png"
        f = open( "LoLMatchups/MasteryImages/" + str( mastery_id ) + ".png", 'wb' )
        f.write(urllib.request.urlopen( mastery_image_url ).read() )
        f.close()
        
        if len( output_values ) != 0:
            output_values += ",\n"
        output_values += "(" + mastery_id + ",\"" + mastery_data["name"] + "\","

        for i in range( 4 ):
            try:
                description = mastery_data["description"][i]
                description = description.replace( '(', '\(' )
                description = description.replace( ')', '\)' )
                description = description.replace( '<', '\<' )
                description = description.replace( '>', '\>' )
                description = description.replace( '+', '\+' )
                description = description.replace( '-', '\-' )
                output_values += '"' + description + '",'
            except IndexError:
                output_values += "NULL,"

        outer_tree_index = str( mastery_id )[1]
        inner_tree_index = None
        if outer_tree_index == '1':
            output_values += "\"Offense\","
            inner_tree_index = tree_values["Offense"].index( int( mastery_id ) )
        elif outer_tree_index == '2':
            output_values += "\"Defense\","
            inner_tree_index = tree_values["Defense"].index( int( mastery_id ) )
        elif outer_tree_index == '3':
            output_values += "\"Utility\","
            inner_tree_index = tree_values["Utility"].index( int( mastery_id ) )
        else:
            print( "Unknown outer_tree_index value: " + outer_tree_index )

        output_values += str( inner_tree_index ) + ",\"" + str( mastery_id ) + ".png\")"

    f = open( TARGET_DIR + "/mastery_insert.sql", 'w' )
    f.write( output + output_values + ";" )
    return

main()
