#include "DigiKeyboard.h"

void setup() {
	DigiKeyboard.sendKeyStroke(KEY_R , MOD_GUI_LEFT);
  DigiKeyboard.print("cmd && cmd");
  DigiKeyboard.sendKeyStroke(KEY_ENTER, MOD_SHIFT_LEFT + MOD_CONTROL_LEFT);
  DigiKeyboard.delay(800);
  DigiKeyboard.sendKeyStroke(KEY_ARROW_LEFT);
  DigiKeyboard.sendKeyStroke(KEY_ENTER);
}

void loop() {
  DigiKeyboard.delay(300);
  DigiKeyboard.print("start");  
  DigiKeyboard.sendKeyStroke(KEY_ENTER);
}
