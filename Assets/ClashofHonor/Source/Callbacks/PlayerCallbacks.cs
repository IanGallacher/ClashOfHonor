

using UnityEngine;
using System.Collections;

[BoltGlobalBehaviour("BoltTutorial","PvP","BigMap", "BigMap2", "SmallMap")]
public class PlayerCallbacks : Bolt.GlobalEventListener {
	GameObject _playerCam;
	public override void SceneLoadLocalDone(string map) {
		GameUI.Instantiate();	
	}

	public override void ControlOfEntityGained(BoltEntity arg) {	
		if (arg.prefabId == BoltPrefabs.PlayerPrefab)
		{
			print("shut the front door");
			print (arg.name);
			GameObject.Find ("NZZ_CameraTransform").GetComponent<NZZ_FirstPersonCamera>().SetTarget(arg);
		}
	}
}



//     THIRD PERSON VERSION 
//using UnityEngine;
//using System.Collections;
//
//[BoltGlobalBehaviour("BoltTutorial","PvP")]
//public class PlayerCallbacks : Bolt.GlobalEventListener
//{
//    public override void SceneLoadLocalDone(string map)
//    {
//        // ui
//        GameUI.Instantiate();
//
//        // camera
//        PlayerCamera.Instantiate();
//    }
//
//    public override void ControlOfEntityGained(BoltEntity arg, Bolt.IProtocolToken token)
//    {
//        BoltLog.Info("ControlGained-Token: {0}", token);
//
//        if (arg.prefabId == BoltPrefabs.PlayerPrefab)
//        {
//
//            // add audio listener to our character
//            arg.gameObject.AddComponent<AudioListener>();
//
//            // set camera callbacks
//            PlayerCamera.instance.getAiming = () => arg.GetState<IPlayerState>().Aiming;
//            PlayerCamera.instance.getHealth = () => arg.GetState<IPlayerState>().health;
//            PlayerCamera.instance.getPitch = () => arg.Ge	tState<IPlayerState>().pitch;
//
//            // set camera target
//            PlayerCamera.instance.SetTarget(arg);
//        }
//    }
//}

