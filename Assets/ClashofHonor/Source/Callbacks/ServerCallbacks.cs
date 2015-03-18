using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

[BoltGlobalBehaviour(BoltNetworkModes.Server, "BoltTutorial","PvP","BigMap","BigMap2","SmallMap")]
public class ServerCallbacks : Bolt.GlobalEventListener
{
    public static bool ListenServer = true;

    void Awake()
    {
        if (ListenServer)
        {
            Player.CreateServerPlayer();
            Player.serverPlayer.name = "SERVER";
        }
    }

    void FixedUpdate()
    {
        foreach (Player p in Player.allPlayers)
        {
            // if we have an entity, it's dead but our spawn frame has passed
            if (p.entity && p.state.Dead && p.state.respawnFrame <= BoltNetwork.serverFrame)
            {
                p.Spawn();
            }
        }
    }

    public override void ConnectRequest(UdpKit.UdpEndPoint endpoint, Bolt.IProtocolToken token)
    {
        BoltNetwork.Accept(endpoint);
    }

    public override void Connected(BoltConnection c, Bolt.IProtocolToken token)
    {
        c.userToken = new Player();
        c.GetPlayer().connection = c;
        c.GetPlayer().name = "CLIENT:" + c.remoteEndPoint.Port;
    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        connection.GetPlayer().InstantiateEntity();

    }

	public GameObject[] spawners;
    private void instantiateTheWhiteSphere() // called every second.
    {
		if (spawners == null)
			spawners = GameObject.FindGameObjectsWithTag("NZZ_MeteorSpawner");
		
		foreach (GameObject spawner in spawners) {
			//Instantiate(respawnPrefab, spawner.transform.position, spawner.transform.rotation) as GameObject;
			BoltEntity tws = BoltNetwork.Instantiate(BoltPrefabs.TheWhiteSphere, new TestToken(), spawner.transform.position, Quaternion.identity);
		}

		//GameObject.Find ("NZZ_MeteorSpwanwer")
        //BoltEntity tws = BoltNetwork.Instantiate(BoltPrefabs.TheWhiteSphere, new TestToken(), new Vector3(0, 15f, 0), Quaternion.identity);
    }


    public static void instantiateItem(Vector3 location)
    {
        BoltEntity item = BoltNetwork.Instantiate(BoltPrefabs.Item, new TestToken(), location, Quaternion.identity);
    }

    public override void SceneLoadLocalDone(string map)
    {
        if (Player.serverIsPlaying)
        {
            Player.serverPlayer.InstantiateEntity();
        }
        if (BoltNetwork.isServer)
		{
			InvokeRepeating("instantiateTheWhiteSphere", 0.5f, 5.0f);
        }
    }


    public override void SceneLoadLocalBegin(string map)
    {
        foreach (Player p in Player.allPlayers)
        {
            p.entity = null;
        }
    }
}
//
//
//
//anyway to fix an issue where the Bolt Scenes window is empty
//12:52:42 AM
//	Ian65
//	Everything that I spawn is done on the server
//	12:52:44 AM
//	fholm
//	@dhw add the scenes to the build settings
//	12:52:52 AM
//	dhw
//	ahhh yeah
//	12:52:57 AM
//	must have removed it when I reverted
//	12:53:08 AM
//	thank you as always
//	12:53:18 AM
//	fholm
//	np ​​
//	12:53:21 AM
//	@Ian65 okey, so then what you need is an event which gets sent from the client to the server to ask for a color change
//		12:53:27 AM
//			depending on how critical it is I would make it an entity event (if it's not critical) or a global event (if it's super critical)
//			12:53:50 AM
//			Ian65
//			How can I tell the event which object needs to change color
//			12:53:54 AM
//			fholm
//			you add a property to the event of type 'Entity'
//			12:54:04 AM
//			and set the entity to that
//			12:54:07 AM
//			it will magically exist on the other end ​​
//			12:54:11 AM
//			as the full object
//			12:54:14 AM
//			so what you would do is this:
//			12:54:32 AM
//			a) One event with two properties: "TargetEntity" and "Color".
//			12:54:49 AM
//			b) You raise this event as a global event, for everyone that wants to change color, and you do raise it with MyColorChangeEvent.Raise(Bolt.GlobalTargets.OnlyServer)
//			12:55:22 AM
//			d) On the server you listen for OnEvent(MyColorChangeEvent evnt) ...
//				12:55:39 AM
//					(I missed c... lol ah well)
//					12:55:43 AM
//					e) inside the event listener on the server, you do: evnt.TargetEntity.GetState<IMyState>().color = evnt.Color;
//12:56:07 AM
//and then you also need to hookup a callback to the changed 'color' property on the state
//	12:56:26 AM
//	so y ou can change the material color or w/e you wanna do
//	12:56:31 AM
//	dhw
//	Is there anyway to make a Bolt State without using the editor?
//	12:57:05 AM
//	dramamine0001
//	@fholm I'm loving the tutorials. In the advanced tutorial, chapter 4, the link to chapter 5 is broken. Also, the very last part of ch 4, setting animation damping...I don't see that script (or many others in the last screenshot). Is that something that changed?
//	12:57:06 AM
//	fholm
//	@dhw currently no, there is a half-done text based format, but it's not finalized yet
//12:57:49 AM
//dhw
//okay
//12:58:00 AM
//fholm
//@dramamine0001 yeah chapter 5 is broken atm, and the last image on chapter 4 is wrong you find the damping setting on the properties on the state itself
//12:58:13 AM
//Ian65
//@fholm is there any reason I can't change the state of the object to a certain number, and then in the Update() of that object set its color to the matching state? Performance issues aside.
//	12:58:19 AM
//	fholm
//	my writer is working on it
//	12:58:22 AM
//	trob just entered bolt.
//	12:58:25 AM
//	fholm
//	@Ian65 no you can do that also ​​