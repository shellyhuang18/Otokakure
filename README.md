# MusicApp
__Senior capstone project for Hunter College, Spring 2018.__

---


## About
MusicApp is an application developed for the iOS and Android devices. It's goal is to introduce users into the world of singing by presenting a friendly method of learning. This is done by creating a game where the user attempts to match pitches using the phone's built-in microphone.  

## Development
Developed primarily written in C# and using the Unity game engine, the application also makes use of Firebase as our database choice.

## Contributers (Team 7 Members)
Jack Chen  
Shelly Huang  
Rubaiyat Rashid  
Sacit Gonen  
Naseeb Gafar  

## Capstone Advisor
Prof. Anna Wisniewska

## Folder Organization
    MusicApp -> MusicApp 

      Assets - Sprites in the app as well as other resources are in this folder. 

        Assets -> Firebase - Firebase related files. (Library)

        Assets -> prefabs - All assets that have components and are gameobjects interactible with scenes. 

        Assets -> Resources - All sprites in this folder can be loaded onto the game scene through script.

        Assets -> scripts - All current scripts in the app.

          scripts -> Game UI - everything related to UI in the game window.
          scripts -> GameWindow - scripts related to the game window.

            GameWindow -> Conductor - scripts that relate to outputting notes. 
            GameWindow -> Module - scripts that relate to modules. This includes Pitch and Intervals. Things like randomly generated strings are made here.
            GameWindow -> Note - scripts that relate to the notes that fly across the screen. This includes physical behavior as well as the logic behind a note(the class). This includes the logic behind the sfs.
            GameWindow -> PitchDetector - scripts that relate to detecting pitch on the game window.
            GameWindow -> PitchLine - scripts that control the pitch line. This includes the arrow attached to the pitch line which dispays the user's current pitch as well as detecting the notes that pass the line.

          scripts -> Home - scripts that relate to the UI in the Home page.

            Home -> Practice - scripts that pertain to the UI in the Practice page.

          scripts -> SignIn - scripts related to storing and retrieving user information. Includes registration, login, and displaying user information.

          scripts -> Utility - scripts that function as utility. Things like error checking and altering pitch are stored in here. 

        Assets -> scenes - All scenes in the app. 



    
---


