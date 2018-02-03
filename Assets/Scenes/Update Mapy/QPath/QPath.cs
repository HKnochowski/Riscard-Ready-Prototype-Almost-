using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QPath
{

    public static class QPath
    {

//        public static IQPathTile[] FindPath(IQPathWorld world, 
//            IQPathUnit unit, 
//            IQPathTile startTile,
//            IQPathTile endTile,
//            CostEstimateDelegate costEstimateFunc)
//        {
//
//            if (world == null || unit == null || startTile == null || endTile == null)
//            {
//                return null;
//            }
//
//            IQPath_AStar resolver = new IQPath_AStar(world, unit, startTile, endTile, costEstimateFunc);
//            return resolver.GetList();

//       }

    }

    public delegate float CostEstimateDelegate(IQPathTile A, IQPathTile B);
    

}
