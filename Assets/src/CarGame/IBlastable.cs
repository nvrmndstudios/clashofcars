using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlastable
{
    bool IsBlasted { get;}
    void Blast();
    void OnBlast();
}
