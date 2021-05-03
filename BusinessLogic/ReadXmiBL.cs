using System;
using System.Collections.Generic;
using System.Xml;
using master_project.Models;
using master_project.Models.UML;
using master_project.Utils;
using Newtonsoft.Json;


namespace master_project.BusinessLogic
{
    public class ReadXmiBL
    {
        ApplicationUser _identity;
        public ReadXmiBL(ApplicationUser Identity)
        {
            _identity = Identity;
        }
        public ReadXmiResponse readXmi(UploadRequest uploadRequest)
        {
            ReadXmiResponse readXmiResponse = new ReadXmiResponse();
            if (!validateXmi(uploadRequest.fileSource))
            {
                readXmiResponse.IsSuccess = false;
                readXmiResponse.Message = "Invalid XML File";
                return readXmiResponse;
            }
            // validate version input
            /*
            if (uploadRequest.xmiVersion != XmiVersions.One.Value && uploadRequest.xmiVersion != XmiVersions.Two.Value)
            {
                readXmiResponse.IsSuccess = false;
                readXmiResponse.Message = "Invalid XMI version";
                return readXmiResponse;
            }
            */
            List<UMLElement> resultElements = new List<UMLElement>();

            string currentVersion = "";
            if (ValidateVersion(uploadRequest.fileSource, out currentVersion))
            {

                if (currentVersion == "1.1" || currentVersion == "1.2")
                {
                    XmiVersionOneBL xmiVersionOne = new XmiVersionOneBL(_identity);
                    bool versionValid = xmiVersionOne.readContent(uploadRequest.fileSource, out resultElements);
                    if (!versionValid)
                    {
                        readXmiResponse.IsSuccess = false;
                        readXmiResponse.Message = "Invalid XMI content for version " + currentVersion;
                        return readXmiResponse;
                    }
                }
                else if (currentVersion == "2.1")
                {
                    XmiVersionTwoBL xmiVersionTwoBL = new XmiVersionTwoBL(_identity);
                    bool versionValid = xmiVersionTwoBL.readContent(uploadRequest.fileSource, out resultElements);
                    if (!versionValid)
                    {
                        readXmiResponse.IsSuccess = false;
                        readXmiResponse.Message = "Invalid XMI content for version " + currentVersion;
                        return readXmiResponse;
                    }

                }
                else
                {
                    readXmiResponse.IsSuccess = false;
                    readXmiResponse.Message = "XMI file with version " + currentVersion + " is not supported";
                    return readXmiResponse;
                }
            }
            else
            {
                readXmiResponse.IsSuccess = false;
                readXmiResponse.Message = "Invalid XMI file";
                return readXmiResponse;
            }
            IdentifyDiagramBL identifyDiagramBL=new IdentifyDiagramBL(_identity);
           readXmiResponse.DiagramName = identifyDiagramBL.GetDiagramName(resultElements);
           readXmiResponse.Name = uploadRequest.name;
           readXmiResponse.XmlContent = uploadRequest.fileSource;
            readXmiResponse.IsSuccess = true;
            readXmiResponse.Message = "Success";
            readXmiResponse.resultElements = JsonConvert.SerializeObject(resultElements);;
            return readXmiResponse;
        }
        private bool validateXmi(string xmiContent)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.LoadXml(xmiContent);

            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }
        private bool ValidateVersion(string xmiContent, out string version)
        {
            version = "";
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmiContent);
            XmlNodeList XMI = xDoc.GetElementsByTagName("XMI");
            if (XMI.Count == 0)
            {
                XMI = xDoc.GetElementsByTagName("xmi:XMI");
                if (XMI.Count == 0)
                {
                    XMI = xDoc.GetElementsByTagName("uml:Model");
                    if (XMI.Count == 0)
                    {
                        return false;
                    }

                }



            }
            XmlNode xmlRootNode = XMI[0];
            XmlAttribute xmlAttribute = xmlRootNode.Attributes["xmi.version"];
            if (xmlAttribute == null)
            {
                xmlAttribute = xmlRootNode.Attributes["xmi:version"];
                if (xmlAttribute == null)
                {
                    return false;
                }

            }
            version = xmlAttribute.Value;

            return true;

        }

    }
}