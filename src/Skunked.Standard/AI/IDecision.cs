using System;

namespace Skunked.AI
{
    public abstract class NodeBase<TClient, TResult>
    {
        public abstract TResult Evaluate(TClient client);
    }


    public class EndNode<TClient, TResult> : NodeBase<TClient, TResult>
    {
        private readonly Func<TClient, TResult> _eval;

        public EndNode(Func<TClient, TResult> eval)
        {
            _eval = eval;
        }

        public override TResult Evaluate(TClient client)
        {
            return _eval(client);
        }
    }

    public class DecisionNode<TClient, TResult> : NodeBase<TClient, TResult>
    {
        private readonly Func<TClient, bool> _eval;
        private readonly NodeBase<TClient, TResult> _positive;
        private readonly NodeBase<TClient, TResult> _negative;

        public DecisionNode(Func<TClient, bool> eval, NodeBase<TClient, TResult> positive, NodeBase<TClient, TResult> negative)
        {
            _eval = eval;
            _positive = positive;
            _negative = negative;
        }

        public override TResult Evaluate(TClient client)
        {
            return _eval(client)
                ? _positive.Evaluate(client)
                : _negative.Evaluate(client);
        }
    }
}