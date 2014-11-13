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

public class SpriteMove : MonoBehaviour
{

    public KeyCode moveRight;
    public KeyCode moveLeft;
    public KeyCode jump;
    public Vector2 jumpForce = new Vector2 (0, 50);
    private Animator animator;
    private SpriteRenderer[] renderers;

    void Awake ()
    {
      
        // Get the animator
    //    animator = GetComponent<Animator> ();
      //  renderers = GetComponentsInChildren<SpriteRenderer> ();
    
    }


    // Use this for initialization
    void Start ()
    {
       
    }

    void FixedUpdate ()
    {
        if (Input.GetKey (moveLeft)) {          
            transform.Translate (new Vector2 (-0.03f, 0.0f));
        }

        if (Input.GetKey (moveRight)) {  
            transform.Translate (new Vector2 (0.03f, 0.0f));
        }
        if (Input.GetKeyDown (jump)) {  
            rigidbody2D.AddForce (jumpForce);               
            
        }
    }
    
    // Update is called once per frame
    void Update ()
    {
        /*
        foreach (SpriteRenderer aRenderer in renderers) {
            if (aRenderer.sprite = null) {
                aRenderer.sprite.texture.filterMode = FilterMode.Point;//This disable the antialias filter  
                aRenderer.sprite.texture.wrapMode = TextureWrapMode.Clamp;
            }
        }

        if (Input.GetKeyDown (moveLeft)) {          
            animator.SetBool ("WalkRight", true); 
            animator.SetBool ("WalkLeft", false); 
            animator.SetBool ("IdleLeft", false); 
            animator.SetBool ("IdleRight", false); 
        }
        
        if (Input.GetKeyDown (moveRight)) {  
            animator.SetBool ("WalkRight", false); 
            animator.SetBool ("WalkLeft", true); 
            animator.SetBool ("IdleLeft", false); 
            animator.SetBool ("IdleRight", false); 
        }

        if (Input.GetKeyUp (moveLeft)) {          
            animator.SetBool ("WalkRight", false); 
            animator.SetBool ("WalkLeft", false);
            animator.SetBool ("IdleRight", false); 
            animator.SetBool ("IdleLeft", true); 
        }

        if (Input.GetKeyUp (moveRight)) {          
            animator.SetBool ("WalkRight", false); 
            animator.SetBool ("WalkLeft", false);
            animator.SetBool ("IdleRight", true); 
            animator.SetBool ("IdleLeft", true); 
        }
        */
    }
}
