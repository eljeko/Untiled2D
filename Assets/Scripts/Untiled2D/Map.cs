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
using System;
using System.Collections;
using System.Collections.Generic;
 
namespace Untiled2D
{
    public class Map
    {
        public string orientation { get; set; }

        public int imageheight { get; set; }

        public int imagewidth { get; set; }

        public string imagesrc { get; set; }

        public int tileheight { get; set; }

        public int tilewidth { get; set; }

        private  List<Layer>  layers = new  List<Layer> ();
        private  List<ObjectGroup>  objectGroups = new  List<ObjectGroup> ();

        public string version { get; set; }

        public int width { get; set; }

        public int height { get; set; }

        public Map ()
        {
        }

        public void AddLayer (Layer layer)
        {
            layers.Add (layer);
        }

        public int GetLayersCount ()
        {
            return layers.Count;
        }

        public List<Layer> GetLayers ()
        {
            return layers;
        }

        public Layer GetLayer (int i)
        {
            return layers [i];
        }

        public void AddObjectGroup (ObjectGroup objectGroup)
        {
            objectGroups.Add (objectGroup);
        }

        public List<ObjectGroup> GetObjectGroup ()
        {
            return objectGroups;
        }
        
        public override string ToString ()
        {
            return "v: " + version + " w:" + this.width + " h:" + this.height + " layers: " + layers.Count + " ObjectGroup: " + objectGroups.Count;
        }
        


    }
}

