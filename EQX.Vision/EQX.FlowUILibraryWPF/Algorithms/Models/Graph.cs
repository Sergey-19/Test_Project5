using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.FlowUILibraryWPF.Algorithms.Models
{
    public class Graph<T>
    {
        #region Properties
        public Dictionary<T, List<T>> Edges { get; set; }
        #endregion

        #region Constructors
        public Graph()
        {
            Edges = new Dictionary<T, List<T>>();
        }
        #endregion
        #region Methods
        public void AddEdge(T Source, T Target)
        {
            if (!Edges.ContainsKey(Source))
            {
                Edges[Source] = new List<T>();
            }

            if (!Edges[Source].Contains(Target))
            {
                Edges[Source].Add(Target);
            }
        }
        public void RemoveEdge(T Source, T Target)
        {
            if (Edges.ContainsKey(Source)) Edges[Source].Remove(Target);
        }
        public void RemoveNode(T node)
        {
            Edges.Remove(node);
            foreach (var item in Edges.Values) if (item.Contains(node)) item.Remove(node);
        }
        public bool HasCycle()
        {
            var visited = new HashSet<T>();
            var recStack = new HashSet<T>();

            foreach (var node in Edges.Keys)
            {
                if (HasCycleUtil(node, visited, recStack))
                    return true;
            }
            return false;
        }
        private bool HasCycleUtil(T node, HashSet<T> visited, HashSet<T> recStack)
        {
            if (recStack.Contains(node))
                return true;

            if (visited.Contains(node))
                return false;

            visited.Add(node);
            recStack.Add(node);

            if (Edges.ContainsKey(node))
            {
                foreach (var neighbor in Edges[node])
                {
                    if (HasCycleUtil(neighbor, visited, recStack))
                        return true;
                }
            }
            recStack.Remove(node);
            return false;
        }

        #endregion
    }
}
