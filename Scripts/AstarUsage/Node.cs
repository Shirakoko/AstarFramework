using System;
using System.Collections.Generic;


/// <summary>
/// 节点，继承自IAStarNode<Node>接口
/// </summary>
public class Node : IAStarNode<Node>, IComparable<Node>, IEquatable<Node>
{
    public string Id { get; } // 节点唯一标识
    public float SelfCost { get; set; } // 移动到该节点的单步代价
    public float GCost { get; set; } // 从起点到该节点的累计代价
    public float HCost { get; set; } // 到终点的启发式估计代价
    public float FCost => GCost + HCost; // 总代价
    public Node Parent { get; set; } // 父节点，用于回溯路径
    public Dictionary<Node, float> Neighbors { get; } // 邻居节点及其连接代价

    public Node(string id)
    {
        Id = id;
        Neighbors = new Dictionary<Node, float>();
    }

    // 添加邻居节点
    public void AddNeighbor(Node neighbor, float cost)
    {
        Neighbors[neighbor] = cost;
    }

    // 启发式函数：基于节点之间的连接代价（边的权重）估计到目标节点的代价
    public float GetDistance(Node other)
    {
        // 如果当前节点是目标节点，返回 0
        if (this.Equals(other))
            return 0;

        // 如果当前节点有直接连接到目标节点的边，返回该边的代价
        if (Neighbors.ContainsKey(other))
            return Neighbors[other];

        // 否则，返回一个保守估计值（如最小边的代价）
        float minCost = float.MaxValue;
        foreach (var cost in Neighbors.Values)
        {
            if (cost < minCost)
                minCost = cost;
        }
        return minCost == float.MaxValue ? 0 : minCost; // 如果没有邻居，返回 0
    }

    // 获取邻居节点
    public List<Node> GetSuccessors(object nodeMap)
    {
        var successors = new List<Node>();
        foreach (var neighbor in Neighbors.Keys)
        {
            neighbor.SelfCost = Neighbors[neighbor]; // 设置邻居节点的单步代价
            successors.Add(neighbor);
        }
        return successors;
    }

    // 实现 IComparable，用于优先队列排序
    public int CompareTo(Node other)
    {
        // 首先比较总代价 f(n)
        int result = FCost.CompareTo(other.FCost);
        // 如果总代价相同，则比较启发式代价 h(n)
        if (result == 0)
            result = HCost.CompareTo(other.HCost);
        return result;
    }

    // 实现 IEquatable，用于判断节点是否相等
    public bool Equals(Node other)
    {
        return Id == other.Id;
    }

    // 重写 GetHashCode，用于 HashSet 和 Dictionary
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}