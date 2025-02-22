博客地址：[游戏算法-A*搜索算法知识梳理和通用框架 | 白雪团子](https://www.shirakoko.xyz/article/astar)

- /MyHeap：自定义优先队列数据结构
- /AstarFramework：通用的Astar搜索算法框架
  - AstarNode.cs：节点泛型接口，继承该接口的节点可作为Astar算法中的节点
  - AstarSearcher.cs：Astar算法搜索器，使用时需传入节点类类型和地图类类型泛型
- /AstarUsage：一个使用示例
