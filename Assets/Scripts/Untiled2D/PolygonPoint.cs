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
using System;
namespace Untiled2D
{
    public class PolygonPoint
    {
        public int x {
            get;
            set;
        }

        public int y {
            get;
            set;
        }

        public PolygonPoint (string point)
        {
            string[] pointSplit = point.Split (',');
            this.x = Convert.ToInt16 (pointSplit [0]);
            this.y = Convert.ToInt16 (pointSplit [1]);
        }

        public PolygonPoint (int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}

