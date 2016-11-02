using UnityEngine;
using System.Collections;

public class ColorBridge : OverworldObject {

    public bool isRed;
    public bool isBlue;
    float rotationDegrees1 = 0.0f;
    float rotationDegrees2 = 0.5f;

    void Start()
    {
        base.Start();
        sr.sortingOrder = (int)-transform.position.y - 200;
    }

    public override void Activate()
    {
        //StartCoroutine(RotateBridge());
        transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), 90.0f);
    }
    //IEnumerator RotateBridge()
    //{
    //    if(transform.rotation.z == rotationDegrees1)
    //    {
    //        while(transform.rotation.z < rotationDegrees2)
    //        {
    //            transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), 90.0f);
    //            print(transform.rotation.z);
    //            yield return new WaitForSeconds(0.1f);
    //        }
    //    }
    //}
}
