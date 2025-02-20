using System.Collections.Generic;

public class Graph
{
    private Dictionary<string, Node> nodes = new Dictionary<string, Node>();
    public Dictionary<string, Node> Nodes => nodes;

    // 添加节点
    public void AddNode(string id)
    {
        if (!nodes.ContainsKey(id))
        {
            nodes[id] = new Node(id);
        }
    }

    // 添加有向边
    public void AddEdge(string fromId, string toId, float cost)
    {
        if (nodes.ContainsKey(fromId) && nodes.ContainsKey(toId))
        {
            nodes[fromId].AddNeighbor(nodes[toId], cost);
        }
    }

    // 获取节点
    public Node GetNode(string id)
    {
        return nodes.ContainsKey(id) ? nodes[id] : null;
    }
}
