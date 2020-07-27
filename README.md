# glfw-sharp

<p align="center">
  <a href="https://www.nuget.org/packages/glfw-sharp">
    <img src="https://buildstats.info/nuget/glfw-sharp">
  </a>
  <a href="https://travis-ci.org/jpbruyere/glfw-sharp">
    <img src="https://img.shields.io/travis/jpbruyere/glfw-sharp.svg?&logo=travis&logoColor=white">
  </a>
  <a href="https://ci.appveyor.com/project/jpbruyere/glfw-sharp">
    <img src="https://img.shields.io/appveyor/ci/jpbruyere/glfw-sharp?logo=appveyor&logoColor=lightgrey">
  </a>  
  <img src="https://img.shields.io/github/license/jpbruyere/glfw-sharp.svg?style=flat-square">
  <a href="https://www.paypal.me/GrandTetraSoftware">
    <img src="https://img.shields.io/badge/Donate-PayPal-blue.svg?style=flat-square">
  </a>
</p>

Minimal [glfw](https://www.glfw.org/) bindings for dotnet used in my projects;

The naming of glfw library being different on windows and linux, I choosed to use the windows name ("glfw3"). Consider adding a dll map on linux in `/etc/mono/config`:
```xml
<dllmap dll="glfw3" target="libglfw.so" os="!windows"/>
```
