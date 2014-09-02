Arduino SLI Dash
--------------

USE
--------------
button 1 - lap counter, gear, speed
button 2 - lap counter, gear, fuel percent left
button 3 - lap counter, gear, boost pressure
button 4 - boost pressure, gear, speed
button 5 - gear, engine rpms
button 6 - toggle 8/16 LEDS for shift lights
button 7 - increase brightness
button 8 - decrease brightness




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


***App should detect which com port your arduino is on automatically, by pressing 'home' you can 
   see the log in the console to see which com port it has connected to. 
  *IF app doesn't connect to the correct com port you can override it in the config.ini file by 
   changing the 'port' option.


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

example using a .bat file to launch the client:
##
cd SLI Dash\iRacing Client
start iRacingSLI.exe COM5
##


author: Turnermator13
Based on work by Fernando Birck - http://fergotech.net/diy-shift-lights-and-dashboard/