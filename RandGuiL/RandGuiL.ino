#include <DigiKeyboard.h>
#include <Entropy.h>

long min = 60000;
long max = 300000;

void setup() {
  Entropy.initialize();
}

void loop() {
  long tmp = Entropy.random(min, max);
  DigiKeyboard.delay(tmp);
  lock();  
}

void lock() { 
  DigiKeyboard.sendKeyStroke(0);
  DigiKeyboard.sendKeyStroke(KEY_L , MOD_GUI_LEFT);
}
