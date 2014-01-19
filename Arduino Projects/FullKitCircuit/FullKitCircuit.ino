void setup()
{
  serialSetup();
  setUpButton();
  setUpLEDs();
}

void loop()
{
  // read the potentiometers:
  serialSend();

  if (getButtonState1()) setLEDs(1023);
  else if(getButtonState2())
  {
    int sensorReading = analogRead(A0);
    setLEDs(sensorReading);
  }
  else setLEDs(0);
}


