/*
 * 
 * Copyright 2014 Stefano Linguerri
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *    
 *    http://www.apache.org/licenses/LICENSE-2.0
 *    
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 */

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
