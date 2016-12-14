// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Collections;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming.Layouts
{
    public sealed class Route
    {
        //Property variables
        private int _grain;
        private Model _model;
        private bool _avoid;
        private Size _padding;
        private Layer _layer;
        private Shape _start;
        private Shape _end;

        //Working variables        
        private MovementCostQueue _movementList; //An open list of equal totalcost items prioritised by highest movement cost
        private TotalCostQueue _openList; //An open list of lower priority items

        private BinaryGrid<RouteNode> _grid; //A grid containing all items visited
        private BinaryGrid<TerrainPoint> _terrain; //A grid containing obstacles
        private ListFactory<RouteNode> _nodeFactory; //A recyclable list of route nodes

        private Rectangle _boundary;
        private RouteNode _solution;
        private RouteNode _goal;

        #region Interface

        //Constructors
        public Route()
        {
            _movementList = new MovementCostQueue();
            _openList = new TotalCostQueue();
            _grid = new BinaryGrid<RouteNode>();
            _nodeFactory = new ListFactory<RouteNode>();
            _terrain = new BinaryGrid<TerrainPoint>();

            Reset();
            _grain = 10;
        }

        public Route(Model model)
        {
            _movementList = new MovementCostQueue();
            _openList = new TotalCostQueue();
            _grid = new BinaryGrid<RouteNode>();
            _nodeFactory = new ListFactory<RouteNode>();
            _terrain = new BinaryGrid<TerrainPoint>();

            Reset();
            _grain = 10;

            Model = model;
        }

        //Properties
        public Model Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
            }
        }

        public Layer Layer
        {
            get
            {
                return _layer;
            }
            set
            {
                if (value != _layer)
                {
                    Reform();
                    _layer = value;
                }
            }
        }

        public bool Avoid
        {
            get
            {
                return _avoid;
            }
            set
            {
                _avoid = value;
            }
        }

        public Rectangle Boundary
        {
            get
            {
                return _boundary;
            }
            set
            {
                _boundary = value;
            }
        }

        public Size Padding
        {
            get
            {
                return _padding;
            }
            set
            {
                if (!_padding.Equals(value))
                {
                    Reform();
                    _padding = value;
                }
            }
        }

        public Shape Start
        {
            get
            {
                return _start;
            }
            set
            {
                if (_start != value)
                {
                    Reform();
                    _start = value;
                }
            }
        }

        public Shape End
        {
            get
            {
                return _end;
            }
            set
            {
                if (_end != value)
                {
                    Reform();
                    _end = value;
                }
            }
        }

        //Methods
        public void Reset()
        {
            _movementList.Clear();
            _openList.Clear();
            _grid = new BinaryGrid<RouteNode>();
            _nodeFactory.Clear();
            _solution = null;
            _goal = null;
        }

        //Determines if the impassable structures must be recreated
        public void Reform()
        {
            _terrain.Clear();
        }

        public List<PointF> GetRoute(PointF begin, PointF end)
        {
            //Move the begin and end onto the grid
            Point beginAligned = Point.Round(begin);
            Point endAligned = Point.Round(end);

            beginAligned = new Point((beginAligned.X / _grain) * _grain, (beginAligned.Y / _grain) * _grain);
            endAligned = new Point((endAligned.X / _grain) * _grain, (endAligned.Y / _grain) * _grain);

            //Recreate the rectangles surrounding the shapes in the model
            if (_terrain.Count == 0) CreateTerrain(beginAligned, endAligned);

            //Check for exclusions
            if (Start != null && End != null)
            {
                Rectangle startRect = RectangleToGrid(GetInflatedRectangle(Start));
                Rectangle endRect = RectangleToGrid(GetInflatedRectangle(End));

                startRect.Inflate(_grain, _grain);
                endRect.Inflate(_grain, _grain);

                //If rectangles intersect or are adjacent then just make connector
                if (startRect.IntersectsWith(endRect) || Geometry.AreAdjacent(startRect, endRect))
                {
                    return MakeConnector(begin, end);
                }
            }

            //Check to see if a route must be calculated, or just a connector shape created
            if (_terrain != null && !_terrain.IsEmpty)
            {
                RouteNode start = _nodeFactory.Create();
                RouteNode goal = _nodeFactory.Create();

                start.SetPoint(beginAligned);
                goal.SetPoint(endAligned);

                if (CalculateNodes(start, goal))
                {
                    List<PointF> solution = GetSolution();
                    if (solution != null && solution.Count > 1)
                    {
                        AlignSolution(solution, begin, end);
                        return solution;
                    }
                }
            }

            return MakeConnector(begin, end);
        }

        #endregion

        #region Implementation

        //Creates the solution from the calculated nodes as vectors
        public List<PointF> GetSolution()
        {
            ///Add an additional node to the end if the solution didnt match the goal
            if (_solution.Equals(_goal) || _solution.Parent == null)
            {
                _goal = _solution;
            }
            else
            {
                //If in line then add the goal as a node to the solution, else move the solution node
                if (_solution.X == _goal.X || _solution.Y == _goal.Y)
                {
                    _goal.Parent = _solution;
                }
                else
                {
                    RouteNode extra = new RouteNode();
                    extra.Parent = _solution;

                    //Set node coordinates
                    extra.X = (_solution.X == _solution.Parent.X) ? _solution.X : _goal.X;
                    extra.Y = (_solution.Y == _solution.Parent.Y) ? _solution.Y : _goal.Y;

                    //Link the goal to the new node and add it
                    _goal.Parent = extra;
                }
            }

            List<PointF> list = new List<PointF>();

            RouteNode previous = _goal;
            RouteNode node = _goal.Parent;

            list.Add(new PointF(previous.X, previous.Y));

            //Only one or two items
            if (node == null || node.Parent == null) return list;

            while (node.Parent != null)
            {
                if (!((previous.X == node.Parent.X) || (previous.Y == node.Parent.Y)))
                {
                    list.Insert(0, new PointF(node.X, node.Y));
                    previous = node;
                }
                node = node.Parent;
            }

            //Add the start node
            PointF start = new PointF(node.X, node.Y);
            if (!start.Equals((PointF)list[0])) list.Insert(0, start);

            return list;
        }

        private bool CalculateNodes(RouteNode start, RouteNode goal)
        {
            _goal = goal;

            //Set up the movement costs and heuristic 
            start.MovementCost = 0;
            start.Heuristic = GetHeuristic(start);
            start.TotalCost = start.Heuristic; //Since movement cost is 0
            start.Direction = NodeDirection.Down; //Set manually to down

            //Add the start node to the movement list
            //_openList.Push(start);
            _movementList.Push(start);

            //Keep looping until goal is found or there are no more open nodes
            while (_movementList.Count > 0 || _openList.Count > 0)
            {
                //Remove from open list
                RouteNode parent = PopNode();	// pops node off open list based on total cost			

                //Check to see if we have found the goal node
                //if (parent.Near(goal, mGrain)) 
                if (parent.Equals(goal))
                {
                    _solution = parent;
                    return true;
                }

                //Close the node
                parent.Closed = true;

                //Add or check four adjacent squares to the open list
                //The nodes that are added last will pop off first off the binary heap
                if (parent.Direction == NodeDirection.Down)
                {
                    AddAdjacentNode(0, -_grain, _grain, parent); //up
                    AddAdjacentNode(-_grain, 0, _grain, parent); //left
                    AddAdjacentNode(_grain, 0, _grain, parent);  //right
                    AddAdjacentNode(0, _grain, _grain, parent);  //down
                }
                else if (parent.Direction == NodeDirection.Right)
                {
                    AddAdjacentNode(-_grain, 0, _grain, parent); //left
                    AddAdjacentNode(0, -_grain, _grain, parent); //up
                    AddAdjacentNode(0, _grain, _grain, parent);  //down
                    AddAdjacentNode(_grain, 0, _grain, parent);  //right
                }
                else if (parent.Direction == NodeDirection.Left)
                {
                    AddAdjacentNode(_grain, 0, _grain, parent);  //right
                    AddAdjacentNode(0, -_grain, _grain, parent); //up
                    AddAdjacentNode(0, _grain, _grain, parent);  //down
                    AddAdjacentNode(-_grain, 0, _grain, parent); //left
                }
                else if (parent.Direction == NodeDirection.Up)
                {
                    AddAdjacentNode(0, _grain, _grain, parent);  //down
                    AddAdjacentNode(_grain, 0, _grain, parent);  //right
                    AddAdjacentNode(-_grain, 0, _grain, parent); //left
                    AddAdjacentNode(0, -_grain, _grain, parent); //up
                }
            }

            return false;
        }

        //Returns the next node in the open list
        private RouteNode PopNode()
        {
            //Move more nodes onto the movement list if there are none
            if (_movementList.Count == 0)
            {
                RouteNode node = _openList.Pop();
                _movementList.Push(node);

                //Keep moving nodes with the same totalcost onto the movement list
                //So that they can be sorted by movement cost
                while (_openList.Count > 0 && node.TotalCost == _openList.TotalCost)
                {
                    _movementList.Push(_openList.Pop());
                }
            }
            return _movementList.Pop();
        }

        //Add a node to a parent node, ignoring if impassable or already closed,
        //adding if new, updating if new and better route
        private void AddAdjacentNode(int dx, int dy, int cost, RouteNode parent)
        {
            int x = parent.X + dx;
            int y = parent.Y + dy;
            int newCost = parent.MovementCost + cost;

            //Get terrain cost. -1 is not passable, 0 no cost, 1,2,3 etc higher cost
            int terraincost = GetTerrainCost(x, y);
            if (terraincost == -1) return;

            //Check if item has been added to the grid already
            RouteNode existing = _grid.Get(x, y);
            if (existing != null)
            {
                if (!existing.Closed && newCost < existing.MovementCost)
                {
                    existing.MovementCost = newCost;
                    existing.TotalCost = newCost + existing.Heuristic;
                    existing.Parent = parent;

                    if (_movementList.Contains(existing))
                    {
                        _movementList.Update(existing);
                    }
                    else
                    {
                        _openList.Update(existing);
                    }
                    
                }
                return;
            }

            //Create a new node
            RouteNode node = new RouteNode();

            node.X = x;
            node.Y = y;

            //Set the parent after the location so that the Direction can be set correctly
            node.Parent = parent;

            node.MovementCost = newCost;	//Add the cost to the parent cost
            node.Heuristic = GetHeuristic(node);
            node.TotalCost = newCost + node.Heuristic;

            PushNode(node);
            
            //Add to the grid
            _grid.Add(x, y, node);
        }

        //Pushes the node supplied onto the correct list
        private void PushNode(RouteNode node)
        {
            //Add to the movement or open list
            //Add to movement if both counts zero
            if (_movementList.Count == 0 && _openList.Count == 0)
            {
                _movementList.Push(node);
            }
            //Decide when movement list count > zero
            else if (_movementList.Count > 0)
            {
                if (node.TotalCost <= _movementList.TotalCost)
                {
                    //Move all nodes from the movement list to the open list that are greater than the totalcost of the new node
                    if (node.TotalCost < _movementList.TotalCost)
                    {
                        while (_movementList.Peek() != null)
                        {
                            _openList.Push(_movementList.Pop());
                        }
                    }
                    _movementList.Push(node);
                }
                else
                {
                    _openList.Push(node);
                }
            }
            //openlist count > 0
            else
            {
                if (node.TotalCost < _openList.TotalCost)
                {
                    _movementList.Push(node);
                }
                else
                {
                    _openList.Push(node);
                }
            }
        }

        //Calculates the distance as the best guess distance to move to goal. 
        //Must not be greater than possible distance

        //More accurately the heurisitc must be the cost of getting to the current node + the shortest distance estimate to complete
        //However we take the movement cost into account later using the routenode's total cost

        //We can use the shortest straight line distance to the goal as the second part of the heuristic.
        //or simply calculate the orthoganal distance
        private int GetHeuristic(RouteNode current)
        {
            //Euclidean
            //double x2 = Math.Pow(Math.Abs(_goal.X - current.X), 2);
            //double y2 = Math.Pow(Math.Abs(_goal.Y - current.Y), 2);
            //return Convert.ToInt32(Math.Sqrt(x2 + y2));

            //Manhattan
            int x = Math.Abs(_goal.X - current.X);
            int y = Math.Abs(_goal.Y - current.Y);
            return x + y;
        }

        //Check that a node can be created here
        private int GetTerrainCost(int x, int y)
        {
            //Check the boundary
            if (!_boundary.Contains(x, y)) return -1;

            TerrainPoint node = _terrain.Get(x, y);
            if (node.IsEmpty) return 0;

            return -1;
        }

        //Loop through each rectangle and create a terrain grid
        //Make sure grid has been cleared
        private void CreateTerrain(Point startPoint, Point endPoint)
        {
            if (Model == null) return;

            //Add all shapes if avoid is on, otherwise only the start and end
            if (Avoid)
            {
                foreach (Shape shape in Model.Shapes.Values)
                {
                    //Reduce the terrain rectangle if rectangle intersection 
                    //and starting or ending shapes
                    if (shape == Start || shape == End)
                    {
                        Rectangle rect = GetInflatedRectangle(shape);
                        AddRectangleToTerrain(rect);
                    }
                    else
                    {
                        Rectangle rect = GetInflatedRectangle(shape);
                        if (!rect.Contains(startPoint) && !rect.Contains(endPoint)) AddRectangleToTerrain(rect);
                    }
                }
            }
            else
            {
                if (Start != null) AddRectangleToTerrain(GetInflatedRectangle(Start));
                if (End != null) AddRectangleToTerrain(GetInflatedRectangle(End));
            }
        }

        private Rectangle GetInflatedRectangle(Shape shape)
        {
            Rectangle rect = Rectangle.Round(shape.Bounds);
            if (shape.UpdateElement != null) rect = Rectangle.Round(shape.UpdateElement.Bounds);
            rect.Inflate(_padding.Width / 2, _padding.Height / 2);
            return rect;
        }

        private void AddRectangleToTerrain(Rectangle rect)
        {
            //Round down rect
            int x1 = Convert.ToInt32(rect.X / _grain) * _grain;
            int y1 = Convert.ToInt32(rect.Y / _grain) * _grain;
            int x2 = Convert.ToInt32(rect.Right / _grain) * _grain;
            int y2 = Convert.ToInt32(rect.Bottom / _grain) * _grain;

            //Add the top and bottom, if already exists then is ignored
            for (int i = x1; i <= x2; i += _grain)
            {
                _terrain.Combine(i, y1, new TerrainPoint(i, y1));
                _terrain.Combine(i, y2, new TerrainPoint(i, y2));
            }

            //Add the left and right, if already exists then is ignored
            for (int i = y1; i <= y2; i += _grain)
            {
                _terrain.Combine(x1, i, new TerrainPoint(x1, i));
                _terrain.Combine(x2, i, new TerrainPoint(x2, i));
            }
        }

        private List<PointF> MakeConnector(PointF start, PointF end)
        {
            List<PointF> output = new List<PointF>();

            //Create start and end points
            output.Add(start);
            output.Add(end);

            //Check if horizontal or vertical
            if (start.X == end.X || start.Y == end.Y) return output;

            //Insert two mid points
            output.Insert(1, new PointF()); //new point 1
            output.Insert(2, new PointF()); //new point 2

            bool horizontal = false;

            //Work out if vertical or horizontal
            horizontal = end.X - start.X > end.Y - start.Y;

            //Get mid point
            PointF mid = new PointF((start.X + end.X) / 2, (start.Y + end.Y) / 2);
            PointF new1 = new PointF();
            PointF new2 = new PointF();

            //Create 2 new mid points
            if (horizontal)
            {
                new1.X = mid.X;
                new2.X = mid.X;
                new1.Y = start.Y;
                new2.Y = end.Y;
            }
            else
            {
                new1.X = start.X;
                new2.X = end.X;
                new1.Y = mid.Y;
                new2.Y = mid.Y;
            }
            output[1] = new1;
            output[2] = new2;

            return output;
        }

        //Check the terrain between the two points
        private bool CheckTerrain(Point a, Point b)
        {
            if (_terrain.Count == 0) return true;

            int start;
            int end;

            if (a.X == b.X)
            {
                if (a.Y < b.Y)
                {
                    start = a.Y;
                    end = b.Y;
                }
                else
                {
                    start = b.Y;
                    end = a.Y;
                }
            }
            else
            {
                if (a.X < b.X)
                {
                    start = a.X;
                    end = b.X;
                }
                else
                {
                    start = b.X;
                    end = a.X;
                }
            }

            //Round the start and end onto the grid
            start = Convert.ToInt32(start / _grain) * _grain;
            end = Convert.ToInt32(end / _grain) * _grain;

            //Round the other coordinate of the pair
            int div = (a.X == b.X) ? a.X : a.Y;
            div = Convert.ToInt32(div / _grain) * _grain;

            //Get the starting movement cost
            TerrainPoint node = _terrain.Get(div, start);
            int cost = (node.IsEmpty) ? 0 : -1;

            //Loop through the terrain checking for a node with movement cost
            for (int i = start; i < end; i += _grain)
            {
                node = new TerrainPoint();

                if (a.X == b.X)
                {
                    node = _terrain.Get(div, i);
                }
                else
                {
                    node = _terrain.Get(i, div);
                }

                if (!node.IsEmpty) return false;
            }

            return true;
        }

        //move the solution off the grid and back in line with the start and end
        private void AlignSolution(List<PointF> solution, PointF begin, PointF end)
        {
            //Only process if 2 or more points
            if (solution.Count < 2) return;

            int upper = solution.Count - 1;

            PointF a = (PointF)solution[0];
            PointF b = (PointF)solution[1];

            solution[0] = begin;
            if (a.X == b.X)
            {
                solution[1] = new PointF(begin.X, b.Y);
            }
            else
            {
                solution[1] = new PointF(b.X, begin.Y);
            }

            //Return if less than 3 points
            if (solution.Count == 2) return;

            a = (PointF)solution[upper];
            b = (PointF)solution[upper - 1];

            solution[upper] = end;
            if (a.X == b.X)
            {
                solution[upper - 1] = new PointF(end.X, b.Y);
            }
            else
            {
                solution[upper - 1] = new PointF(b.X, end.Y);
            }
        }

        private Rectangle RectangleToGrid(RectangleF rectangle)
        {
            //Round down rect
            int x1 = Convert.ToInt32(rectangle.X / _grain) * _grain;
            int y1 = Convert.ToInt32(rectangle.Y / _grain) * _grain;
            int x2 = Convert.ToInt32(rectangle.Right / _grain) * _grain;
            int y2 = Convert.ToInt32(rectangle.Bottom / _grain) * _grain;

            return new Rectangle(x1, y1, Math.Abs(x2 - x1), Math.Abs(y2 - y1));
        }

        #endregion
    }
}
