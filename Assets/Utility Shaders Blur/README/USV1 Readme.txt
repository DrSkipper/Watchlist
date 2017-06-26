Distortion Shaders README


RECCOMENDED:  Textures should use mip-maps, texture wrapping should be disabled (set to clamp) 
and should be no smaller than 128x128.  Failure to do so may affect quality.  For larger objects, 
a higher quality of blur is recommended.  These are not recommended for screen-space post processing.




//	Blur, Directional Blur, Radial Blur, Zoom Blur

Basic Box Blurs available at several quality levels (Low, Medium, High, Ultra and Max) with associated 
performance impact. 
 
Use the Blur slider to change how much the texture is blurred, and the texture size is important for 
helping the blur algorithm use mip chains.  

Bias changes the blur offset weightings to produce a somewhat more defined blur (test to see if it’s 
appropriate for your use). 

Blur Max Gauss 5x5 is an approximation of a Gaussian 5x5 blur as a realtime shader.  It’s the most 
expensive to render and has the lowest blur distance, but for a nice, small soft blur it’s the highest 
quality option.

Directional Blur Field uses a vector field to control the blur direction.  This is the Red and Green 
(RG) channels of a texture, and will accept normal maps. 



//	Drop Shadow

Works much the same as a High Quality Blur (see above).  Image used must have Alpha Channel, with white 
being the object to cast a shadow and black indicating transparency.  This does not blur the image 
directly, but creates, locates and blurs a shadow created from the alpha map.

You can toggle the transparency on the displayed image, and set a background colour if required. 


//	Cooldown

These shaders are similar to the ones used as ability cooldowns in LoL and DOTA 2. 
They all use a Cooldown Remaining variable clamped between (0 to 1) so they won’t accept a ‘seconds to 
ready’ value.  This Cooldown Remaining variable is “ready to use” at 0 and “just used” at 1, so upon 
using the ability, it will go from 1 to 0. If it is halfway through the cooldown, it will be 0.5, at 
¾ done it will be 0.25, and so on. 

Note that multiplied colours are doubled; so you can actually brighten the image if you need.  
Neutral gray (0.5 or 128) will leave the image unchanged.

Cooldown 1: Simple Toggle; change saturation, slight blur, multiply by colour.

Cooldown 2: Graduated Toggle; change saturation from 0 to [value] over time, slight blur, multiply by colour.

Cooldown 3: Linear Wipe; vertical/horizontal wipe (toggle directions), change saturations, slight blur, multiply by colours.

Cooldown 4: Radial Wipe; toggle direction, change saturations, slight blur, multiply by colours.

Cooldown 5: Circular Wipe; toggle direction, change saturations, slight blur, multiply by colours.

Cooldown 6: Custom Wipe; Texture Gradient based wipe, toggle direction, change saturations, slight blur, multiply by colours.



//	Distortion

These distort the supplied image by several methods and fade to [color].  Where appropriate, 
insert the texture size.  They respond primarily to the first slider in the shader/material, 
usually the name of the effect. 

Distortion Mip:  Simply slides through the mip chain of the texture to [color].  Very cheap.

Distortion Blur:   Simple blur and mip the texture to [color]. Cheap.

Distortion Pixelate:   Will do a down-sample of the texture to a lower resolution, pixelating.
 
Distortion Noise: As above, but with a randomly sampled noise offset.

Distortion Texture:  Distorts an image by a second texture’s RG channels.
 

