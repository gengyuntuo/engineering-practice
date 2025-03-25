namespace NaturalLanguageProcessing
{
    internal static class ApplicationMainHelpers
    {

        /// <summary>
        /// 生成TreeNode树形结构
        /// </summary>
        /// <param name="nodes">NodeList</param>
        /// <returns></returns>
        public static TreeNode? GenerateTree(List<DepParserAnalysisItemModel> nodes)
        {
            // 用于存储每个节点的引用
            Dictionary<int, TreeNode> nodeMap = [];
            TreeNode? root = null;

            // 首先创建所有节点并存储到字典中
            foreach (DepParserAnalysisItemModel node in nodes)
            {
                // 节点文本
                string text = node.Word + "(" + BaiduApiInvoker.DEPRELTable[node.Deprel ?? ""] + ")";
                nodeMap[node.Id] = new TreeNode(text);
            }

            // 构建树结构
            foreach (DepParserAnalysisItemModel node in nodes)
            {
                if (node.Head == 0)
                {
                    // 根节点
                    root = nodeMap[node.Id];
                }
                else
                {
                    // 找到父节点并将当前节点添加为其子节点
                    TreeNode parent = nodeMap[node.Head];
                    parent.Nodes.Add(nodeMap[node.Id]);
                }
            }
            return root;
        }
    }
}