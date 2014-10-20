#include <avr/pgmspace.h>
#include <EEPROM.h>
#include <TM1638.h>
#include <InvertedTM1638.h>

#define GREEN 0xFF00
#define RED 0xFF00

#define DIO 8
#define CLK 7
#define STB 9


PROGMEM  prog_uchar VERSION[] = {2, 0, 7};
PROGMEM  prog_uint16_t ledsLong[2][17] = {{0, 1, 3, 7, 15, 31, 63, 127, 255, 256, 768, 1792, 3840, 7936, 7968, 8032, 8160}, {0, 1, 3, 7, 15, 31, 63, 127, 255, 1, 3, 7, 15, 31, 8223, 24607, 57375}};
PROGMEM  prog_uint16_t ledsShort[2][9] = {{0, 256, 768, 1792, 3840, 7936, 7968, 8032, 8160}, {0, 1, 3, 7, 15, 31, 8223, 24607, 57375}};

TM1638 module1(DIO, CLK, STB);
InvertedTM1638 module2(DIO, CLK, STB);
TM1638* modules[2] = {&module1,&module2};

byte bsettings, base, buttons, oldbuttons, page, oldpage;
int intensity, oldintensity, ledNum, pitLimiterColor, deltaneg, delta, blinkVal;
byte gear, spd_h, spd_l, shift, rpm_h, rpm_l, delta_h, delta_l, engine, lap, invert, ledCRL;
String boost;
int fuel, spd;
word rpm;
boolean changed, blinkrpm, ledOff;
unsigned long milstart, milstart2 = 0;


void setup() {
        Serial.begin(9600);
        Serial.print(pgm_read_byte_near(VERSION + 0));
        Serial.print(pgm_read_byte_near(VERSION + 1));
        Serial.print(pgm_read_byte_near(VERSION + 2));
                  
        modules[0]->setupDisplay(true, 0);
        modules[1]->setupDisplay(true, 0);
        
        invert = EEPROM.read(0);
        if (invert > 1){
          invert = 0;
          EEPROM.write(0, invert);
        }       
        
        ledNum = EEPROM.read(1);
        if (ledNum != 8 && ledNum != 16){
          ledNum = 16;
          EEPROM.write(1, ledNum);
        }  

	ledCRL = EEPROM.read(2);
        if (ledCRL > 1){
          ledCRL = 0;
          EEPROM.write(2, ledCRL);
        }  
       
       blinkVal = EEPROM.read(3);
        if (blinkVal > 16){
          blinkVal = 16;
          EEPROM.write(3, blinkVal);
        } 

        modules[invert]->setDisplayToString("EDT", 0, 0);
        modules[invert]->setDisplayDigit(pgm_read_byte_near(VERSION + 0), 5, 1);
        modules[invert]->setDisplayDigit(pgm_read_byte_near(VERSION + 1), 6, 1);
        modules[invert]->setDisplayDigit(pgm_read_byte_near(VERSION + 2), 7, 0); 

	oldbuttons = 0;
	page = 0;
        oldpage = 0;
        changed = false;
        blinkrpm = false;
        ledOff = false;
        intensity = 0;
        oldintensity = 0;
        pitLimiterColor = GREEN;
        
        delay(3000);
        modules[invert]->clearDisplay();
}

void update(TM1638* module) {
	if (Serial.available() > 0) {
                if (Serial.available() == 1 ) {
                    bsettings = Serial.read();
                    Serial.print(pgm_read_byte_near(VERSION + 0));
                    Serial.print(pgm_read_byte_near(VERSION + 1));
                    Serial.print(pgm_read_byte_near(VERSION + 2));
                } else if (Serial.available() > 14) {
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
                                
                                if (page == 0 && base > 0) {
                                    page = 1 << (base-1);
                                    oldbuttons = page; 
                                } 
                                
                                spd = (spd_h << 8) | spd_l;
				rpm = (rpm_h << 8) | rpm_l;
                                delta = (delta_h << 8)| delta_l;
                         }
                }
	}

	buttons = module->getButtons();
        
	if (buttons != 0) {
		if (buttons != oldbuttons) {
			oldbuttons = buttons;
                        oldpage = page;
			page = buttons;
                        //module->clearDisplay();
                        changed = true;
                        milstart = millis();

                        switch (page) {
                            case 1: // button 1 - gear, lap & speed
                                module->setDisplayToString("L G  SPD", 0, 0);
                                break;
                            case 2: // button 2 - lap & gear & fuel
                                module->setDisplayToString("L G FUEL", 0, 0);
                                break;
                            case 4: // button 3 - lap & gear & boost
                                module->setDisplayToString("L G  BST", 0, 0);
                                break;
                            case 8: // button 4 - boost & gear & speed
                                module->setDisplayToString("BST G SP", 0, 0);
                                break;
                            case 16: // button 5 - boost & rpm
                                module->setDisplayToString(" G   ENG", 0, 0);
                                break;
                            case 32: // button 6 - gear & lap delta
                                module->setDisplayToString(" G DELTA", 0, 0);
                                break;
                            case 64:
                                module->setDisplayToString("SP G DLT", 0, 0);
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
        			    module->setDisplayToString("R", 0, 3);
                                else if (gear == 1)
                                    module->setDisplayToString("N", 0, 3);
                                else
                                    module->setDisplayToString(String(gear - 1, DEC), 0, 3);
                                
                                // start of lap display
                                if (lap < 10) {
                                    module->clearDisplayDigit(1, false);
                                    module->setDisplayToString(String(lap, DEC), 0, 0);
                                }else if (lap >= 100 && lap < 110){
                                    String lapstr = String(lap - 100, DEC);
                                    module->setDisplayToString(String("0" + lapstr), 0, 0);
                                }else if (lap >= 110){
                                    module->setDisplayToString(String(lap - 100, DEC), 0, 0);
                                }else{
                                    module->setDisplayToString(String(lap, DEC), 0, 0);
                                }  
                                // end of lap display
                                
                                if (spd < 10) {
                                    module->clearDisplayDigit(5, false);
                                    module->clearDisplayDigit(6, false);
                                    module->setDisplayToString(String(spd, DEC), 0, 7);
                                }else if (spd < 100){
                                    module->clearDisplayDigit(5, false); 
                                    module->setDisplayToString(String(spd, DEC), 0, 6); 
                                }else if (spd >= 100){ 
        			    module->setDisplayToString(String(spd, DEC), 0, 5);
                                }
        			break;
                        }
        
                        case 2:{ // button 2 - lap & gear & fuel  
        
                                if (gear == 0) 
        			    module->setDisplayToString("R", 0, 3);
                                else if (gear == 1)
                                    module->setDisplayToString("N", 0, 3);
                                else
                                    module->setDisplayToString(String(gear - 1, DEC), 0, 3);
                                
                                // start of lap display
                                if (lap < 10) {
                                    module->clearDisplayDigit(1, false);
                                    module->setDisplayToString(String(lap, DEC), 0, 0);
                                }else if (lap >= 100 && lap < 110){
                                    String lapstr = String(lap - 100, DEC);
                                    module->setDisplayToString(String("0" + lapstr), 0, 0);
                                }else if (lap >= 110){
                                    module->setDisplayToString(String(lap - 100, DEC), 0, 0);
                                }else{
                                    module->setDisplayToString(String(lap, DEC), 0, 0);
                                }  
                                // end of lap display
                         
                                //Fuel
                                String fuelstr = "";
                                if (fuel == 100){
                                  fuelstr = String(99, DEC);
                                } else {
                                  fuelstr = String(fuel, DEC);
                                }
                                
                                module->setDisplayToString(String("F"), 0, 5);
        			module->setDisplayToString(String(fuelstr + " "), 0, 6);
                                                                  
                                break;
                        }

                        case 4:{ // button 3 - lap & gear & boost
        
                                if (gear == 0) 
        			    module->setDisplayToString("R", 0, 3);
                                else if (gear == 1)
                                    module->setDisplayToString("N", 0, 3);
                                else
                                    module->setDisplayToString(String(gear - 1, DEC), 0, 3);
                                
                                // start of lap display
                                if (lap < 10) {
                                    module->clearDisplayDigit(1, false);
                                    module->setDisplayToString(String(lap, DEC), 0, 0);
                                }else if (lap >= 100 && lap < 110){
                                    String lapstr = String(lap - 100, DEC);
                                    module->setDisplayToString(String("0" + lapstr), 0, 0);
                                }else if (lap >= 110){
                                    module->setDisplayToString(String(lap - 100, DEC), 0, 0);
                                }else{
                                    module->setDisplayToString(String(lap, DEC), 0, 0);
                                }  
                                // end of lap display
                         
                                //Boost
                                module->setDisplayToString(String("P"), 0, 5);
                                if (boost.length() == 1) {
                                  module->setDisplayDigit(boost.charAt(1), 6, 1);
                                  module->setDisplayDigit(boost.charAt(0), 7, 0);
                                } else {
                                  module->setDisplayDigit(boost.charAt(0), 6, 1);
                                  module->setDisplayDigit(boost.charAt(1), 7, 0);
        			}
        
                                break;
                        }
                        
                                                
        		case 8:{ // button4 - boost & gear & speed

                                //boost
                                if (boost.length() == 1) {
                                  module->setDisplayDigit(boost.charAt(1), 0, 1);
                                  module->setDisplayDigit(boost.charAt(0), 1, 0);
                                } else {
                                  module->setDisplayDigit(boost.charAt(0), 0, 1);
                                  module->setDisplayDigit(boost.charAt(1), 1, 0);
                                }
                                  
                                //gear  
                                if (gear == 0) 
          			  module->setDisplayToString("R", 0, 3);
                                  else if (gear == 1)
                                      module->setDisplayToString("N", 0, 3);
                                  else
                                      module->setDisplayToString(String(gear - 1, DEC), 0, 3);
                                    
                                //speed
                                if (spd < 10) {
                                    module->clearDisplayDigit(5, false);
                                    module->clearDisplayDigit(6, false);
                                    module->setDisplayToString(String(spd, DEC), 0, 7);
                                }else if (spd < 100){
                                    module->clearDisplayDigit(5, false); 
                                    module->setDisplayToString(String(spd, DEC), 0, 6); 
                                }else if (spd >= 100){ 
        			    module->setDisplayToString(String(spd, DEC), 0, 5);
                                }                                
                                break; 
                         } 
                         
        		case 16:{ // button 5 - gear & engine RPMs  
        
                                //Gear
                                if (gear == 0) 
        			    module->setDisplayToString("R", 0, 1);
                                else if (gear == 1)
                                    module->setDisplayToString("N", 0, 1);
                                else
                                    module->setDisplayToString(String(gear - 1, DEC), 0, 1);
                                  
                                //RPM
                                if (rpm < 10){
                                  module->clearDisplayDigit(6, false);
                                  module->clearDisplayDigit(5, false);
                                  module->clearDisplayDigit(4, false);
                                  module->clearDisplayDigit(3, false);
                                  module->setDisplayToString(String(rpm, DEC), 0, 7);
                                }else if (rpm < 100){
                                  module->clearDisplayDigit(5, false);
                                  module->clearDisplayDigit(4, false);
                                  module->clearDisplayDigit(3, false);
                                  module->setDisplayToString(String(rpm, DEC), 0, 6);
                                }else if (rpm < 1000){
                                  module->clearDisplayDigit(4, false);
                                  module->clearDisplayDigit(3, false);
                                  module->setDisplayToString(String(rpm, DEC), 0, 5);
                                }else if (rpm < 10000){
                                  module->clearDisplayDigit(3, false);
                                  module->setDisplayToString(String(rpm, DEC), 0, 4);
                                }else{
                                  module->setDisplayToString(String(rpm, DEC), 0, 3);
                                }
                                
                                break;  
                        } 
   
                        case 32:{ // gear & delta
                                                                                       
                                //Gear
                                if (gear == 0) 
        			    module->setDisplayToString("R", 0, 1);
                                else if (gear == 1)
                                    module->setDisplayToString("N", 0, 1);
                                else
                                    module->setDisplayToString(String(gear - 1, DEC), 0, 1);                               
                                
                                //Delta
                                if (deltaneg == 1) {
                                  module->setDisplayToString("-", 0, 3);
                                } else {
                                  module->clearDisplayDigit(3, false);
                                }
                                
                                if (delta < 10){
                                  module->setDisplayDigit(0, 4, 1);
                                  module->setDisplayDigit(0, 5, 0);
                                  module->setDisplayDigit(0, 6, 0);
                                  module->setDisplayDigit(String(delta).charAt(0), 7, 0);
                                } else if (delta < 100) {
                                  module->setDisplayDigit(0, 4, 1);
                                  module->setDisplayDigit(0, 5, 0);
                                  module->setDisplayDigit(String(delta).charAt(0), 6, 0);
                                  module->setDisplayDigit(String(delta).charAt(1), 7, 0);
                                } else if (delta < 1000) {
                                  module->setDisplayDigit(0, 4, 1);
                                  module->setDisplayDigit(String(delta).charAt(0), 5, 0);
                                  module->setDisplayDigit(String(delta).charAt(1), 6, 0);
                                  module->setDisplayDigit(String(delta).charAt(2), 7, 0);
                                } else {
                                  module->setDisplayDigit(String(delta).charAt(0), 4, 1);
                                  module->setDisplayDigit(String(delta).charAt(1), 5, 0);
                                  module->setDisplayDigit(String(delta).charAt(2), 6, 0);
                                  module->setDisplayDigit(String(delta).charAt(3), 7, 0);
                                }
                                break;
                        }   
      
                        case 64:{ // speed & gear & short delta
                        
                                //speed
                                if (spd < 10) {
                                    module->setDisplayToString(String(spd, DEC), 0, 0);
                                    module->clearDisplayDigit(1, false);
                                    module->clearDisplayDigit(2, false);
                                }else if (spd < 100){
                                    module->setDisplayToString(String(spd, DEC), 0, 0); 
                                    module->clearDisplayDigit(2, false); 
                                }else if (spd >= 100){ 
        			    module->setDisplayToString(String(spd, DEC), 0, 0);
                                }  
                                
                                //Gear
                                if (gear == 0) 
        			    module->setDisplayToString("R", 0, 4);
                                else if (gear == 1)
                                    module->setDisplayToString("N", 0, 4);
                                else
                                    module->setDisplayToString(String(gear - 1, DEC), 0, 4);                               
                                
                                //Delta
                                if (deltaneg == 1) {
                                  module->setDisplayToString("-", 0, 5);
                                } else {
                                  module->clearDisplayDigit(5, false);
                                }
                                
                                if (String(delta).length() == 1) { 
                                  delta = 0;
                                } else if (String(delta).charAt(String(delta).length()-2) >= 53 && delta < 9949){
                                   delta = delta + 50; 
                                }
                                if (delta < 100){
                                  module->setDisplayDigit(0, 6, 1);
                                  module->setDisplayDigit(0, 7, 0);
                                } else if (delta < 1000) {
                                  module->setDisplayDigit(0, 6, 1);
                                  module->setDisplayDigit(String(delta, DEC).charAt(0), 7, 0);
                                } else {
                                  module->setDisplayDigit(String(delta, DEC).charAt(0), 6, 1);
                                  module->setDisplayDigit(String(delta, DEC).charAt(1), 7, 0);
                                }
                                break;
                        }                  
        	}
        } else {
            if ((millis() - milstart) > 2000) {
                changed = false;
                module->clearDisplay();
            }
        }
        
        
        if ((engine & 0x10) == 0 & ledOff == false) {
            if ((ledNum == 16 & shift >= blinkVal)|(ledNum == 8 & shift >= floor(blinkVal/2))) {
                if ((millis() - milstart2) > 50) {
                    if (blinkrpm == false) {
                        module->setLEDs(0x0000);
                        blinkrpm = true;
                    } else {
                        if (ledNum == 8){
                          module->setLEDs(pgm_read_word_near(ledsShort[ledCRL] + shift));
                        }else{
                          module->setLEDs(pgm_read_word_near(ledsLong[ledCRL] + shift));
                        }
                        blinkrpm = false;
                    }
                    milstart2 = millis();
                }
            } else {
                if (ledNum == 8){
                  module->setLEDs(pgm_read_word_near(ledsShort[ledCRL] + shift));
                }else{
                  module->setLEDs(pgm_read_word_near(ledsLong[ledCRL] + shift));
                }
            }
        } else {
            if ((millis() - milstart2) > 200 & ledOff == false) {
                if (blinkrpm == false) {
                    module->setLEDs(0x0000);
                    blinkrpm = true;
                } else {
                    module->setLEDs(pitLimiterColor); // Pit Limiter color
                    blinkrpm = false;
                }
                milstart2 = millis();
            }
        }  
        
        // button 8 + button 1 - toggle number of LEDs to use
        if (module->getButtons() == 0b10000001){
            if (ledNum == 8){
               ledNum = 16;
            }else{
               ledNum = 8;
            }
            EEPROM.write(1, ledNum);
            delay(200);
        } 

        // button 8 + button 2 - toggle led colours
        if (module->getButtons() == 0b10000010){
            if (ledCRL == 1){ 
              ledCRL = 0;
            } else {
              ledCRL = 1;
            }
            EEPROM.write(2, ledCRL); 
            delay(200);  
        }

        // button 8 + button 3 - toggle module inversion
        if (module->getButtons() == 0b10000100){
            if (invert == 1){ 
              invert = 0;
            } else {
              invert = 1;
            }
            EEPROM.write(0, invert); 
            delay(200);  
        }   
        
        // button 8 + button 6 - dec blink val
        if (module->getButtons() == 0b10100000){
            if (blinkVal > 4){ 
              blinkVal -= 1;
            }
            EEPROM.write(3, blinkVal); 
            module->setDisplayToString("Blink " + String(blinkVal, DEC), 0, 0);
            delay(1200);  
            module->clearDisplay();
        }
        
        // button 8 + button 7 - inc blink val
        if (module->getButtons() == 0b11000000){
            if (blinkVal < 16){ 
              blinkVal += 1;
            }
            EEPROM.write(3, blinkVal); 
            module->setDisplayToString("Blink " + String(blinkVal, DEC), 0, 0);
            delay(1200); 
            module->clearDisplay(); 
        }
}

void loop() {
  update(modules[invert]);
  
  if (intensity != oldintensity) {   
    modules[0]->setupDisplay(true, intensity); 
    modules[1]->setupDisplay(true, intensity);  
  }
}
