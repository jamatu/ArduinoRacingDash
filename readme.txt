Arduino SLI Dash
--------------

USE
--------------
button 1 - lap counter, gear, speed
button 2 - lap counter, gear, fuel percent left
button 3 - lap counter, gear, boost pressure
button 4 - boost pressure, gear, speed
button 5 - gear, engine rpms
button 6 - gear, delta (to thousandth)
button 7 - speed, gear, delta (to tenth)
button 8 - toggle 8/16 LEDS for shift lights




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
Pin 9       STB1


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


***App should detect which com port your arduino is on automatically, by pressing 'home' whilst driving you can 
   see the log in the console to see which com port it has connected to, OR looking on the gui (v1.6+)
  *IF app doesn't connect to the correct com port you can override it in the gui by changing the text in the input 
   box (remember to press enter, box is a bit weird if you don't), you can find which COM port you need to connect 
   to by looking in the console at the avalible ports, (if unsure trial and error).


Install -- iRacing
-------------------

*Load arduino program onto arduino

*Start iRacing session

*Start SLI client AFTER load has completed!

*Select your COM port from the drop down and press start

*Profit


iRacing - advanced
------------------
app can except start args to allow for easy starting without using any of the apps buttons

example using a .bat file to launch the client, set COM port to COM5, set speed units to KPH and set display intensity(brightness) to 2:
##
cd SLI Dash\iRacing Client
start iRacingSLI.exe --Port COM5 --Unit MPH --Intensity 2
##


author: Turnermator13
Based on work by Fernando Birck - http://fergotech.net/diy-shift-lights-and-dashboard/