using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUpdater : MonoBehaviour
{
    private void Update()
    {
        for(int i = 0; i < MonoUpdater.AllUpdate.Count; i++)
        {
            MonoUpdater.AllUpdate[i].Tick();
        }
    }
}
