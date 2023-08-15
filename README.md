# Custom-Progress-Bar
A customizable progress bar package for Unity3D.

Ever felt alone trying to create a progress bar out of the default Unity Slider? Weird image alterations after you dive down and edit the components... Well, at least I have been there. 
Aside from the pain of forging the Unity Slider to your own needs, it is quite poor performing because the component itself runs a lot of code that you probably dont ever need. So, the solution here is a custom made component from scratch.

The Custom Progress Bar is capable of customizing with color combinations, options such as discrete and continuous bar steps, increment/decrement buttons and so much more coming on...

This completely free package is easy to use, just with a right click to hierarchy, UI>Custom>Progress Bar and all is ready to use!

Feel free to give credits. (I would realllyy appreciate it!)


## Creating Progress Bar
![Create Progress Bar](https://github.com/batuhanegesen/Custom-Progress-Bar/assets/31734928/2e7c8bef-49ae-4625-82d4-1b2cd31587ef)

Right click and open the context menu, navigate as follows: `UI\Custom\Progress Bar`.
> [!NOTE]
> If the Canvas or the Event System missing, they will be added to the scene automatically.

The Progress Bar name in the hierarchy panel changes dynamically if you change it from inspector.

## Tweaking the Progress Bar
![Testing the progress bar](https://github.com/batuhanegesen/Custom-Progress-Bar/assets/31734928/db0c913c-7531-494e-a16d-be76260e89f4)

There are several options in this version of Custom Progress Bar.
* Icon
* Bar Title
* Object Pool Size
* Starting Value
* Slice Mode / Continuous Mode
* Show Value
* Show Buttons
* Show as Percentage
* Enable/Disable Colors

> [!IMPORTANT]
> The Bar Length can not be greater than the Object Pool Size since the segments are Dequeued from pool.
> In order to have greater number of segments, create the pool with a greater number.

> [!WARNING]
> Modifying the original prefab in the resources might give in unwexpected results. Always back-up your work when you are working experimentally. 

