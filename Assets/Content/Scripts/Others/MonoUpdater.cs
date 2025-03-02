using Assets.Content.Scripts.Enemy;
using Assets.Content.Scripts.Player;
using Assets.Content.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoUpdater : MonoBehaviour
{
    public static List<MonoUpdater> AllUpdate = new List<MonoUpdater>(500);

    private void OnEnable() => AllUpdate.Add(this);
    private void OnDisable() => AllUpdate.Remove(this);
    private void OnDestroy() => AllUpdate.Remove(this);

    public void Tick() => OnTick();
    public virtual void OnTick() { }
}
