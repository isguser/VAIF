# Overview

## What is VAIF?
The Virtual Agent Interaction Framework (VAIF) is a Unity package that allows users to create intelligent agents with minimal effort.

## What is an Agent?
An Embodied Conversational Agent is a virtual character who is able to mimic human-like characteristics, ranging from speech to gestures. 

## Features of VAIF
### Current Features
Wildcards: handle general verbal inputs (no need for recognition)
Responses: handle specific responses (recognition by keyword phrases)
Multi-Agent interaction: navigating between Conversations to interact with various agent(s). A user can leave a conversation, and return to it to replay the previous event.
### Soon-To-Come Features
GazeManager: control agent eye-gaze and head movement -- requires blendshape (facial expressions)
MemoryManager: save and use events with each agent/user combination
GestureManager: recognize gestures from users during interaction
### Future Work
Network Manager: add multiplayer support
EmotionCheckManager: based on memory (state of mind) -- requires blendshape (facial expressions)

## History of Releases
### Current Version: VAIF 3.2
### VAIF 3.1
Refactored Conversation code to working order.
### VAIF 3.0
Added Conversations. Needs major debugging.

### VAIF 2.0
The tool now allows for generalized state management (AgentStatusManager and ESV), rather than relying on only listening/speaking.
Responses: Fixed to recognize inputs.
JumpManager handles the sequence of events within a conversation.

### VAIF 1.0
This previous version tracks when EventIM type instances (Response, Dialog, Animation, etc.) are completed. No more running through a list of events!

# Required: Install Unity (version 2017.1 or later)
1. Install Unity 2017.1 or later at this page: https://unity3d.com/get-unity/download
2. Visit the VAIF package from the GitHub repository (available at https://github.com/isguser/VAIF).
3. Download the VAIF Unity Package. <br/>
Tutorial Video: https://youtu.be/vQVJNQF6blQ

# Downloading the GitHub Repository to your Project
1. Visit github.com
2. In the searchbar at GitHub.com, search for VAIF.
3. Click on isguser/VAIF
4. Click the VAIF.unitypackage file in the repository.
5. Click the Download option and the wait for the file to finish downloading to your computer.
6. In Unity, create a new Unity Project. You can also import VAIF to a saved Unity project.
7. Select “Assets” > “Import Package” > “Custom Package”
8. Navigate to the directory where you downloaded the VAIF Unity Package, and select the VAIF.unitypackage file.
9. Click “Open” and wait for Unity to unpack all the materials.
Tutorial Video: https://youtu.be/PBTecJiWUFU

# The Example Scene
Included in the downloadable package on Unity is an example scene in Unity called VAIF. This package contains an example of each of the types of events to create. You may have to convert this project to a different version of Unity, since it is set for Unity 2017.3.1f1.
1. Download the entire GitHub repository as a ZIP folder.
2. Extract the zip folder.
3. Open Unity and click Open. Navigate to the directory where you extracted the project folder.
4. Look at the example of two agents and an example of each of type of Event.

# Create a Timeline
A timeline consists of one or more conversations and represent the sequence of events in your scene and when they will be played. 
1. From the top menu add an empty GameObject. Click on the GameObject dropdown menu and select Create Empty. (or use keyboard shortcut Ctrl+Shift+N)
2. Rename this GameObject to Timeline.
3. Create a new Tag in the Timeline GameObject and call it Timeline. Attach this Tag to the Timeline GameObject.
4. Double click on the Timeline GameObject to open the Inspector Window. Click ‘Add Component’ to add the following components (in the following order) to the Timeline:
	1. Conversation IM - Script
	2. Interaction Manager - Script
	3. Emotion Check Manager - Script
	4. Response Manager - Script
	5. Trigger Manager - Script
	6. Wait Manager - Script
	7. Wildcard Manager - Script
	8. Memory Check Manager - Script
5. Now that you’ve created a Timeline, you can add Conversations to it. <br/>
Tutorial Video: https://youtu.be/14CE7vWCBog

## Create a Conversation
You need to create a Conversation in order to add Events to play.
1. Click on the Timeline GameObject in the hierarchy.
2. In the Conversation IM (Script) component, click on the Add Conversation button. This will add the conversation and the appropriate scripts needed for it. The conversation will be added to the hierarchy of Timeline.
		_Developers are working on a feature of different types of Conversations._
3. Click on the Conversation GameObject. Create a new Tag in the Conversation GameObject and call it the same name as you chose in Step 2 (in our example, ConversationA). Attach this Tag to the Conversation GameObject you created (see the hierarchy on the left).
4. Now that you’ve created a Conversation, you can add Events to it. <br/>
Tutorial Video: https://youtu.be/rHPDXOi37Uo

## Types of Events
Before adding Events to a Conversation, scene designers must understand the different types of Events. Each list contains different requirements in order to run.
* __General Events__ - These events do not require to specify the Next Event to play in the Timeline:
	* Animation
	* Dialog
	* Move
	* RotateTo
	* Wait
* __Specific Events__ - These events require different “Next Event” specifications in order to play in the Timeline. These are helpful for branching out inside the interaction that are conditional to user’s responses:
	* Response
	* Wildcard <br/>
Tutorial Video: https://youtu.be/n6UcQq88gk4

## Adding General Events to a Conversation
You need to create a Conversation in order to run Events.
1. Click on the Conversation GameObject in the hierarchy to which you want to add events.
2. In the Inspector window, look at the “Event IM (Script) component”.
3. Select the Agent that the Event will be attached to. For example, an Animation event attached to your Agent of choice.
4. Click Add Event by choosing the type of event to add in the “Event IM (Script) component”.
5. Clicking "Add Event" will add the event and the appropriate scripts needed for that event. The event will be added to the hierarchy of Conversation.
6. Every type of event requires Settings and GameObjects to be assigned before run/play of a scene. See the Exceptions section for additional details on these additional settings:
	1. Next Event,
	2. Agent,
	3. Want In Range, and
	4. Want Looked At. <br/>
Tutorial Video: https://youtu.be/NRj5FynCpLs

### Editing the Fields and Settings for Events
Each event type has the following settings and fields that need to be modified.
#### Some Exceptions: General Events
* If the event is an Animation, you must define the animation to use (this is from the list of animations in that Agent's “AnimationManager”; it must be in the _resources folder).
* If the event is a Dialog, you must define the dialog to use (this is from the list of dialogs that must be in the _resources folder).
* If the event is the last event of the Conversation, it's “Next Event” can be empty, but you must check the “Is Last Event” field.
#### Editing an Event’s “Next Event” Field
The Next Event is saved as a GameObject. This means you will have to drag and drop into this field the event you want to play after the event you are editing completes.
1. Find the next event to play (after this one) in the hierarchy of the Conversation.
2. Drag and drop the event to the field of Next Event.
3. Click on the box; it should highlight in yellow (in the hierarchy) which event is this event’s Next Event (the one you dragged).
#### Editing an Event’s “Agent” Field
The Agent is saved as a GameObject. This means you will have to drag and drop into this field the agent which will be targeted for this event.
1. Find the agent in the hierarchy window.
2. Drag and drop the agent to the field of Agent.
3. Click on the box; it should highlight in yellow (in the hierarchy window) which agent is this event’s Agent (the one you dragged).
#### Editing an Event’s “Want In Range” Field
An event’s “Want In Range” is a required setting. This setting, like the Want Looked At, has three possibilities that you must choose from as a condition to starting this event (in combination with Want Looked At):
* TRUE (the user must be near the agent referenced in Agent),
* FALSE (the user must not be near the agent referenced in Agent), or
* DONTCARE (the event will play regardless of the user’s location).
#### Editing an Event’s “Want Looked At” Field
An event’s Want Looked At is a required setting. This setting, like the Want In Range, has three possibilities that you must choose from as a condition to starting this event (in combination with Want In Range):
1. TRUE (the user must look at the agent referenced in Agent),
2. FALSE (the user must not look at the agent referenced in Agent), or
3. DONTCARE (the event will play regardless of the user’s gaze direction).<br/>
Tutorial Video: https://youtu.be/VkH8iyRZmsY

### Exceptions: Specific Events
#### Response Events
The settings for a “Response” event are unique. The settings for the following fields are the same as General Events:
* Agent
* Want In Range
* Want Looked At
* Is Last Event
This event does not fill the “Next Event” field until run time. The following fields require additional settings:
1. Response Items - A list of the phrases which will be recognized as a response to a previously played Dialog event.
	1. Adjust the size. The Size of Response Items must match the Size of Jump IDs.
	2. Fill each of the Elements with a phrase.
	3. For each Response Item Element, you must set a Jump ID (see below).
		* For example, a Response Item like ‘no’ at Element 0 may jump to some Animation event. This means that you will drag and drop the Animation event to Element 0 of the Jump ID list.
		_NOTE_:
			* Adding a ‘yes’, adds the following phrases as similar recognitions:
				* yeah / uh-huh / yup/yep / okay / sure / sounds good / that sounds good / cool / alright
			* Adding a ‘no’, adds the following phrases as similar recognitions:
				* nay / nah / nope / no way / no, thank you no, thanks / i don’t think so 
			* Adding an ‘I don’t know’, adds the following phrases as similar recognitions:
				* I’m unsure / I’m not sure / I dunno / maybe / I guess / I have no idea / I have no clue
			* If the user asks a question requesting a repeat, such as any of the phrases listed below, the system will automatically replay the previous Dialog event:
				* what / huh / I don't know what you said / what was that / what did you say / say that again / say it again / come again / can you repeat that / can you repeat what you said / can you repeat what you just said / can you repeat the question / can you say that again / can you say it again / repeat it / repeat that / repeat yourself / repeat please / repeat what you said / repeat what you just said / repeat the question
2. Jump IDs - A list of events to choose which event plays next when a Response Item is uttered by the user (and recognized by the system).
	1. Adjust the size. The Size of Jump IDs must match the Size of Response Items.
	2. Drag and drop the event from the hierarchy window to this field to play this event when its matching Response Item is uttered and recognized.
	3. Each Jump ID must be set for a Response Item (see above).
	4. Like in the above example, you will drag and drop an Animation event to Element 0 of the Jump ID list to be the Next Event when ‘no’ (stored in Element 0 of the Response Items list) is uttered and recognized.
3. Timeout - The time (in seconds) to wait for an utterance by the user before jumping to the “Timeout Jump ID” event.
4. Timeout Jump ID - The event to play when the time in “Timeout” is exceeded.
5. Misrecognitions
		_Developers are working on this feature._
6. Base Case Jump ID
		_Developers are working on this feature._ <br/>
Tutorial Video: https://youtu.be/P-UZLwPy8Ps

#### Wildcard Events
The settings for a “Wildcard” event are unique. The settings for the following fields are the same as General Events:
* Agent
* Next Event
* Want In Range
* Want Looked At
* Is Last Event
This event does fill the “Next Event” field. The following fields require additional settings:
1. Timeout - The time (in seconds) to wait for an utterance by the user before jumping to the “Next Event”.
2. Annotation
		_Developers are working on this feature._ <br/>
Tutorial Video: https://youtu.be/0PoUe0W9IAk

## Creating Your Agent
### Adding the Managers
Your Agent will be composed of several managers. Depending on the goal of your application you may want to add all managers, or omit some managers depending on which features you would like to include.
1. Agent Status Manager
This keeps track of what the agent is currently doing. Below are the states and when they are marked true:
* _Speaking_ - The agent is playing a dialog.
* _Listening_ - The agent is listening to the user
* _Waiting_ - The agent is waiting until the next event
* _In Range_ - The user is close enough to the Agent
* _Moving_ - The agent is currently moving towards a given target location
* _Looked At_ - The agent is being looked at by the user
2. Move Manager
Dictates to the agent where to move next and calculates which animation to play with. 
The required scripts and components are: 
* _Third Person Character_ - Script
* _Nav Mesh Agent_ - Script
* \_agent\_animation\_WLocoMotion - Animator 
	* Located in Resources > \_Animations
3. Memory Manager (Future Implementation)
Develops a memory database for each user, keeping track of the agent and events in an interaction.
4. Emote Manager (Future Implementation)
With facial blendshapes, your agent will be able to display specific facial expressions as controlled by the emote event. 
5. Dialog Manager
Grabs the audio file from the dialog event and plays the audio file with the correct phonemes to this agent.
6. Animation Manager
This manager grabs all the animations possible from the animator attached to the agent in alphabetical order. So when an Animation event occurs, this manager plays the correct animation.
7. Look At Collisions
A box collider that is attached to the Player to keep track if the Player is looking at the Agent. 
8. Nav Mesh Agent 
This allows your Agent to move based on any given target.
9. Personalities (Future Implementation)
Eventually, Agents will be able to represent themselves based on the type of personality you instruct. 
10. Emotions (Future Implementation)
This manager keeps track of display of emotions for the agent.
11. Gaze (Future Implementation)
Make the agent look at camera (participant inside unity scene) and other specific objects inside the unity scene.
