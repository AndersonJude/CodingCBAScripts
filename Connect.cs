using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
public class Connect : MonoBehaviourPunCallbacks
{

    void Awake()
    {
        //Temporary way of deciding the ID of each player. May be changed in future.
        ConnectToPhoton(RoomLoader.Name);
        Destroy(GameObject.Find("Player"));
    }
    //Connecting to Photon Servers
    private void ConnectToPhoton(string name)
    {
        PhotonNetwork.AuthValues = new AuthenticationValues(name);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = name;
        PhotonNetwork.ConnectUsingSettings();
    }
    //Creating or joining preexisting room.
    private void CreateOrJoinRoom(string roomname)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 8;
        roomOptions.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom(roomname, roomOptions, TypedLobby.Default);
    }
    public override void OnConnectedToMaster()
    {
        //When connected to Photon servers join lobby.
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    public override void OnJoinedLobby()
    {
        
        CreateOrJoinRoom("ROOM");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("JOINED");
        GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        player.GetComponent<PhotonView>().RequestOwnership();
        player.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
        
        //player.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = player.GetComponent<PhotonView>().Owner.NickName;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //base.OnDisconnected(cause);
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
    }

}
