using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using static Core.MapGen.Tile;

namespace Core.MapGen
{
    public class Dungeon : MonoBehaviour
    {
        [SerializeField]
        private List<Room> RoomPrefabs;

        [SerializeField]
        private int Width;

        [SerializeField]
        private int Height;

        [SerializeField]
        private Vector2Int Start;

        [SerializeField]
        private Vector2Int End;

        public ConnectionPoint[,] Layout;
        public Tileset tileset;

        private RectInt rect;
        private Dictionary<RectInt, Room> rooms = new Dictionary<RectInt, Room>();

        private void Awake()
        {
            tileset.Initialize();
            GenerateUsingBSP();

            foreach (var r in rooms)
            {
                var roomInstance = Instantiate(r.Value, new Vector3(r.Key.position.x * 5, 0, r.Key.position.y * 5), Quaternion.identity, transform);
                roomInstance.GenerateRoom(tileset, rect);
            }
        }

        [Button]
        private void GenerateUsingBSP()
        {
            Layout = new ConnectionPoint[Height, Width];
            rect = new RectInt(new Vector2Int(0, 0), new Vector2Int(Width, Height));
            BSPGenerator BSP = new BSPGenerator();
            BSPGenerator.Result result = BSP.Generate(rect);

            int iteration = 0;
            int totalHeight = 0;
            int totalWidth = 0;
            // Fill the dungeon grid by pre-defined room
            while (totalHeight <= Height && totalWidth <= Width && iteration < 1000)
            {
                iteration++;
                // Pick random sized room 
                var randomRoom = RoomPrefabs[Random.Range(0, RoomPrefabs.Count)];
                Vector2Int roomSize = new Vector2Int(randomRoom.Width, randomRoom.Height);
                if (result.rectBySize.TryGetValue(roomSize, out var possibleNodes))
                {
                    var randomNode = possibleNodes[Random.Range(0, possibleNodes.Count)];

                    if (result.allRectangles.ContainsKey(randomNode.rect))
                    {
                        rooms.Add(randomNode.rect, randomRoom);
                        // totalHeight += randomRoom.Height;
                        // totalWidth += randomRoom.Width;

                        Queue<BSPGenerator.BSPNode> q = new Queue<BSPGenerator.BSPNode>();
                        q.Enqueue(randomNode);
                        while (q.Count > 0)
                        {
                            var node = q.Dequeue();
                            result.allRectangles.Remove(node.rect);
                            if (node.childA != null) q.Enqueue(node.childA);
                            if (node.childB != null) q.Enqueue(node.childB);
                        }
                    }
                }
            }
            Debug.Log($"iterations : {iteration}");
        }
    }
}