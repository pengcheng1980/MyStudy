using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
    class DataNode
    {
        private int m_value = 0;
        private List<DataNode> m_subNodes = new List<DataNode>();
        private string m_guid = string.Empty;

        public DataNode(int value)
        {
            m_value = value;
        }

        public int Value
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
            }
        }
        public List<DataNode> SubNodes
        {
            get
            {
                return m_subNodes;
            }
        }
        public string GUID
        {
            get
            {
                return m_guid;
            }
        }

        public List<int> ToIntList()
        {
            List<int> intList = new List<int>();
            AddToIntLIst(this, intList);

            return intList;
        }
        private void AddToIntLIst(DataNode dataNode, List<int> intList)
        {
            if (dataNode.SubNodes.Count == 0)
            {
                intList.Add(dataNode.Value);
                return;
            }
            for (int i = 0; i < dataNode.SubNodes.Count; i++)
            {
                AddToIntLIst(dataNode.SubNodes[i], intList);
            }
        }
        public List<DataNode> SplitNodes(int target)
        {
            Guid guid = Guid.NewGuid();
            List<DataNode> splitedNodes = new List<DataNode>();
            if (Value > target)
            {
                return splitedNodes;
            }
            int count = target/Value;
            for(int i=0; i<count; i++)
            {
                DataNode dataNode = new DataNode(Value * (i+1));
                for (int j = 0; j < i + 1; j++)
                {
                    dataNode.SubNodes.Add(new DataNode(Value));
                }
                dataNode.m_guid = guid.ToString();
                splitedNodes.Add(dataNode);
            }
            return splitedNodes;
        }
    }
    public class Solution
    {
        public List<List<int>> CombinationSum(int[] candidates, int target)
        {
            List<List<int>> list = new List<List<int>>();
            List<DataNode> dataNodes = GetCombinationSumNodes(candidates, target);
            for (int i = 0; i < dataNodes.Count; i++)
            {
                list.Add(dataNodes[i].ToIntList());
            }
            return list;
        }
        private List<DataNode> GetCombinationSumNodes(int[] candidates, int target)
        {
            List<DataNode> dataNodes = InitCandidateNodes(candidates, target);
            List<DataNode> combinationSumNodeNodes = new List<DataNode>();
            CombiNodes(dataNodes, combinationSumNodeNodes, target);
            return combinationSumNodeNodes;
        }
        private void CombiNodes(List<DataNode> dataNodes, List<DataNode> combinationSumNodeNodes, int target)
        {
            List<DataNode> combiNodes = new List<DataNode>();
            for (int i = 0; i < dataNodes.Count; i++)
            {
                if (dataNodes[i].Value == target)
                {
                    combinationSumNodeNodes.Add(dataNodes[i]);
                }
                for (int j = i+1; j < dataNodes.Count; j++)
                {
                    if (dataNodes[i].GUID != string.Empty && dataNodes[j].GUID != string.Empty
                        && dataNodes[i].GUID == dataNodes[j].GUID)
                    {
                        continue;
                    }
                    if (dataNodes[i].Value + dataNodes[j].Value > target)
                    {
                        continue;
                    }
                    else if (dataNodes[i].Value + dataNodes[j].Value == target)
                    {
                        DataNode dataNode = new DataNode(target);
                        dataNode.SubNodes.Add(dataNodes[i]);
                        dataNode.SubNodes.Add(dataNodes[j]);
                        combinationSumNodeNodes.Add(dataNode);
                        continue;
                    }
                    else
                    {
                        DataNode dataNode = new DataNode(dataNodes[i].Value + dataNodes[j].Value);
                        dataNode.SubNodes.Add(dataNodes[i]);
                        dataNode.SubNodes.Add(dataNodes[j]);
                        combiNodes.Add(dataNode);
                        continue;
                    }
                }
            }
            if (combiNodes.Count > 0)
            {
                CombiNodes(combiNodes, combinationSumNodeNodes, target);
            }

            return;
        }

        private List<DataNode> InitCandidateNodes(int[] candidates, int target)
        {
            List<DataNode> dataNodes = new List<DataNode>();
            for (int i = 0; i < candidates.Length; i++)
            {
                if (candidates[i] > target)
                {
                    continue;
                }
                DataNode dataNode = new DataNode(candidates[i]);
                dataNodes.AddRange(dataNode.SplitNodes(target));
            }
            return dataNodes;
        }
    }
}
