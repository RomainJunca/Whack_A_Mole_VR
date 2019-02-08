# Whack_A_Mole_VR
Whack_A_Mole VR Game made with Unity
========

The Whack_A_Mole VR environment provides a test for the Pupil Labs Calibration environment, used with the Pupil Labs plugin for eye-tracking in VR. The main goal
of this project is to be used in clinics for rehabilitation of visual neglected people.
This kind of game helps the patients to focus and think faster, it pushes him to observe more his environment. In addition to the fun of playing the game for the patient, which is
a change compared to other kind of tests which are less funnier, patient who went through rehab with this game started to recognize increasing capabilities in everyday life
The creation of this game in virtual reality and with the use of the Pupil labs plugin will help us more to analyze the patient’s behaviors and reactions. We can also change game’s
parameters such as the apparition’s speed, and also add distractors into it.

Different moles will appear, represented by spheres which will lights in green or red, while producing a sound. With ray from his controller, the
user will have ton point at a mole, which will then shine, and then press the trigger to "Whack" the mole.
The user must not whack the red moles, as it takes off one point (one red moles "whacked").

The incurved wall, in which the moles will appear, is always at the same distance from the user. The user can freely move is head, but if he changes 
his position, the wall will follow.

In the menu, you can ether choose the mode by pressing "1/2/3/4" on the keyboard, or (in some version of Unity) directly click on the mode.
A double click or an enter key pressed will launch the mode.

There is four modes on this game, four differents difficulty modes in which the different parameters are changed:

In the three first moles, the life time of a mole when it appears is 5s.
Easy Mode :
A maximum of 2 moles can appear at the same time.
You can miss 5 of them (including if a red mole is whacked which add a missed mole to the count) before game over.
The range of time between two moles'appearances can go from 3s to 4s.

Medium Mode :
A maximum of 4 moles can appear at the same time.
You can miss 3 of them (including if a red mole is whacked which add a missed mole to the count) before game over.
The range of time between two moles'appearances can go from 1.5s to 3s.

Hard Mode :
A maximum of 6 moles can appear at the same time.
You can miss 2 of them (including if a red mole is whacked which add a missed mole to the count) before game over.
The range of time between two moles'appearances can go from 0.5s to 1.5s.

Gradual Mode :
The difficulty of this mode will be gradually increased as the game goes on. At each step (each number of whacked green moles), the parameters are changed.
Until 4 whacked moles :
A maximum of 2 moles can appear at the same time.
You can miss 5 of them (including if a red mole is whacked which add a missed mole to the count) before game over.
The life time of a mole when it appears is 5s.
The range of time between two moles'appearances can go from 2.75s to 3.5s.
From 4 to 9 whacked moles :
Max moles : 4
Max missed : 4
Life time : 4s
Range :  From 2s to 2.75s
From 9 to 16 whacked moles :
Max moles : 6
Max missed : 3
Life time : 3s
Range : From 1.25s to 2s
From 16 whacked moles :
Max moles : 8
Max missed : 2
Life time : 2s
Range : From 0.5s to 1.25s

At each step, the counters are put down to zero (If you missed 4 moles in the first step, the missed moles count will be zero again at the beginning 
of the second step). The total numbers are saved to be displayed on the game over wall.


When the user misses more moles than the maximum number of missed whole allowed, the game stop and a game over wall appears, in which we can
see the total number of moles whacked, moles missed, and red moles whacked.

A lot of things can still be modified, or added (changing the sounds of the red moles/adding a restart and/or retry key/...), this project is the work of less than 1 week.


Installation
------------

For now, the Pupil Labs eye-tracking plugin has not been integrated to the application. You just have to launch it via the Unity Editor.

