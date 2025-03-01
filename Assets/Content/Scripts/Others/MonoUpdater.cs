using Assets.Content.Scripts.Enemy;
using Assets.Content.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoUpdater : MonoBehaviour
{
    public List<EnemyUnit> EnemyUnits;

    private void Update()
    {
        for (int i = 0; i < EnemyUnits.Count; i++)
        {
            EnemyUnits[i].Tick();
        }
    }
}
