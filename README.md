# Mao (Multi-Audio Output)

A MudBlazor app for playing multiple audio files at the same time.

## Limitations
* This app uses NAudio, and is therefore limited to **.wav** and **.mp3** files. (**Note:** Vorbis files (**.ogg**) are a planned feature)
* **.wav** files are recommended for looping effects/music due to [limitations of the **.mp3** format.](https://lame.sourceforge.io/tech-FAQ.txt)

## Installation
1. Download the latest release [here.](https://github.com/Eonzenex/mao-mudblazor/releases)
2. Unzip the archive

## Setup
Due to limitations in Javascript file selection and C# file dialog incompatibility with console apps, the app must scan directories for unique audio files.
1. Open the `appsettings.json` file in the root of the project.
2. Under `AudioSourcePaths`, add the directories to scan for audio files. (**Note:** The app will scan all subdirectories of the directories specified.)

> Make sure to escape back slashes (`\\`) in the paths.
> 
> e.g. `C:\Fake\Path\To\Audio\Files` becomes `C:\\Fake\\Path\\To\\Audio\\Files`

4. Save the file.

## Usage
1. Open the `mao_mudblazor_server.exe` file.
2. Check the console to ensure the audio sources were correctly scanned.
3. Open your browser and navigate to the hosted URL (**Note:** The app should automatically open the hosted URL in your default browser).
