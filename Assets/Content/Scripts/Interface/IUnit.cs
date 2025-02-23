using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public void TakeDamage(float damage, Transform transform = null);
}
