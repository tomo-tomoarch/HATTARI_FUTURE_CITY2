using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    [SerializeField] Text connectionText;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Camera sceneCamera;

    GameObject player;

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.logLevel = PhotonLogLevel.Full;//情報を全部ください
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    // Update is called once per frame
    void Update()
    {
        connectionText.text = PhotonNetwork.connectionStateDetailed.ToString();//その情報をテキストに流します

    }

    void OnJoinedLobby()
    {
        RoomOptions ro = new RoomOptions() { isVisible = true, maxPlayers = 8 };//maxPlayerは人数の上限
        PhotonNetwork.JoinOrCreateRoom("Tomo", ro, TypedLobby.Default);//""内は部屋の名前

    }

    void OnJoinedRoom()
    {
        StartSpawnProcess(0f);
    }

    public void StartSpawnProcess(float respawnTime)
    {
        sceneCamera.enabled = true;
        StartCoroutine("SpawnPlayer", respawnTime);
    }

    IEnumerator SpawnPlayer(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);

        int index = Random.Range(0, spawnPoints.Length);
        player = PhotonNetwork.Instantiate("Capsule",
            spawnPoints[index].position,
            spawnPoints[index].rotation,
            0);
        sceneCamera.enabled = false;
    }
}
