# Code Samples

### A collection of code from a personal projects and uni work

#### C# Unity Project Code

An ongoing project to make a digital version of a board game. Of particular note is: Test, Zoom, StartingPosition, CharacterController, and Hotbar

Test is in charge of the main game loop and most inputs.

Zoom is in charge of the camera and facilitates zooming towards/away from the mouse, keeping the camera view within bounds, smoothing the zoom and moving the camera with inputs.

Starting position highlights specific tiles on the game's grid where the player characters can be spawned at. The list of possible spawn locations it can do is: The left & right edges, the bottom edge, 4 corners n away from an object in the centre, and all tiles n away from an object in the centre.

CharacterController is in charge of the player characters and what they can do on their turn.

Hotbar is in charge of the UI features such as buttons on the bottom of the screen,


#### C++ Zork

A uni project to make a zork-esque game. Task13 is the main zork game, it will load the data from the text document map and facilitate the game by taking text input.

Task19 is a simplified version of Task13 to showcase using it with a message board system, which would allow greater decoupling.


#### Python GUI

The part of a uni group project that I solely worked on. The greater project was to use machine learning to predict traffic flow to find the fastest route. This code is for the graphics user interface, allowing you too see the intersections as nodes, the roads, choosing the destination and source and cycling through the possible routes.
