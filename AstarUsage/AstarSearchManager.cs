using System.Collections.Generic;
using UnityEngine;

public class AstarSearchManager : MonoBehaviour
{
    private Graph graph;

    void Start()
    {
        // 创建有向图
        graph = new Graph();

        // 添加节点
        graph.AddNode("A");
        graph.AddNode("B");
        graph.AddNode("C");
        graph.AddNode("D");
        graph.AddNode("E");

        // 添加有向边
        graph.AddEdge("A", "B", 1.0f);
        graph.AddEdge("A", "C", 2.0f);
        graph.AddEdge("B", "D", 3.0f);
        graph.AddEdge("C", "D", 1.0f);
        graph.AddEdge("D", "E", 2.0f);

        // 获取起点和终点
        var startNode = graph.GetNode("A");
        var targetNode = graph.GetNode("E");

        // 初始化每个节点的 GCost，如果不是起点，就设置为无穷大
        foreach (var node in graph.Nodes.Values)
        {
            node.GCost = float.MaxValue; // 默认设置为无穷大
        }
        startNode.GCost = 0; // 起点的 GCost 设置为 0

        // 创建 A* 搜索器
        var searcher = new AStarSearcher<Graph, Node>(graph);

        // 用于存储路径
        var path = new Stack<Node>();

        // 执行搜索
        searcher.FindPath(startNode, targetNode, path);

        // 输出路径
        Debug.Log("Path from A to E:");
        while (path.Count > 0)
        {
            var node = path.Pop();
            Debug.Log(node.Id);
        }
    }
}