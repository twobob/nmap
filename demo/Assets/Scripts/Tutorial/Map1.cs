using Delaunay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Map
{
    public class Map1
    {
        private int _pointCount = 500;
        float _lakeThreshold = 0.3f;
        public const float Width = 500;
        public const float Height = 500;
        const int NUM_LLOYD_RELAXATIONS = 2;

        public Graph Graph { get; private set; }
        public Center SelectedCenter { get; private set; }

        public List<Vector2> OriPoints = new List<Vector2>();

        public Map1(bool needRelax = false)
        {
            List<uint> colors = new List<uint>();

            for (int i = 0; i < _pointCount; i++)
            {
                colors.Add(0);
                OriPoints.Add(new Vector2(
                        UnityEngine.Random.Range(0, Width),
                        UnityEngine.Random.Range(0, Height))
                );
            }

            if (needRelax)
            {
                for (int i = 0; i < NUM_LLOYD_RELAXATIONS; i++)
                    OriPoints = Graph.RelaxPoints(OriPoints, Width, Height).ToList();
            }

            var voronoi = new Voronoi(OriPoints, colors, new Rect(0, 0, Width, Height));

            Graph = new Graph(OriPoints, voronoi, (int)Width, (int)Height, _lakeThreshold);
        }
    }
}
