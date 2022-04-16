# For Jump

* Some potential setups:
    * You might want to download the new Unity Input System package
    * `Edit` -> `Project Settings` -> `Player` -> Scroll down and find `Active Input Handling`, select `Both`
    * Change the level to `Level2WithFSM`, where I have already set up a Player2 ready to be used

* Scripts to "Touch" Jumping State (I don't think these are needed to fix the jump)
    * A new "Player" folder can be found in the Scripts folder
    * `PlayerData` object allows you to change the value of jump velocity
    * Don't change the values inside the script, just change it through the object in unity inspector
    * There are only 2 states (or script) that can essentially go into the Jump State - AirborneState and GroundedState,
    * They can be found in `Player` -> `SuperStates`, but you don't really need to touch them

* Scripts that handle the jump physics (I thik this is most important)
    * The JumpState calls the function that handles jumping, which is "Jump()" in `Player.cs`
    * So actually you only need to change the `Jump()` function in `Player.cs`
    * According to Tian Fang, you need to be careful of AddForce() for the slomo compatibility
    * Sorry I haven't managed to integrate the slomo in yet so you prolly can't test it


## Can ignore whatever below
* Player.cs - Everything about the player
    * StateMachine lives here
    * Animator lives here
    * All states live here

* PlayerState - Mother of all states, all states have these ref
  * Reference to the player
  * Reference to statemachine
  * Reference to player data
  * Reference to animation name

* PlayerInputHandler - Using new input system
  * `Edit -> Project Settings -> Player -> Active Input Handling -> Both`
  * `Window -> Package Manager -> seach "Input System" -> Install`
