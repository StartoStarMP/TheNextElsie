using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public static ClientManager current;

    public List<ClientUnlockState> clients;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            current = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}

[System.Serializable]
public class ClientUnlockState
{
    public Client client;
    // Whether this Client is unlocked or not.
    public bool unlocked = false;
}
