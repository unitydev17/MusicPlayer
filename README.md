# Music player

Plays wav, mp3 from the Tracks folder.

Important note: 
due to the usage of third party library in the project (NAudio) we need to set up Build Settings -> Player Settings -> Configuration -> Api Compatibility Level = .NET 2.0 when build project


Knows problems:
1) Memory leaks appear. Probably NAudioConverter.cs contains some weak logic.
2) Clicking on the playback slider while playing the music does not change current playpoint.