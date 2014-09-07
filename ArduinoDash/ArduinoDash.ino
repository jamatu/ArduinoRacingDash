#include <TM1638.h>

TM1638 module(8, 7, 9, true, 0); // last digit is display intensity, 0 (low) - 7 (high)

//word ledsLong [17] = {0, 256, 768, 1792, 3840, 7936, 16128, 32512, 65280, 1, 3, 7, 15, 31, 63, 127, 255};
word ledsLong [17] = {0, 1, 3, 7, 15, 31, 63, 127, 255, 256, 768, 1792, 3840, 7936, 7968, 8032, 8160};
word ledsShort [9] = {0, 256, 768, 1792, 3840, 7936, 7968, 8032, 8160};

byte bsettings, base, buttons, oldbuttons, page, oldpage;
int intensity, oldintensity, ledNum, pitLimiterColor, deltaneg, delta;
byte gear, spd_h, spd_l, shift, rpm_h, rpm_l, delta_h, delta_l, engine, lap;
String boost;
int fuel, spd;
word rpm;
boolean changed, blinkrpm, ledOff;
unsigned long milstart, milstart2 = 0;

void setup() {
	Serial.begin(9600);

	oldbuttons = 0;
	page = 0;
        oldpage = 0;
        changed = false;
        blinkrpm = false;
        ledOff = false;
        intensity = 0; // set this to match the intensity used on line 3
        oldintensity = 0;
        ledNum = 16; // Default number of LEDs to use, 8 or 16
        pitLimiterColor = 0xFF00; // 0xFF00 = green, 0x00FF = red
}

void loop() {
	if (Serial.available() > 0) {
		if (Serial.available() > 14) {
			if (Serial.read() == 255) {
                                bsettings = Serial.read();                                
				gear = Serial.read();
				spd_h = Serial.read();
                                spd_l = Serial.read();
				rpm_h = Serial.read();
				rpm_l = Serial.read();
				fuel = Serial.read();
				if (ledNum == 8){
				  shift = Serial.read() / 2;
                                }else{
                                  shift = Serial.read();
                                }
				engine = Serial.read();
                                lap = Serial.read();
                                boost = String(Serial.read());
                                delta_h = Serial.read();
				delta_l = Serial.read();
                                
                                base = bsettings & 15;
                                intensity = (bsettings & 112) >> 4;
                                deltaneg = (bsettings & 128) >> 7;
                                if (intensity != oldintensity) {   
                                  module.setupDisplay(true, intensity);  
                                }
                                
                                if (page == 0) {
                                    page = base;
                                    oldbuttons = page; 
                                } 
                                
                                spd = (spd_h << 8) | spd_l;
				rpm = (rpm_h << 8) | rpm_l;
                                delta = (delta_h << 8)| delta_l;
                         }
		}
	}

	buttons = module.getButtons();
        
	if (buttons != 0) {
		if (buttons != oldbuttons) {
			oldbuttons = buttons;
                        oldpage = page;
			page = buttons;
                        module.clearDisplay();
                        changed = true;
                        milstart = millis();

                        switch (page) {
                            case 1: // button 1 - gear, lap & speed
                                module.setDisplayToString("L G  SPD", 0, 0);
                                break;
                            case 2: // button 2 - lap & gear & fuel
                                module.setDisplayToString("L G FUEL", 0, 0);
                                break;
                            case 4: // button 3 - lap & gear & boost
                                module.setDisplayToString("L G  BST", 0, 0);
                                break;
                            case 8: // button 4 - boost & gear & speed
                                module.setDisplayToString("BST G SP", 0, 0);
                                break;
                            case 16: // button 5 - boost & rpm
                                module.setDisplayToString(" G   ENG", 0, 0);
                                break;
                            case 32: // button 6 - gear & lap delta
                                module.setDisplayToString(" G DELTA", 0, 0);
                                break;
                            case 64:
                                module.setDisplayToString("SP G DLT", 0, 0);
                                break;
                            case 128:
                                page = oldpage;
                                changed = false;
                                break;
                        }
		}
	}
	
        if (changed == false) {
        	switch (page) {
        		case 1:{ // button 1 - lap & gear & speed
                                if (gear == 0) 
        			    module.setDisplayToString("R", 0, 3);
                                else if (gear == 1)
                                    module.setDisplayToString("N", 0, 3);
                                else
                                    module.setDisplayToString(String(gear - 1, DEC), 0, 3);
                                
                                // start of lap display
                                if (lap < 10) {
                                    module.clearDisplayDigit(1, false);
                                    module.setDisplayToString(String(lap, DEC), 0, 0);
                                }else if (lap >= 100 && lap < 110){
                                    String lapstr = String(lap - 100, DEC);
                                    module.setDisplayToString(String("0" + lapstr), 0, 0);
                                }else if (lap >= 110){
                                    module.setDisplayToString(String(lap - 100, DEC), 0, 0);
                                }else{
                                    module.setDisplayToString(String(lap, DEC), 0, 0);
                                }  
                                // end of lap display
                                
                                if (spd < 10) {
                                    module.clearDisplayDigit(5, false);
                                    module.clearDisplayDigit(6, false);
                                    module.setDisplayToString(String(spd, DEC), 0, 7);
                                }else if (spd < 100){
                                    module.clearDisplayDigit(5, false); 
                                    module.setDisplayToString(String(spd, DEC), 0, 6); 
                                }else if (spd >= 100){ 
        			    module.setDisplayToString(String(spd, DEC), 0, 5);
                                }
        			break;
                        }
        
                        case 2:{ // button 2 - lap & gear & fuel  
        
                                if (gear == 0) 
        			    module.setDisplayToString("R", 0, 3);
                                else if (gear == 1)
                                    module.setDisplayToString("N", 0, 3);
                                else
                                    module.setDisplayToString(String(gear - 1, DEC), 0, 3);
                                
                                // start of lap display
                                if (lap < 10) {
                                    module.clearDisplayDigit(1, false);
                                    module.setDisplayToString(String(lap, DEC), 0, 0);
                                }else if (lap >= 100 && lap < 110){
                                    String lapstr = String(lap - 100, DEC);
                                    module.setDisplayToString(String("0" + lapstr), 0, 0);
                                }else if (lap >= 110){
                                    module.setDisplayToString(String(lap - 100, DEC), 0, 0);
                                }else{
                                    module.setDisplayToString(String(lap, DEC), 0, 0);
                                }  
                                // end of lap display
                         
                                //Fuel
                                String fuelstr = "";
                                if (fuel == 100){
                                  fuelstr = String(99, DEC);
                                } else {
                                  fuelstr = String(fuel, DEC);
                                }
                                
                                module.setDisplayToString(String("F"), 0, 5);
        			module.setDisplayToString(String(fuelstr + " "), 0, 6);
                                                                  
                                break;
                        }

                        case 4:{ // button 3 - lap & gear & boost
        
                                if (gear == 0) 
        			    module.setDisplayToString("R", 0, 3);
                                else if (gear == 1)
                                    module.setDisplayToString("N", 0, 3);
                                else
                                    module.setDisplayToString(String(gear - 1, DEC), 0, 3);
                                
                                // start of lap display
                                if (lap < 10) {
                                    module.clearDisplayDigit(1, false);
                                    module.setDisplayToString(String(lap, DEC), 0, 0);
                                }else if (lap >= 100 && lap < 110){
                                    String lapstr = String(lap - 100, DEC);
                                    module.setDisplayToString(String("0" + lapstr), 0, 0);
                                }else if (lap >= 110){
                                    module.setDisplayToString(String(lap - 100, DEC), 0, 0);
                                }else{
                                    module.setDisplayToString(String(lap, DEC), 0, 0);
                                }  
                                // end of lap display
                         
                                //Boost
                                module.setDisplayToString(String("P"), 0, 5);
                                if (boost.length() == 1) {
                                  module.setDisplayDigit(boost.charAt(1), 6, 1);
                                  module.setDisplayDigit(boost.charAt(0), 7, 0);
                                } else {
                                  module.setDisplayDigit(boost.charAt(0), 6, 1);
                                  module.setDisplayDigit(boost.charAt(1), 7, 0);
        			}
        
                                break;
                        }
                        
                                                
        		case 8:{ // button4 - boost & gear & speed

                                //boost
                                if (boost.length() == 1) {
                                  module.setDisplayDigit(boost.charAt(1), 0, 1);
                                  module.setDisplayDigit(boost.charAt(0), 1, 0);
                                } else {
                                  module.setDisplayDigit(boost.charAt(0), 0, 1);
                                  module.setDisplayDigit(boost.charAt(1), 1, 0);
                                }
                                  
                                //gear  
                                if (gear == 0) 
          			  module.setDisplayToString("R", 0, 3);
                                  else if (gear == 1)
                                      module.setDisplayToString("N", 0, 3);
                                  else
                                      module.setDisplayToString(String(gear - 1, DEC), 0, 3);
                                    
                                //speed
                                if (spd < 10) {
                                    module.clearDisplayDigit(5, false);
                                    module.clearDisplayDigit(6, false);
                                    module.setDisplayToString(String(spd, DEC), 0, 7);
                                }else if (spd < 100){
                                    module.clearDisplayDigit(5, false); 
                                    module.setDisplayToString(String(spd, DEC), 0, 6); 
                                }else if (spd >= 100){ 
        			    module.setDisplayToString(String(spd, DEC), 0, 5);
                                }                                
                                break; 
                         } 
                         
        		case 16:{ // button 5 - gear & engine RPMs  
        
                                //Gear
                                if (gear == 0) 
        			    module.setDisplayToString("R", 0, 1);
                                else if (gear == 1)
                                    module.setDisplayToString("N", 0, 1);
                                else
                                    module.setDisplayToString(String(gear - 1, DEC), 0, 1);
                                  
                                //RPM
                                if (rpm < 10){
                                  module.clearDisplayDigit(6, false);
                                  module.clearDisplayDigit(5, false);
                                  module.clearDisplayDigit(4, false);
                                  module.clearDisplayDigit(3, false);
                                  module.setDisplayToString(String(rpm, DEC), 0, 7);
                                }else if (rpm < 100){
                                  module.clearDisplayDigit(5, false);
                                  module.clearDisplayDigit(4, false);
                                  module.clearDisplayDigit(3, false);
                                  module.setDisplayToString(String(rpm, DEC), 0, 6);
                                }else if (rpm < 1000){
                                  module.clearDisplayDigit(4, false);
                                  module.clearDisplayDigit(3, false);
                                  module.setDisplayToString(String(rpm, DEC), 0, 5);
                                }else if (rpm < 10000){
                                  module.clearDisplayDigit(3, false);
                                  module.setDisplayToString(String(rpm, DEC), 0, 4);
                                }else{
                                  module.setDisplayToString(String(rpm, DEC), 0, 3);
                                }
                                
                                break;  
                        } 
   
                        case 32:{ // gear & delta
                                                                                       
                                //Gear
                                if (gear == 0) 
        			    module.setDisplayToString("R", 0, 2);
                                else if (gear == 1)
                                    module.setDisplayToString("N", 0, 2);
                                else
                                    module.setDisplayToString(String(gear - 1, DEC), 0, 2);                               
                                
                                //Delta
                                if (deltaneg == 1) {
                                  module.setDisplayToString("-", 0, 4);
                                } else {
                                  module.clearDisplayDigit(4, false);
                                }
                                
                                if (delta < 10){
                                  module.setDisplayDigit(0, 5, 1);
                                  module.setDisplayDigit(0, 6, 0);
                                  module.setDisplayDigit(String(delta).charAt(0), 7, 0);
                                } else if (delta < 100) {
                                  module.setDisplayDigit(0, 5, 1);
                                  module.setDisplayDigit(String(delta).charAt(0), 6, 0);
                                  module.setDisplayDigit(String(delta).charAt(1), 7, 0);
                                } else {
                                  module.setDisplayDigit(String(delta).charAt(0), 5, 1);
                                  module.setDisplayDigit(String(delta).charAt(1), 6, 0);
                                  module.setDisplayDigit(String(delta).charAt(2), 7, 0);
                                }
                                break;
                        }   
      
                        case 64:{ // speed & gear & short delta
                        
                                //speed
                                if (spd < 10) {
                                    module.setDisplayToString(String(spd, DEC), 0, 0);
                                    module.clearDisplayDigit(1, false);
                                    module.clearDisplayDigit(2, false);
                                }else if (spd < 100){
                                    module.setDisplayToString(String(spd, DEC), 0, 0); 
                                    module.clearDisplayDigit(2, false); 
                                }else if (spd >= 100){ 
        			    module.setDisplayToString(String(spd, DEC), 0, 0);
                                }  
                                
                                //Gear
                                if (gear == 0) 
        			    module.setDisplayToString("R", 0, 4);
                                else if (gear == 1)
                                    module.setDisplayToString("N", 0, 4);
                                else
                                    module.setDisplayToString(String(gear - 1, DEC), 0, 4);                               
                                
                                //Delta
                                if (deltaneg == 1) {
                                  module.setDisplayToString("-", 0, 5);
                                } else {
                                  module.clearDisplayDigit(5, false);
                                }
                                
                                
                                if (String(delta).charAt(String(delta).length()-1) >= 53){
                                   delta = delta + 5; 
                                }
                                if (delta < 10){
                                  module.setDisplayDigit(0, 6, 1);
                                  module.setDisplayDigit(0, 7, 0);
                                } else if (delta < 100) {
                                  module.setDisplayDigit(0, 6, 1);
                                  module.setDisplayDigit(String(delta, DEC).charAt(0), 7, 0);
                                } else {
                                  module.setDisplayDigit(String(delta, DEC).charAt(0), 6, 1);
                                  module.setDisplayDigit(String(delta, DEC).charAt(1), 7, 0);
                                }
                                break;
                        }                  
        	}
        } else {
            if ((millis() - milstart) > 2000) {
                changed = false;
                module.clearDisplay();
            }
        }
        
        
        if ((engine & 0x10) == 0 & ledOff == false) {
            if (shift == ledNum) {
                if ((millis() - milstart2) > 50) {
                    if (blinkrpm == false) {
                        module.setLEDs(0x0000);
                        blinkrpm = true;
                    } else {
                        if (ledNum == 8){
                          module.setLEDs(ledsShort[shift]); // Blink when hit limiter
                        }else{
                          module.setLEDs(ledsLong[shift]); // Blink when hit limiter
                        }
                        blinkrpm = false;
                    }
                    milstart2 = millis();
                }
            } else {
                if (ledNum == 8){
                  module.setLEDs(ledsShort[shift]); // Set LEDs to RPM
                }else{
                  module.setLEDs(ledsLong[shift]); // Set LEDs to RPM
                }
            }
        } else {
            if ((millis() - milstart2) > 200 & ledOff == false) {
                if (blinkrpm == false) {
                    module.setLEDs(0x0000);
                    blinkrpm = true;
                } else {
                    module.setLEDs(pitLimiterColor); // Pit Limiter color
                    blinkrpm = false;
                }
                milstart2 = millis();
            }
        }  
        
        // button 8 - toggle number of LEDs to use
        if (module.getButtons() == 0b10000000){
           if (ledNum == 8){
              ledNum = 16;
           }else{
              ledNum = 8;
           }
           delay(200);
        }                              
}
