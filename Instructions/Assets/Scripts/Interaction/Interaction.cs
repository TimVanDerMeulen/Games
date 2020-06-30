using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction
{
    public GameObject source { get; private set; }

    public InteractionType type { get; private set; }

    public Interaction(InteractionType type, GameObject source)
    {
        this.type = type;
        this.source = source;
    }
}
