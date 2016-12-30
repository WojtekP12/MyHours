var isSubstituteCheckBox = document.getElementById('IsSubstituteCheckBox');

isSubstituteCheckBox.onchange = function()
{

    if (this.childNodes[1].checked) {
        $('#SubDescr').removeAttr('disabled');
    }
    else {

        $('#SubDescr').attr('disabled', 'disabled');
    }

};