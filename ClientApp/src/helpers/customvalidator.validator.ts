import { FormGroup, AbstractControl, ValidatorFn } from "@angular/forms";

export function ValidateExtension(
    control: AbstractControl,
) {
  
    
    if(control.value!=null&&control.value!=""){
        let fileName:string=control.value.name;
        let extension:string=fileName.substring(fileName.lastIndexOf('.'),fileName.length).toLowerCase();
        if (extension!=".xml" && extension!=".xmi" ) {
            return{xmlExtension:true}
        } else {
            if(control.errors && control.errors.xmlExtension!=null){
                return null;
            }
            
        }
    }else{
        return null;
    }
    
  
}
