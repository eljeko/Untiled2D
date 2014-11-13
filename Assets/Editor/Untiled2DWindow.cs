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
using UnityEditor;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System;
using Untiled2D;

class Untiled2DWindow : EditorWindow
{
    //
    private Map map;
    public string mapFileName = "";
    public string mapObjectName = "";
    public int startOffsetX = 0;
    public int startOffsetY = 0;
    public bool drawOnStrat = false;
    //
    private Texture2D tilesTexture;
    private GameObject mapParent;
    private int TILEWIDTH = 0;
    private int TILEHEIGHT = 0;
    //
    float TILEWIDTH_to_world_units = 0f;
    float TILEHEIGHT_to_world_units = 0f;

    [MenuItem ("Untiled2D/Importer")]
    
    public static void  ShowWindow ()
    {
        EditorWindow.GetWindow (typeof(Untiled2DWindow));
    }
    
    void OnGUI ()
    {
        GUILayout.Label ("Choose file map to load", EditorStyles.boldLabel);

        mapObjectName = EditorGUILayout.TextField ("Map object name", mapObjectName);       
        string path = "";

        if (GUILayout.Button ("Load Tiled Map", new GUILayoutOption[]{ GUILayout.Width (300)})) {           
            path = EditorUtility.OpenFilePanel ("Load Tiled map file", "", "tmx");
            TiledMapImporter tmi = new TiledMapImporter (path);                     
        }

    }

   
}