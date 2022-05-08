using System.Collections.Generic;
using UnityEngine;

namespace TubeMeshGeneration
{
    public class DeadEndTube : Tube
    {
        private readonly Tube _mainTube;
        public DeadEndTube(Tube tube) : base(tube)
        {
            _mainTube = tube;
            InitializeMesh();
        }

        private void InitializeMesh()
        {
            Terrain = new Mesh();
            var mainVertices = _mainTube.Terrain.vertices;
            var vertices = new Vector3[2];
            Vector3 difference = _mainTube.LastDirectionWasUp ? new Vector3(Size, 0, 0) : new Vector3(0, Size, 0);

            vertices[0] = mainVertices[mainVertices.Length - 1] + difference;
            vertices[1] = mainVertices[mainVertices.Length - 2] + difference;

            Terrain.vertices = vertices;
        }

        protected internal void CreateDeadEnd(bool up)
        {
            AddVertices(up);
        }

        protected override void CreateColliders(bool up)
        {
            var vertices = Terrain.vertices;
            var length = vertices.Length - 1;
            CreateCollidersFromPairs(new []
            {
                new KeyValuePair<Vector3, Vector3>(vertices[length], vertices[length - 1]),
                new KeyValuePair<Vector3, Vector3>(vertices[length - 1], vertices[length - 2]),
                new KeyValuePair<Vector3, Vector3>(vertices[length], vertices[length - 3]),
            });
        }
    }
}