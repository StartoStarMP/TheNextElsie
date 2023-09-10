using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCanvas : MonoBehaviour
{
    public static TransitionCanvas current;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        current = this;
    }

    public void Animate(string anim)
    {
        GetComponent<Animator>().SetTrigger(anim);
    }
    
    /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void RuntimeInit()
    {
        if (!Debug.isDebugBuild || FindObjectOfType<TransitionCanvas>() != null)
            return;

        Instantiate(Resources.Load("TransitionCanvas"));
    }*/
}
