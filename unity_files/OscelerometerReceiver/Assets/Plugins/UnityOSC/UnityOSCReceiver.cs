using UnityEngine;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using OSC.NET;

//public class jsUnityOSCReceiver {}

public class UnityOSCReceiver : MonoBehaviour {
	
	private bool connected = false;
	public int port = 3333;
	private OSCReceiver receiver;
	private Thread thread;
	
//		private Dictionary<int, OpenTSPSPerson> people = new Dictionary<int, OpenTSPSPerson>(32);
//		private List<OpenTSPSListener> listeners = new List<OpenTSPSListener>();
	
	//thread safe Producer/Consumer Queue for incoming messages
	private List<OSCMessage> processQueue = new List<OSCMessage>();
	
	public UnityOSCReceiver() {}

	public int getPort() {
		return port;
	}
			
	public void Start() {
		Debug.Log("starting connect on port " + port);

		try {
			receiver = new OSCReceiver(port);
			connected = true;
		    thread = new Thread(new ThreadStart(listen));
		    thread.Start();

		} catch (Exception e) {
			Debug.Log("failed to connect to port "+port);
			Debug.Log(e.Message);
		}
	}
	
	/**
	 * Call update every frame in order to dispatch all messages that have come
	 * in on the listener thread
	 */
	public void Update() {
		//processMessages has to be called on the main thread
		//so we used a shared proccessQueue full of OSC Messages
		lock(processQueue){
			foreach( OSCMessage message in processQueue){
				SendMessage("OSCMessageReceived", message, SendMessageOptions.DontRequireReceiver);
			}
			processQueue.Clear();
		}
	}
	
	public void OnApplicationQuit(){
		Debug.Log("disconnecting");
		disconnect();
	}
	
	public void disconnect() {
      	if (receiver!=null){
      		 receiver.Close();
      	}
      	
       	receiver = null;
		connected = false;
	}

	public bool isConnected() { return connected; }

	private void listen() {
		while(connected) {

			try {
				OSCPacket packet = receiver.Receive();
				if (packet!=null) {					
					lock(processQueue){
						
						//Debug.Log( "adding  packets " + processQueue.Count );
						if (packet.IsBundle()) {
							ArrayList messages = packet.Values;
							for (int i=0; i<messages.Count; i++) {
								processQueue.Add( (OSCMessage)messages[i] );
							}
						} else{
							processQueue.Add( (OSCMessage)packet );
						}
					}
				} else Debug.Log("null packet");
			} catch (Exception e) { 
				Debug.Log( e.Message );
			}
		}
	}
}
