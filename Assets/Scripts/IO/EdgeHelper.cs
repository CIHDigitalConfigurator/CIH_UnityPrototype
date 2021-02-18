using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EdgeHelper
{
    public struct Edge
    {
        public int v1;
        public int v2;
        public int triangleIndex;
        public double v1x;
        public double v1z;
        public double v2x;
        public double v2z;
        public Edge(int aV1, int aV2, int aIndex, Vector3[] vertices)
        {
            v1 = aV1;
            v2 = aV2;
            triangleIndex = aIndex;
            v1x = Math.Round(vertices[v1].x, 2);
            v1z = Math.Round(vertices[v1].z, 2);
            v2x = Math.Round(vertices[v2].x, 2);
            v2z = Math.Round(vertices[v2].z, 2);

        }
    }

    public static List<Edge> GetEdges(int[] aIndices, Vector3[] vertices)
    {
        List<Edge> result = new List<Edge>();
        for (int i = 0; i < aIndices.Length; i += 3)
        {
            int v1 = aIndices[i];
            int v2 = aIndices[i + 1];
            int v3 = aIndices[i + 2];
            result.Add(new Edge(v1, v2, i, vertices));
            result.Add(new Edge(v2, v3, i, vertices));
            result.Add(new Edge(v3, v1, i, vertices));
        }

        return result;
    }

    public static List<Edge> FindBoundary(this List<Edge> aEdges)
    {
        List<Edge> result = new List<Edge>(aEdges);
        for (int i = result.Count - 1; i > 0; i--)
        {
            for (int n = i - 1; n >= 0; n--)
            {
               
                if (result[i].v1x == result[n].v1x && result[i].v1z == result[n].v1z && result[i].v2x == result[n].v2x && result[i].v2z == result[n].v2z || result[i].v1x == result[n].v2x && result[i].v1z == result[n].v2z && result[i].v2x == result[n].v1x && result[i].v2z == result[n].v1z)
                {
                    // shared edge so remove both
                    result.RemoveAt(i);
                    result.RemoveAt(n);
                    i--;
                    break;
                }
            }
        }
        return result;
    }
    public static List<Edge> SortEdges(this List<Edge> aEdges)
    {
        List<Edge> result = new List<Edge>(aEdges);
        for (int i = 0; i < result.Count - 2; i++)
        {
            Edge E = result[i];
            for (int n = i + 1; n < result.Count; n++)
            {
                Edge a = result[n];
                if (E.v2x == a.v1x && E.v2z == a.v1z)
                {
                    // in this case they are already in order so just continoue with the next one
                    if (n == i + 1)
                        break;
                    // if we found a match, swap them with the next one after "i"
                    result[n] = result[i + 1];
                    result[i + 1] = a;
                    break;
                }
            }
        }
        return result;
    }

    public static bool MeshesHaveCommondge(GameObject a, GameObject b)
    {
        int commonVertices = 0;

        Vector3[] verticesA = a.GetComponent<MeshFilter>().mesh.vertices;
        Vector3[] verticesB = b.GetComponent<MeshFilter>().mesh.vertices;

        Vector3[] aworld = verticesA.Select(vertice => a.transform.TransformPoint(vertice)).ToArray();
        Vector3[] bworld = verticesB.Select(vertice => b.transform.TransformPoint(vertice)).ToArray();


        for (int i = 0; i < aworld.Length; i++)
        {
            for (int j = 0; j < bworld.Length; j++)
            {
                if (Math.Round(aworld[i].x, 2) == Math.Round(bworld[j].x, 2)
                    && Math.Round(aworld[i].y, 2) == Math.Round(bworld[j].y, 2)
                    && Math.Round(aworld[i].z, 2) == Math.Round(bworld[j].z, 2))
                {
                    commonVertices++;
                }
            }
        }

        return commonVertices > 1;
    }



}
