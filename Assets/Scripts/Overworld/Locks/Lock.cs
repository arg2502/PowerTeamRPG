using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : OverworldObject {

    // the initial state of the lock
    // normally starts locked, but maybe we can do fun puzzle things where it starts unlocked,
    // but then activating certain panels would change it to locked
    public bool isLocked;

    BoxCollider2D boxCollider2D;

    private void Start()
    {
        base.Start();
        boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D != null)
            boxCollider2D.enabled = isLocked;
    }

    public void ToggleLock()
    {
        isLocked = !isLocked;
        boxCollider2D.enabled = isLocked;

        PostToggle(isLocked);
    }

    protected virtual void PostToggle(bool newState) { }
}
