using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace TubeMeshGeneration
{
    public class Tube
    {
        private static readonly float StartingSize = 3;
        private static readonly float MinimumSize = .8f;
        protected static float Size = 3;
        private float _minTubeLength = 6;
        private float _decreaseSizeFactor = 50;
        private const float MapHeight = 10;
        
        private static Transform _transform;
        private static Transform _colliderParent;
        private static Transform _triggerParent;
        private static Transform _deadEndParent;
        
        private static GameObject _removeTrigger;
        private static GameObject _scoreTrigger;
        
        private int _triggerStartIndex = 5;
        protected internal bool LastDirectionWasUp;
        public Mesh Terrain { get; protected set; }
        private Material _mat;

        #region Initializiation
        public Tube(MeshGenerator meshGenerator)
        {
            _transform = meshGenerator.transform;
            _removeTrigger = meshGenerator.removeTrigger;
            _scoreTrigger = meshGenerator.scoreTrigger;
            _mat = meshGenerator.mat;
            CreateTransformContainers();
            InitializeMesh();
        }

        protected Tube(Tube tube)
        {
        }

        public static void ResetSize()
        {
            Size = StartingSize;
        }

        public static float GetSize()
        {
            return Size;
        }

        private void DecreaseSize(float factor)
        {
            if (Size < MinimumSize) return;
            Size -= factor;
            _minTubeLength = Size * 2;
        }
        
        private void CreateTransformContainers()
        {
            _colliderParent = new GameObject
                {name = "Colliders", transform = {parent = _transform, localPosition = Vector3.zero}}.transform;
            _triggerParent = new GameObject 
                {name = "Triggers", transform = {parent = _transform, localPosition = Vector3.zero}}.transform;
            _deadEndParent = new GameObject 
                {name = "Dead Ends", transform = {parent = _transform, localPosition = Vector3.zero, localRotation = Quaternion.Euler(Vector3.zero)}}.transform;
        }

        private void InitializeMesh()
        {
            Terrain = new Mesh();
            var vertices = new Vector3[2];

            vertices[0] = new Vector3(Size / 2, -Size);
            vertices[1] = new Vector3(Size / 2, 0);

            Terrain.vertices = vertices;
        }
        #endregion

        #region MeshGeneration
        public void AddVertices(bool up)
        {
            var vertices = Terrain.vertices.ToList();
            var triangles = Terrain.triangles.ToList();
            var count = vertices.Count - 1;

            vertices.AddRange(up ? AddToTop(vertices) : AddSideways(vertices));
            Terrain.vertices = vertices.ToArray();

            triangles.AddRange(new[] {count, count + 1, count + 3, count + 1, count + 2, count + 3});
            Terrain.triangles = triangles.ToArray();
            
            
            bool deadEndGetsCreated = Random.Range(0f, 1f) > 0.7f;
            CreateColliders(up);
            LastDirectionWasUp = up;

            if(!(this is DeadEndTube) && deadEndGetsCreated)
                CreateDeadEndTrap();
            DecreaseSize(Size/_decreaseSizeFactor);
        }

        private Vector3[] AddSideways(List<Vector3> vertices)
        {
            //   ________
            //  |  |____|        leftTop -------- rightTop
            //  |  |                |     Trigger    |
            //  |__|          leftBottom -------- rightBottom

            var count = vertices.Count - 1;
            var tubeLength = GetRandomLength(false, _transform.TransformPoint(vertices[count - 1]).y);
            var leftTop = vertices[count];
            var leftBottom = leftTop - new Vector3(0, Size);
            var rightTop = leftTop + new Vector3(tubeLength, 0);
            var rightBottom = rightTop - new Vector3(0, Size);

            CreateTrigger(_transform.TransformPoint(leftTop - new Vector3(Size/2, Size/2, 0)));

            return new[] {leftBottom, rightBottom, rightTop};
        }


        private Vector3[] AddToTop(List<Vector3> vertices)
        {
            //        __               leftTop -- rightTop
            //       |  |                   | Trig |
            //   ____|__|                   | ger  |
            //  |_______|           leftBottom -- rightBottom

            var count = vertices.Count - 1;
            var tubeLength = GetRandomLength(true, _transform.TransformPoint(vertices[count - 1]).y);
            var rightBottom = vertices[count];
            var leftBottom = rightBottom - new Vector3(Size, 0);
            var rightTop = rightBottom + new Vector3(0, tubeLength);
            var leftTop = rightTop - new Vector3(Size, 0);
            
            CreateTrigger(_transform.TransformPoint(leftBottom + new Vector3(Size/2, -Size/2, 0)));

            return new[] {leftBottom, leftTop, rightTop};
        }

        private float GetRandomLength(bool dirUp, float yPos)
        {
            var maxLength = CalculateMaxPossibleLength(dirUp, yPos);
            var length = Random.Range(_minTubeLength, maxLength);

            return length;
        }

        protected virtual float CalculateMaxPossibleLength(bool dirUp, float yPos)
        {
            var maxLength = dirUp
                ? Mathf.Sqrt(2 * Mathf.Pow(MapHeight - yPos, 2))
                : Mathf.Sqrt(2 * Mathf.Pow(yPos, 2));
            // Subtract the Width of the Tube
            maxLength -= Mathf.Sqrt(2 * Mathf.Pow(Size, 2));
            return maxLength;
        }

        private void CreateDeadEndTrap()
        {
            GameObject deadEnd = new GameObject{transform = { parent = _deadEndParent, localPosition = Vector3.zero, localRotation = Quaternion.Euler(Vector3.zero)}};
            DeadEndTube deadEndTube = new DeadEndTube(this);
            Object.Destroy(_colliderParent.GetChild(_colliderParent.childCount - 1).gameObject);
            deadEndTube.CreateDeadEnd(LastDirectionWasUp);
            deadEnd.AddComponent<MeshRenderer>().material = _mat;
            deadEnd.AddComponent<MeshFilter>().mesh = deadEndTube.Terrain;
            deadEnd.AddComponent<BoxCollider2D>().isTrigger = true;
        }
        
        #endregion
        
        public void RemoveLastVertices()
        {
            List<Vector3> vertices = Terrain.vertices.ToList();
            List<int> triangles = Terrain.triangles.ToList();
            
            vertices.RemoveRange(0, 3);
            triangles.RemoveRange(triangles.Count-6, 6);
            
            Terrain.triangles = triangles.ToArray();
            Terrain.vertices = vertices.ToArray();
            
            AddVertices(!LastDirectionWasUp);
        }
        
        private void CreateTrigger(Vector3 position)
        {
            if (this is DeadEndTube) return;
            Object.Instantiate(_scoreTrigger,position,Quaternion.Euler(new Vector3(0,0,-45)), _triggerParent);
            if (_triggerStartIndex > 0) 
            {
                _triggerStartIndex--;
                return;
            }
            Object.Instantiate(_removeTrigger,position,Quaternion.Euler(new Vector3(0,0,-45)), _triggerParent);
        }

        protected virtual void CreateColliders(bool up)
        {
            var vertices = Terrain.vertices;
            var length = vertices.Length - 1;
            Vector3 difference = up ? new Vector3(0, Size, 0) : new Vector3(Size, 0, 0);
            CreateCollidersFromPairs(new []
            {
                // leave Gap for next Section
                new KeyValuePair<Vector3, Vector3>(vertices[length] - difference, vertices[length - 3]),
                new KeyValuePair<Vector3, Vector3>(vertices[length - 1], vertices[length - 2]),
                new KeyValuePair<Vector3, Vector3>(vertices[length], vertices[length - 1]),
            });
        }
        
        protected void CreateCollidersFromPairs(KeyValuePair<Vector3, Vector3>[] colliderPairs)
        {
            foreach (var border in colliderPairs) 
                CreateEdgeColliderComponent(border.Key, border.Value);
        }
        
        private void CreateEdgeColliderComponent(Vector3 firstPosition, Vector3 secondPosition)
        {
            firstPosition = _transform.TransformPoint(firstPosition);
            secondPosition = _transform.TransformPoint(secondPosition);

            var collider = new GameObject {transform = {parent = _colliderParent}, name = "Col: " + _transform.childCount, layer = 3};
            var edgeCollider2D = collider.AddComponent<EdgeCollider2D>();
            edgeCollider2D.isTrigger = true;
            edgeCollider2D.points = new Vector2[] {firstPosition, secondPosition};
        }

    }
}