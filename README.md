## GlowingObjectOutlines
A method of rendering Glowing Object Outlines in Unity.

Dead-simple to use. Simply add the GlowObject script to whatever object you want to make glow. That's it, you're DONE.

Will make child objects glow as well (for ease of use with compound objects), but very simple to change if that's unwanted.

Does not support rendering to multiple cameras (as it's intended as a local HUD effect), but should be easy to implement if that's what you want,; just put the GlowController on an object separate from the cameras and add the buffer (in GlowController) to any camera that should render the glow (possibly using OnWillRenderObject() to optimize by only adding to cameras that can see a glowing object).


This fork is an optimization of the original repository. Read [the code](Assets/Scripts/GlowController.cs) for details.