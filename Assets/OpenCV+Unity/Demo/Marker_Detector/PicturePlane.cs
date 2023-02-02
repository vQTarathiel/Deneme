using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenCvSharp.Demo
{
    class PicturePlane : MonoBehaviour
    {
        private Vector3[] vertices = new Vector3[6];
        private int[] triangles = new int[6];
        private Mesh mesh;

        public void MoveToPosition(List<Point2f> coordinates)
        {
            var picturePlane = GameObject.Find("PicturePlane");
            // Set the vertices of the plane
            vertices[0] = new Vector3(coordinates[3].X, coordinates[3].Y, -5f);
            vertices[1] = new Vector3(coordinates[2].X, coordinates[2].Y, -5f);
            vertices[2] = new Vector3(coordinates[0].X, coordinates[0].Y, -5f);
            vertices[3] = new Vector3(coordinates[1].X, coordinates[1].Y, -5f);

            // Set the triangles of the plane
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;

            // Get the MeshFilter component and create a new mesh
            mesh = new Mesh();
            mesh = picturePlane.GetComponent<MeshFilter>().mesh;

            mesh.Clear();

            // Set the vertices and triangles of the mesh
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            
        }

    }
}
