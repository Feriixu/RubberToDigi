#include <DigiKeyboardDe.h>
#include <avr/pgmspace.h>

const char line1[] PROGMEM = "cmd";
const char line4[] PROGMEM = "del copycontest.txt";
const char line6[] PROGMEM = "copy con copycontest.txt";
const char line8[] PROGMEM = "1";
const char line10[] PROGMEM = "2";
const char line12[] PROGMEM = "3";

char buffer[25];
#define GetPsz( x ) (strcpy_P(buffer, (char*)x))

void waitFor( int d ) {DigiKeyboardDe.delay(d);}
void sendModKey( int key, int mod ) { DigiKeyboardDe.sendKeyStroke(key, mod); DigiKeyboardDe.update(); }
void sendKey( int key ) {  DigiKeyboardDe.sendKeyStroke(key); DigiKeyboardDe.update(); }
void print(char t[]) {DigiKeyboardDe.print(t); DigiKeyboardDe.update();}
void println(char t[]) {DigiKeyboardDe.println(t); DigiKeyboardDe.update();}

void setup() 
{
	sendModKey(KEY_R , MOD_GUI_LEFT);
	println(GetPsz (line1));
	waitFor(200);
	println(GetPsz (line4));
	println(GetPsz (line6));
	println(GetPsz (line8));
	println(GetPsz (line10));
	println(GetPsz (line12));
	sendModKey(KEY_C, MOD_CONTROL_LEFT);
	
}

void loop() { }
