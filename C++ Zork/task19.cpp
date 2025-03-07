#include <iostream>
#include <stack>
#include <stdlib.h>
#include <algorithm>
#include <vector>
using namespace std;

class Message{
    public:
    string from;
    string to;
    string message;
    string data;

    Message(string f, string t, string m){
        from = f;
        to = t;
        message = m;
        data = "";
    }
    Message(string f, string t, string m, string d){
        from = f;
        to = t;
        message = m;
        data = d;
    }

};

class Board{
    public: 
    vector<Message> messages;

    void addMessage(Message m){
        messages.push_back(m);
    }
    
    vector<Message> findMessages(string name){
        vector<Message> mess;
        for (auto p: messages){
            if (p.to == name){
                mess.push_back(p);
            }
	    }
        return mess;
    }

    void removeMessages(){
        messages.clear();
    }
};

class Objects{
    public:
    vector<Message> inbox;
    string name;

    virtual void update(Board &brd){

    }

    virtual void checkMessages(Board &brd){
        inbox = brd.findMessages(name);
    }

};

class Player: public Objects{
    public:
    vector<Message> inbox;
    string name = "player";
    int score = 0;

    void update(Board &brd){

        for (auto p: inbox){
            if (p.message == "score"){
                score += 1;
                cout << "score += 1" << endl;
            }
            if (p.message == "look"){
                cout << "The most stunningly handsome goblin the world has ever seen, by the name Player" << endl;
            }
	    }

        inbox.clear();
    }
    void checkMessages(Board &brd){
        inbox = brd.findMessages(name);
    }
        
};

class Window: public Objects{
public:
    vector<Message> inbox;
    string name = "window";

    void update(Board &brd){

        for (auto p: inbox){
            if (p.message == "look"){
                cout << "The most beautifully painting of a window this cave has ever seen. Staring into it is like being transported to another world" << endl;
            }
	    }
        inbox.clear();
    }
    void checkMessages(Board &brd){
        inbox = brd.findMessages(name);
    }
};

class Chest: public Objects{
    public:
    vector<Message> inbox;
    string name = "chest";
    bool closed = true;
    

    void update(Board &brd){
        for (auto p: inbox){
            if (p.message == "open"){
                if (closed){
                    if (p.data == "12345"){
                        cout << "The code works! The chest opens to reveal a single gold coing!" << endl;
                        Message mess(name,p.from,"score");
                        brd.addMessage(mess);
                    }
                    else {
                        cout << "It's locked" << endl;
                    }
                }
                else {
                    cout << name << " is already opened";
                }
            }
            if (p.message == "look"){
                cout << "A chest with a combination lock. A sticky note next to the lock says '12345'" << endl;
            }
	    }

        inbox.clear();
    }
    void checkMessages(Board &brd){
        inbox = brd.findMessages(name);
    }
        
};




class Input{
    public:
    
    void input(Board &brd){
        string line;
        string temp;
        int j = 0;
        string command;
        string text1;
        string text2;
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
                }
            }
        }
        Message mess("player",text1,command,text2);
        brd.addMessage(mess);
    }
};




int main(){
    Board blackboard;
    bool run = true;
    vector<Objects*> objs;
    Input putter;
    objs.push_back(new Player);
    objs.push_back(new Window);
    objs.push_back(new Chest);
    cout << endl;
    cout << "You're in a room. There is a chest with a combination lock. There is also a window." << endl;
    while (run){
        
        if (blackboard.messages.size() == 0){
            cout << endl;
            putter.input(blackboard);       
        }
        
        for (auto p: objs){
            p->checkMessages(blackboard);
	    }
        blackboard.messages.clear();
        for (auto p: objs){
            p->update(blackboard);
	    }


    }
    return 0;
}