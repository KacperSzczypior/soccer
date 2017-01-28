using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace readyforsomesoccer
{
    public class SoccerBoard
    {
        protected Node[,] nodes;

        public SoccerBoard(int x, int y)
        {
            nodes = new Node[x + 1, y + 3];
            for (int i = 0; i < x + 1; i++)
            {
                for (int j = 0; j < y + 3; j++)
                {
                    nodes[i, j] = new Node();
                    nodes[i, j].x = i;
                    nodes[i, j].y = j;
                }
            }
            createEdges();
            drawUnnecessary();
        }

        private void createEdges()
        {
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    createEdgesForNode(i, j);
                }
            }
        }

        private void drawUnnecessary()
        {
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                if (i == nodes.GetLength(0) / 2)
                    continue;
                markAllEdgesFromNode(i, 0);
                markAllEdgesFromNode(i, nodes.GetLength(1) - 1);
            }
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                if (i == nodes.GetLength(0) / 2 || i == nodes.GetLength(0) / 2 - 1)
                    continue;
                markEdgeSafe(i, 1, 1, 0);
                getNodeSafe(i, 1).set = true;
                markEdgeSafe(i, nodes.GetLength(1) - 2, 1, 0);
                getNodeSafe(i, nodes.GetLength(1) - 2).set = true;
            }
            getNodeSafe(nodes.GetLength(0) / 2 - 1, 1).set = true;
            getNodeSafe(nodes.GetLength(0) / 2 - 1, nodes.GetLength(1) - 2).set = true;

            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                markEdgeSafe(0, j, 0, 1);
                getNodeSafe(0, j).set = true;
                markEdgeSafe(nodes.GetLength(0) - 1, j, 0, 1);
                getNodeSafe(nodes.GetLength(0) - 1, j).set = true;
            }
        }

        public void createEdgesForNode(int x, int y)
        {
            foreach (Node node in getNodesAround(x, y))
            {
                getNodeSafe(x, y).nodes.Add(new Edge(node));
            }
        }

        public IEnumerable<Node> getNodesAround(int x, int y)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    if (getNodeSafe(x + i, y + j) != null)
                        yield return getNodeSafe(x + i, y + j);
                }
            }
        }

        private void markEdgeSafe(int x, int y, int addX, int addY, bool reset = false)
        {
            if (getNodeSafe(x, y) != null && getNodeSafe(x + addX, y + addY) != null)
            {
                Edge edge = getNodeSafe(x, y).nodes.Where(u => u.node == getNodeSafe(x + addX, y + addY)).FirstOrDefault();
            edge.drawn = !reset;
            edge = getNodeSafe(x + addX, y + addY).nodes.Where(u => u.node == getNodeSafe(x, y)).FirstOrDefault();
            edge.drawn = !reset;
            }
        }

        private Node getNodeSafe(Point point)
        {
            return getNodeSafe(point.X, point.Y);
        }

        private Node getNodeSafe(int x, int y)
        {
            if (x < 0 || y < 0 || x >= nodes.GetLength(0) || y >= nodes.GetLength(1))
                return null;
            else
            return nodes[x, y];
        }


        public Node getNode(Point point)
        {
            return getNode(point.X, point.Y);
        }

        public Node getNode(int x, int y)
        {
             return nodes[x, y];
        }


        private void markAllEdgesFromNode(int x, int y)
        {
            foreach (Edge edge in getNode(x, y).nodes)
            {
                edge.drawn = true;
            }

            foreach (Node node in getNodesAround(x, y))
            {
                Edge secondEdge = node.nodes.Where(u => u.node == getNode(x, y)).FirstOrDefault();
                secondEdge.drawn = true;
            }
        }
    }
}
