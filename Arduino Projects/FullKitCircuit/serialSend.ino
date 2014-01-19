// these constants won't change:
const int analogPins[] = {A0, A1, A2, A3, A4, A5};
const int ackLEDPin = 6;

// the follow variables is a long because the time, measured in miliseconds,
// will quickly become a bigger number than can be stored in an int.
long previousSend = 0;        // will store last time we sent a value
long previousAck = 0;         // will store last time we got a value ack
long interval = 100;          // interval at which to send (milliseconds)

void serialSetup()
{
  Serial.begin(9600);
}

int serialSend()
{
  unsigned long currentMillis = millis();
 
  if(currentMillis - previousSend > interval)
  {
    // save the last time you sent a value
    previousSend = currentMillis;
    
    for (int i = 0; i < 6; i++)
    {
      int sensorValue = analogRead(analogPins[i]);
      Serial.print("A");
      Serial.print(i);
      Serial.print(":");
      Serial.println(sensorValue);
    }
  }

  if(currentMillis - previousAck > interval / 2)
  {
    if (Serial.available())
    {
      Serial.read();
      digitalWrite(ackLEDPin, HIGH); 
      previousAck = currentMillis;
    }
    else
    {
      digitalWrite(ackLEDPin, LOW); 
    }
  }
}

