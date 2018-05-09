$(document).ready(function () {
   // debugger;
    $("#Choose").click(function () {
        $("#file").click();
        $("#filePathChoose").val($("#file").val());
    });

    $("#Reset").click(function () {
        $("#filePathChoose").val("");
    });

    var filePath = "";

    if ($("#file").val() != null) {
        filePath = $.trim($("#file").val());
    }

});

//$("#Choose").click(function () {
//    //debugger;
//    $("#filePath1").click();
//    $("#filePathChoose").val($("#filePath1").val());
//    $("#file").val($("#filePath1").val());
//});

//$("#Reset").click(function () {
//    $("#filePathChoose").val("");
//});

//$("#Import").click(function () {

//   // debugger;
//    var filePath = "";
//    if ($("#filePath1").val() != null) {
//        filePath = $.trim($("#filePath1").val());
//        // filePath = GetRootDirectory() + '\\USCTRD01\\RDServer\\RD\\Database\\Export\\ForNewDataBase\\TestImport.csv';
//    }

//    $('#lblFileName').text("Importing file: " + filePath);
//    var errorMsg;
//    //if (!filePath.includes(selectedTestType)) {
//    //    errorMsg = "Import Error: Please check the correct file is imported";
//    //}
//   // debugger;
//    var options = {
//        filePath: filePath,
//        //   Message1: filePath
//    };

//    $.ajax({
//        type: 'post',
//        url: GetRootDirectory() + '/Employee/SaveImportEmployees',
//        data: options
//    })
// .done(function (data) {
//     if (data && data.isSuccess) {
//         $('#lblImported').text(data.message + ' ' + data.LMSEmployeeId);         
//     }
//     else {
//        // dialog.modal('hide');
//         bootbox.alert(data.message);
//     }
// })
// .fail(function (x, y, x) {
//     alert("error");
// });

//    //$.ajax({

//    //    type: 'post',
//    // //   url: Api + 'api/ImportData',
//    //    headers: {
//    //        Token: GetToken()
//    //    },
//    //    data: options
//    //})
//    //.done(function (data) {
//    //    if (data) {
//    //        $('#lblImported').text("Imported: " + selectedTestType + "data");
//    //    }
//    //    else {
//    //        errorMsg = "Import Error: Please check if the file is open";
//    //        $('#lblImported').text(errorMsg);
//    //    }
//    //});
//});

