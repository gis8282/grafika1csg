using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;

namespace Csg
{
    public class SphereGrammar : Grammar
    {
        public const string TREEOPERATION = "treeOp";
        public const string SPHERE = "sphere";
        public const string NUMBER = "number";
        public const string POSITION = "postion";
        public const string COLOR = "color";
        public const string RADIUS = "radius";
        public const string ADD = "addition";
        public const string SUB = "subtraction";
        public const string INTER = "intersection";
        public const string SIGNEDNUMBER = "signedNumber";
        public const string MINUS = "-";

        public SphereGrammar()
        {
            var position = new NonTerminal(POSITION);
            var color = new NonTerminal(COLOR);
            var radius = new NonTerminal(RADIUS);
            var signedNumber = new NonTerminal(SIGNEDNUMBER);
            var number = new NumberLiteral(NUMBER);
            var treeOp = new NonTerminal(TREEOPERATION);
            var addition = new NonTerminal(ADD);
            var subtraction = new NonTerminal(SUB);
            var intersection = new NonTerminal(INTER);
            var sphere = new NonTerminal(SPHERE);
            var minus = new NonTerminal(MINUS);

            Root = treeOp;

            minus.Rule = ToTerm("-");
            signedNumber.Rule = minus + number | number;

            position.Rule = "pos" + signedNumber + signedNumber + signedNumber;
            color.Rule = "col" + number + number + number;
            radius.Rule = "radius" + number;

            sphere.Rule = "sphere" + position + color + radius;

            treeOp.Rule = sphere | addition | subtraction | intersection | "(" + treeOp + ")";
            addition.Rule = sphere + "+" + sphere | treeOp + "+" + treeOp | sphere + "+" + treeOp | treeOp + "+" + sphere;
            subtraction.Rule = sphere + "-" + sphere | treeOp + "-" + treeOp | sphere + "-" + treeOp | treeOp + "-" + sphere;
            intersection.Rule = sphere + "*" + sphere | treeOp + "*" + treeOp | sphere + "*" + treeOp | treeOp + "*" + sphere;

            MarkPunctuation("+", "-", "(", ")", "*", "pos", "col", "sphere");
        }
    }
}
