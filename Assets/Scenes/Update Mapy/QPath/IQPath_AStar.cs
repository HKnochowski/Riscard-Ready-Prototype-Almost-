using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QPath
{

    public class IQPath_AStar
    {
        public IQPath_AStar(IQPathWorld world, IQPathUnit unit, IQPathTile startTile, IQPathTile endTile)
        {
            this.world = world;
            this.unit = unit;
            this.startTile = startTile;
            this.endTile = endTile;
            this.costEstimateFunc = costEstimateFunc;
        }

        IQPathWorld world;
        IQPathUnit unit;
        IQPathTile startTile;
        IQPathTile endTile;
        CostEstimateDelegate costEstimateFunc;

        Queue<IQPathTile> path;

        public void DoWork()
        {
//            path = new Queue<IQPathTile>();
//
//            HashSet<IQPathTile> closedSet = new HashSet<IQPathTile>();
//            
//            PathfindingPriorityQueue<IQPathTile> openSet = new HashSet<IQPathTile>();
//            openSet.Enqueue(startTile, 0);
//
//            Dictionary<IQPathTile, IQPathTile> came_From = new Dictionary<IQPathTile, IQPathTile>();
//
//            Dictionary<IQPathTile, float> g_score = new Dictionary<IQPathTile, float>();
//            g_score[startTile] = 0;
//
//            Dictionary<IQPathTile, float> f_score = new Dictionary<IQPathTile, float>();
//            f_store[startTile] = costEstimate(startTile, endTile);


        }

        float CostEstimate(IQPathTile src, IQPathTile dst)
        { 
            return 1;
        }

        public IQPathTile[] GetList()
        {
            return path.ToArray();
        }
    }
}
