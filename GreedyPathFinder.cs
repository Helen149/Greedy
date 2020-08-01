using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
	public class GreedyPathFinder : IPathFinder
	{
		public List<Point> FindPathToCompleteGoal(State state)
		{
			var notVisitedTarget = state.Chests.ToList();
			var searcher = new DijkstraPathFinder();
			var start = state.Position;
			var energy = state.Energy;
			var result = new List<Point>();
			while (state.Chests.Count- notVisitedTarget.Count != state.Goal)
			{
				var path = searcher.GetPathsByDijkstra(state, start, notVisitedTarget).FirstOrDefault();
				if (path == null) return new List<Point>();
				energy -= path.Cost;
				if (energy<0) return new List<Point>();
				start = path.End;
				notVisitedTarget.Remove(path.End);
				result.AddRange(path.Path.Skip(1));
			}
			return result;
		}
	}
}