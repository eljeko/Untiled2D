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
using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System;
using Untiled2D;

class Untiled2DWindow : EditorWindow
{
    //
    private Map map;
    public string mapFileName;
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
            string parentPath = "";

            if (Application.platform == RuntimePlatform.OSXEditor) {
                parentPath = path.Substring (0, path.LastIndexOf ("/") + 1); 
            } else if (Application.platform == RuntimePlatform.WindowsEditor) {
                parentPath = path.Substring (0, path.LastIndexOf ("\\") + 1); 
            }

            string xmldata = System.IO.File.ReadAllText (path);

            //Create a new XML document out of the loaded data
            XmlDocument xmlDoc = new XmlDocument ();
            xmlDoc.LoadXml (xmldata);
            
            map = ParseMap (xmlDoc.SelectNodes ("map"));
            
            TILEHEIGHT = map.tileheight;
            TILEWIDTH = map.tilewidth;
            //Calculat the Tile w/h in world units
            TILEWIDTH_to_world_units = TILEWIDTH / 100f;
            TILEHEIGHT_to_world_units = TILEHEIGHT / 100f;
            
            tilesTexture = new Texture2D (map.imagewidth, map.imageheight);
            tilesTexture.LoadImage (System.IO.File.ReadAllBytes (parentPath + map.imagesrc));
            tilesTexture.name = "Tileset"; 
            
            mapParent = Instantiate (Resources.Load ("Dummy")) as GameObject;
            mapParent.name = "TileMap [" + map.imagesrc + "]";
            
            mapParent.transform.position = new Vector3 (0, 0, 0.0f);
                         
            DrawMap (startOffsetX, startOffsetY);             
            
        }

    }

    void DrawMap (int offsetx, int offsety)
    {
        DrawLayers (offsetx, offsety);
        DrawObjects (offsetx, offsety);
    }
    
    void DrawObjects (int offsetx, int offsety)
    {
        foreach (ObjectGroup anObjectGroup in map.GetObjectGroup()) { 
            
            GameObject objectGroupParent = Instantiate (Resources.Load ("Dummy")) as GameObject;
            objectGroupParent.name = "ObjectGroups: " + anObjectGroup.name;
            
            foreach (MapObject aMapObject in anObjectGroup.GetMapObjects()) {
                
                if (aMapObject.type.Equals ("BoxCollider2d")) {
                    
                    GameObject boxColliderParent = Instantiate (Resources.Load ("Dummy")) as GameObject;
                    boxColliderParent.name = "Collider: " + aMapObject.name;
                    
                    BoxCollider2D aBoxCollider = boxColliderParent.AddComponent<BoxCollider2D> ();
                    
                    aBoxCollider.size = new Vector2 (aMapObject.width / 100f, aMapObject.height / 100f);
                    
                    float x = offsetx / 100f + ((aMapObject.x + aMapObject.width / 2) / 100f);
                    float y = offsety / 100f + (-1 * (aMapObject.y + aMapObject.height / 2) / 100f);
                    
                    boxColliderParent.transform.position = new Vector2 (x, y);
                    
                    boxColliderParent.transform.parent = objectGroupParent.transform;
                    
                }
                
                if (aMapObject.type.Equals ("PolygonCollider2d")) {
                    
                    GameObject boxColliderParent = Instantiate (Resources.Load ("Dummy")) as GameObject;
                    boxColliderParent.name = "Collider: " + aMapObject.name;
                    
                    PolygonCollider2D aPolygonCollider2D = boxColliderParent.AddComponent<PolygonCollider2D> ();
                    
                    List<Vector2> pathPoints = new List<Vector2> ();
                    
                    foreach (PolygonPoint aPoint in aMapObject.GetPoints()) { 
                        float px = offsetx / 100f + (aMapObject.x + aPoint.x) / 100f;
                        float py = offsety / 100f + (-1 * (aMapObject.y + aPoint.y) / 100f);
                        
                        Vector2 p = new Vector2 (px, py);
                        pathPoints.Add (p);
                    }
                    
                    Vector2[] path = pathPoints.ToArray ();
                    
                    aPolygonCollider2D.SetPath (0, path);
                    /*
                    aBoxCollider.size = new Vector2 (aMapObject.width / 100f, aMapObject.height / 100f);
                    */
                    
                    //  float x = offsetx / 100f + ((aMapObject.x + aMapObject.width / 2) / 100f);
                    //  float y = offsety / 100f + (-1 * (aMapObject.y + aMapObject.height / 2) / 100f);
                    
                    //boxColliderParent.transform.position = new Vector2 (x, y);
                    
                    boxColliderParent.transform.parent = objectGroupParent.transform;
                    
                }
                
            }
            
            objectGroupParent.transform.parent = mapParent.transform;
            
        }
    }
    
    void DrawLayers (int offsetx, int offsety)
    {
        
        int width = map.width;
        int height = map.height;
        
        
        
        int currentLayer = map.GetLayers ().Count;
        
        foreach (Layer aLayer in map.GetLayers()) { 
            int currentRow = 0;
            int currentCol = 0;
            float xPos = 0;
            float yPos = 0;
            GameObject layerparent = Instantiate (Resources.Load ("Dummy")) as GameObject;
            layerparent.name = "Layer: " + aLayer.name;
            
            Debug.Log ("Drawing " + aLayer.name);
            
            foreach (Tile aTile in aLayer.GetTiles()) {
                
                if (aTile.gid != 0) {
                    xPos = offsetx / 100f + (currentCol * TILEWIDTH_to_world_units);
                    yPos = offsety / 100f + (currentRow * TILEHEIGHT_to_world_units * -1);                    
                    CreateTile (aTile.gid, xPos, yPos, currentLayer, TILEWIDTH, TILEHEIGHT, layerparent);  
                }
                
                if (currentCol < width - 1) {
                    currentCol++;
                } else {
                    currentCol = 0;
                    currentRow++;
                    //  Debug.Log ("New Row " + currentRow);
                }
            }
            currentLayer--;
            layerparent.transform.parent = mapParent.transform;
        }
    }
    
    private Map ParseMap (XmlNodeList nodes)
    {
        Map map = new Map ();
        
        foreach (XmlNode node in nodes) {
            
            //Map Info
            map.version = (node.Attributes.GetNamedItem ("version").InnerText);
            map.orientation = (node.Attributes.GetNamedItem ("version").InnerText);
            map.width = (Convert.ToInt16 (node.Attributes.GetNamedItem ("width").InnerText));
            map.height = (Convert.ToInt16 (node.Attributes.GetNamedItem ("height").InnerText));
            
            //Tile info
            map.tilewidth = (Convert.ToInt16 (node.Attributes.GetNamedItem ("tilewidth").InnerText));
            map.tileheight = (Convert.ToInt16 (node.Attributes.GetNamedItem ("tileheight").InnerText));      
            var imageNode = node.SelectSingleNode ("//image");
            
            map.imagesrc = imageNode.Attributes ["source"].Value;
            map.imagewidth = Convert.ToInt16 (imageNode.Attributes ["width"].Value);
            map.imageheight = Convert.ToInt16 (imageNode.Attributes ["height"].Value);
            
            //Loop the layers
            foreach (XmlNode aLayerNode in node.SelectNodes("layer")) {
                string layerName = aLayerNode.Attributes ["name"].Value;
                Layer aLayer = new Layer (layerName); 
                //loop the tiles
                Debug.Log ("Found " + layerName);
                foreach (XmlNode aTile in aLayerNode.SelectNodes("//layer[@name='"+layerName+"']/data/tile")) {
                    Tile t = new Tile ();
                    t.gid = Convert.ToInt16 (aTile.Attributes ["gid"].Value);
                    aLayer.AddTile (t);
                }
                map.AddLayer (aLayer);
            }
            
            //Loop the Object Groups
            foreach (XmlNode anObjectGroup in node.SelectNodes("objectgroup")) {               
                string groupName = anObjectGroup.Attributes ["name"].Value;
                ObjectGroup objectGroup = new ObjectGroup (groupName);
                foreach (XmlNode anObject in anObjectGroup.SelectNodes("//objectgroup[@name='"+groupName+"']/object")) {
                    if (anObject.Attributes ["name"] != null) {
                        string objName = anObject.Attributes ["name"].Value;
                        
                        MapObject mo = new MapObject (objName);
                        
                        int x = Convert.ToInt16 (anObject.Attributes ["x"].Value);
                        int y = Convert.ToInt16 (anObject.Attributes ["y"].Value);
                        
                        if (anObject.Attributes ["width"] != null) {
                            int width = Convert.ToInt16 (anObject.Attributes ["width"].Value);
                            mo.width = width;
                        }
                        
                        if (anObject.Attributes ["height"] != null) {
                            int height = Convert.ToInt16 (anObject.Attributes ["height"].Value);
                            mo.height = height;
                        }
                        
                        string type = anObject.Attributes ["type"].Value;
                        
                        if (type.Equals ("PolygonCollider2d")) {
                            XmlNode polygonInfo = anObjectGroup.SelectSingleNode ("//objectgroup[@name='" + groupName + "']/object[@name='" + objName + "']/polygon");
                            
                            string pointsAsString = polygonInfo.Attributes ["points"].Value;
                            string[] pointsSplit = pointsAsString.Split (' ');
                            
                            foreach (string aPoint in pointsSplit) {
                                PolygonPoint polygonPoint = new PolygonPoint (aPoint); 
                                mo.AddPoint (polygonPoint);
                            }
                        }
                        
                        mo.type = type; 
                        mo.x = x;
                        mo.y = y;
                        
                        objectGroup.AddObject (mo);
                    }
                }
                map.AddObjectGroup (objectGroup);
            }
        }
        
        return map;
    }

    void CreateTile (int gid, float x, float y, float z, int TILEWIDTH, int TILEHEIGHT, GameObject tilesparent)
    {
        
        if (gid > 0) {
            int tiles_cols = map.imagewidth / TILEWIDTH;
            int tiles_rows = map.imageheight / TILEHEIGHT;
            
            int row = 0;
            int col = 0;   
            
            decimal i = gid / tiles_cols;
            
            int moduleRows = gid % tiles_cols;
            
            if (moduleRows > 0) {
                col = moduleRows - 1;
                row = (int)Math.Ceiling (i);
            } else {
                col = tiles_cols - 1;
                row = (int)Math.Ceiling (i) - 1;
            }
            
            int tile_x = col * TILEWIDTH;
            int tile_y = map.imageheight - ((row * TILEHEIGHT) + TILEHEIGHT);
            
            GameObject newTile = Instantiate (Resources.Load ("Dummy")) as GameObject;
            newTile.name = "Tile g:" + gid + " at (" + x + ":" + y + ")";
            
            SpriteRenderer renderer = newTile.AddComponent<SpriteRenderer> ();
            Sprite sprite = Sprite.Create (tilesTexture, 
                                           new Rect (tile_x, tile_y, TILEWIDTH, TILEHEIGHT), 
                                           new Vector2 (0.0f, 1.0f),//the pivot is relative 1 is max 0.5 half 0.0 min 
                                           100);
            //we want pixelperfect!
            sprite.texture.filterMode = FilterMode.Point;//This disable the antialias filter  
            sprite.texture.wrapMode = TextureWrapMode.Clamp;
            sprite.name = "Tile Sprite gid:" + gid;
            renderer.sprite = sprite;
            
            newTile.transform.position = new Vector3 (x, y, z);
            newTile.transform.parent = tilesparent.transform;
            
        }
    }
    
    public Texture2D getTileTexture2D (int x, int y, int width, int height)
    {
        // get the block of pixels
        Color[] pixels = tilesTexture.GetPixels (x, y, width, height);
        // create new texture to copy the pixels into
        Texture2D aTileTexture = new Texture2D (width, height);
        aTileTexture.SetPixels (pixels);
        //no ansiotropic stuff
        aTileTexture.anisoLevel = 1;
        // important to save changes
        aTileTexture.Apply ();       
        return aTileTexture;
    }
    
    private void PrintMapInfo (Map map)
    {
        Debug.Log ("Version :" + map.version);
        Debug.Log ("Width   :" + map.width);
        Debug.Log ("Height  :" + map.height);    
        map.GetLayer (0);
    }

   
}