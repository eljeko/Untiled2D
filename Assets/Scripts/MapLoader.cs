﻿/*
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
using System.Xml;
using System.Collections;
using System;
using Untiled2D;

class MapLoader : MonoBehaviour
{
    public KeyCode printInfoKey;
    public KeyCode generateTiles;
    public KeyCode drawMap;
    public GUIText infolabel;
    private Map map;
    //
    private Texture2D tilesTexture;
    private GameObject tilesparent;
    private int TILEWIDTH = 0;
    private int TILEHEIGHT = 0;
 
    void Start ()
    {
        //  int tileCountX = tilesTexture.width / TILEWIDTH;
        //  int tileCountY = tilesTexture.height / TILEHEIGHT;
        
        string xmldata = System.IO.File.ReadAllText (Application.streamingAssetsPath + "/Maps/platformmap.tmx");

        //Debug.Log ("Loaded following XML " + xmldata);
    
        //Create a new XML document out of the loaded data
        XmlDocument xmlDoc = new XmlDocument ();
        xmlDoc.LoadXml (xmldata);
             
        map = CreateMap (xmlDoc.SelectNodes ("map"));

        TILEHEIGHT = map.tileheight;
        TILEWIDTH = map.tilewidth;

        tilesTexture = new Texture2D (map.imagewidth, map.imageheight);
        tilesTexture.LoadImage (System.IO.File.ReadAllBytes (Application.streamingAssetsPath + "/Maps/" + map.imagesrc));
        tilesTexture.name = "Tileset"; 

        tilesparent = Instantiate (Resources.Load ("Dummy")) as GameObject;
        tilesparent.name = "TileMap [" + map.imagesrc + "]";

        tilesparent.transform.position = new Vector3 (0, 0, 0.0f);
        
    }
    
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown (printInfoKey)) {  
            PrintMapInfo (map);
            if (infolabel != null) {
                infolabel.text = "MAP " + map.ToString () + " Tiles on layer 0 " + map.GetLayer (0).getTiles ().Count;
            }
        }

        if (Input.GetKeyDown (generateTiles)) {          
            if (infolabel != null) {
                infolabel.text = "Loaded Prefabs ";
                CreatePrefabs ();
            }
        }

        if (Input.GetKeyDown (drawMap)) {          
            if (infolabel != null) {
                infolabel.text = "Draw Map";
                DrawMap (0, 0);
            }
        }

    }

    void DrawMap (int offsetx, int offsety)
    {

        int width = map.width;
        int height = map.height;

        float TILEWIDTH_to_world_units = TILEWIDTH / 100f;
        float TILEHEIGHT_to_world_units = TILEHEIGHT / 100f;
    
        int currentLayer = map.GetLayers().Count;

        foreach (Layer aLayer in map.GetLayers()) { 
            int currentRow = 0;
            int currentCol = 0;
            float xPos = 0;
            float yPos = 0;
            GameObject layerparent = Instantiate (Resources.Load ("Dummy")) as GameObject;
            layerparent.name = aLayer.name;

            Debug.Log("Drawing " + aLayer.name);

            foreach (Tile aTile in aLayer.getTiles()) {
                if (aTile.gid != 0) {
                    xPos = offsetx + (currentCol * TILEWIDTH_to_world_units);
                    yPos = offsety + (currentRow * TILEHEIGHT_to_world_units * -1);                    
                    //Debug.Log ("Tile g["+aTile.Gid+"]  x:" + xPos + " y:" + yPos+ " ["+currentRow+":"+currentCol+"]");
                    CreateTile (aTile.gid, xPos, yPos,currentLayer, TILEWIDTH, TILEHEIGHT, layerparent);  
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
            layerparent.transform.parent = tilesparent.transform;
        }
    }
    
    private Map CreateMap (XmlNodeList nodes)
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
      
        }

        return map;
    }
  
    private void CreatePrefabs ()
    {             
        CreateTile (21, 0.0f, 0.0f,0.0f, TILEWIDTH, TILEHEIGHT, tilesparent);    
    }

    /// <summary> 
    /// Enter description for method bb. 
    /// ID string generated is "M:N.X.bb(System.String,System.Int32@,System.Void*)".
    /// </summary> 
    /// <param name="gid">Describe parameter.</param>
    /// <param name="x">Describe parameter.</param>
    /// <param name="y">Describe parameter.</param>
    /// <param name="TILEWIDTH">Describe parameter.</param>
    /// <param name="TILEHEIGHT">Describe parameter.</param>
    /// <param name="tilesparent">Describe parameter.</param>
    void CreateTile (int gid, float x, float y,float z, int TILEWIDTH, int TILEHEIGHT, GameObject tilesparent)
    {

        if (gid > 0) {
            int tiles_cols = map.imagewidth / TILEWIDTH;
            int tiles_rows = map.imageheight / TILEHEIGHT;
        
            decimal i = gid / tiles_cols;
            int row = (int)Math.Ceiling (i);
        
            int moduleRows = gid % tiles_cols;
        
            int col = 0;   
        
            if (moduleRows > 0) {
                col = moduleRows - 1;
            } else {
                col = tiles_cols - 1;
            }
        
            int tile_x = col * TILEWIDTH;
            int tile_y = map.imageheight - ((row * TILEHEIGHT) + TILEHEIGHT);

            //Texture2D tileTexture = getTileTexture2D (tile_x, tile_y, TILEWIDTH, TILEHEIGHT); 


            GameObject newTile = Instantiate (Resources.Load ("Dummy")) as GameObject;

            newTile.name = "Tile g:" + gid + " at (" + x + ":" + y + ")";
            SpriteRenderer renderer = newTile.AddComponent<SpriteRenderer> ();

            //Sprite sprite = Sprite.Create (tileTexture, new Rect (0, 0, TILEWIDTH, TILEHEIGHT), new Vector2 (0f, 0f), 100);
            Sprite sprite = Sprite.Create (tilesTexture, new Rect (tile_x, tile_y, TILEWIDTH, TILEHEIGHT), new Vector2 (0f, 0f), 100);

            //we want pixelperfect!
            sprite.texture.filterMode = FilterMode.Point;//This disable the antialias filter
            //in unity 2D always 100
            sprite.name = "Demo";
            renderer.sprite = sprite;
            newTile.transform.position = new Vector3 (x, y, z);

            newTile.transform.parent = tilesparent.transform;

//            newTile.transform.localScale = new Vector3(0.16f,0.16f,0f);
        }
    }

    public Texture2D getTileTexture2D (int x, int y, int width, int height)
    {
        Color[] pixels = tilesTexture.GetPixels (x, y, width, height);
        // get the block of pixels
        Texture2D aTileTexture = new Texture2D (width, height);
        // create new texture to copy the pixels to it
        aTileTexture.SetPixels (pixels);
        aTileTexture.anisoLevel = 1;
        aTileTexture.alphaIsTransparency = true;
        aTileTexture.Apply ();
        // important to save changes
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