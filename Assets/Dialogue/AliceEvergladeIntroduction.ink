INCLUDE Globals.ink
Hi there {PlayerName}! #speaker:Alice_E #emotion:Neutral 
#encounter:AliceIntroduction,TalkedToAlice
-> main

=== main ===
how are you doing today?
    + [Good]
        Splendid! That's how we like to start the day.
    + [Bad]
        That's unfortunate, I hope tomorrow will be better.
    + [Fine]
        I feel you, sometimes it's just good enough you know.

- Anyhow, I should get back to work. goodluck today! :D

ALIIIIICE I need you to help me fix something and by help i mean do it for me!!! #speaker:Toadie #portrait:ToadieNeutral #encounter:ToadieIntroduction,HeardToadieSpeak

Yes yes I'll be there in a second! Sorry, really need to get back to work. speak to you later {PlayerName}! #speaker:Alice Everglade #portrait:AliceEvergladeNeutral
-> END