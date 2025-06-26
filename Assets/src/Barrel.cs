using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IBlastable
{
    public event Action<GameObject> OnBarrelBlasted;

    public void OnBlast()
    {
        OnBarrelBlasted?.Invoke(gameObject);
    }

    public bool IsBlasted { get; }
    public void Blast()
    {
        if (TryGetComponent(out BlastEffect blastEffect))
        {
            blastEffect.Blast();
            OnBlast();
        }
    }
}
