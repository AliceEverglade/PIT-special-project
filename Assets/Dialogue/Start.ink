INCLUDE Globals.ink

#speaker:Narrator #layout:Right #encounter:Introduction,TalkedToNarrator
{ PlayerName == "": -> main | -> alreadyChosen}

=== main ===
This is the start of this story.
It's a very good story.
So once upon a time there was a...
I forgot, can you remind me who you are?
    * [a princess in a tower]
        -> chosen("Rapunzel")
    * [a man who got reincarnated as a slime]
        -> chosen("Rimuru")
    * [a girl who got chased by her ancestor for a ritual]
        -> chosen("Stella")


=== chosen(choice) ===
Ah right right, {choice}. how could I forget. #encounter:Introduction,ChoseName
~ PlayerName = choice
The story hasn't ended yet, go on and write the rest.
-> END

=== alreadyChosen ===
Hi {PlayerName}, I'm sorry I'm quite busy right now.
Could you come back later?
-> END