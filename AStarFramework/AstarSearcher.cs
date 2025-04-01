using System;
using System.Collections.Generic;
using JufGame.Collections.Generic;

/// <summary>
/// A星搜索器，T_Node额外实现IComparable用于优先队列的比较，实现IEquatable用于HashSet和Dictionary等同一性的判断
/// </summary>
/// <typeparam name="T_Map">搜索的图类</typeparam>
/// <typeparam name="T_Node">搜索的节点类</typeparam>
public class AStarSearcher<T_Map, T_Node> where T_Node: IAStarNode<T_Node>, IComparable<T_Node>, IEquatable<T_Node>
{   
    // 探索集，是一个哈希集合
    private readonly HashSet<T_Node> closeList;
    // 边缘集，是一个堆
    private readonly MyHeap<T_Node> openList;
    // 搜索空间（地图）
    private readonly T_Map nodeMap;
    public AStarSearcher(T_Map map, int maxNodeSize = 200)
    {
        nodeMap = map;
        closeList = new HashSet<T_Node>();
        // maxNodeSize用于限制路径节点的上限，避免陷入无止境搜索的情况
        openList = new MyHeap<T_Node>(maxNodeSize);
    }

    /// <summary>
    /// 搜索（寻路）
    /// </summary>
    /// <param name="start">起点</param>
    /// <param name="target">终点</param>
    /// <param name="pathRes">生成的路径</param>
    public void FindPath(T_Node start, T_Node target, Stack<T_Node> pathRes)
    {
        T_Node currentNode;
        pathRes.Clear();
        closeList.Clear();
        openList.Clear();
        openList.PushHeap(start);
        while (!openList.IsEmpty)
        {
            // 取出边缘集中最小代价的节点（堆顶元素）
            currentNode = openList.Top;
            openList.PopHeap();
            // 拟定移动到该节点，将其放入探索集
            closeList.Add(currentNode);
            // 如果找到了或图都搜完了也没找到时
            if (currentNode.Equals(target) || openList.IsFull)
            {
                // 生成路径并保存到pathRes中
                GenerateFinalPath(start, currentNode, pathRes);
                return;
            }
            // 更新探索集和边缘集
            UpdateTwoLists(currentNode, target);
        }
    }

    /// <summary>
    /// 从指定起点到所有节点的最优路径
    /// </summary>
    /// <param name="start">起点</param>
    /// <param name="allPaths">用于存储所有节点的最优路径的字典</param>
    public void FindAllPaths(T_Node start, Dictionary<T_Node, Stack<T_Node>> allPaths)
    {
        // 清空传入的字典
        allPaths.Clear();
    
        // 初始化开放列表和关闭列表
        closeList.Clear();
        openList.Clear();
    
        // 将起点加入开放列表
        openList.PushHeap(start);
    
        // 开始搜索
        while (!openList.IsEmpty)
        {
            // 取出开放列表中最小代价的节点
            var currentNode = openList.Top;
            openList.PopHeap();
    
            // 将当前节点加入关闭列表
            closeList.Add(currentNode);
    
            // 如果当前节点尚未在路径字典中，生成路径并保存
            if (!allPaths.ContainsKey(currentNode))
            {
                var path = new Stack<T_Node>();
                GenerateFinalPath(start, currentNode, path);
                allPaths[currentNode] = path;
            }
    
            // 更新探索集和边缘集，传入 default(T_Node) 表示不计算启发式（Dijkstra）
            UpdateTwoLists(currentNode, default);
        }
    }

    private void GenerateFinalPath(T_Node startNode, T_Node endNode, Stack<T_Node> pathStack)
    {
        pathStack.Push(endNode); // 因为回溯，所以用栈储存生成的路径
        var tpNode = endNode.Parent;
        while (!tpNode.Equals(startNode))
        {
            pathStack.Push(tpNode);
            tpNode = tpNode.Parent;
        }
        pathStack.Push(startNode);
    }

    private void UpdateTwoLists(T_Node curNode, T_Node endNode)
    {
        T_Node sucNode; // 用于存储当前节点的后继节点
        float tpCost; // 用于存储从起点到后继节点的临时总代价
        bool isNotInOpenList; // 用于标记后继节点是否不在开放列表中

        // 找出当前节点的所有后继节点
        var successors = curNode.GetSuccessors(nodeMap);
        if(successors == null)
        {
            return;
        }

        // 遍历所有后继节点
        for (int i = 0; i < successors.Count; ++i)
        {
            sucNode = successors[i]; // 获取当前后继节点

            // 如果后继节点已经在边缘集中（已被探索过），则跳过
            if (closeList.Contains(sucNode))
                continue;
            
            // 计算从起点到当前后继节点的总代价
            tpCost = curNode.GCost + sucNode.SelfCost;

            // 检查后继节点是否不在开放列表中
            isNotInOpenList = !openList.Contains(sucNode);

            // 如果后继节点不在开放列表中，或者新的总代价比之前的更小
            if (isNotInOpenList || tpCost < sucNode.GCost)
            {
                sucNode.GCost = tpCost; // 更新后继节点的总代价
                // 计算后继节点的启发式估计值（到终点的估计代价），仅在 endNode 非 null 时计算启发式（A*），否则 HCost = 0（Dijkstra）
                sucNode.HCost = !endNode.Equals(default) ? sucNode.GetDistance(endNode) : 0;
                sucNode.Parent = curNode; // 设置后继节点的父节点为当前节点，方便回溯

                // 如果后继节点不在探索集中，将其加入开放列表
                if (isNotInOpenList)
                {
                    openList.PushHeap(sucNode);
                }
            }
        }
    }
}
