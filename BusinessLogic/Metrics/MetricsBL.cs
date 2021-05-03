using System;
using System.Collections.Generic;
using System.Xml;
using master_project.Models;
using master_project.Models.UML;
using master_project.Utils;
using Newtonsoft.Json;


namespace master_project.BusinessLogic.Metrics
{
    public class MetricsBL
    {
        public double CalculateMetric(string sDiagramElements, string MetricCode,string elementRef){
            double result=-1;

            List<UMLElement> diagramElements=JsonConvert.DeserializeObject<List<UMLElement>>(sDiagramElements);
            if(MetricCode.ToLower()==AppEnums.Metric.NPM.ToString().ToLower()){
                NPM npm=new NPM(diagramElements);
                result= npm.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.NCM.ToString().ToLower()){
                NCM ncm=new NCM(diagramElements);
                result= ncm.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.NAM.ToString().ToLower()){
                NAM nam=new NAM(diagramElements);
                result= nam.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.NUM.ToString().ToLower()){
                NUM num=new NUM(diagramElements);
                result= num.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.NOM.ToString().ToLower()){
                NOM nom=new NOM(diagramElements);
                result= nom.Evaluate();
            }
            else if(MetricCode.ToLower()==AppEnums.Metric.NMM.ToString().ToLower()){
                NMM nmm=new NMM(diagramElements);
                result= nmm.Evaluate();
            }
            else if(MetricCode.ToLower()==AppEnums.Metric.NASM.ToString().ToLower()){
                NASM nasm=new NASM(diagramElements);
                result= nasm.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.NAGM.ToString().ToLower()){
                NAGM nagm=new NAGM(diagramElements,sDiagramElements);
                result= nagm.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.NIM.ToString().ToLower()){
                NIM nim=new NIM(diagramElements);
                result= nim.Evaluate();
            }
            else if(MetricCode.ToLower()==AppEnums.Metric.NATC1.ToString().ToLower()){
                NATC1 natc1=new NATC1(diagramElements,elementRef,sDiagramElements);
                result= natc1.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.NATC2.ToString().ToLower()){
                NATC2 natc2=new NATC2(diagramElements,elementRef,sDiagramElements);
                result= natc2.Evaluate();
            }
            else if(MetricCode.ToLower()==AppEnums.Metric.NOPC1.ToString().ToLower()){
                NOPC1 naopc1=new NOPC1(diagramElements,elementRef,sDiagramElements);
                result= naopc1.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.NOPC2.ToString().ToLower()){
                NOPC2 naopc2=new NOPC2(diagramElements,elementRef,sDiagramElements);
                result= naopc2.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.NASC.ToString().ToLower()){
                NASC nasc=new NASC(diagramElements,elementRef,sDiagramElements);
                result= nasc.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.CBC.ToString().ToLower()){
                CBC cbc=new CBC(diagramElements,elementRef,sDiagramElements);
                result= cbc.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.DIT.ToString().ToLower()){
                DIT dit=new DIT(diagramElements,elementRef,sDiagramElements);
                result= dit.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.NSUPC.ToString().ToLower()){
                NSUPC nsupc=new NSUPC(diagramElements,elementRef,sDiagramElements);
                result= nsupc.Evaluate();
            }else if(MetricCode.ToLower()=="NSUPC*".ToLower()){
                NSUPC2 nsupc2=new NSUPC2(diagramElements,elementRef,sDiagramElements);
                result= nsupc2.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.NSUBC.ToString().ToLower()){
                NSUBC nsubc=new NSUBC(diagramElements,elementRef,sDiagramElements);
                result= nsubc.Evaluate();
            }else if(MetricCode.ToLower()=="NSUBC*".ToLower()){
                NSUBC2 nsupc2=new NSUBC2(diagramElements,elementRef,sDiagramElements);
                result= nsupc2.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.DAC.ToString().ToLower()){
                DAC dac=new DAC(diagramElements,elementRef,sDiagramElements);
                result= dac.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.SIZE2.ToString().ToLower()){
                SIZE2 size2=new SIZE2(diagramElements,elementRef,sDiagramElements);
                result= size2.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.MHF.ToString().ToLower()){
                MHF mhf=new MHF(diagramElements,sDiagramElements);
                result= mhf.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.AHF.ToString().ToLower()){
                AHF ahf=new AHF(diagramElements,sDiagramElements);
                result= ahf.Evaluate();
            }else if(MetricCode.ToLower()==AppEnums.Metric.MIF.ToString().ToLower()){
                MIF mif=new MIF(diagramElements,sDiagramElements);
                result= mif.Evaluate();
            }else if(MetricCode.ToLower()=="AIF".ToLower()){
                AIF mif=new AIF(diagramElements,sDiagramElements);
                result= mif.Evaluate();
            }

            return result;
        }
    }

}