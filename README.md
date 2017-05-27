# FancyScans 
#### A Shaders + Basic scripts pair for Unity3D for animated scan lines over transparent geometry.

![](https://github.com/cjurjiu/FancyScans/blob/master/gifs/transparent_plane.gif)

Possible uses: No Man's Sky like scan lines over terrain, as seen below:

![](https://github.com/cjurjiu/FancyScans/blob/master/gifs/first_person_terrain.gif)

This is basically a version of the shader+script pair presented [here](https://www.youtube.com/watch?v=OKoNp2RqE9A), modified to additionally support transparent geometry. 

Instead of using the depth buffer & an image effect for the scan lines, it adds the scan lines to the object itself in order to support transparent objects. 

This project contains two shader&script pairs, in the **FancyScans** folder:
 * *SingleScanShader.shader* & *SingleScanController.cs* - supports only one scan line active at a time.
 * *MultiScanShader.shader* & *MultiScanController.cs* - supports up to 5 scan lines rendered simultaneously. 
   * New scans are triggered automatically at set intervals. 
   * When the 6th line is created, the oldest line is discarded.
   * If you need more than 5 lines, you'll have to edit the shader & the [Range...] annotation in the script file.

The shaders could easily be converted to a plain-old, more efficient, vertex/fragment shaders, instead of being Unity's surface shaders. 

Sample project with the scenes used to generate the gif in the **FancyScansSampleProject** folder.

Enjoy!
