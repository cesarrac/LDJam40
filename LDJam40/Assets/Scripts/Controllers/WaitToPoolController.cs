using UnityEngine;
using System.Collections;

public class WaitToPoolController : MonoBehaviour {

    public float waitTime = 1f;

    float count;

    void Update()
    {
        if (count >= waitTime)
        {
            count = 0;
            ObjectPool.instance.PoolObject(this.gameObject);
        }
        else
        {
            count += Time.deltaTime;
        }
    }
    public void ResetTo(float newWait){
        count = 0;
        waitTime = newWait;
    }
    void OnDisable(){
        count = 0;
    }
}
