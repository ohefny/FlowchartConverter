// (c) Copyright Crainiate Software 2010




using System;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Flowcharting
{
    public class Decision
    {
        public Shape DecisionShape;
        public Link DecisionLine;
        public Shape TrueShape;
        public Link TrueLine;
        public Shape FalseShape;
        public Link FalseLine;
        public Port Port;
    }
}
