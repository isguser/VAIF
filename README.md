# VAIF
The Virtual Agent Interaction Framework (VAIF) is a Unity package that allows users to create intelligent agents with minimal effort.

## How to Use
### Import To Unity
1. Download the code from the GitHub repository.
2. Copy the contents of the Assets folder to your Unity Project's Assets folder.
3. Open Unity for use! (may take some time to load these new additions to the project)

### Create a Timeline (an empty gameobject in the hierarchy)
1. Add to this gameobject the following components (in the following order): ConversationIM (Script), InteractionManager (Script), EmotionCheckManager (Script), ResponseManager (Script), TriggerManager (Script), WaitManager (Script), WildcardManager (Script), MemoryCheckManager (Script).
2. Using the ConversationIM (Script) component, add a Conversation to this Timeline by clicking on the "Add Conversation" button on the ConversationIM component. This will add the Conversation to the list of conversations for this Timeline.
3. A new Conversation gameobject component will appear in the hierarchy under Timeline. It will contain the following components: Conversation (Script), JumpManager (Script), and EventIM (Script).

### Create a Conversation
1. Once a conversation is created, events can be added to it.
2. Click on the Conversation to which you want to add events.
3. Add an Event to a Conversation by choosing the type of event to add in the EventIM (Script) component.
4. Clicking "Add Event" upon selection of the type of event will add the event and the appropriate scripts needed for that event. The event will be added to the hierarchy of Conversation.
5. Every type of event requires the following objects to be assigned BEFORE runtime: NextEvent (see Exceptions for more details), Agent, WantInRange, and wantLookedAt.

#### Exceptions
1. If the event is a Response, the nextEvents list MUST be filled to match the choices of responses. For each possible response, a nextEvent must be defined in this list.
2. If the event is a Animation, you MUST define the animation to use (this is from the list of animations in that Agent's AnimationManager; it must be in the _resources folder).
3. If the event is a Dialog, you MUST define the dialog to use (this is from the list of dialogs in the _resources folder).
4. If the event is the last event of the Conversation, it's nextEvent can be empty, but you must check the IsLastEvent field.

## Features of VAIF
### Current Features
1. Wildcards: handle general verbal inputs (no need for recognition)
2. Responses: handle specific responses (recognition by keyword phrases)
3. Multi-Agent interaction: navigating between Conversations to interact with various agent(s). A user can leave a conversation, and return to it to replay the previous event.

### Soon-To-Come Features
1. GazeManager: control agent eye-gaze and head movement -- requires blendshape (facial expressions)

### Future Work
1. Network Manager: add multiplayer support
2. EmotionCheckManager: based on memory (state of mind) -- requires blendshape (facial expressions)

## History of Releases
### Current: VAIF v3.2


### VAIF v3.1
Refactored Conversation code to working order.

### VAIF v3.0
Added Conversations. Needs major debugging.

### VAIF v2.0
The tool now allows for generalized state management (AgentStatusManager and ESV), rather than relying on only listening/speaking.
1. Responses: Fixed to recognize inputs
2. JumpManager handles the sequence of events within a conversation.

### VAIF v1.0
This previous version tracks when EventIM type instances (Response, Dialog, Animation, etc.) are completed. No more running through a list of events!