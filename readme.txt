Arduino SLI Dash
--------------

USE
--------------
button 1 - lap counter, gear, speed
button 2 - lap counter, gear, fuel percent left
button 3 - gear, fuel
button 4 - boost pressure, gear, speed
button 5 - gear, engine rpms
button 6 - gear, delta (to thousandth)
button 7 - speed, gear, delta (to tenth)
button 8 - Shift Modifier

button 8 + button 1 - change shift light mode (8/16 LED)
button 8 + button 2 - change shift light colours (red-->green/green-->red)
button 8 + button 3 - toggle module inversion (flips screen, LEDs, buttons ect)
(works best if you hold button 8 first then press the other button you want) 

button 8 + button 6 - decrement blinkVal (min 4)
button 8 + button 7 - increment blinkVal (max 16)



Setup - Arduino
-----------------
requires 1 TM1638.h module  --> get it here: http://www.dx.com/p/8x-digital-tube-8x-key-8x-double-color-led-module-81873

Connections: --> see: http://tronixstuff.com/2012/03/11/arduino-and-tm1638-led-display-modules/  
                 for pin locations on ribbon cable

Arduino     Display
VCC         VCC
GND         GND
Pin 7       CLK
Pin 8       DIO 
Pin 9       STB0




IMPORTANT
-------------------
install this library into your arduino library folder: https://code.google.com/p/tm1638-library/


Install -- AC
-----------------

*Load arduino program onto arduino

*Copy 'acSLI' folder to the apps/python folder in asseto corsa 
 Program Files (x86)\Steam\steamapps\common\assettocorsa\apps\python)
 
*Run Asseto Corsa and enable app in the settings panel > general

*Profit, no need to have the app on-screen, it can run in the background (hopefully)

*See forum post for detailed instructions: http://www.assettocorsa.net/forum/index.php?threads/arduino-dashboard-for-tm1638-module-v2-0-4-for-ac-v1-0-rc-rewrite-auto-updater-new-guis.14172/


Install -- iRacing
-------------------

*Load arduino program onto arduino

*Start iRacing session

*Start SLI client AFTER load has completed!

*Your Arduino should be automatically selected and serial automatically opened, if not select your COM port from the dropdown 

*Profit



Shift Lights Modifications
-----------------------------
The TM1638 Module uses a simple binary system to set the LEDs, each LED contains a red LED and a green LED. The LEDs are numbered left to right;
- 0-7 for GREEN.
- 8-15 for RED.

Now because they use a binary system;
- 2^0 (1)will light the leftmost GREEN LED
- 2^7(128)will light the rightmost GREEN LED
- 2^8 (256)will light the leftmost RED LED
- 2^15 (32768)will light the rightmost RED LED

The advantages of this system are you could pass;
2^0 + 2^1 to light the first 2 green LEDs.
OR 
2^0 + 2^9 will light the leftmost green LED and the red LED next to it.

This is because each LED has its own individual number and so you can light any combination by adding their respective numbers together!
Colours CANNOT be combined (you can't make yellow).


In the arduino code supplied you will find 2 arrays 'ledsLong' for 16 shift lights and 'ledsShort' for 8 shift lights. The arduino receives command 
from the host computer telling it how many LEDs to light (0-16, increases with % of max rpm) and it picks the number out of the selected array and sets 
the modules LED value to it. For example 255 will light every GREEN LED OR 8160 will light 5 RED LEDs followed by 3 GREEN LEDs.

You can replace any value in these arrays to change the LEDs shown at each shift light stage.
Below is the array 'ledsShort' to have the colours go from green to red (default is red to green):
	"word ledsShort [9] = {0, 1, 3, 7, 15, 31, 8223, 24607, 57375};"


author: Turnermator13
Based on work by Fernando Birck - http://fergotech.net/diy-shift-lights-and-dashboard/