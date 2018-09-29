# Overview

## What is VAIF?
The Virtual Agent Interaction Framework (VAIF) is a Unity package that allows users to create intelligent agents with minimal effort.

## What is an Agent?
An Embodied Conversational Agent (ECA) is a virtual character who is able to mimic human-like characteristics, ranging from speech to gestures. 

## Features of VAIF
### Current Features
1. Unity GUI Usability: hover over fields in the script components of the Unity GUI (see Section 1.3.1)
2. Multi-Agent interaction: navigating between Conversations to interact with various agent(s). A user can leave a conversation, and return to it to replay the previous event.
3. Wildcards: handle general verbal inputs (no need for recognition)
4. Responses: handle specific responses (recognition by keyword phrases)
### Soon-to-Come Features
1. GazeManager: control agent eye-gaze and head movement -- requires blendshape (facial expressions)
2. MemoryManager: save and use events with each agent/user combination
3. GestureManager: recognize gestures from users during interaction
### Future Work
1. Network Manager: add multiplayer support
2. EmotionCheckManager: based on memory (state of mind) -- requires blendshape (facial expressions)

## History of Releases
### Current Version: VAIF 3.2
Improved usability and organization
### VAIF 3.1
Refactored Conversation code to working order.
### VAIF 3.0
Added Conversations. Needs major debugging.
### VAIF 2.0
1. The tool now allows for generalized state management (AgentStatusManager and ESV), rather than relying on only listening/speaking.
2. Responses: Fixed to recognize inputs.
2. JumpManager handles the sequence of events within a conversation.
### VAIF 1.0
This previous version tracks when EventIM type instances (Response, Dialog, Animation, etc.) are completed. No more running through a list of events!

# What the Unity Project Directory Looks Like (with VAIF imported)
The parent directory of the Unity Project is generally organized into several folders:
* Assets
	* \_scripts (where you can add custom C# scripts for your game)
	* \_steamVR (which is part of the SteamVR asset library)
	* Editor (which contains Unity GUI scripts)
	* InteractionManager (which contains the scripts of the VAIF system)
	* Oculus (which contains required prefabs and components for OVRLipSync)
	* OVRLipSync (which is part of the OVR asset library)
	* Resources (which is where you will organize your Agents, Animations, Dialogs, Prefabs, and Scenes)
	* Runtime (which you can ignore)
	* Utilities (which you can also ignore)
	* VRTK (which contains the VR Toolkit asset library)
* Library
* ProjectSettings
* UnityPackageManager <br/>
The Library, ProjectSettings, and UnityPackageManager all contain files needed for Unity, they are generally unmodified by the average user.

# Included in VAIF Package
## The Quick Help Guide and Links to Tutorial Videos
See the Quick Help Guide for step-by-step guidance for each of the following steps to using VAIF.
### Required: Install Unity (version 2017.1 or later; Section 2)
You must install Unity 2017.1 or later.
### Downoading and Importing VAIF to your Project (Sections 3 and 4, respectively)
Tutorial Video: https://youtu.be/vQVJNQF6blQ <br/>
Tutorial Video: https://youtu.be/PBTecJiWUFU
### Create a Timeline (Section 6)
A timeline consists of one or more conversations and represent the sequence of events in your scene and when they will be played. <br/>
In Assets > Resources > \_Prefabs Go to the Prefabs folder and drag Timeline Prefab into the hierarchchy. <br/>
Tutorial Video: https://youtu.be/14CE7vWCBog
### Create a Conversation (Section 6.1)
You need to create a Conversation in order to add Events to play. Conversations are groups of events where players (or users) will interact with ECAs or agents. <br/>
Change the name of the object to reflect the mission of your conversation. For example, Conversation will be renamed to Conversation_AgentB to reflect a conversation with AgentB. <br/>
Tutorial Video: https://youtu.be/rHPDXOi37Uo
### Types of Events (Section 6.2)
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

### Adding Events to a Conversation (Section 6.3)
You need to create a Conversation in order to run Events. See Section 6.1 Create a Conversation. <br/>
Tutorial Video: https://youtu.be/NRj5FynCpLs
#### Editing the Fields and Settings for General Events
Section 6.3.1 Editing the Fields and Settings of Events <br/>
Tutorial Video: https://youtu.be/VkH8iyRZmsY
#### Exceptions: Specific Events (Response Events)
The settings for a “Response” event are unique. The settings for the following fields are the same as General Events:
* Agent
* Want In Range
* Want Looked At
* Is Last Event <br/>
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
				* what / huh / I don't know what you said / what was that / what did you say / say that again / say it again / come again / can you repeat that / can you repeat what you said / can you repeat what you just said / can you repeat the question / can you say that again / can you say it again / repeat it / repeat that / repeat yourself / repeat please / repeat what you said / repeat what you just said / repeat the question <br/>
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
#### Exceptions: Specific Events (Wildcard Events)
The settings for a “Wildcard” event are unique. The settings for the following fields are the same as General Events:
* Agent
* Next Event
* Want In Range
* Want Looked At
* Is Last Event <br/>
This event does fill the “Next Event” field. The following fields require additional settings:
1. Timeout - The time (in seconds) to wait for an utterance by the user before jumping to the “Next Event”.
2. Annotation
		_Developers are working on this feature._ <br/>
Tutorial Video: https://youtu.be/0PoUe0W9IAk
### How to Create Agents (Section 7 Creating Your Agents)
#### Import the Agent
Two Agents are included in the VAIF asset. The 3D object files (.fbx) of these agents can be found in: Assets > Resources > _Agent > Laura.fbx or Taema.fbx. To customize any of the agents, you can modify the materials of the model to change the agent's appearance in hair or clothes.
#### Add the Managers
Your Agent will be composed of several managers. Depending on the goal of your application you may want to add all managers, or omit some managers depending on which features you would like to include. <br/>
Tutorial Video: _to come soon_

## The ExampleScene
Included in the downloadable package on Unity is an example scene in Unity called ExampleScene. This Unity scene contains an example of the different types of events to create. You may have to convert this project to a different version of Unity, since it is set for Unity 2017.3.1f1.
