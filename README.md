# glfw-sharp

<p align="center">
  <a href="https://www.nuget.org/packages/glfw-sharp">
    <img src="https://buildstats.info/nuget/glfw-sharp">
  </a>
  <a href="https://travis-ci.org/jpbruyere/glfw-sharp">
    <img src="https://travis-ci.org/jpbruyere/glfw-sharp.svg?branch=master">
  </a>
  <a href="https://ci.appveyor.com/project/jpbruyere/glfw-sharp">
    <img src="https://ci.appveyor.com/api/projects/status/fdwb4e3ru7y8v3sp/branch/master?svg=true">
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
