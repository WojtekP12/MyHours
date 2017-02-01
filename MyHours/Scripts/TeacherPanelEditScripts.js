var isSubstituteCheckBox = document.getElementById('IsSubstituteCheckBox');

isSubstituteCheckBox.onchange = function()
{

    if (this.childNodes[1].checked) {
        $('#SubDescr').removeAttr('disabled');
    }
    else {

        $('#SubDescr').attr('disabled', 'disabled');
    }

    if (this.childNodes[1].checked) {
        
        $('#SubPerson').attr("disabled", "disabled");
    }
    else {

        $('#SubPerson').removeAttr("disabled");
    }

};