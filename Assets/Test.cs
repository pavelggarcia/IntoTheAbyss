using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

   private void Start() 
   {
    Destroy(this.gameObject, 2f);
   }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(new Vector3(Mathf.Sin(Time.time*6),(1), 0)  *15 *Time.deltaTime);
    }
    
}
