#include <iostream>
#include <stack>
#include <stdlib.h>
#include <algorithm>
#include <vector>
#include <map>
#include <fstream>
using namespace std;

enum class STATES
{
    MENU,
    SELECTADV,
    HALLFAME,
    HELP,
    ABOUT,
    QUIT,
    GAMEPLAY,
    GMENU,
    SFAME,
    NEW_SCORE
};

class GameManager;

class Command{
    public: 
    virtual ~Command() = default;
};

class Attribute{
    public:
    string name;
    int value;
    string location;
    string direction;

    Attribute(string nam){
        if (nam == "gold1"){
        name = "gold";
        value = 1;
        }
        else if (nam == "gold2"){
        name = "gold";
        value = 2;
        }
        else if (nam == "gold3"){
        name = "gold";
        value = 3;
        }
        else if (nam == "gold4"){
        name = "gold";
        value = 4;
        }


        else if (nam == "enemy1"){
        name = "enemy";
        value = 1;
        }
        else if (nam == "enemy2"){
        name = "enemy";
        value = 2;
        }
        else if (nam == "enemy3"){
        name = "enemy";
        value = 3;
        }
        else if (nam == "enemy4"){
        name = "enemy";
        value = 4;
        }

        else if (nam == "weapon1"){
        name = "weapon";
        value = 1;
        }
        else if (nam == "weapon2"){
        name = "weapon";
        value = 2;
        }
        else if (nam == "weapon3"){
        name = "weapon";
        value = 3;
        }
        else if (nam == "weapon4"){
        name = "weapon";
        value = 4;
        }
        else if (nam.find("(") != string::npos){
            name = "openable";
            int q;
            q = nam.find("(");

            nam.erase(0,q+1);
            q = nam.find("!");
            location = nam.substr(0,q);
            nam.erase(0,q+1);
            direction = nam;
        }
        else {
        name = nam;
        value = 0;

        
        }
    }
};



class Entity{
    public:
    string name;
    string desc;
    vector<Attribute> atributes;
    vector<Entity> entities;

    Entity(string nam, string des, vector<Attribute> atr, vector<Entity> ent):name(nam),desc(des),entities(ent){
        atributes = atr;
    }

    Entity(){}

};

class State
{
protected:
    GameManager *_manager = nullptr;

public:
    virtual STATES update() = 0;
    virtual void render() = 0;
};

class Scene
{
public:
    string name;
    string desc;
    vector<pair<string, string>> locations;
    vector<Entity> objects;

    Scene()
    {
    }

    Scene(string nam, string des, vector<pair<string, string>> loc, vector<Entity> obj)
    {
        name = nam;
        desc = des;
        locations = loc;
        objects = obj;
    }

    void pushLocation(string location, string direction){
        locations.push_back(make_pair(location,direction));
    }

    string directions()
    {
        string text;
        for (auto q : locations)
        {
            text += q.second + " ";
        }
        return text;
    }

    string entities()
    {
        string text;
        for (auto q : objects)
        {
            text += q.name + " ";
        }
        return text;
    }

    string entitiesAtr(){
        string text;
        for (auto q : objects)
        {
            text += q.name + ": Attributes:";
            for (auto p : q.atributes){
                text += " " + p.name;
            }
            text += ". Containing the entities: ";
            for (auto p : q.entities){
                text += p.name + ": ";
                for (auto q : p.atributes){
                text += q.name + " ";
                }
            }
            text += "\n";
        }
        return text;
    }

    string compareScene(string text)
    {
        string location;
        for (auto q : locations)
        {
            if (text == q.second)
            {
                location = q.first;
                return location;
            }
        }
        return text;
    }

    string compareObj(string text)
    {
        string obj;
        for (auto q : objects)
        {
            if (text == q.name)
            {
                obj = q.desc;
                return obj;
            }
        }
        return text;
    }
};



class Map
{
public:
    map<string, Scene> map;
    string location;
    void adventure(string text)
    {
        fstream reader;
        reader.open(text, ios::in);
        string line;
        int j = 0;
        string name;
        string namedesc;
        string temp;
        Scene place;
        vector<Entity> objs;
        vector<pair<string, string>> locations;
        bool start = false;
        while (getline(reader, line))
        {
            if (line != "" && line.at(0) != '#')
            {
                j = line.find(";");
                name = line.substr(0, j);
                line.erase(0, j + 1);
                j = line.find(";");
                namedesc = line.substr(0, j);
                line.erase(0, j + 1);
                j = line.find(";");
                temp = line.substr(0, j);
                locations = splitter(temp);
                line.erase(0, j + 1);
                objs = entityCreator(line);
                place.name = name;
                place.desc = namedesc;
                place.locations = locations;
                place.objects = objs;
                // Scene(name,namedesc,locations,objs)
                map[name] = place;
                if (!start)
                {
                    location = name;
                    start = true;
                }
            }
        }
        reader.close();
    }

    //blood,still wet though strangely there are no bodies:final-boss,Black wings sprout from a vague shifting dark mass,end.enemy4.gold4>key,a golden key with an airship symbol inscribed on it,takeable.key.gold1

    vector<Entity> entityCreator(string temp){
        int j = 0;
        int i = 0;
        int ii = 0;
        vector<Entity> ents;
        while ((j = temp.find(":")) != string::npos){
            string bit;
            bit = temp.substr(0, j);
            ents.push_back(entitySplitter(bit));
            temp.erase(0,j+1);
        }
        ents.push_back(entitySplitter(temp));
        return ents;
    }

    Entity  entitySplitter(string bit){
        int j = 0;
        int i = 0;
        int ii = 0;
        vector<Entity> ents;
        string name;
        string desc;
        string atrName;
        vector<Entity> entsInEnts;
        vector<Attribute> atrs;
        Entity ent;
        
        i = bit.find(",");
        name = bit.substr(0, i);
        bit.erase(0, i + 1);

        i = bit.find(",");
        if (i == string::npos){
            desc = bit;
        }
        else {
            desc = bit.substr(0, i);
            bit.erase(0, i + 1);
        }

    
        if (i != string::npos){
            i = bit.find(".");
            ii = bit.find(">");
            if (ii == string::npos || (i != string::npos && ii != string::npos && ii > i)){
                while ((i = bit.find(".")) != string::npos){
                    atrName = bit.substr(0, i);
                    bit.erase(0, i + 1);
                    Attribute attr(atrName);
                    atrs.push_back(attr);
                }
                ii = bit.find(">");
                if (ii != string::npos){
                    atrName = bit.substr(0, ii);
                    bit.erase(0, ii + 1);
                    Attribute attr(atrName);
                    atrs.push_back(attr);
                    entsInEnts.push_back(entitySplitter(bit));
                }
                else {
                    atrs.push_back(bit);
                }
            }
            else{
                atrName = bit.substr(0,ii);
                if (atrName != ""){
                    Attribute attr(atrName);
                    atrs.push_back(attr);
                    bit.erase(0,ii+1);
                    entsInEnts.push_back( entitySplitter(bit));
                }
                else{
                    bit.erase(0,ii+1);
                    entsInEnts.push_back( entitySplitter(bit));
                }
            }
        }
        ent.name = name;
        ent.desc = desc;
        ent.atributes = atrs;
        ent.entities = entsInEnts;
        return ent;
    }

    vector<pair<string, string>> splitter(string temp)
    {
        int j = 0;
        int i = 0;
        string bit;
        string bit1;
        vector<pair<string, string>> bit4;
        while ((j = temp.find(":")) != string::npos)
        {
            bit = temp.substr(0, j);
            i = bit.find(",");
            bit1 = bit.substr(0, i);
            bit.erase(0, i + 1);

            bit4.push_back(make_pair(bit1, bit));
            temp.erase(0, j + 1);
        }
        bit = temp.substr(0, j);
        i = bit.find(",");
        bit1 = bit.substr(0, i);
        bit.erase(0, i + 1);
        bit4.push_back(make_pair(bit1, bit));
        temp.erase(0, j + 1);
        return bit4;
    }
};



class Player
{
public:
    vector<Entity> inv;
    int score = 0;


    void addItem(Entity item)
    {
        
        
        for (auto &p : item.atributes)
        {
            if (p.name=="gold"){
                score +=p.value;
                cout << "Score + "<< p.value << endl;
            }
        }
        inv.push_back(item);
    }
};

class Go : public Command {
    public:

    Go(Map *map, string text){
        string text2 = map->map[map->location].compareScene(text);
        if (text2 == text){
            cout << "This is not a valid direction" << endl;
        }
        else{
            map->location = text2;
            cout << endl << map->map[map->location].desc << endl << endl; 
        }
        delete this;
    }
    

};

class Attack : public Command{
    public:

    Attack(string text, string text2, string text3, Player *player, Map *map, bool *win, bool *lose){
        int hp = 0;
        int php = 0;
        bool enemy = false;
        bool weapon = false;
        bool end = false;
        bool open = false;
        string location;
        string direction;
        string temp;
        int opn = 0;
        int tomp = 0;
        int itr = 0;
        int enm = 0;
        int score = 0;
        int q;
        if (text2 == "with"){
            for (auto &l : map->map[map->location].objects){
                if (l.name == text){
                    enm = itr;
                    for (auto &k : l.atributes){
                        if (k.name == "enemy"){
                            enemy = true;
                            hp = k.value;
                        }
                        if (k.name == "end"){
                            end = true;
                        }
                        if (k.name == "gold"){
                            score = k.value;
                        }
                        if (k.name == "openable"){
                            location = k.location;
                            direction = k.direction;
                            opn = tomp;
                            open = true;
                        }
                        tomp++;
                        
                        
                    }
                }
                itr++;
            }
            if (enemy){
                for (auto &p : player->inv){
                    if (p.name == text3){
                        for (auto &j : p.atributes){
                            if (j.name == "weapon"){
                            weapon = true;
                            php = j.value;
                            }
                        }
                    }
                }
            }
            else {
                cout << "You cannot attack " << text << endl;
            }
            if (weapon){
                if (php >= hp){
                    if (!end){
                        cout << "You destroy the " << text << endl;
                        player->score += score;
                        cout << "Score + " << score << endl;

                        if(open){
                            for (auto &l : map->map[map->location].objects){
                                if (l.name == text){
                                    map->map[map->location].pushLocation(location,direction);
                                    cout << "The way " << direction << " is clear" << endl;
                                }
                            }
                        }
                     map->map[map->location].objects.erase( map->map[map->location].objects.begin()+enm);

                    }
                    else{
                        player->score += score;
                        *win = true;
                        cout << "Score + " << score << endl;
                        cout << "THE DEMON IS SLAIN! VICTORY!" << endl;
                    }
                }
                else{
                    if (!end){
                        cout << "The " << text << " is unfazed" << endl;
                    }
                    else{
                        *lose = true;
                        cout << "No one will ever find your bones. YOU LOSE!" << endl;
                    }
                }
            }
            else{
                cout << "You cannot attack with " << text3 << endl;
            }
        }
    }
};

class Open : public Command{
    public:

    Open(string text, string text2, string text3, Player *player, Map *map){
        int q = 0;
        string temp;
        string location;
        string direction;
        bool locked = false;
        bool correct = false;
        bool key =false;
        int lck = 0;
        int opn = 0;
        
        for (auto &l : map->map[map->location].objects){
            if (l.name == text){
                for (auto &k : l.atributes){
                    if (k.name == "openable"){
                        correct = true;
                        for (auto &o : l.atributes){
                            if (o.name == "locked"){
                                locked = true;
                                if (text2 == "with"){
                                    for (auto &p : player->inv){
                                        if (p.name == text3){
                                            for (auto &j : p.atributes){
                                                if (j.name == "key"){
                                                    location = k.location;
                                                    direction = k.direction;
                                                    map->map[map->location].pushLocation(location,direction);
                                                    l.atributes.erase(l.atributes.begin()+lck);
                                                    l.atributes.erase(l.atributes.begin()+opn);
                                                    cout << "You opened the " << text << " with a " << text3 << endl;
                                                    cout << "The direction " << direction << " is now open to you" << endl;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (!key){
                                    cout << text << " couldn't be opened as it is locked" << endl;
                                }
                            }
                            lck++;
                        }
                        if (!locked){
                            location = k.location;
                            direction = k.direction;
                            map->map[map->location].pushLocation(location,direction);
                            l.atributes.erase(l.atributes.begin()+opn);
                            cout << "You opened the " << text;
                            cout << "The direction " << direction << " is now open to you" << endl;
                        }
                    }
                opn++;
                }
                if (!correct){
                    cout << text << " cannot be opened" << endl;
                }
            }
        }  

    }
};

class Take : public Command{
    public:

    Take(Map *map, string text, string text2, string text3, Player *player){
        int i = 0;
        bool correct = false;
        if (text2 == "from"){
            for (auto &p : map->map[map->location].objects){
                if (p.name == text3){
                    if (!p.entities.empty()){
                        for (auto &q : p.entities){
                            if (q.name == text){
                                for (auto &l : q.atributes){
                                    if (l.name == "takeable"){
                                        correct = true;
                                        player->addItem(q);
                                        cout <<"You grab the " << q.name << " and put it in your inventory" << endl;
                                        p.entities.erase(p.entities.begin()+i);
                                    }
                                }
                            }
                            i++;
                        }
                    }
                    else {
                        cout << "You cannot take anything from " << text2 << endl;
                    }
                }
            }
            if (!correct){
                cout << text << " is not a valid takeable item" << endl;
            }
        }
        else {
            for (auto &p : map->map[map->location].objects){
                if (p.name == text){
                    for (auto &q : p.atributes){
                        if (q.name == "takeable"){
                            correct = true;
                            player->addItem(p);
                            cout <<"You grab the " << p.name << " and put it in your inventory" << endl;
                            map->map[map->location].objects.erase(map->map[map->location].objects.begin()+i);
                        }
                    }
                }
                i++;
            }
            if (!correct){
                cout << text << " is not a valid takeable item" << endl;
            }
        }
    }
};

class Helper: public Command{
    public:

    Helper(vector<pair<string,string>> *list){
        cout<< "Commands are:" << endl;
        for (auto &q : *list)
        {
            if ("Alias" == q.first)
            {
                cout << q.second << " - Remaps the command to another word. Syntax is: " << q.second << " [command being remapped] [new command name]" << endl;
            }
            if ("Help" == q.first){
                cout << q.second << " - Displays commands, their usage and syntax. Syntax is: " << q.second << endl;
            }
            if ("Look" == q.first){
                cout << q.second << " - Displays a description about an entity. Syntax is either: " << q.second << " [entity] OR " << q.second << " at [entity] OR " << q.second << " in [entity]" << endl;
            }
            if ("Inventory" == q.first){
                cout << q.second << " - Displays the current entities in your inventory. Syntax is: " << q.second;
            }
            if ("Go" == q.first){
                cout << q.second << " - Moves the character from their current location towards another. Syntax is: " << q.second << " [direction]" << endl;
            }
            if ("Take" == q.first){
                cout << q.second << " - Takes an item and puts it in your inventory. Syntax is: " << q.second << " [item] OR " << q.second << " [item] from [entity]";
            }
        }
        delete this;
    }
    
};

class Inventory: public Command{
    public:

    Inventory(Player *player){
        cout << "Current Inventory: ";
        for (auto &p : player->inv)
        {
            cout << p.name << ", ";
        }
        cout << endl;
        delete this;
    }
    

};

class Look: public Command{
    public:


    Look(string text, string text2, Scene location){
        string desc;
        if (text == "at"){
            desc = location.compareObj(text2);
            if (desc != ""){
                cout << desc << endl;
            }
            else{
                cout << "You cannot look at " << text2 << endl;
            }
        }
        else if (text == "in"){
            desc = location.compareObj(text2);
            if (desc != ""){
                for (auto &p : location.objects)
                {
                    if (p.name == text2){
                        if (!p.entities.empty()){
                            for (auto &q : p.entities){
                                cout << "There is " << q.desc << " in the " << text2 << endl;
                            }
                        }
                        else {
                            cout << "You cannot look in " << text2 << endl;
                        }
                    }
                }
            }
        }
        else{
            desc = location.compareObj(text);
            if (desc != ""){
                cout << desc << endl;
            }
            else{
                cout << "You cannot look at " << text << endl;
            }
        }
        delete this;
    }

};

class Alias: public Command{
    public:
    vector<pair<string, string>> *commands;
    string text;
    string _command;

    Alias(vector<pair<string, string>> *list, string line, string line2): commands(list), text(line), _command(line2){
        int j = 0;
        
        for (auto &q : *commands)
        {
            if (_command == q.second)
            {
                bool duplicate = false;
                
                for (auto &p : *commands){
                    if (text == p.second && q.first != p.first){
                        duplicate = true;
                    }
                }
                if (!duplicate){
                cout << "Changed command: " << _command << " to: " << text << endl;
                q.second = text;
                }
                else{
                    cout << "Cannot have commands with the same name" << endl;
                }
            }
        }
        delete this;
    }

};

class Debug: public Command{
    public:

    Debug(Scene location, vector<pair<string, string>> *commands){
        cout << "Current Location: " << location.name << endl;
        cout << "Possible directions to travel: " << location.directions() << endl;
        cout << "Possible entities to interact with: " << location.entitiesAtr() << endl;
        cout << "Commands: ";
        for (auto &p : *commands)
        {
            cout << p.second << " ";
        }
        cout << endl;
        delete this;
    }

};

class CommandManager{
    public:
    vector<pair<string, string>> commands;
    Map *map;
    Player *player;
    bool win = false;
    bool lose = false;
    
    CommandManager(){
        commands.push_back(make_pair("Go","go"));
        commands.push_back(make_pair("Help","help"));
        commands.push_back(make_pair("Inventory","inventory"));
        commands.push_back(make_pair("Look","look"));
        commands.push_back(make_pair("Alias","alias"));
        commands.push_back(make_pair("Debug","debug"));
        commands.push_back(make_pair("Take","take"));
        commands.push_back(make_pair("Open","open"));
        commands.push_back(make_pair("Attack","attack"));
    }

    void addMap(Map *paper){
        map = paper;
    }

    void addPlayer(Player *playa){
        player = playa;
    }

    void input(){
        string line;
        string temp;
        int j = 0;
        string command;
        string text1;
        string text2;
        string text3;
        //cin.ignore();
        getline(cin, line);

        if (line != ""){
            j = line.find(" ");
            command = line.substr(0, j);
            if (command == ""){
                command = line;
            }
            line.erase(0, j + 1);
            if (line != ""){
                j = line.find(" ");
                text1 = line.substr(0, j);
                if (text1 == ""){
                    text1 = line;
                }
                line.erase(0, j + 1);
                if (line != ""){
                    j = line.find(" ");
                    text2 = line.substr(0, j);
                    if (text2 == ""){
                        text2 = line;
                    }
                    line.erase(0, j + 1);
                    if (line != ""){
                        j = line.find(" ");
                        text3 = line.substr(0, j);
                        if (text3 == ""){
                            text3 = line;
                        }
                        line.erase(0, j + 1);
                    }
                }
            }
        }
        
        for (auto q : commands)
        {
            if (command == q.second)
            {
                if (q.first == "Alias"){
                    
                    if (text1 != "" && text2 != ""){
                    new Alias(&commands, text2, text1);
                    }
                }
                else if (q.first == "Take"){
                    if (text1 != ""){
                        new Take(map, text1, text2, text3, player);
                    }
                }
                else if (q.first == "Open"){
                    if (text1 != ""){
                        new Open(text1, text2, text3, player, map);
                    }
                }
                else if (q.first == "Attack"){
                    if (text1 != "" && text2 == "with" && text3 !=""){
                        new Attack(text1, text2, text3, player, map, &win, &lose);
                    }
                }

                else if (q.first == "Help"){
                    new Helper(&commands);
                }
                else if (q.first == "Look"){
                    if ((text1 != "" && text1 != "at") || text2 != ""){
                        new Look(text1, text2, map->map[map->location]);
                    }
                }

                else if (q.first == "Debug"){
                    new Debug(map->map[map->location], &commands);
                }

                else if (q.first == "Inventory"){
                    new Inventory(player);
                }
                else if (q.first == "Go"){
                    
                    new Go(map,text1);
                }

            }

        }
        
    }

};

class Menu : public State
{
public:
    STATES update()
    {
        string command;
        cin >> command;
        if (command == "1")
        {
            return STATES::SELECTADV;
        }
        else if (command == "2")
        {
            return STATES::HALLFAME;
        }
        else if (command == "3")
        {
            return STATES::HELP;
        }
        else if (command == "4")
        {
            return STATES::ABOUT;
        }
        else if (command == "5")
        {
            return STATES::QUIT;
        }
        else
            return update();
    }
    void render()
    {
        cout << "Zorkish :: Main Menu\n--------------------------------------------------------\nWelcome to Zorkish Adventures\n1. Select Adventure and Play\n2. Hall Of Fame\n3. Help\n4. About\n5. Quit\nSelect 1-5:";
    }
};

class About : public State
{
public:
    STATES update()
    {
        string command;
        cin >> command;
        return STATES::MENU;
    }
    void render()
    {
        cout << "Zorkish :: About\n--------------------------------------------------------\nWritten by: 102569367\nEnter anything to return to the Main Menu";
    }
};

class Help : public State
{
public:
    STATES update()
    {
        string command;
        cin >> command;
        return STATES::MENU;
    }
    void render()
    {
        cout << "Zorkish :: Help\n--------------------------------------------------------\nThe following commands are supported:\nquit,\nhighscore (for testing)\nEnter anything to return to the Main Menu";
    }
};

string adv;

class SelectAdv : public State
{
public:
    STATES update()
    {
        string command;
        cin >> command;
        if (command == "1")
        {
            adv="map.txt";
            return STATES::GAMEPLAY;
        }
        else
            return update();
    }
    void render()
    {
        cout << "Zorkish :: Select Adventure\n--------------------------------------------------------\nChoose your adventure:\n1. Test World\nSelect 1:";
    }
};

class HallFame : public State
{
public:
    STATES update()
    {
        string command;
        cin >> command;
        return STATES::MENU;
    }
    void render()
    {
        cout << "Zorkish :: Hall Of Fame\n--------------------------------------------------------\nTop 10 Zorkish Adventure Champions\n"
             << "Enter anything to return to the Main Menu";
    }
};

class NewScore : public State
{
public:
    STATES update()
    {
        string command;
        cin >> command;
        return STATES::SFAME;
    }
    void render()
    {
        cout << "Zorkish :: New High Score\n--------------------------------------------------------\nCongratulations!\nYou have made it to the Zorkish Hall Of Fame\nAdventure: " /*the world*/ << "\nScore:" /*[the players score]*/ << "\nMoves:" /*[number of moves player made]*/ << "\nPlease type your name and press enter: ";
    }
};

class Gameplay : public State
{
public:
    Player jim;
    Map venture;
    CommandManager cmd;
    bool first = true;
    STATES update()
    {
        cmd.input();
        
        // cin.ignore();
        /*getline(cin, command);
        if (command == "quit"){
            cout << "Your adventure has ended without fame or fortune.\n";
            
        }
        else if (command == "highscore"){
            cout << "You have entered the magic word and will now see the \"New High Score\" screen.\n";
            return STATES::NEW_SCORE;
        }
        else if (command == "get spade"){
            jim.addItem("spade");
            cout << "spade added to inventory\n";
        }
        else if (command == "remove spade"){
            jim.removeItem("spade");
            cout << "spade removed from inventory\n";
        }
        else if (command == "show inv"){
            jim.showInv();
        }
        */
        //cout << venture.map[venture.location].directions() << endl;
        //cout << venture.map[venture.location].entities() << endl;
        
        //cin.ignore();
        //getline(cin, command);
        if (cmd.win){
            return STATES::NEW_SCORE;
        }
        if (cmd.lose){
            return STATES::GMENU;
        }


        return update();
    }
    void render()
    {
        //cout << "Welcome to Zorkish: Test World\nThis world exists to test gaining or deleting items\nCommands are: 'get spade', 'remove spade', 'show inv'\n";
        if (first){
            first = false;
            venture.adventure(adv);
            cmd.addMap(&venture);
        cmd.addPlayer(&jim);
        }
        cout << endl << venture.map[venture.location].desc << endl;
        
    }
};

class GameManager
{
private:
    STATES _state = STATES::MENU;
    std::stack<State *> _states;

public:
    bool running() const { return !_states.empty(); }
    State *current() { return _states.top(); }
    void push_state(State *state) { _states.push(state); }
    void pop_state()
    {
        delete _states.top();
        _states.pop();
    }
    ~GameManager()
    {
        while (!_states.empty())
            pop_state();
    }
    GameManager()
    {
        push_state(new Menu);
    };
    void update()
    {
        _state = current()->update();
        if (_state == STATES::MENU)
        {
            pop_state();
        }
        else if (_state == STATES::ABOUT)
        {
            push_state(new About);
        }
        else if (_state == STATES::HELP)
        {
            push_state(new Help);
        }
        else if (_state == STATES::SELECTADV)
        {
            push_state(new SelectAdv());
        }
        else if (_state == STATES::HALLFAME)
        {
            push_state(new HallFame);
        }
        else if (_state == STATES::NEW_SCORE)
        {
            push_state(new NewScore);
        }
        else if (_state == STATES::GAMEPLAY)
        {
            push_state(new Gameplay());
        }
        else if (_state == STATES::SFAME)
        {
            pop_state();
            pop_state();
            pop_state();
            push_state(new HallFame);
        }
        else if (_state == STATES::GMENU)
        {
            pop_state();
            pop_state();
        }
        else if (_state == STATES::QUIT)
        {
            pop_state();
        }
    }
    void render() { current()->render(); }
};

int main()
{
    GameManager manager;
    while (manager.running())
    {
        manager.render();
        manager.update();
    }
    return 0;
}