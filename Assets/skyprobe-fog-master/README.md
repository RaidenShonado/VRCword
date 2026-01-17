# SkyProbe Fog

Area fog for Unity that uses the reflection probe to create fake atmospheric scattering. 

![Preview](https://gitlab.com/s-ilent/skyprobe-fog/-/wikis/uploads/3a960d33ed87708627e8333d8fbf3751/Unity_2020-11-28_13-03-00c.png)

## Installation

Download the repository. Then place the Shader/ folder with the shader into your Assets/ directory.


## Usage

Provided in this package are:

- The main SkyProbe shader.
- An example material.
- An example Prefab with a generic set up.

Drag the prefab to your scene and resize it to fit. This shader will always render over other objects, as it handles clipping as part of the fogging. It will fade the fog towards the horizon, both in depth and in world space

Note that, like other shaders which depend on the depth buffer, you'll need a directional light or other depth buffer activator (like disabled depth of field) to see the effect properly.

## License

This work is provided under the MIT license.