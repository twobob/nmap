using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Map
{
    public class MapTexture
    {
        int _textureScale;
        public MapTexture(int textureScale)
        {
            _textureScale = textureScale;
        }

        public void AttachTexture(GameObject plane, Map map,NoisyEdges noisyEdge)
        {

            int _textureWidth = (int)Map.Width * _textureScale;
            int _textureHeight = (int)Map.Height * _textureScale;

            Texture2D texture = new Texture2D(_textureWidth, _textureHeight);
            texture.SetPixels(Enumerable.Repeat(Color.magenta, _textureWidth * _textureHeight).ToArray());

            var lines = map.Graph.edges.Where(p => p.v0 != null).Select(p => new[] 
            { 
                p.v0.point.x, p.v0.point.y,
                p.v1.point.x, p.v1.point.y
            }).ToArray();

            foreach (var c in map.Graph.centers)
                texture.FillPolygon(c.corners.Select(p => new Vector2(p.point.x * _textureScale, p.point.y * _textureScale)).ToArray(), BiomeProperties.Colors[c.biome]);

            foreach (var line in lines)
                DrawLine(texture, line[0], line[1], line[2], line[3], Color.black);

            foreach (var line in map.Graph.edges.Where(p => p.river > 0 && !p.d0.water && !p.d1.water))
                DrawLine(texture, line.v0.point.x, line.v0.point.y, line.v1.point.x, line.v1.point.y, Color.blue);

            //绘制扰乱的边缘
            foreach (Center p in map.Graph.centers)
            {
                foreach (var r in p.neighbors)
                {
                    Edge edge = map.Graph.lookupEdgeFromCenter(p, r);
                    if (!noisyEdge.path0.ContainsKey(edge.index) || !noisyEdge.path1.ContainsKey(edge.index))
                    {
                        // It's at the edge of the map, where we don't have
                        // the noisy edges computed. TODO: figure out how to
                        // fill in these edges from the voronoi library.
                        continue;
                    }
                    List<Vector2> edge0 = noisyEdge.path0[edge.index];
                    for (int i = 0; i < edge0.Count - 1; i++)
                        DrawLine(texture, edge0[i].x, edge0[i].y, edge0[i + 1].x, edge0[i + 1].y, Color.red);

                    List<Vector2> edge1 = noisyEdge.path1[edge.index];
                    for (int i = 0; i < edge1.Count - 1; i++)
                        DrawLine(texture, edge1[i].x, edge1[i].y, edge1[i + 1].x, edge1[i + 1].y, Color.red);
                }
            }

            texture.Apply();

            plane.GetComponent<Renderer>().material.mainTexture = texture;
            plane.transform.localPosition = new Vector3(Map.Width / 2, Map.Height / 2, 1);
        }

        private void DrawLine(Texture2D texture, float x0, float y0, float x1, float y1, Color color)
        {
            texture.DrawLine((int)(x0 * _textureScale), (int)(y0 * _textureScale), (int)(x1 * _textureScale), (int)(y1 * _textureScale), color);
        }
    }
}
