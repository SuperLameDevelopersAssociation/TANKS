using UnityEngine;
using System.Collections;
using TrueSync;

public class PowerUp : TrueSyncBehaviour
{
    public void SetPosition(TSVector newPosition)
    {
        tsTransform.position = new TSVector(TSRandom.Range(-50, 50), 0, TSRandom.Range(-50, 50));
        //tsTransform.position = newPosition;
    }
}
