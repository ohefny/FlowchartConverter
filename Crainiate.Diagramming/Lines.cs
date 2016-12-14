// (c) Copyright Crainiate Software 2010

using System;
using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
    public class Lines: Elements<Line>
    {
        public Lines(Model model): base(model)
        {

        }

        //For deserialization only
        internal Lines(): base()
        {

        }

        protected internal override void ElementInsert(Line line)
        {
            base.ElementInsert(line);

            //If a connector and is not auto routed and is not being deserialized then calculate points
            if (line is Connector)
            {
                Connector connector = line as Connector;
                if (connector.Points == null) connector.CalculateRoute();
            }

            line.DrawPath();
        }

    }
}
