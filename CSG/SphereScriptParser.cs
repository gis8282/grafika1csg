using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;

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

        private List<TreeSphere> _allSpheres = new List<TreeSphere>();

        public class SphereGrammar : Grammar
        {
            public SphereGrammar()
            {
                var position = new NonTerminal(POSITION);
                var color = new NonTerminal(COLOR);
                var radius = new NonTerminal(RADIUS);
                var number = new NumberLiteral(NUMBER);
                var treeOp = new NonTerminal(TREEOPERATION);
                var addition = new NonTerminal(ADD);
                var subtraction = new NonTerminal(SUB);
                var intersection = new NonTerminal(INTER);
                var sphere = new NonTerminal(SPHERE);

                Root = treeOp;

                position.Rule = "pos" + number + number + number;
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

        public TreeNode ParseScene(string script)
        {
            var sg = new SphereGrammar();

            var language = new LanguageData(sg);
            var parser = new Parser(language);

            var x = parser.Parse(@"(sphere pos 1.0 0.0 0.0 col 255 128 0 radius 0.9
                                   + sphere pos 3.0 0.0 0.0 col 252 18 222 radius 0.9) 
                                   - sphere pos 2.0 0.0 0.0 col 0 128 255 radius 0.9 
                                    
            ");
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

                var positionNumbers = positionNode.ChildNodes.Where(ptn => ptn.Term.Name == NUMBER).Select(pn => pn.Token.Value).Cast<double>();

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
    }
}
