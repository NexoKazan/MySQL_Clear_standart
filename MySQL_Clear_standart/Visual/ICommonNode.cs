using System.Collections.Generic;
namespace MySQL_Clear_standart
{
    public interface ICommonNode
    {
        string Type { get; }
        int Index { get; }
        string BranchText { get; }
        string Text { get; }        
        int Count { get; }           
        IEnumerable<ICommonNode> Children { get; }
    }
}