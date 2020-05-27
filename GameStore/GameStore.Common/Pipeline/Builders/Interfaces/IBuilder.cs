namespace GameStore.Common.Pipeline.Builders.Interfaces
{
    public interface IBuilder<out TPipeline, in TNode>
    {
        IBuilder<TPipeline, TNode> WithNode(TNode node);
        TPipeline Build();
    }
}