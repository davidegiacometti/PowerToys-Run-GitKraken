# PowerToys Run: GitKraken plugin

Simple [PowerToys Run](https://learn.microsoft.com/windows/powertoys/run) experimental plugin for quickly open [GitKraken](https://www.gitkraken.com/) repositories.

## Requirements

- GitKraken 9.x
- PowerToys minimum version 0.76.0

## Installation

- Download the [latest release](https://github.com/davidegiacometti/PowerToys-Run-GitKraken/releases/) by selecting the architecture that matches your machine: `x64` (more common) or `ARM64`
- Close PowerToys
- Extract the archive to `%LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins`
- Open PowerToys

## Localization

Pull requests for new languages or improvements to existing translations are welcome.
- Fork and clone this repository
- Open `Community.PowerToys.Run.Plugin.GitKraken.sln` in Visual Studio
- In Solution Explorer, navigate to `Community.PowerToys.Run.Plugin.GitKraken\Properties`
- To enhance existing translations, open and update the relevant `.resx` file
- To add a new language, create a new resource file named `Resources.xx-XX.resx` (e.g. `Resources.it-IT.resx` for Italian)
- Once you're done, commit your changes, push to GitHub, and make a pull request
