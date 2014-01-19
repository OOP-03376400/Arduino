/*
  MultiLED
  Turns on 6 LEDs on for a half second each, then off for one second, repeatedly.
 */

int boardLED = 13;
int ledPin1 = 11;
int ledPin2 = 10;
int ledPin3 = 9;
int ledPin4 = 8;
int ledPin5 = 7;
int ledPin6 = 6;

void setup() {                
  // initialize the digital pin as an output.
  pinMode(boardLED, OUTPUT);     
  pinMode(ledPin1, OUTPUT);     
  pinMode(ledPin2, OUTPUT);     
  pinMode(ledPin3, OUTPUT);     
  pinMode(ledPin4, OUTPUT);     
  pinMode(ledPin5, OUTPUT);     
  pinMode(ledPin6, OUTPUT);     
}

void loop() {
  digitalWrite(boardLED, HIGH);   // set the LED on
  digitalWrite(ledPin1, HIGH);   // set the LED on
  delay(500);              // wait for a second
  digitalWrite(ledPin1, LOW);    // set the LED off
  digitalWrite(ledPin2, HIGH);   // set the LED on
  delay(500);              // wait for a second
  digitalWrite(ledPin2, LOW);    // set the LED off
  digitalWrite(ledPin3, HIGH);   // set the LED on
  delay(500);              // wait for a second
  digitalWrite(ledPin3, LOW);    // set the LED off
  digitalWrite(ledPin4, HIGH);   // set the LED on
  delay(500);              // wait for a second
  digitalWrite(ledPin4, LOW);    // set the LED off
  digitalWrite(ledPin5, HIGH);   // set the LED on
  delay(500);              // wait for a second
  digitalWrite(ledPin5, LOW);    // set the LED off
  digitalWrite(ledPin6, HIGH);   // set the LED on
  delay(500);              // wait for a second
  digitalWrite(ledPin6, LOW);    // set the LED off

  digitalWrite(boardLED, LOW);    // set the LED off
  delay(1000);              // wait for a second
}
