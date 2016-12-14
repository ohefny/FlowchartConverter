// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Collections;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
    public class TextCommand: Command
    {
        //Property variables
        private string _commandText;
        private string _commandValue;

        private LabelList _labels;
        private List<LabelMomento> _momentos;

        //Constructors
        public TextCommand(Controller controller): base(controller)
        {
            if (controller == null) throw new ArgumentNullException();
            _labels = new LabelList();
        }

        public virtual LabelList Labels
        {
            get
            {
                return _labels;
            }
            set
            {
                _labels = value;
            }
        }
       
        public virtual string CommandText
        {
            get
            {
                return _commandText;
            }
            set
            {
                _commandText = value;
            }
        }

        public virtual string CommandValue
        {
            get
            {
                return _commandValue;
            }
            set
            {
                _commandValue = value;
            }
        }

        //Implementation
        public override void Execute()
        {
            if (Labels == null) return;

            //Create the momentos of the current labels
            _momentos = new List<LabelMomento>();

            //Loop through and add a momento for each label
            foreach (Label label in _labels)
            {
                LabelMomento momento = new LabelMomento(label);
                _momentos.Add(momento);
            }

            //Make comand lower case
            string command = CommandText.ToLower();

            switch (command)
            {
                case "bold":
                    Controller.Suspend();
                    DoCommandText(true, false, false, false);
                    Controller.Resume();
                    Controller.Invalidate();
                    break;
                case "italic":
                    Controller.Suspend();
                    DoCommandText(false, true, false, false);
                    Controller.Resume();
                    Controller.Invalidate();
                    break;
                case "strikeout":
                    Controller.Suspend();
                    DoCommandText(false, false, true, false);
                    Controller.Resume();
                    Controller.Invalidate();
                    break;
                case "underline":
                    Controller.Suspend();
                    DoCommandText(false, false, false, true);
                    Controller.Resume();
                    Controller.Invalidate();
                    break;
                case "align left":
                    Controller.Suspend();
                    DoCommandAlign(true, false, false);
                    Controller.Resume();
                    Controller.Invalidate();
                    break;
                case "align center":
                    Controller.Suspend();
                    DoCommandAlign(false, true, false);
                    Controller.Resume();
                    Controller.Invalidate();
                    break;
                case "align right":
                    Controller.Suspend();
                    DoCommandAlign(false, false, true);
                    Controller.Resume();
                    Controller.Invalidate();
                    break;
                case "align top":
                    Controller.Suspend();
                    DoCommandVerticalAlign(true, false, false);
                    Controller.Resume();
                    Controller.Invalidate();
                    break;
                case "align middle":
                    Controller.Suspend();
                    DoCommandVerticalAlign(false, true, false);
                    Controller.Resume();
                    Controller.Invalidate();
                    break;
                case "align bottom":
                    Controller.Suspend();
                    DoCommandVerticalAlign(false, false, true);
                    Controller.Resume();
                    Controller.Invalidate();
                    break;
                case "font":
                    Controller.Suspend();
                    DoCommandFont(Controller.Model.SelectedShapes(), CommandValue);
                    Controller.Resume();
                    Controller.Invalidate();
                    break;
                case "font size":
                    Controller.Suspend();
                    DoCommandFontSize(Controller.Model.SelectedShapes(), CommandValue);
                    Controller.Resume();
                    Controller.Invalidate();
                    break;
            }
        }


        public override void Undo()
        {
            UndoRedo();
        }

        public override void Redo()
        {
            UndoRedo();
        }

        public virtual bool GetCommand(string command)
        {
            //Make comand lower case
            command = command.ToLower();

            switch (command)
            {
                case "undo":
                    return (Controller.UndoStack.Count > 0);
                    break;
                case "redo":
                    return (Controller.RedoStack.Count > 0);
                    break;
                case "bold":
                    return GetCommandTextAvailable();
                    break;
                case "italic":
                    return GetCommandTextAvailable();
                    break;
                case "strikeout":
                    return GetCommandTextAvailable();
                    break;
                case "underline":
                    return GetCommandTextAvailable();
                    break;
                case "bold status":
                    return GetCommandTextStatus(true, false, false, false);
                    break;
                case "italic status":
                    return GetCommandTextStatus(false, true, false, false);
                    break;
                case "strikeout status":
                    return GetCommandTextStatus(false, false, true, false);
                    break;
                case "underline status":
                    return GetCommandTextStatus(false, false, false, true);
                    break;
                case "align left":
                    return GetCommandTextAvailable();
                    break;
                case "align center":
                    return GetCommandTextAvailable();
                    break;
                case "align right":
                    return GetCommandTextAvailable();
                    break;
                case "align left status":
                    return GetAlignStatus(true, false, false);
                    break;
                case "align center status":
                    return GetAlignStatus(false, true, false);
                    break;
                case "align right status":
                    return GetAlignStatus(false, false, true);
                    break;
                case "align top":
                    return GetCommandTextAvailable();
                    break;
                case "align middle":
                    return GetCommandTextAvailable();
                    break;
                case "align bottom":
                    return GetCommandTextAvailable();
                    break;
                case "align top status":
                    return GetVerticalAlignStatus(true, false, false);
                    break;
                case "align middle status":
                    return GetVerticalAlignStatus(false, true, false);
                    break;
                case "align bottom status":
                    return GetVerticalAlignStatus(false, false, true);
                    break;
                case "read clipboard":
                    Controller.Clipboard.Read();
                    return true;
                case "write clipboard":
                    Controller.Clipboard.Write();
                    return true;
            }

            return false;
        }

        public virtual string GetCommandString(string command)
        {
            //Make comand lower case
            command = command.ToLower();

            switch (command)
            {
                case "font status":
                    return GetFontNameStatus(Controller.Model.SelectedShapes());
                    break;
                case "font size status":
                    return GetFontSizeStatus(Controller.Model.SelectedShapes());
                    break;
            }

            return "";
        }

        private void DoCommandText(bool bold, bool italic, bool strikeout, bool underline)
        {
            bool boldflag = false;
            bool italicflag = false;
            bool strikeflag = false;
            bool underlineflag = false;

            //Get settings becuase may be mixed mode
            if (bold) boldflag = !GetCommand("bold status");
            if (italic) italicflag = !GetCommand("italic status");
            if (strikeout) strikeflag = !GetCommand("strike status");
            if (underline) underlineflag = !GetCommand("underline status");

            foreach (Label label in Labels)
            {
                if (bold) label.Bold = boldflag;
                if (italic) label.Italic = italicflag;
                if (strikeout) label.Strikeout = strikeflag;
                if (underline) label.Underline = underlineflag;
            }
        }

        private bool GetCommandTextStatus(bool bold, bool italic, bool strikeout, bool underline)
        {
            foreach (Label label in Labels)
            {
                if (bold && !label.Bold) return false;
                if (italic && !label.Italic) return false;
                if (strikeout && !label.Strikeout) return false;
                if (underline && !label.Underline) return false;
            }

            return true;
        }

        private void DoCommandAlign(bool left, bool center, bool right)
        {
            bool leftflag = false;
            bool centerflag = false;
            bool rightflag = false;

            //Get settings becuase may be mixed mode
            if (left) leftflag = !GetCommand("align left status");
            if (center) centerflag = !GetCommand("align center status");
            if (right) rightflag = !GetCommand("align right status");

            foreach (Label label in Labels)
            {
                if (label is Label)
                {
                    Label textLabel = label as Label;
                    if (left && leftflag) textLabel.Alignment = StringAlignment.Near;
                    if (center && centerflag) textLabel.Alignment = StringAlignment.Center;
                    if (right && rightflag) textLabel.Alignment = StringAlignment.Far;
                }
            }
        }

        private bool GetAlignStatus(bool left, bool center, bool right)
        {
            foreach (Label label in Labels)
            {
                if (left && label.Alignment != StringAlignment.Near) return false;
                if (center && label.Alignment != StringAlignment.Center) return false;
                if (right && label.Alignment != StringAlignment.Far) return false;
            }

            return true;
        }

        private void DoCommandVerticalAlign(bool top, bool middle, bool bottom)
        {
            bool topflag = false;
            bool middleflag = false;
            bool bottomflag = false;

            //Get settings becuase may be mixed mode
            if (top) topflag = !GetCommand("align top status");
            if (middle) middleflag = !GetCommand("align middle status");
            if (bottom) bottomflag = !GetCommand("align bottom status");

            foreach (Label label in Labels)
            {
                if (top && topflag) label.LineAlignment = StringAlignment.Near;
                if (middle && middleflag) label.LineAlignment = StringAlignment.Center;
                if (bottom && bottomflag) label.LineAlignment = StringAlignment.Far;
            }
        }

        private bool GetVerticalAlignStatus(bool top, bool middle, bool bottom)
        {
            foreach (Label label in Labels)
            {
                if (top && label.LineAlignment != StringAlignment.Near) return false;
                if (middle && label.LineAlignment != StringAlignment.Center) return false;
                if (bottom && label.LineAlignment != StringAlignment.Far) return false;
            }

            return true;
        }

        //Determines if all selected fonts are the same
        private string GetFontNameStatus(Shapes elements)
        {
            string fontname = "";

            foreach (Label label in Labels)
            {
                if (fontname != "") if (label.FontName != fontname) return "";
                fontname = label.FontName;
            }

            return fontname;
        }

        private string GetFontSizeStatus(Shapes elements)
        {
            float fontsize = 0;

            foreach (Label label in Labels)
            {
                if (fontsize != 0) if (label.FontSize != fontsize) return "";
                fontsize = label.FontSize;
            }

            if (fontsize == 0) return "";
            return fontsize.ToString();
        }

        private bool DoCommandFont(Shapes elements, string fontname)
        {
            foreach (Label label in Labels)
            {
                label.FontName = fontname;
            }

            return true;
        }

        private void DoCommandFontSize(Shapes elements, string fontsize)
        {
            float size = 0;

            //Try convert string size to a single
            try
            {
                size = Convert.ToSingle(fontsize);
            }
            catch
            {

            }

            if (size == 0) return;

            foreach (Label label in Labels)
            {
                label.FontSize = size;
            }
        }

        //Determine if there are any selected elements with text
        private bool GetCommandTextAvailable()
        {
            return (Labels != null && Labels.Count > 0);
        }

        private void UndoRedo()
        {
            int index = 0;

            //Loop through each element and write the contents of the matching momento back into the element
            foreach (Label label in Labels)
            {
                //Get a momento of the label in its current state
                LabelMomento momento = new LabelMomento(label);

                //Write the momento into the element and save the new momento
                _momentos[index].WriteItem(label);
                _momentos[index] = momento;

                index++;
            }
        }
    }
}
