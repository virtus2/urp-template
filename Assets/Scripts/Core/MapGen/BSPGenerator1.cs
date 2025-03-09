using System.Collections.Generic;
using UnityEngine;

namespace Core.MapGen
{

    public class BSPGenerator
    {
        public class BSPNode
        {
            public BSPNode parent;
            public BSPNode childA;
            public BSPNode childB;
            public RectInt rect;
            public int depth;
            public bool xAxis; // for test
            public bool isA; // for test
        }

        public class Result
        {
            public BSPNode root;
            public Dictionary<RectInt, BSPNode> allRectangles = new Dictionary<RectInt, BSPNode>();
            public Dictionary<Vector2Int, List<BSPNode>> rectBySize = new Dictionary<Vector2Int, List<BSPNode>>();
        }

        enum Axis { X, Y };

        // TODO: Divide random position 
        // TODO: Add minimum rectangle size

        public Result Generate(RectInt rect)
        {
            BSPNode root = new BSPNode();
            root.parent = null;
            root.rect = rect;
            root.depth = 0;

            Result result = new Result();
            result.root = root;
            result.allRectangles.Add(rect, root);

            if (Random.Range(0, 2) == 0)
            {
                BSP(result, root, Axis.X);
            }
            else
            {
                BSP(result, root, Axis.Y);
            }

            return result;
        }
        private void BSP(Result result, BSPNode node, Axis axis)
        {
            if (axis == Axis.X)
            {
                if (node.rect.width == 1) return;
                // X axis
                int randomX = (node.rect.xMin + node.rect.xMax) / 2;//Random.Range(node.rect.xMin+1, node.rect.xMax);

                RectInt leftRect = new RectInt(node.rect.xMin, node.rect.yMin, randomX - node.rect.xMin, node.rect.height);
                BSPNode left = new BSPNode();
                left.parent = node;
                left.rect = leftRect;
                left.depth = node.depth + 1;
                left.isA = true;
                left.xAxis = true;

                RectInt rightRect = new RectInt(randomX, node.rect.yMin, node.rect.xMax - randomX, node.rect.height);
                BSPNode right = new BSPNode();
                right.parent = node;
                right.rect = rightRect;
                right.depth = node.depth + 1;
                right.isA = false;
                right.xAxis = true;

                node.childA = left;
                node.childB = right;

                // Save child nodes to dictionary
                result.allRectangles.Add(leftRect, left);
                result.allRectangles.Add(rightRect, right);

                // Save chile nodes to rect size map
                Vector2Int leftSize = new Vector2Int(leftRect.width, leftRect.height);
                if (!result.rectBySize.ContainsKey(leftSize))
                {
                    result.rectBySize[leftSize] = new List<BSPNode>();
                }
                result.rectBySize[leftSize].Add(left);

                Vector2Int rightSize = new Vector2Int(leftRect.width, leftRect.height);
                if (!result.rectBySize.ContainsKey(rightSize))
                {
                    result.rectBySize[rightSize] = new List<BSPNode>();
                }
                result.rectBySize[rightSize].Add(right);

                // Use child nodes to BSP
                BSP(result, left, Axis.Y);
                BSP(result, right, Axis.Y);
            }
            else
            {
                if (node.rect.height == 1) { return; }
                // Y axis
                int randomY = (node.rect.yMin + node.rect.yMax) / 2;//Random.Range(node.rect.yMin+1, node.rect.yMax);

                RectInt topRect = new RectInt(node.rect.xMin, randomY, node.rect.width, node.rect.yMax - randomY);
                BSPNode top = new BSPNode();
                top.parent = node;
                top.rect = topRect;
                top.depth = node.depth + 1;
                top.isA = true;
                top.xAxis = false;

                RectInt bottomRect = new RectInt(node.rect.xMin, node.rect.yMin, node.rect.width, randomY - node.rect.yMin);
                BSPNode bottom = new BSPNode();
                bottom.parent = node;
                bottom.rect = bottomRect;
                bottom.depth = node.depth + 1;
                bottom.isA = false;
                bottom.xAxis = false;

                node.childA = top;
                node.childB = bottom;

                // Save child nodes to dictionary
                result.allRectangles.Add(topRect, top);
                result.allRectangles.Add(bottomRect, bottom);

                // Save chile nodes to rect size map
                Vector2Int topSize = new Vector2Int(topRect.width, topRect.height);
                if (!result.rectBySize.ContainsKey(topSize))
                {
                    result.rectBySize[topSize] = new List<BSPNode>();
                }
                result.rectBySize[topSize].Add(top);

                Vector2Int bottomSize = new Vector2Int(bottomRect.width, bottomRect.height);
                if (!result.rectBySize.ContainsKey(bottomSize))
                {
                    result.rectBySize[bottomSize] = new List<BSPNode>();
                }
                result.rectBySize[bottomSize].Add(bottom);

                // Use child nodes to BSP
                BSP(result, top, Axis.X);
                BSP(result, bottom, Axis.X);
            }
        }
    }
}