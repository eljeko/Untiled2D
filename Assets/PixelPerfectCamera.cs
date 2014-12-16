/*
 * 
 * Copyright 2014 Stefano Linguerri
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *      
 *      http://www.apache.org/licenses/LICENSE-2.0
 *      
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 */

/**
 * For this Script thanks to:
 * 
 * http://aidtech-game.com/pixel-perfect-camera-unity-2d/
 * by Alan Mcklin
 * 
 */
using UnityEngine;

using System.Collections;

public class PixelPerfectCamera : MonoBehaviour
{
    

    float unitsPerPixel;
    public float textureUnitsSize = 100f;
    //this will let you do  a 2x,3x,4x perfecet pixel Zoom.
    public int multiplier = 1;
 

    void Start ()
    {
 
        unitsPerPixel = 1f / textureUnitsSize;
        
        Camera.main.orthographicSize = ((Screen.height / 2f) * unitsPerPixel) / multiplier;
        
    }
    
}
