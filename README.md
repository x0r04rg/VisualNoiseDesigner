## Visual Noise Designer for Unity
<p align="center">
  <img alt="Noise Designer Image" src="http://i.imgur.com/9FPLzQW.png" width="60%"/>
</p>

Unity editor extension for working with [LibNoise.Unity](https://github.com/ricardojmendez/LibNoise.Unity) based on [Node Editor Framework](https://github.com/Baste-RainGames/Node_Editor). Both packages are included.

### Features
- Supports all noiselib modules except Curve, Terrace (it uses Unity's AnimationCurve instead) and Cache
- Noise calculations are multithreaded
- Resulting texture can be applied directly to TerrainData as heightmap or saved as PNG

### Usage
Open Visual Noise Designer window from Window/Visual Noise Designer. Right click for placing modules. Modules descriptions can be found [here](http://libnoise.sourceforge.net/docs/group__modules.html).

### License
My own code is released under MIT license. For included packages see: [LibNoise.Unity](https://github.com/ricardojmendez/LibNoise.Unity), [Node Editor Framework](https://github.com/Baste-RainGames/Node_Editor).