using UnityEngine;
using System.Collections;

public class Pixellate : MonoBehaviour {

    void Start () {
        gameObject.renderer.material.mainTexture.filterMode = FilterMode.Point;
        gameObject.renderer.material.mainTexture.wrapMode = TextureWrapMode.Repeat;
    }
    
    // Update is called once per frame
    void Update () {
        /*
        if (Input.GetKey (left)) {      
            target.transform.Translate (new Vector2 (-1.0f, 0.0f));
        }
        
        if (Input.GetKey (right)) {
            target.transform.Translate (new Vector2 (1.0f, 0.0f));
        }
        if (Input.GetKey (up)) {        
            target.transform.Translate (new Vector2 (0.0f, 1.0f));
        }
        
        if (Input.GetKey (down)) {
            target.transform.Translate (new Vector2 (0.0f, -1.0f));
        }*/
    }
}
