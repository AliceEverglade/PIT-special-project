-> toadie_main
=== toadie_main ===
Oh, hey, what's up? #speaker:Toadie #emotion:Neutral #layout:right #encounter:ToadieIntroduction,TalkedToToadie
What, Alice?
Yeah, I've got her fixing my code again.
She's great.
Sometimes a little blind, but that's what I'm here for.
Y'know... as opposed to doing it myself!
It'd help if you actually let me do my work for once. #speaker:Alice_E #emotion:Neutral #encounter:AliceIntroduction,HeardAliceSpeak
Nahhhhhh, you don't need to focus on that! #speaker:Toadie #emotion:Neutral
Anyways...
-> toadie_question

=== toadie_question ===
Did you need anything?
    + [Yes]
        Well, shit.
    + [No]
        Well, great!
        -> toadie_end
    + [Maybe]
        Very helpful.
    + [I don't know]
        Uh... what?
    + [Could you repeat the question?]
        Alright, uh...
        -> toadie_question
    
- -> toadie_continue
    
=== toadie_continue ===

Unfortunately I don't think I can help you with that.

-> toadie_end

=== toadie_end ===

Now if you'll excuse me, I've got to keep bothering Alice while she's fixing my shit.

-> END