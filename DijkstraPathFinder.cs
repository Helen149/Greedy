using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Greedy.Architecture;
using System.Drawing;

namespace Greedy
{
	class DijkstraData
	{
		public Point Previous { get; set; }
		public int Price { get; set; }
	}

	public class DijkstraPathFinder
    {
        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start,
            IEnumerable<Point> targets)
        {
			var notVisitedTarget = targets.ToList();
			var notVisitedPoint = new List<Point>();
			var track = new Dictionary<Point, DijkstraData>();
			track[start] = new DijkstraData { Price = 0, Previous = new Point(-1, -1) };
			notVisitedPoint.Add(start);
			while(notVisitedTarget.Count != 0)
			{
				var result = Dijkstra(state, notVisitedPoint, track, notVisitedTarget);
				if (result == null) yield break;
				yield return result;
				notVisitedTarget.Remove(result.End);
			}
		}

		private PathWithCost Dijkstra(State state, List<Point> notVisitedPoint, Dictionary<Point, DijkstraData> track, IEnumerable<Point> targets)
		{
			Point end;
			while (true)
			{
				var toOpen = FindPointToOpen(notVisitedPoint, track);
				if (toOpen == new Point(-1, -1)) return null;
				if (targets.Contains(toOpen))
				{
					end = toOpen;
					break; 
				}
				OpenPoint(toOpen, state, notVisitedPoint, track);
			}
			return CreatePath(end, track);
		}

		private Point FindPointToOpen(List<Point> notVisited, Dictionary<Point, DijkstraData> track)
		{
			Point toOpen = new Point(-1,-1);
			var bestPrice = double.PositiveInfinity;
			foreach (var e in notVisited)
			{
				if (track.ContainsKey(e) && track[e].Price < bestPrice)
				{
					bestPrice = track[e].Price;
					toOpen = e;
				}
			}
			return toOpen;
		}

		private void OpenPoint(Point toOpen, State state, List<Point> notVisitedPoint, Dictionary<Point, DijkstraData> track)
		{
			for (var dy = -1; dy <= 1; dy++)
				for (var dx = -1; dx <= 1; dx++)
					if (dx != 0 && dy != 0) continue;
					else
					{
						var nextPoint = new Point(toOpen.X + dx, toOpen.Y + dy);
						if (track.ContainsKey(nextPoint) || !state.InsideMap(nextPoint) || state.IsWallAt(nextPoint)) continue;
						notVisitedPoint.Add(nextPoint);
						var currentPrice = track[toOpen].Price + state.CellCost[nextPoint.X, nextPoint.Y];
						if (!track.ContainsKey(nextPoint) || track[nextPoint].Price > currentPrice)
							track[nextPoint] = new DijkstraData { Previous = toOpen, Price = currentPrice };
					}
			notVisitedPoint.Remove(toOpen);
		}

		private PathWithCost CreatePath(Point end, Dictionary<Point, DijkstraData> track)
		{
			var result = new PathWithCost(track[end].Price, end);
			end = track[end].Previous;
			while (end != new Point(-1, -1))
			{
				result.Path.Add(end);
				end = track[end].Previous;
			}
			result.Path.Reverse();
			return result;
		}
	}
}
