using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tasks : MonoBehaviour
{
    public abstract bool Exec(StatusWorld obj, EventData data = null);
}
