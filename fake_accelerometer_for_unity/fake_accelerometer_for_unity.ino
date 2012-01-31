void setup(){
  Serial.begin(9600);
  randomSeed(analogRead(0));
}

void loop(){
  Serial.print(random(10,30));
  Serial.print(",");
  Serial.print(random(120,140));
  Serial.print(",");
  Serial.println(random(120,180));
}
