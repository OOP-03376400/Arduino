// constants won't change. They're used here to 
// set pin numbers:
const int buttonPin1 = 2;     // the number of the pushbutton pin
const int buttonPin2 = 3;     // the number of the pushbutton pin

bool lastButton2State = false;
bool currentState2 = false;

void setUpButton()
{
  pinMode(buttonPin1, INPUT);     
  pinMode(buttonPin2, INPUT);     
}

bool getButtonState1()
{
  // read the state of the pushbutton value:
  int buttonState = digitalRead(buttonPin1);

  // check if the pushbutton is pressed.
  // if it is, the buttonState is HIGH:
  return (buttonState == HIGH);
}

bool getButtonState2()
{
  // read the state of the pushbutton value:
  bool buttonState = (digitalRead(buttonPin2) == HIGH);
  if(buttonState && !lastButton2State)
  {
    currentState2 = !currentState2;
  }
  lastButton2State = buttonState;
  return currentState2;
}
