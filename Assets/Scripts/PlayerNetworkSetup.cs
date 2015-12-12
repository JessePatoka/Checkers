using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkSetup : NetworkBehaviour {
    [SerializeField] Camera CharacterCam;

    // Use this for initialization
    public override void OnStartLocalPlayer () {
        if (isLocalPlayer)
        {
            CharacterCam.enabled = true;
            GameObject.Find("SceneCamera").SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
