
$(document).ready(function () {
    $('#btnCancel').on('click', function () {
        if (typeof dialog !== 'undefined' && dialog)
            dialog.modal('hide');
    });
    $('#LMSCourseId').attr("data-live-search", "true");
    $('#LMSCourseId').selectpicker();
    $('#LMSBusinessUnitId').attr("data-live-search", "true");
    $('#LMSBusinessUnitId').selectpicker();
    $('#ClassDate').datepicker({ autoclose: true, todayHighlight: true, todayBtn: "linked" });   
    $('#ClassDate').datepicker("update", new Date());
});
$('#btnPrint').on('click', function () {
    var rows = 12;
    $('#ptbody').html("")
    for (var i = 0; i < rows; i++) {
        $('#ptbody').append('<tr height=50px><td width="400px"></td><td></td><td></td></tr>');
    }
    printDiv('printdiv');
});

function printDiv(div) {

    //adding here 12/1/2016 - in this function

    var divToPrint = document.getElementById(div);
    var courseName = $("#LMSCourseId option:selected").text();
    if (!courseName || courseName === 'Please Select')
        courseName = '_________________________________________________';
    //added here
    var businessUnit = $("#LMSBusinessUnitId option:selected").text();
    if (!businessUnit || businessUnit === 'Please Select')
        businessUnit = 'CANTON';

    var classDate = $("#ClassDate").data('datepicker').getFormattedDate('mm-dd-yyyy');
    if (!classDate)
        classDate = '________________________';

    $(divToPrint).find('#lblCls').html('Class Title: ' + courseName);
   
    // Changed  Business Unit to  Location 11/21/2016
    //$(divToPrint).find('#lblBU').html('Business Unit : ' + businessUnit);
    //changing 12/1/2016
    // $(divToPrint).find('#lblBU').html('Location : ' + businessUnit);  

    var business_Unit = businessUnit.toUpperCase();
    $(divToPrint).find('#lblBU').html(business_Unit);

    $(divToPrint).find('#lblDate').html('Date: ' + classDate);
    var newWin = window.open('', 'Print-Window');
    newWin.document.open();
    newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
    newWin.document.close();

    //setTimeout(function () { newWin.close(); }, 10);
}