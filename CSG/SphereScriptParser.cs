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
        public TreeNode ParseScene(string fileName)
        {
            var sg = new SphereGrammar();

            var language = new LanguageData(sg);
            var parser = new Parser(language);

            var script = ReadFile(fileName);

            var x = parser.Parse(script);
            TreeNode root = Generate(x);

            return root;
        }

        private string ReadFile(string fileName)
        {
            var script = new StreamReader(fileName).ReadToEnd();
            return script;
        }

        private TreeNode Generate(ParseTree x)
        {
            if (x.Status == ParseTreeStatus.Parsed)
            {
                var root = Generate(x.Root);

                return root;
            }
            return null;
        }

        private TreeNode Generate(ParseTreeNode parseTreeNode)
        {
            if (parseTreeNode.Term.Name == SphereGrammar.TREEOPERATION)
            {
                return Generate(parseTreeNode.ChildNodes.First());
            }
            else if (parseTreeNode.Term.Name == SphereGrammar.SPHERE)
            {
                return GenerateSphere(parseTreeNode);
            }
            else if (parseTreeNode.Term.Name == SphereGrammar.ADD)
            {
                return new TreeOperation(OperationType.union, Generate(parseTreeNode.ChildNodes.First()), Generate(parseTreeNode.ChildNodes.Last()));
            }
            else if (parseTreeNode.Term.Name == SphereGrammar.SUB)
            {
                return new TreeOperation(OperationType.difference, Generate(parseTreeNode.ChildNodes.First()), Generate(parseTreeNode.ChildNodes.Last()));
  
            }
            else if (parseTreeNode.Term.Name == SphereGrammar.INTER)
            {
                return new TreeOperation(OperationType.intersection, Generate(parseTreeNode.ChildNodes.First()), Generate(parseTreeNode.ChildNodes.Last()));
            }
            return null;
        }

        private TreeNode GenerateSphere(ParseTreeNode parseTreeNode)
        {
            var radiusNode = parseTreeNode.ChildNodes.First(ptn => ptn.Term.Name == SphereGrammar.RADIUS);
            var radius = (double)radiusNode.ChildNodes.First(ptn => ptn.Term.Name == SphereGrammar.NUMBER).Token.Value;

            var positionNode = parseTreeNode.ChildNodes.First(ptn => ptn.Term.Name == SphereGrammar.POSITION);
            var positionNumbers = positionNode.ChildNodes.Where(ptn => ptn.Term.Name == SphereGrammar.SIGNEDNUMBER).Select(pn => GetSignedNumber(pn));

            var colorNode = parseTreeNode.ChildNodes.First(ptn => ptn.Term.Name == SphereGrammar.COLOR);
            var colorNumbers = colorNode.ChildNodes.Where(ptn => ptn.Term.Name == SphereGrammar.NUMBER).Select(pn => pn.Token.Value).Cast<int>();

            var sphere = new Sphere(positionNumbers.ToArray(), radius, colorNumbers.ToArray());
            var treeSphere = new TreeSphere(sphere);

            return treeSphere;
        }

        private double GetSignedNumber(ParseTreeNode pn)
        {
            var number = (double)pn.ChildNodes.First(ptn => ptn.Term.Name == SphereGrammar.NUMBER).Token.Value;
            var minus = pn.ChildNodes.FirstOrDefault(ptn => ptn.Term.Name == SphereGrammar.MINUS);

            if (minus != null)
            {
                number *= -1;
            }

            return number;
        }
    }
}
