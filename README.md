# Code Samples

### A collection of code from a personal project/game I'm working on in C#/unity

Test is in charge of the main game loop and most inputs.

Zoom is in charge of the camera and facilitates zooming towards/away from the mouse, keeping the camera view within bounds, smoothing the zoom and moving the camera with inputs.

Starting position highlights specific tiles on the game's grid where the player characters can be spawned at. The list of possible spawn locations it can do is: The left & right edges, the bottom edge, 4 corners n away from an object in the centre, and all tiles n away from an object in the centre.

CharacterController is in charge of the player characters and what they can do on their turn.

Hotbar is in charge of the UI features such as buttons on the bottom of the screen,
