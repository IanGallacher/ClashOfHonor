using UnityEngine;
using System.Collections;

public class NZZ_MainMenuGUI : MonoBehaviour {
//	private const string typeName = "UniqueGameName";
//	private const string gameName = "RoomName";
	public GameObject playerPrefab;
	
	public bool myOculusEnable = false;
	public bool hailHydraEnable = false;
	
	public float PlayerColorR = 0.0f;
	public float PlayerColorG = 0.0f;
	public float PlayerColorB = 0.0f;
	
	GameObject gameRules = null;
	void Start() 
	{	
		//this is a potential race condition if our core game logic uses multiple cores. 
		gameRules = GameObject.Find ("NZZ_GameSettings");
		if (!gameRules)
		{
			print ("LOOKOUT YOU NUMBSKULL!!! gameRules not found in NZZ_MAINMENUGUI");
		}
	}
	
	private void StartServer () 
	{
		Network.InitializeServer (4, 25000, !Network.HavePublicAddress ());
//		MasterServer.RegisterHost (typeName, gameName);
		if (myOculusEnable) 
		{
			GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			mainCamera.GetComponent<Camera>().enabled = false;
			
			// the current oculus sdk does only draws the lens. The goggle shape where the lens's are not needs to be
			//  cleared when we start the server with oculus enabled. 
			GL.Clear(true, true, new Color(0f,0f,0f,1f));
			
			GameObject theCam = GameObject.FindGameObjectWithTag("NZZ_Camera");
			theCam.GetComponent<OVRCameraRig>().enabled = true;
			theCam.GetComponent<OVRManager>().enabled = true;
			theCam.SetActive(true);
			
			GameObject gameSettings = GameObject.Find("GameSettings");
			gameSettings.GetComponent<NZZ_GameRules>().useHydra = hailHydraEnable;
		}
		if(hailHydraEnable) {gameRules.GetComponent<NZZ_GameRules>().useHydra = true;}
	}
	
	void OnServerInitialized ()
	{
		SpawnPlayer ();
	}
	
	void OnConnectedToServer ()
	{
		SpawnPlayer ();
	}
	
	private void SpawnPlayer ()
	{
		Network.Instantiate (playerPrefab, new Vector3 (0f, 5f, 0f), Quaternion.identity, 0);
	}
	
	void OnGUI ()	
	{    
		//GUI.backgroundColor = new Color(RGBTHING, 0f,0f,1f) ;
		
		//These variables make rearranging the interface a little bit easier if we ever decide it is necissary. 
		int marginx = 20;
		int marginy = 20;
		int coloroffsetx = 80+280+marginx;
		int coloroffsety = 20;
		int colorwidth = 100;
		int colorheight = 200;
			
			GUI.Box(new Rect(70,60,300,400), "Loader Menu");
			
			if (GUI.Button (new Rect (80, 150, 280, 100), "Single Player"))
				Application.LoadLevel("SinglePlayer");
		
			if (GUI.Button (new Rect (80, 270, 280, 100), "Multiplayer"))
				Application.LoadLevel("BoltMenu");
			
			if (hostList != null) {
				for (int i = 0; i < hostList.Length; i++) {
					if (GUI.Button (new Rect (400, 100 + (110 * i), 300, 100), hostList [i].gameName))
						JoinServer (hostList [i]);
				}
			}
			myOculusEnable = GUI.Toggle(new Rect(100, 380, 100, 20), myOculusEnable, " OculusEnable");
			hailHydraEnable = GUI.Toggle(new Rect(100, 400, 100, 20), hailHydraEnable, " HydraEnable");
			
			
			GUI.Box(new Rect(coloroffsetx,60,colorwidth+marginx*2,colorheight), "Player Color");
			PlayerColorR = GUI.HorizontalSlider( new Rect (coloroffsetx+marginx,100+coloroffsety+(0*marginy),colorwidth,20), PlayerColorR, 0f, 1f) ;
			PlayerColorG = GUI.HorizontalSlider( new Rect (coloroffsetx+marginx,100+coloroffsety+(1*marginy),colorwidth,20), PlayerColorG, 0f, 1f) ;
			PlayerColorB = GUI.HorizontalSlider( new Rect (coloroffsetx+marginx,100+coloroffsety+(2*marginy),colorwidth,20), PlayerColorB, 0f, 1f) ;
		
	}
	
	private HostData[] hostList;
	
	private void RefreshHostList ()
	{
//			MasterServer.RequestHostList (typeName);
	}
	
	void OnMasterServerEvent (MasterServerEvent msEvent)
	{
//		if (msEvent == MasterServerEvent.HostListReceived)
//			hostList = MasterServer.PollHostList ();
	}
	
	private void JoinServer (HostData hostData)
	{
//		Network.Connect (hostData);
	}
}

