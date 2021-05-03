import { UMLAssociation, UMLCombinedFragment, UMLConnector, UMLDependency, UMLElement, UMLGeneralization, UMLInformationFlow, UMLInteractionOperand, UMLLifeLine, UMLMessage, UMLObject, UMLPackageImport, UMLPackageMerge, UMLPort, UMLProperty, UMLProvided, UMLRelization, UMLRequired, UMLState, UMLTransition, UMLUsage } from "src/app/models/elements/UMLElements";
import { UMLClassRefined, UMLElementRefined, UMLLifeLineRefined, UMLMessageRefined, UMLObjectRefined, UMLPackageRefined, UMLUseCaseRefined } from '../app/models/elements/UMLElementsRefined';
import { UMLType } from "./utils";
import { from } from "rxjs";
export function clone(cloneObj: any): any {

    for (var attribut in this) {
        if (typeof this[attribut] === "object") {
            cloneObj[attribut] = this[attribut].clone();
        } else {
            cloneObj[attribut] = this[attribut];
        }
    }
    return cloneObj;
}
export function convertUMLEmlementsRefined(data: any[], sData: string, parent?: any): UMLElementRefined[] {
    let result: UMLElementRefined[] = [];
    let newDate = JSON.parse(sData);

    data.forEach(element => {
        if (element.Type == 0) {
            let packageElement: UMLElementRefined = element;
            packageElement.TreeName = packageElement.Name;
            packageElement.IsElementDetail = false;
            if (element.Childs != null) {
                packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
            }
            result.push(packageElement);
        } else
            if (element.Type == UMLType.Class || element.Type == UMLType.Interface
                || element.Type == UMLType.InnerClass || element.Type == UMLType.InnerInterface
                || element.Type == UMLType.Component) {
                let packageElement: UMLClassRefined = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                    if (packageElement.Parent.Type == UMLType.Class || packageElement.Parent.Type == UMLType.Interface
                        || packageElement.Parent.Type == UMLType.InnerClass || packageElement.Parent.Type == UMLType.InnerInterface) {
                        if (packageElement.Type == UMLType.Class) {
                            packageElement.Type = UMLType.InnerClass;
                        } else if (packageElement.Type == UMLType.Interface) {
                            packageElement.Type = UMLType.InnerInterface;
                        }
                    }
                }

                packageElement.Associations = GetClassAssociations(packageElement,newDate, packageElement.XmiId);
                packageElement.Dependencies = GetClassDependencies(packageElement,newDate, packageElement.XmiId);
                packageElement.Generalizations = GetClassGeneralizations(packageElement,newDate, packageElement.XmiId);
                packageElement.Relizations = GetClassRealizations(packageElement,newDate, packageElement.XmiId);
                packageElement.Usages = GetPackageUsages(packageElement,newDate, packageElement.XmiId);

                packageElement.sType = GetElementTypeName(packageElement.Type);
                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '') + (packageElement.Visibility != null ? ' | ' + packageElement.Visibility : '');
                if ((packageElement.Attributes == null || packageElement.Attributes.length == 0) && (packageElement.Operations == null || packageElement.Operations.length == 0)) {
                    packageElement.IsElementDetail = false;
                } else {
                    packageElement.IsElementDetail = true;
                }
                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }

                //add the connections to the tree childs
                if (packageElement.Associations != null && packageElement.Associations.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Associations;
                    } else {
                        packageElement.Childs.push(...packageElement.Associations)
                    }
                }
                if (packageElement.Generalizations != null && packageElement.Generalizations.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Generalizations;
                    } else {
                        packageElement.Childs.push(...packageElement.Generalizations)
                    }
                }
                if (packageElement.Dependencies != null && packageElement.Dependencies.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Dependencies;
                    } else {
                        packageElement.Childs.push(...packageElement.Dependencies)
                    }
                }
                if (packageElement.Usages != null && packageElement.Usages.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Usages;
                    } else {
                        packageElement.Childs.push(...packageElement.Usages)
                    }
                }

                if (packageElement.Relizations != null && packageElement.Relizations.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Relizations;
                    } else {
                        packageElement.Childs.push(...packageElement.Relizations)
                    }
                }

                result.push(packageElement);
            } else if (element.Type == UMLType.Object) {
                let packageElement: UMLObjectRefined = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }

                packageElement.sType = 'Object'
                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '');
                if ((packageElement.Attributes == null || packageElement.Attributes.length == 0) && packageElement.InstancedClassName == null) {
                    packageElement.IsElementDetail = false;
                } else {
                    packageElement.IsElementDetail = true;
                }
                packageElement.Associations = GetClassAssociations(packageElement,newDate, packageElement.XmiId);
                packageElement.Dependencies = GetClassDependencies(packageElement,newDate, packageElement.XmiId);
                packageElement.Generalizations = GetClassGeneralizations(packageElement,newDate, packageElement.XmiId);
                packageElement.Relizations = GetClassRealizations(packageElement,newDate, packageElement.XmiId);

                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                //add the connections to the tree childs
                if (packageElement.Associations != null && packageElement.Associations.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Associations;
                    } else {
                        packageElement.Childs.push(...packageElement.Associations)
                    }
                }
                if (packageElement.Generalizations != null && packageElement.Generalizations.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Generalizations;
                    } else {
                        packageElement.Childs.push(...packageElement.Generalizations)
                    }
                }
                if (packageElement.Dependencies != null && packageElement.Dependencies.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Dependencies;
                    } else {
                        packageElement.Childs.push(...packageElement.Dependencies)
                    }
                }


                if (packageElement.Relizations != null && packageElement.Relizations.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Relizations;
                    } else {
                        packageElement.Childs.push(...packageElement.Relizations)
                    }
                }
                result.push(packageElement);
            } else if (element.Type == UMLType.Package) {
                let packageElement: UMLPackageRefined = element;
                
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.sType = 'Package'
                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '');
                packageElement.IsElementDetail = false;
                packageElement.Associations = GetClassAssociations(packageElement,newDate, packageElement.XmiId);
                packageElement.Dependencies = GetClassDependencies(packageElement,newDate, packageElement.XmiId);
                packageElement.Generalizations = GetClassGeneralizations(packageElement,newDate, packageElement.XmiId);
                packageElement.Relizations = GetClassRealizations(packageElement,newDate, packageElement.XmiId);
                packageElement.Usages = GetPackageUsages(packageElement,newDate, packageElement.XmiId);

                if (element.Childs != null) {
                    packageElement.ImportedPackages = GetImportedPackages(packageElement,packageElement.Childs);
                    packageElement.MergedPackage = GetMergedPackages(packageElement,packageElement.Childs);
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                //add the connections to the tree childs
                if (packageElement.Associations != null && packageElement.Associations.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Associations;
                    } else {
                        packageElement.Childs.push(...packageElement.Associations)
                    }
                }
                if (packageElement.Generalizations != null && packageElement.Generalizations.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Generalizations;
                    } else {
                        packageElement.Childs.push(...packageElement.Generalizations)
                    }
                }
                if (packageElement.Dependencies != null && packageElement.Dependencies.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Dependencies;
                    } else {
                        packageElement.Childs.push(...packageElement.Dependencies)
                    }
                }
                if (packageElement.Usages != null && packageElement.Usages.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Usages;
                    } else {
                        packageElement.Childs.push(...packageElement.Usages)
                    }
                }

                if (packageElement.Relizations != null && packageElement.Relizations.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Relizations;
                    } else {
                        packageElement.Childs.push(...packageElement.Relizations)
                    }
                }

                if (packageElement.ImportedPackages != null && packageElement.ImportedPackages.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.ImportedPackages;
                    } else {
                        packageElement.Childs.push(...packageElement.ImportedPackages)
                    }
                }
                if (packageElement.MergedPackage != null && packageElement.MergedPackage.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.MergedPackage;
                    } else {
                        packageElement.Childs.push(...packageElement.MergedPackage)
                    }
                }
                result.push(packageElement);
            } else if (element.Type == UMLType.Actor) {
                let packageElement: UMLPackageRefined = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.sType = 'Actor'
                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '');
                packageElement.IsElementDetail = false;
                packageElement.Associations = GetClassAssociations(packageElement,newDate, packageElement.XmiId);
                packageElement.Dependencies = GetClassDependencies(packageElement,newDate, packageElement.XmiId);
                packageElement.Generalizations = GetClassGeneralizations(packageElement,newDate, packageElement.XmiId);
                packageElement.Relizations = GetClassRealizations(packageElement,newDate, packageElement.XmiId);

                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                //add the connections to the tree childs
                if (packageElement.Associations != null && packageElement.Associations.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Associations;
                    } else {
                        packageElement.Childs.push(...packageElement.Associations)
                    }
                }
                if (packageElement.Generalizations != null && packageElement.Generalizations.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Generalizations;
                    } else {
                        packageElement.Childs.push(...packageElement.Generalizations)
                    }
                }
                if (packageElement.Dependencies != null && packageElement.Dependencies.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Dependencies;
                    } else {
                        packageElement.Childs.push(...packageElement.Dependencies)
                    }
                }


                if (packageElement.Relizations != null && packageElement.Relizations.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Relizations;
                    } else {
                        packageElement.Childs.push(...packageElement.Relizations)
                    }
                }
                result.push(packageElement);
            } else if (element.Type == UMLType.UseCase) {
                let packageElement: UMLUseCaseRefined = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.sType = 'Use Case';
                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '');
                packageElement.IsElementDetail = false;
                packageElement.Associations = GetClassAssociations(packageElement,newDate, packageElement.XmiId);
                packageElement.Dependencies = GetClassDependencies(packageElement,newDate, packageElement.XmiId);
                packageElement.Generalizations = GetClassGeneralizations(packageElement,newDate, packageElement.XmiId);
                packageElement.Relizations = GetClassRealizations(packageElement,newDate, packageElement.XmiId);

                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                //add the connections to the tree childs
                if (packageElement.Associations != null && packageElement.Associations.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Associations;
                    } else {
                        packageElement.Childs.push(...packageElement.Associations)
                    }
                }
                if (packageElement.Generalizations != null && packageElement.Generalizations.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Generalizations;
                    } else {
                        packageElement.Childs.push(...packageElement.Generalizations)
                    }
                }
                if (packageElement.Dependencies != null && packageElement.Dependencies.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Dependencies;
                    } else {
                        packageElement.Childs.push(...packageElement.Dependencies)
                    }
                }


                if (packageElement.Relizations != null && packageElement.Relizations.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.Relizations;
                    } else {
                        packageElement.Childs.push(...packageElement.Relizations)
                    }
                }
                result.push(packageElement);
            } else if (element.Type == UMLType.Interaction) {
                let packageElement: UMLElementRefined = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.sType = 'Interaction';
                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '');

                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                packageElement.IsElementDetail = false;
                result.push(packageElement);
            } else if (element.Type == UMLType.LifeLine) {
                let packageElement: UMLLifeLineRefined = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.sElmenetType = GetElementTypeName(packageElement.ElmenetType);
                if (packageElement.IncomingMessages != null) {
                    packageElement.IncomingMessages = GetMessagesTypes(packageElement,packageElement.IncomingMessages, true);

                }
                if (packageElement.OutgoingMessages != null) {
                    packageElement.OutgoingMessages = GetMessagesTypes(packageElement,packageElement.OutgoingMessages, false);

                }
                packageElement.sType = 'Life Line';
                let directionType:string='';
                packageElement.IsOtherParty=true;
                if(packageElement.Parent.Type==UMLType.Message){
                    if(packageElement.XmiId==(packageElement.Parent as UMLMessageRefined).ElementFromRef){
                        directionType='From > ';
                    }
                    if(packageElement.XmiId==(packageElement.Parent as UMLMessageRefined).ElementToRef){
                        directionType='To > '
                        
                    }
                }
                packageElement.TreeName = directionType+ packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '');

                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                if (packageElement.ElementName == null || packageElement.ElementName == '') {
                    packageElement.IsElementDetail = false;
                } else {
                    packageElement.IsElementDetail = true;
                }
                if (packageElement.IncomingMessages != null && packageElement.IncomingMessages.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.IncomingMessages;
                    } else {
                        packageElement.Childs.push(...packageElement.IncomingMessages)
                    }
                }
                if (packageElement.OutgoingMessages != null && packageElement.OutgoingMessages.length > 0) {
                    if (packageElement.Childs == null) {
                        packageElement.Childs = packageElement.OutgoingMessages;
                    } else {
                        packageElement.Childs.push(...packageElement.OutgoingMessages)
                    }
                }
                result.push(packageElement);
            } else if (element.Type == UMLType.Message) {
                let packageElement: UMLMessage = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.IsElementDetail = false;
                packageElement.sType = 'Message';
                
                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '');

                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                if ((packageElement.StereoType == null || packageElement.StereoType == '')
                    && (packageElement.MessgaeKind == null || packageElement.MessgaeKind == '')
                    && (packageElement.MessgaeSort == null || packageElement.MessgaeSort == '')
                ) {
                    packageElement.IsElementDetail = false;
                } else {
                    packageElement.IsElementDetail = true;
                }
                //packageElement.IsOtherParty=true;
                result.push(packageElement);
            } else if (element.Type == UMLType.CombinedFragment) {
                let packageElement: UMLCombinedFragment = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.sType = GetElementTypeName(packageElement.Type);
                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '') + ' : ' + packageElement.InteractionOperator;
                packageElement.IsElementDetail = false;
                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                result.push(packageElement);
            }
            else if (element.Type == UMLType.InteractionOperand) {
                let packageElement: UMLInteractionOperand = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.sType = GetElementTypeName(packageElement.Type);
                packageElement.TreeName = packageElement.sType + (packageElement.Expression != null ? ': ' + packageElement.Expression + '' : '');
                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                packageElement.IsElementDetail = false;
                result.push(packageElement);

            }
            else if (element.Type == UMLType.CompositeState) {
                let packageElement: UMLElementRefined = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.sType = GetElementTypeName(packageElement.Type);
                packageElement.IsElementDetail = false;
                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '');
                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                result.push(packageElement);

            }
            else if (element.Type == UMLType.PseudoState || element.Type == UMLType.SimpleState
                || element.Type == UMLType.FinalState || element.Type == UMLType.ActionState) {
                let packageElement: UMLState = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                if (packageElement.Kind != null || packageElement.Entry != null || packageElement.Do != null || packageElement.Exit) {
                    packageElement.IsElementDetail = true;
                } else {
                    packageElement.IsElementDetail;
                }
                packageElement.sType = GetElementTypeName(packageElement.Type);

                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '');
                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                result.push(packageElement);

            }
            else if (element.Type == UMLType.ActivityPartition ||element.Type == UMLType.InterruptibleActivityRegion) {
                let packageElement: UMLElementRefined = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.IsElementDetail = false;

                packageElement.sType = GetElementTypeName(packageElement.Type);

                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '');
                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                result.push(packageElement);

            } else if (element.Type == UMLType.OutputPin || element.Type == UMLType.InputPin
                || element.Type == UMLType.ExceptionInput
                ||element.Type == UMLType.Activity ) {
                let packageElement: UMLElementRefined = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.IsElementDetail = false;

                packageElement.sType = GetElementTypeName(packageElement.Type);

                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '');
                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                result.push(packageElement);

            } else if (element.Type == UMLType.Action || element.Type == UMLType.InitalNode
                || element.Type == UMLType.DecisionNode || element.Type == UMLType.ActivityFinalNode
                || element.Type == UMLType.ForkNode || element.Type == UMLType.FlowFinalNode
                || element.Type == UMLType.DataStoreNode || element.Type == UMLType.CentralBufferNode
                || element.Type == UMLType.ExceptionHandler || element.Type == UMLType.AcceptEventAction) {
                let packageElement: UMLState = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.IsElementDetail = false;

                packageElement.sType = GetElementTypeName(packageElement.Type);

                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '');
                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                result.push(packageElement);

            }
            else if (element.Type == UMLType.Transition || element.Type == UMLType.ControlFlow
                || element.Type == UMLType.ObjectFlow|| element.Type == UMLType.InterruptingEdge) {
                let packageElement: UMLTransition = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.sType = GetElementTypeName(packageElement.Type);
                packageElement.sSourceType = GetElementTypeName(packageElement.SourceType);
                packageElement.sTargetType = GetElementTypeName(packageElement.TargetType);
                packageElement.IsOtherParty=false;
                let otherParty: string = "";
                let otherPartyType: string = "";
                if (packageElement.SourceRef == packageElement.Parent.XmiId) {
                    otherParty = packageElement.TargetName;
                    otherPartyType=packageElement.sTargetType;
                } else {
                    otherParty = packageElement.SourceName;
                    otherPartyType=packageElement.sSourceType;

                }

                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '') + ' : ' + (otherParty!=null?otherParty:'')
                +(otherParty!==''?' ( '+otherPartyType+' )':'');
                if(otherPartyType!=""){
                    packageElement.IsOtherParty=true;
                }
                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                if (packageElement.Trigger != null || packageElement.Guard != null) {
                    packageElement.IsElementDetail = true;
                } else {
                    packageElement.IsElementDetail = false;
                }
                result.push(packageElement);

            }
            else if (element.Type == UMLType.Port) {
                let packageElement: UMLPort = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.sType = GetElementTypeName(packageElement.Type);

                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '');
                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                packageElement.IsElementDetail = false;

                result.push(packageElement);

            }
            else if (element.Type == UMLType.Property) {
                let packageElement: UMLProperty = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.sType = GetElementTypeName(packageElement.Type);
                packageElement.sPropertyType = GetElementTypeName(packageElement.PropertyType);

                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '') + (packageElement.PropertyName != null ? ' : ' + packageElement.PropertyName + '' : '');
                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }

                packageElement.IsElementDetail = false;

                result.push(packageElement);

            } else if (element.Type == UMLType.Provided) {
                let packageElement: UMLProvided = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.sType = GetElementTypeName(packageElement.Type);
                if (packageElement.ProvidedName != null) {
                    packageElement.sProvidedType = GetElementTypeName(packageElement.ProvidedType);
                }


                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '') + (packageElement.ProvidedName != null ? ' : ' + packageElement.ProvidedName + '' : '');
                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }

                packageElement.IsElementDetail = false;

                result.push(packageElement);

            }
            else if (element.Type == UMLType.Required) {
                let packageElement: UMLRequired = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.sType = GetElementTypeName(packageElement.Type);
                if (packageElement.RequiredName != null) {
                    packageElement.sRequiredType = GetElementTypeName(packageElement.RequiredType);
                }

                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '') + (packageElement.RequiredName != null ? ' : ' + packageElement.RequiredName + '' : '');
                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }

                packageElement.IsElementDetail = false;

                result.push(packageElement);

            }
            else if (element.Type == UMLType.Connector) {
                let packageElement: UMLConnector = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.sType = GetElementTypeName(packageElement.Type);
                if (packageElement.Element1Name != null) {
                    packageElement.sElement1Type = GetElementTypeName(packageElement.Element1Type);
                }

                if (packageElement.Element2Name != null) {
                    packageElement.sElement2Type = GetElementTypeName(packageElement.Element2Type);
                }
                let otherParty: string = "";
                if (packageElement.Parent.XmiId == packageElement.Element1Ref) {
                    otherParty = packageElement.Element2Name;
                } else if (packageElement.Parent.XmiId == packageElement.Element2Ref) {
                    otherParty = packageElement.Element1Name;
                }
                packageElement.IsOtherParty=true;
                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '') + (otherParty != "" ? ' : ' + otherParty + '' : '')
                    + (packageElement.Kind != null ? " <<" + packageElement.Kind + ">>" : "");
                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                packageElement.IsElementDetail = false;



                result.push(packageElement);

            }
            else if (element.Type == UMLType.InformationFlow) {
                let packageElement: UMLInformationFlow = element;
                if (parent != null) {
                    packageElement.Parent = parent;
                }
                packageElement.sType = GetElementTypeName(packageElement.Type);
                if (packageElement.SourceName != null) {
                    packageElement.sSourceType = GetElementTypeName(packageElement.SourceType);
                }

                if (packageElement.TargetName != null) {
                    packageElement.sTargetType = GetElementTypeName(packageElement.TargetType);
                }
                let otherParty: string = "";
                if (packageElement.Parent.XmiId == packageElement.SourceRef) {
                    otherParty = packageElement.TargetName;
                } else if (packageElement.Parent.XmiId == packageElement.TargetRef) {
                    otherParty = packageElement.SourceName;
                }
                packageElement.IsOtherParty=true;

                packageElement.TreeName = packageElement.sType + (packageElement.Name != null ? '(' + packageElement.Name + ')' : '') + (otherParty != "" ? ' : ' + otherParty + '' : '');
                if (element.Childs != null) {
                    packageElement.Childs = convertUMLEmlementsRefined(element.Childs, sData, element);
                }
                packageElement.IsElementDetail = false;



                result.push(packageElement);

            }
    });
    return result;
}
function GetElementTypeName(elementType: number): string {
    if (elementType == UMLType.Class) {
        return "Class";
    }
    if (elementType == UMLType.Actor) {
        return "Actor";
    }
    if (elementType == UMLType.InnerClass) {
        return "Inner Class";
    }
    if (elementType == UMLType.InnerInterface) {
        return "Inner Interface";
    }
    if (elementType == UMLType.Interface) {
        return "Interface";
    }
    if (elementType == UMLType.Object) {
        return "Object";
    }
    if (elementType == UMLType.Package) {
        return "Package";
    }
    if (elementType == UMLType.UseCase) {
        return "Use Case";
    }

    if (elementType == UMLType.Message) {
        return "Message";
    }

    if (elementType == UMLType.LifeLine) {
        return "Life Line";
    }
    if (elementType == UMLType.Association) {
        return "Association";
    }

    if (elementType == UMLType.Realization) {
        return "Realization";
    }

    if (elementType == UMLType.Generalization) {
        return "Generalization";
    }

    if (elementType == UMLType.Dependency) {
        return "Dependency";
    }

    if (elementType == UMLType.Usage) {
        return "Usage";
    }

    if (elementType == UMLType.PackageImport) {
        return "Package Import";
    }

    if (elementType == UMLType.PackageMerge) {
        return "Package Merge";
    }

    if (elementType == UMLType.CombinedFragment) {
        return "Combined Fragment";
    }

    if (elementType == UMLType.InteractionOperand) {
        return "Interaction Operand";
    }
    if (elementType == UMLType.Component) {
        return "Component";
    }

    if (elementType == UMLType.SimpleState) {
        return "Simple State";
    }

    if (elementType == UMLType.PseudoState) {
        return "Pseudo State";
    }
    if (elementType == UMLType.FinalState) {
        return "Finale State";
    }

    if (elementType == UMLType.Transition) {
        return "Transition";
    }
    if (elementType == UMLType.CompositeState) {
        return "Composite State";
    }
    if (elementType == UMLType.Port) {
        return "Port";
    }
    if (elementType == UMLType.Property) {
        return "Property";
    }

    if (elementType == UMLType.Provided) {
        return "Provided";
    }

    if (elementType == UMLType.Required) {
        return "Required";
    }
    if (elementType == UMLType.Connector) {
        return "Connector";
    }
    if (elementType == UMLType.InformationFlow) {
        return "Information Flow";
    }
    if (elementType == UMLType.CollaborationUse) {
        return "Collaboration Use";
    }

    if (elementType == UMLType.ActionState) {
        return "Action State";
    }
    if (elementType == UMLType.ControlFlow) {
        return "Control Flow";
    }
    if (elementType == UMLType.DecisionNode) {
        return "Decision Node";
    }
    if (elementType == UMLType.ActivityFinalNode) {
        return "Final Node";
    }
    if (elementType == UMLType.InitalNode) {
        return "Initial Node";
    }
    if (elementType == UMLType.Action) {
        return "Action";
    } if (elementType == UMLType.ActivityPartition) {
        return "Activity Partition";
    } if (elementType == UMLType.ForkNode) {
        return "Fork Node";
    }if (elementType == UMLType.FlowFinalNode) {
        return "Flow Final Node";
    }
    if (elementType == UMLType.ObjectFlow) {
        return "Object Flow";
    }
    if (elementType == UMLType.OutputPin) {
        return "Output Pin";
    }
    if (elementType == UMLType.InputPin) {
        return "Input Pin";
    }
    if (elementType == UMLType.Activity) {
        return "Activity";
    }
    if (elementType == UMLType.DataStoreNode) {
        return "Data Store Node";
    }
    if (elementType == UMLType.CentralBufferNode) {
        return "Central Buffer Node";
    }
    if (elementType == UMLType.ExceptionHandler) {
        return "Exception Handler";
    }
    if (elementType == UMLType.ExceptionInput) {
        return "Exception Input";
    }

    if (elementType == UMLType.InterruptibleActivityRegion) {
        return "Interruptible Activity Region";
    }
    if (elementType == UMLType.AcceptEventAction) {
        return "Accept Event Action";
    }
    if (elementType == UMLType.InterruptingEdge) {
        return "Interrupting Edge";
    }
}
function GetClassAssociations(parentElement:UMLElementRefined, data: any[], XmiId: string): UMLAssociation[] {
    let result: UMLAssociation[] = [];

    data.forEach(element => {
        if (element.Type == UMLType.Association) {
            let associationElement: UMLAssociation = element;
            associationElement.sType = GetElementTypeName(associationElement.Type);

            if (associationElement.Element1Ref == XmiId || associationElement.Element2Ref == XmiId) {
                if (associationElement.Element1Ref == XmiId) {
                    associationElement.OtherPartyRef=associationElement.Element2Ref;
                    associationElement.Name = associationElement.Element2Name;
                } else {
                    associationElement.Name = associationElement.Element1Name;
                    associationElement.OtherPartyRef=associationElement.Element1Ref;

                }
                associationElement.IsOtherParty=true;
                associationElement.Parent=parentElement;
                associationElement.TreeName = associationElement.sType + (associationElement.StereoType != null ? ' (' + associationElement.StereoType + ') ' : '') + ' : ' + associationElement.Name
                associationElement.sElement1Type = GetElementTypeName(associationElement.Element1Type);
                associationElement.sElement2Type = GetElementTypeName(associationElement.Element2Type);
                if ((associationElement.StereoType == null || associationElement.StereoType == "")
                    && (associationElement.Element1Aggregation == null || associationElement.Element1Aggregation == "" || associationElement.Element1Aggregation == "none")
                    && (associationElement.Element2Aggregation == null || associationElement.Element2Aggregation == "" || associationElement.Element2Aggregation == "none")
                    && (associationElement.Element1Multiplicity == null || associationElement.Element1Multiplicity == "")
                    && (associationElement.Element2Multiplicity == null || associationElement.Element2Multiplicity == "")
                ) {
                    associationElement.IsElementDetail = false;
                } else {
                    associationElement.IsElementDetail = true;
                }
                result.push(Object.assign({}, associationElement));
            }
        }
        if (element.Childs != null) {
            let childResult: UMLAssociation[] = GetClassAssociations(element, element.Childs, XmiId);
            if (childResult.length > 0) {
                result.push(...childResult);
            }
        }
    });
    return result;
}
function GetImportedPackages(parentElement:UMLElementRefined,data: any[]): UMLPackageImport[] {
    let result: UMLPackageImport[] = [];

    data.forEach(element => {
        if (element.Type == UMLType.PackageImport) {
            let associationElement: UMLPackageImport = element;
            associationElement.sType = GetElementTypeName(associationElement.Type);
            associationElement.TreeName = associationElement.sType + ' : ' + associationElement.ImportedPackageName
            associationElement.IsElementDetail = false;
            associationElement.IsOtherParty = true;
            associationElement.OtherPartyRef = associationElement.ImportedPackageRef;
            associationElement.Parent=parentElement;
            result.push(associationElement);
        }

    });
    return result;
}
function GetMergedPackages(parentElement:UMLElementRefined,data: any[]): UMLPackageMerge[] {
    let result: UMLPackageMerge[] = [];

    data.forEach(element => {
        if (element.Type == UMLType.PackageMerge) {
            let associationElement: UMLPackageMerge = element;
            associationElement.sType = GetElementTypeName(associationElement.Type);
            associationElement.TreeName = associationElement.sType + ' : ' + associationElement.MergedPackageName
            associationElement.IsElementDetail = false;
            associationElement.Parent=parentElement;
            associationElement.IsOtherParty=true;
            associationElement.OtherPartyRef = associationElement.MergedPackageRef;

            result.push(associationElement);
        }

    });
    return result;
}

function GetPackageUsages(parentElement:UMLElementRefined,data: any[], XmiId: string): UMLUsage[] {
    let result: UMLUsage[] = [];

    data.forEach(element => {
        if (element.Type == UMLType.Usage) {
            let associationElement: UMLUsage = element;
            associationElement.sType = GetElementTypeName(associationElement.Type);
            if (associationElement.ClientRef == XmiId || associationElement.SupplierRef == XmiId) {
                associationElement.sClientType = GetElementTypeName(associationElement.ClientType);
                associationElement.sSupplierType = GetElementTypeName(associationElement.SupplierType);

                if (associationElement.ClientRef == XmiId) {
                    associationElement.Name = associationElement.SupplierName;
                } else {
                    associationElement.Name = associationElement.ClientName;
                }
                associationElement.TreeName = associationElement.sType + (associationElement.StereoType != null ? ' (' + associationElement.StereoType + ') ' : '') + ' : ' + associationElement.Name
                associationElement.IsElementDetail = false;
                associationElement.IsOtherParty = true;

                associationElement.Parent=parentElement;
                result.push(Object.assign({}, associationElement));
            }
        }
        if (element.Childs != null) {
            let childResult: UMLUsage[] = GetPackageUsages(element,element.Childs, XmiId);
            if (childResult.length > 0) {
                result.push(...childResult);
            }
        }
    });
    return result;
}
function GetClassDependencies(parentElement:UMLElementRefined,data: any[], XmiId: string): UMLDependency[] {
    let result: UMLDependency[] = [];

    data.forEach(element => {
        if (element.Type == UMLType.Dependency) {
            
            let associationElement: UMLDependency = element;
            associationElement.sType = GetElementTypeName(associationElement.Type);
            
            if (associationElement.ClientRef == XmiId || associationElement.SupplierRef == XmiId) {
                associationElement.sClientType = GetElementTypeName(associationElement.ClientType);
                associationElement.sSupplierType = GetElementTypeName(associationElement.SupplierType);
                let dependencyType='';
                if (associationElement.ClientRef == XmiId) {
                    associationElement.Name = associationElement.SupplierName;
                    dependencyType='Supplier';
                } else {
                    associationElement.Name = associationElement.ClientName;
                    dependencyType='Client';

                }
                associationElement.TreeName = associationElement.sType + (associationElement.StereoType != null ? ' <' + associationElement.StereoType + '> ' : '') + ' : ' + associationElement.Name +' ( '+dependencyType+' )'
                associationElement.IsElementDetail = false;
                associationElement.IsOtherParty = true;
                associationElement.Parent=parentElement;
                result.push(Object.assign({}, associationElement));
            }
        }
        if (element.Childs != null) {
            let childResult: UMLDependency[] = GetClassDependencies(element,element.Childs, XmiId);
            if (childResult.length > 0) {
                result.push(...childResult);
            }
        }
    });
    return result;
}
function GetClassGeneralizations(parentElement:UMLElementRefined,data: any[], XmiId: string): UMLGeneralization[] {
    let result: UMLGeneralization[] = [];

    data.forEach(element => {
        if (element.Type == UMLType.Generalization) {
            let sElement=JSON.stringify( element);
            let associationElement: UMLGeneralization = JSON.parse( sElement);
            associationElement.sType = GetElementTypeName(associationElement.Type);
            if (associationElement.ChildRef == XmiId || associationElement.ParentRef == XmiId) {
                associationElement.sChildType = GetElementTypeName(associationElement.ChildType);
                associationElement.sParentType = GetElementTypeName(associationElement.ParentType);
                let otherPartyType: string = "";
                if (associationElement.ChildRef == XmiId) {
                    associationElement.Name = associationElement.ParentName;
                    associationElement.OtherPartyRef=associationElement.ParentRef;
                    otherPartyType = "Parent"
                } else {
                    associationElement.Name = associationElement.ChildName;
                    associationElement.OtherPartyRef=associationElement.ChildRef;

                    otherPartyType = "Child"
                }
                associationElement.Parent=parentElement;
                
                associationElement.TreeName = associationElement.sType + ' : ( ' + otherPartyType + ' ) ' + associationElement.Name;

                associationElement.IsElementDetail = false;
                associationElement.IsOtherParty = true;
                

                result.push(Object.assign({}, associationElement));
            }
        }
        if (element.Childs != null) {
            let childResult: UMLGeneralization[] = GetClassGeneralizations(element,element.Childs, XmiId);
            if (childResult.length > 0) {
                result.push(...childResult);
            }
        }
    });
    return result;
}

function GetClassRealizations(parentElement:UMLElementRefined,data: any[], XmiId: string): UMLRelization[] {
    let result: UMLRelization[] = [];

    data.forEach(element => {
        if (element.Type == UMLType.Realization) {
            let associationElement: UMLRelization = element;
            associationElement.sType = GetElementTypeName(associationElement.Type);
            if (associationElement.ClientRef == XmiId || associationElement.SupplierRef == XmiId) {
                associationElement.sClientType = GetElementTypeName(associationElement.ClientType);
                associationElement.sSupplierType = GetElementTypeName(associationElement.SupplierType);
                let dependencyType='';

                if (associationElement.ClientRef == XmiId) {
                    associationElement.Name = associationElement.SupplierName;
                    dependencyType='Supplier';
                } else {
                    associationElement.Name = associationElement.ClientName;
                    dependencyType='Client';

                }
                associationElement.TreeName = associationElement.sType + (associationElement.StereoType != null ? ' (' + associationElement.StereoType + ') ' : '') + ' : ' + associationElement.Name +' ( '+dependencyType+' )'
                associationElement.IsElementDetail = false;
                associationElement.IsOtherParty = true;

                associationElement.Parent=parentElement;
                result.push(Object.assign({}, associationElement));
            }
        }
        if (element.Childs != null) {
            let childResult: UMLRelization[] = GetClassRealizations(element,element.Childs, XmiId);
            if (childResult.length > 0) {
                result.push(...childResult);
            }
        }
    });
    return result;
}
function GetMessagesTypes(parentElement:UMLElementRefined,IncomingMessages: UMLMessageRefined[], isIncoming: boolean): UMLMessageRefined[] {
    IncomingMessages.forEach(messageElement => {
        let otherLifeLine: string = '';
        let otherLifeLineType: string = '';
        
        //messageElement.Name = 'Message "'++'" (' + (isIncoming ? 'Incoming' : 'Outgoing') + ') : ' + otherLifeLine;
        messageElement.sElementFromType = GetElementTypeName(messageElement.ElementFromType);
        messageElement.sElementToType = GetElementTypeName(messageElement.ElementToType);
        if (isIncoming) {
            otherLifeLine = messageElement.ElementFromName;
            otherLifeLineType=messageElement.sElementFromType;
        } else {
            otherLifeLine = messageElement.ElementToName;
            otherLifeLineType=messageElement.sElementToType;
        }
        if(otherLifeLineType!=''){
            messageElement.IsOtherParty=true;
        }
        messageElement.TreeName = 'Message ' + (messageElement.Name != null ? '" ' + messageElement.Name + ' "' : '') + ': ' + (otherLifeLine !=null?otherLifeLine+' ( '+otherLifeLineType+' )':'') ;
        messageElement.Direction=(isIncoming ? 'Incoming' : 'Outgoing');
        if ((messageElement.StereoType == null || messageElement.StereoType == '')
            && (messageElement.MessgaeKind == null || messageElement.MessgaeKind == '')
            && (messageElement.MessgaeSort == null || messageElement.MessgaeSort == '')
        ) {
            messageElement.IsElementDetail = false;
        } else {
            messageElement.IsElementDetail = true;
        }
        messageElement.Parent=parentElement;
    })
    return IncomingMessages;
}

