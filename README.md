# TurnBasedFeest
This code is not to be used for any purpose by someone else

A big party

To remember get on track on where development ended, below is a summary of the current master branch version.
Currently the game consists of a state that has:

A previous, current and next event.
A list of hardcoded events with an event counter.
a list of actors.

The architecture of the whole game revolves around events. An event simply encapsulates something that is initialized and executes something returning false if it is not done and returning true if it is done, indicating that the next event can take place. This kind of approach was chosen because everything in a turnbased game could be seen as an event. From interacting with the UI in combat, to going through a conversation dialogue, almost nothing has a deterministic "ending" which we can hardcode, we do not know how long something can take as it is often dependent on player input, this is why everything is modeled as an event that signs when it is done. The event should also take care in setting up the next event that is played.
This is really the meat of the main game loop and apart from some content loading is the main meat of game1.cs.

A texture factory was build to import/access textures in a central place.
An input class was build to keep track of user input.

Two actors get initialized for now to add something to test the events. Actors are characters with a name, color, health, texture, actionset, behaviour and boolean to determine whether or not it is an enemy or an ally. They also have two list of attributes which get used during battles to store afflicted conditions etc.

Other than this nothing interesting happens in the game loop.

Until now there are three events:
event determiner events
battle events
Rest events

The rest events is a simple event that gives the player a choice between resting or exploring, to ask the player this, it makes use of a multiplechoice event.

The event determiner event decides what the new event should be, it can initiate a random event or a hardcoded event based on the event counter.

the battle events start with a battle begin event, followed by a battle turn event, which ends with a battleendevent.
During the battle turn event battle events are executed. Each actor has a turn and chooses an action to do.
