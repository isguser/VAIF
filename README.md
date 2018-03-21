# VAIF
The Virtual Agent Interaction Framework (VAIF) is a Unity package that allows users to create intelligent agents with minimal effort.

## Short-Term Features In Development
The following is a list of features that are currently in the process of development:
1. WildcardManager: handle verbal inputs
2. RotateManager: turn the agent smoothly
3. MemoryCheckManager: alter agent behavior based on memory (interactions with user(s)) -- requires logic/reasoning

### Long-Term Features to Implement
1. Network Manager: add multiplayer support
2. EmotionCheckManager: based on memory (state of mind) -- requires blendshape (facial expressions)
3. GazeManager: control agent eye-gaze and head movement -- requires blendshape (facial expressions)

## History
### Current Release
ResponseManager has been fixed to recognize inputs. JumpManager has been updated, which handles the sequence of events within a conversation. The tool now allows for generalized state management (AgentStatusManager and ESV) rather than relying on only listening/speaking.
### Previous Release
This previous version tracks when EventIM type instances (Response, Dialog, Animation, etc.) are completed. No more running through a list of events!