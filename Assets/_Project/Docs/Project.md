# Quarter-life Project 
This is a learning project for developers to learn the basics of game development.

The initial project is to create a 3D first person shooter in Unity with 
similar movement mechanics and systems to Half-Life.

We are paying homage to the original Half-Life game by Valve Software with the game
recently celebrating its 25th anniversary.

## Scope

### Timeline
The project will be built in thin vertical slices.
The idea is to create limited end-to-end functionality for each slice.

Ambitious Project Goal:
- Complete a basic implementation of the full functionality of the game before end of of 2024

### Mechanics
Our mechanics are based on the the Source engine. 
The movement in central to the game and will be a major focus of the project.

One additional component that is central to our game is we are playing as a 
civilian and the combat suit is what gives us the ability to fight.
Without it we are just a regular person and have no idea how to even use the weapons.

We will be implementing an accuracy mechanic as well where the faster you move the
less accurate your shots will be.

Mechanics overview: 
- Player movement
  - Run by default
  - Walking
  - Crouch
  - Counter-strafing
    - ability to counter player momentum
- Player actions
  - Jump
    - Air Strafing
    - Crouch-Jump
  - Weapon mechanics
    - Shooting
    - Weapon recoil
    - Weapon spread
    - Weapon reload
    - Weapon switch
    - Weapon pickup
    - Weapon drop
    - Reload
  - Interact

### Items
The player will be able to pick up items in the game world that will help them.
Interacting with items will be a key part of the game.

- combat suit
- keycards / access cards
- health items 
- ammo
- armor / suit battery

### Systems
These are core systems that must be implemented.

- UI system
- Save system
- Checkpoint system
- Inventory system
- Combat system
  - Ammo system
  - Armor system
  - Health system
- Enemy AI
  - Fuzzy logic
- Dialogue system / NPC scripting
- Sound system
- Map Interaction System

### Weapons
These are the minimal weapon types we will implement.
At a minimum we will implement one of each weapon type.

- pistol
- shotgun
- rifle / automatic main weapon
- machine gun / heavy weapon
- sub-machine gun / fast firing weapon
- special weapon / experimental weapon
- creature based weapon
- rocket launcher / explosive projectile weapon
- crowbar / melee weapon
- grenades

### Enemies
We will have a variety of enemies in the game.
At a minimum we will have one of each enemy type.

- Mercenaries
- Military
- Strange Creatures

- Our coding skills
... will forever be the bain of our existence

### Levels
For the initial project we will create at least a single level that will
incorporate all the mechanics and systems we have created.

### Story
We have an initial story which we want the development team to expand upon.

#### Initial Story Outline
- The player is a programmer working at a research facility
- An event occurs that causes the facility to go into lockdown
- There is chaos in the facility and no ones knows what is happening
  - Various enemy factions are fighting each other
- The player must escape the facility
