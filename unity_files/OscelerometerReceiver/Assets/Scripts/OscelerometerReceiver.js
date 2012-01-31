
function Update () {

}

function OSCMessageReceived(message : OSC.NET.OSCMessage){
	Debug.Log("Message Received with address " + message.Address + " with " + message.Values[0] + " values");
	
	transform.LookAt(Vector3(message.Values[0],message.Values[1],message.Values[2]));  
}