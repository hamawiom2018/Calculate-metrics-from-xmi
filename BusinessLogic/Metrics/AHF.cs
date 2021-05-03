using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using master_project.Models;
using master_project.Models.UML;
using master_project.Utils;
using Newtonsoft.Json;


namespace master_project.BusinessLogic.Metrics
{
    public class AHF
    {

        List<UMLElement> _diagramElements;
        string _sDiagramElements;
        public AHF(List<UMLElement> diagramElements, string sDiagramElements)
        {
            _diagramElements = diagramElements;
            _sDiagramElements = sDiagramElements;

        }
        public double Evaluate()
        {
            double result = getAHF();

            return result;
        }
        private double getAHF()
        {
            List<UMLClass> allClasses = getAllClasses(_diagramElements);
            MetricHelper metricHelper = new MetricHelper();
            double mdc = 0;
            foreach (UMLClass classElement in allClasses)
            {

                if (classElement.Attributes?.Count > 0)
                {
                    double mdci = 0;
                    foreach (UMLAttribute attribute in classElement.Attributes)
                    {
                        double vmi = 0;
                        double isVisiblePoint = 0;
                        if (attribute.Visibility == "public")
                        {
                            isVisiblePoint += allClasses.Count;
                        }
                        else if (attribute.Visibility == "private")
                        {
                            isVisiblePoint += 1;
                        }
                        else if (attribute.Visibility == "protected")
                        {
                            //search of all chold classes;
                            List<string> childRefs = metricHelper.getAllChildsRefs(classElement, _diagramElements, _sDiagramElements);
                            isVisiblePoint += childRefs.Distinct().Count() + 1;
                        }
                        vmi = isVisiblePoint / (allClasses.Count - 1);

                        mdci += (1 - vmi);
                    }
                    mdc += mdci;
                }

            }
            //
            double totalAttributes = allClasses.Select(classElement => classElement.Attributes  != null ? classElement.Attributes.Count : 0).Sum();
            double result = mdc / totalAttributes;
            return result;
        }
        private List<UMLClass> getAllClasses(List<UMLElement> elements)
        {
            List<UMLClass> allClasses = new List<UMLClass>();
            foreach (var element in elements)
            {
                if (element.Type == (int)AppEnums.Type.Class || element.Type == (int)AppEnums.Type.InnerClass)
                {
                    MetricHelper metricHelper = new MetricHelper();
                    string sClassElement = metricHelper.getElementByXmiId(element.XmiId, _sDiagramElements);
                    UMLClass classElement = JsonConvert.DeserializeObject<UMLClass>(sClassElement);
                    allClasses.Add(classElement);
                }
                if (element.Childs?.Count > 0)
                {
                    allClasses.AddRange(getAllClasses(element.Childs));
                }
            }
            return allClasses;
        }
    }

}