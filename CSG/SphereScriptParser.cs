using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;
using System.IO;

namespace Csg
{
    public class SphereScriptParser : ISceneParser
    {
        private const string TREEOPERATION = "treeOp";
        private const string SPHERE = "sphere";
        private const string NUMBER = "number";
        private const string POSITION = "postion";
        private const string COLOR = "color";
        private const string RADIUS = "radius";
        private const string ADD = "addition";
        private const string SUB = "subtraction";
        private const string INTER = "intersection";
        private const string SIGNEDNUMBER = "signedNumber";
        private const string MINUS = "-";

        private List<TreeSphere> _allSpheres = new List<TreeSphere>();

        public class SphereGrammar : Grammar
        {
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

                MarkPunctuation("[", "]", "{", "}", "+", "-", "(", ")", "*", "pos", "col", "sphere");
            }
        }

        public TreeNode ParseScene(string fileName)
        {
            var sg = new SphereGrammar();

            var language = new LanguageData(sg);
            var parser = new Parser(language);

            var script = new StreamReader(fileName).ReadToEnd();

            var x = parser.Parse(script);
            TreeNode root = ConvertParseTreeToTreeNode(x);

            return root;
        }

        private TreeNode ConvertParseTreeToTreeNode(ParseTree x)
        {
            if (x.Status == ParseTreeStatus.Parsed)
            {
                var root = ConvertParseTreeToTreeNode(x.Root);

                return root;
            }
            return null;
        }

        private TreeNode ConvertParseTreeToTreeNode(ParseTreeNode parseTreeNode)
        {
            if (parseTreeNode.Term.Name == TREEOPERATION)
            {
                return ConvertParseTreeToTreeNode(parseTreeNode.ChildNodes.First());
            }
            else if (parseTreeNode.Term.Name == SPHERE)
            {
                var radiusNode = parseTreeNode.ChildNodes.First(ptn => ptn.Term.Name == RADIUS);
                var radius = (double)radiusNode.ChildNodes.First(ptn => ptn.Term.Name == NUMBER).Token.Value;

                var positionNode = parseTreeNode.ChildNodes.First(ptn => ptn.Term.Name == POSITION);

                var positionNumbers = positionNode.ChildNodes.Where(ptn => ptn.Term.Name == SIGNEDNUMBER).Select(pn => GetSignedNumber(pn));

                var colorNode = parseTreeNode.ChildNodes.First(ptn => ptn.Term.Name == COLOR);

                var colorNumbers = colorNode.ChildNodes.Where(ptn => ptn.Term.Name == NUMBER).Select(pn => pn.Token.Value).Cast<int>();

                var sphere = new Sphere(positionNumbers.ToArray(), radius, colorNumbers.ToArray());
                var treeSphere = new TreeSphere(sphere);

                _allSpheres.Add(treeSphere);

                return treeSphere;
            }
            else if (parseTreeNode.Term.Name == ADD)
            {
                return new TreeOperation(OperationType.union, ConvertParseTreeToTreeNode(parseTreeNode.ChildNodes.First()), ConvertParseTreeToTreeNode(parseTreeNode.ChildNodes.Last()));
            }
            else if (parseTreeNode.Term.Name == SUB)
            {
                return new TreeOperation(OperationType.difference, ConvertParseTreeToTreeNode(parseTreeNode.ChildNodes.First()), ConvertParseTreeToTreeNode(parseTreeNode.ChildNodes.Last()));
  
            }
            else if (parseTreeNode.Term.Name == INTER)
            {
                return new TreeOperation(OperationType.intersection, ConvertParseTreeToTreeNode(parseTreeNode.ChildNodes.First()), ConvertParseTreeToTreeNode(parseTreeNode.ChildNodes.Last()));
            }
            return null;
        }

        private double GetSignedNumber(ParseTreeNode pn)
        {
            var number = (double)pn.ChildNodes.First(ptn => ptn.Term.Name == NUMBER).Token.Value;
            var minus = pn.ChildNodes.FirstOrDefault(ptn => ptn.Term.Name == MINUS);

            if (minus != null)
            {
                number *= -1;
            }

            return number;
        }
    }
}
