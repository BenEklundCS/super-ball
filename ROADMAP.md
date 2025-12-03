Super Ball Racer MVP:

- Create 1-10 levels with a start position and end position. The goal is to reach the end as fast as possible, which is timed by a timer.
- If the player falls off the map, they respawn at the start position (or last checkpoint)
- Physics/maps

Super Ball Arena MVP:

- Create a an arena level in a circular shape
- Create a sphere representing the players. (You can add a character inside the “ball,” or stick with a simple sphere)
- There are two possible control styles. You can either apply forces to the ball directly, causing it to roll, or change the slope of the map, allowing gravity to roll the ball. You can focus on one control scheme if you prefer.
- Implement Multiplayer with two player local and remote coop (esearch how this is done *before* starting the main game, so we can account for MP in the game architecture)
- Add a menu and a UI with a score counter, life counter, and timer.
- Add score and points to P1 when P2 falls, points to P2 when P1 falls. 
- Add a mechanic to knock other players off the map, like a fast roll or charge attack
- Accurately represent the position of both balls in the level, synced for both players playing

Notes:

https://docs.godotengine.org/en/stable/tutorials/networking/high_level_multiplayer.html High level multiplayer library, to avoid using TCP/UDP directly
