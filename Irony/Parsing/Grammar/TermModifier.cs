using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;

namespace Irony.Parsing
{
    public class TermModifier : BnfTerm
    {
        private readonly Action<NonTerminal> _modify;

        public TermModifier() : base("_TermModifier_")
        {
        }

        public TermModifier(Action<NonTerminal> modify) : base("_TermModifier_")
        {
            _modify = modify;
        }

        public TermModifier(AstNodeCreator nodeCreator) : base("_TermModifier_")
        {
            _modify = x =>
            {
                x.AstConfig.NodeCreator = nodeCreator;
                x.Flags &= ~TermFlags.NoAstNode;
            };
        }

        public virtual void Modify(NonTerminal nt)
        {
            _modify(nt);
        }
    }

    public class TransientNode : TermModifier
    {
        public override void Modify(NonTerminal node)
        {
            Grammar.MarkTransient(node);
        }
    }

    public class AstNodeCreatingTerm : TermModifier
    {
        private readonly AstNodeCreator _nodeCreator;

        public AstNodeCreatingTerm(AstNodeCreator nodeCreator)
        {
            _nodeCreator = nodeCreator;
        }

        public override void Modify(NonTerminal node)
        {
            node.Flags = TermFlags.None;
            node.AstConfig.NodeCreator = _nodeCreator;
        }
    }
}
