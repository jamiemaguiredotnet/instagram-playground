using System;
using System.Collections.Generic;
using System.Text;

namespace Insta.Graph.Entity
{
    public class ComputerVisionInsight
    {
        public string ImageDescription { get; set; }
        public List<Item> Tags { get; set; }
        public List<Item> Brands {get;set;}
        public List<Item> DetectedObjects { get; set; }

        public ComputerVisionInsight()
        {
            Tags = new List<Item>();
            Brands = new List<Item>();
            DetectedObjects = new List<Item>();
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public double Confidence { get; set; }
    }

}
