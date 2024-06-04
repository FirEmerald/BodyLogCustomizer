# BodyLog Customizer
Customize your BodyLog's colors to your liking!
## Features:
- Modify almost every aspect of your body log using:
	- Pre-made (amplify) shaders
	- Custom colors
- Complete Fusion sync:
	- Your custom BodyLog settings should automatically be synced and sent over fusion as long as the host has the mod too.
	- BodyLog settings from other players will be cached - if you've been in a server with someone and their BodyLog was synced, it will be cached and applied when you're in a server where the host doesn't have the mod. 

## How to use:
Copy the hex color you'd like to use, then press the button that you want to color with. It should change the button's color to the color you copied - if not, you'd have to make sure what you copied is right. Any spaces or line breaks will cause it to fail.

The mod will also take in the following, built in colors:
- red, cyan, blue, darkblue, lightblue, purple, yellow, lime, fuchsia, white, silver, grey, black, orange, brown, maroon, green, olive, navy, teal, aqua, magenta.

If you want to share your settings with others or modify your settings directly outside of bonelab, the save file can be found at `UserData/BodyLogCustomizer/LocalBodyLogPref.json`

Every time the mod is updated it will backup your old save and create a new one, that is because new parameters can be introduced and it can mess with the mod.

## Advanced Preferences
The MelonPreferences file is located at `UserData/BodyLogCustomizer.cfg`.
In that file, you can see the parameters "`ReplicateOnServer`" and "`ReplicateOnClient`"
- `ReplicateOnServer` - Send your BodyLog settings over to other players.
- `ReplicateOnClient` - Apply the BodyLog settings on your end.
If `ReplicateOnServer` is `True` and `ReplicateOnClient` is `False`, other players will see your colored BodyLog but you won't see it on your end.

## Changelog
#### 1.0.0:
- Initial Release