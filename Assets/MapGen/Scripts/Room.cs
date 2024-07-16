using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using static MapGen.Tile;

namespace MapGen
{
    public class Room : MonoBehaviour
    {
        [System.Serializable]
        struct LayoutElement
        {
            public Vector2Int index;
            public Tile.ConnectionPoint ConnectionPoint;
        }
        public int Width = 1;
        public int Height = 1;
        public Color color;

        [SerializeField]
        private List<LayoutElement> Layout;

        [SerializeField]
        private float GizmoLineLength = 1f;

        private void OnDrawGizmos()
        {
            for(int i=0; i<Layout.Count; i++)
            {
                Vector3 position = transform.position + new Vector3(5*Layout[i].index.x, 0, 5*-Layout[i].index.y);
                Gizmos.color = Color.white;
                Gizmos.DrawCube(position, new Vector3(1, 0, 1));

                Tile.ConnectionPoint connectionPoints = Layout[i].ConnectionPoint;
                Gizmos.color = Color.blue;
                if (connectionPoints.HasFlag(Tile.ConnectionPoint.TopMiddle))
                {
                    Gizmos.DrawLine(position, position + new Vector3(0, 0, GizmoLineLength));
                }
                if (connectionPoints.HasFlag(Tile.ConnectionPoint.BottomMiddle))
                {
                    Gizmos.DrawLine(position, position - new Vector3(0, 0, GizmoLineLength));
                }
                if (connectionPoints.HasFlag(Tile.ConnectionPoint.LeftMiddle))
                {
                    Gizmos.DrawLine(position, position - new Vector3(GizmoLineLength, 0, 0));
                }
                if (connectionPoints.HasFlag(Tile.ConnectionPoint.RightMiddle))
                {
                    Gizmos.DrawLine(position, position + new Vector3(GizmoLineLength, 0, 0));
                }
            }
        }

        [Button]
        private void SaveLayout()
        {

        }

        public Tile.ConnectionPoint[,] GetLayout()
        {
            Tile.ConnectionPoint[,] layout = new Tile.ConnectionPoint[Height, Width];
            for (int i = 0; i < Layout.Count; i++)
            {
                layout[Layout[i].index.y, Layout[i].index.x] = Layout[i].ConnectionPoint;
            }
            return layout;
        }

        public void GenerateRoom(Tileset tileset, RectInt rect)
        {
            Tile.ConnectionPoint[,] layout = new Tile.ConnectionPoint[Height, Width];
            for (int i = 0; i < Layout.Count; i++)
            {
                layout[Layout[i].index.y, Layout[i].index.x] = Layout[i].ConnectionPoint;
            }

            for(int i=0; i<Height; i++)
            {
                for(int j=0; j<Width; j++)
                {
                    if(tileset.ruleTiles.TryGetValue(layout[i,j], out var tile))
                    {
                        var tileInstance = Instantiate(tile[0], transform);
                        tileInstance.transform.localPosition = new Vector3(5 * j, 0, Height*5-5*i);
                    }
                    else
                    {
                        Debug.LogWarning($"{layout[i, j]} not found in tileset({tileset.name}!");
                        var allPoints = Tile.ConnectionPoint.TopMiddle | ConnectionPoint.LeftMiddle | ConnectionPoint.RightMiddle | ConnectionPoint.BottomMiddle;
                        var tileInstance = Instantiate(tileset.ruleTiles[allPoints][0], transform);
                        tileInstance.transform.localPosition = new Vector3(5 * j, 0, Height*5-5 * i);
                        var renderer = tileInstance.GetComponentsInChildren<MeshRenderer>();
                        foreach(var r in renderer)
                        {
                            r.material.color = color;
                        }
                    }
                }
            }
        }
    }
}