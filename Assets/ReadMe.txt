When first opening the project, FMOD doesn't know where the banks are located and so doesn't play audio. 

Please navigate to FMOD on the top toolbar, next to Window and services. Then select Edit Settings.
Under Bank Import, set Source Type to Single Platform Build.
Then click browse under Build Path and navigate to the following location in your unity project.
..\UnityDndMusicController\Assets\FMOD Banks\Desktop

Now everything should work as intended.