const int ledCount = 5;    // the number of LEDs in the bar graph

// an array of pin numbers to which LEDs are attached
int ledPins[] = { 7,8,9,10,11 };

void setUpLEDs()
{
  // loop over the pin array and set them all to output:
  for (int thisLed = 0; thisLed < ledCount; thisLed++) {
    pinMode(ledPins[thisLed], OUTPUT); 
  }
}

void setLEDs(int sensorReading)
{
  // map the result to a range from 0 to the number of LEDs:
  int ledLevel = map(sensorReading, 0, 1023, 0, ledCount);

  // loop over the LED array:
  for (int thisLed = 0; thisLed < ledCount; thisLed++)
  {
    // if the array element's index is less than ledLevel,
    // turn the pin for this element on:
    if (thisLed < ledLevel)
    {
      digitalWrite(ledPins[thisLed], HIGH);
    } 
    // turn off all pins higher than the ledLevel:
    else
    {
      digitalWrite(ledPins[thisLed], LOW); 
    }
  }
}

