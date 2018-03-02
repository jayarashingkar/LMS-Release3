// define the columns in your datasource
var columns = [
    //{
    //    label: 'ID',
    //    property: 'LMSEmployeeId',
    //    sortable: true,
    //    visible: false
    //},    
     {
         label: 'No',
         property: 'LMSEmployeeId',
         sortable: true,
         width: '50px'
     },
       {
           label: 'Employee No',
           property: 'EmployeeNo',
           sortable: true,
           width: '50px'
       },
    {
        label: 'First Name',
        property: 'FirstName',
        sortable: true
    },
    {
        label: 'Last Name',
        property: 'LastName',
        sortable: true
    },
    {
        label: 'Location',
        property: 'LMSBusinessUnit',
        sortable: true
    },
    //{
    //    label: 'Middle Name',
    //    property: 'MiddleName',
    //    sortable: true
    //},

    //{
    //    label: 'HireDate',
    //    property: 'HireDate',
    //    sortable: true
    //},
    {
        label: 'Status',
        property: 'StatusCode',
        sortable: true,
        width: '50px'

    },
    //{
    //    label: 'Shift',
    //    property: 'Shift',
    //    sortable: true
    //},
    {
        label: 'Courses',
        property: 'Courses',
        width: '50px'
    },
    {
        label: 'History',
        property: 'Training',
        width: '50px'
    },
    {
        label: 'Edit',
        property: 'Edit',
        width: '50px',

    },
    {
        label: 'Delete',
        property: 'Delete',
        width: '50px',
    }
];

function customColumnRenderer(helpers, callback) {
    // determine what column is being rendered
    var column = helpers.columnAttr;

    // get all the data for the entire row
    var rowData = helpers.rowData;
    var customMarkup = '';

    // only override the output for specific columns.
    // will default to output the text value of the row item
    switch (column) {
        case 'LMSEmployeeId':
            // let's combine name and description into a single column
            customMarkup = '<div style="font-size:12px;">' + rowData.LMSEmployeeId + '</div>';
            break;
        case 'HireDate':
            // let's combine name and description into a single column
            customMarkup = rowData.StrHireDate;
            break;
        case 'LMSBusinessUnit':
            // let's combine name and description into a single column
            customMarkup = rowData.LMSBusinessUnit.BusinessUnitName;
            break;
        case 'StatusCode':
            // let's combine name and description into a single column
            if (rowData.StatusCode === 'L')
                customMarkup = 'On Leave';
            else if (rowData.StatusCode === 'A')
                customMarkup = 'Active';
                // Removed Retired 11/21/2016
                //else if (rowData.StatusCode === 'R')
                //    customMarkup = 'Retired';
            else if (rowData.StatusCode === 'T')
                customMarkup = 'Terminated';
            else
                customMarkup = rowData.StatusCode;
            break;
        case 'Courses':
            // let's combine name and description into a single column
            customMarkup = '<button onclick="GridCourseClicked(' + rowData.LMSEmployeeId + ')" id="gridCourse" name="gridCourse" class="btn btn-warning btn-sm center-block"><i class="fa fa-book"></i></button>';
            break;
        case 'Training':
            // let's combine name and description into a single column
            customMarkup = '<button onclick="GridTrainingClicked(' + rowData.LMSEmployeeId + ')" id="gridTraining" name="gridTraining" class="btn btn-success btn-sm center-block"><i class="fa fa-history"></i></button>';
            break;
        case 'Edit':
            // let's combine name and description into a single column
            customMarkup = '<button onclick="GridEditClicked(' + rowData.LMSEmployeeId + ')" id="gridEdit" name="gridEdit" class="btn btn-info btn-sm center-block"><i class="fa fa-pencil"></i></button>';
            break;
        case 'Delete':
            // let's combine name and description into a single column
            customMarkup = '<button onclick="GridDeleteClicked(' + rowData.LMSEmployeeId + ')" id="gridDelete" name="gridDelete" class="btn btn-danger btn-sm center-block"><i class="fa fa-trash"></i></button>';
            break;
        default:
            // otherwise, just use the existing text value
            customMarkup = helpers.item.text();
            break;
    }

    helpers.item.html(customMarkup);

    callback();
}

function customRowRenderer(helpers, callback) {
    // let's get the id and add it to the "tr" DOM element
    var item = helpers.item;
    item.attr('id', 'row' + helpers.rowData.LMSEmployeeId);

    callback();
}

// this example uses an API to fetch its datasource.
// the API handles filtering, sorting, searching, etc.
function customDataSource(options, callback) {
    // set options
    var pageIndex = options.pageIndex;
    var pageSize = options.pageSize;
    var search = '';
    if ($('#searchFirstName').val())
        search += ';' + 'FirstName:' + $('#searchFirstName').val();
    if ($('#searchLastName').val())
        search += ';' + 'LastName:' + $('#searchLastName').val();
    if ($('#searchEmployeeNo').val())
        search += ';' + 'EmployeeNo:' + $('#searchEmployeeNo').val();
    if ($('#searchStatusCode').val() && $('#searchStatusCode').val() !== '-1')
        search += ';' + 'StatusCode:' + $('#searchStatusCode').val();
    if ($('#searchLMSBusinessUnitId').val() && $('#searchLMSBusinessUnitId').val() !== '-1')
        search += ';' + 'LMSBusinessUnitId:' + $('#searchLMSBusinessUnitId').val();

    var options = {
        pageIndex: pageIndex,
        pageSize: pageSize,
        sortDirection: options.sortDirection,
        sortBy: options.sortProperty,
        filterBy: options.filter.value || '',
        searchBy: search || ''
    };
    // call API, posting options
    $.ajax({
        type: 'post',
        url: GetRootDirectory() + '/Grid/GetEmployees',
        data: options
    })
    .done(function (data) {
        var items = data.items;
        var totalItems = data.total;
        var totalPages = Math.ceil(totalItems / pageSize);
        var startIndex = (pageIndex * pageSize) + 1;
        var endIndex = (startIndex + pageSize) - 1;

        if (endIndex > items.length) {
            endIndex = items.length;
        }

        // configure datasource
        var dataSource = {
            page: pageIndex,
            pages: totalPages,
            count: totalItems,
            start: startIndex,
            end: endIndex,
            columns: columns,
            items: items
        };

        // invoke callback to render repeater
        callback(dataSource);
    });
}

var dialog = null;
$('#btnAdd').on('click', function (event, param) {
    var div = $('#popup').html();
    dialog = bootbox.dialog({
        message: div,
        size: 'large',
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Cancel',
                className: 'btn-danger'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Save',
                className: 'btn-success',
                // comment here 
                //for confirm option - to save employee
                callback: function (result) {
                    return ConfirmCallback(result, 'confirm');
                }
            },
            next: {
                label: '<i class="fa fa-step-forward"></i> Next',
                className: 'btn-warning',
                callback: function (result) {
                    // comment here 
                    // for confirm option to save employee and go to Employee Course
                    return ConfirmCallback(result, 'next');
                }
            }
        },
        onEscape: function () {
            this.modal('hide');
        }
    })
    if (document.getElementById("hdnPermission").value == "ReadOnly") {
        dialog.find('button[data-bb-handler=confirm]').attr('disabled', 'disabled');
    }
    dialog.find('#LMSDepartmentId').selectpicker();
    dialog.find('#LMSBusinessUnitId').selectpicker();
    dialog.find('#LMSJobTitleId').selectpicker();
    dialog.find('#Shift').selectpicker();
    dialog.find('#StatusCode').selectpicker();
    dialog.find('#HireDate').datepicker({ autoclose: true, todayHighlight: true, todayBtn: "linked" });
    if (typeof param !== 'undefined' && param) {
        dialog.find('#txtId').val(param.LMSEmployeeId);
        dialog.find('#FirstName').val(param.FirstName);
        dialog.find('#LastName').val(param.LastName);
        dialog.find('#EmployeeNo').val(param.EmployeeNo);
        dialog.find('#MiddleName').val(param.MiddleName);
        dialog.find('#HireDate').datepicker("update", param.StrHireDate);
        dialog.find('#LMSDepartmentId').selectpicker('val', param.LMSDepartmentId);
        dialog.find('#LMSBusinessUnitId').selectpicker('val', param.LMSBusinessUnitId);
        dialog.find('#LMSJobTitleId').selectpicker('val', param.LMSJobTitleId);
        dialog.find('#Shift').selectpicker('val', param.Shift);
        dialog.find('#StatusCode').selectpicker('val', param.StatusCode);
    }
    else {
        dialog.find('#HireDate').datepicker("update", new Date());
    }
});

$(document).ready(function () {
    $('#searchLMSBusinessUnitId').selectpicker();
    $('#btnCancel').on('click', function () {
        if (typeof dialog !== 'undefined' && dialog)
            dialog.modal('hide');
    });
});

function GridEditClicked(id) {
    var obj = { id: id };
    $.ajax({
        type: 'post',
        url: GetRootDirectory() + '/Employee/EditEmployee',
        data: obj
    })
    .done(function (data) {
        if (data && data.entity && data.entity.LMSEmployeeId > 0) {
            $("#btnAdd").trigger("click", [{
                LMSEmployeeId: id,
                FirstName: data.entity.FirstName,
                LastName: data.entity.LastName,
                MiddleName: data.entity.MiddleName,
                EmployeeNo: data.entity.EmployeeNo,
                HireDate: data.entity.HireDate,
                StrHireDate: data.entity.StrHireDate,
                Shift: data.entity.Shift,
                LMSDepartmentId: data.entity.LMSDepartmentId,
                LMSBusinessUnitId: data.entity.LMSBusinessUnitId,
                LMSJobTitleId: data.entity.LMSJobTitleId,
                StatusCode: data.entity.StatusCode,
            }]);
        }
    });
}

function GridDeleteClicked(id) {
    DeleteGridRow(id, GetRootDirectory() + '/Employee/DeleteEmployee', 'employeeRepeater');
}

$('#btnSearch').on('click', function () {
    $('#employeeRepeater').repeater('render');
});

$('#btnClear').on('click', function () {
    $('#searchFirstName').val('');
    $('#searchLastName').val('');
    $('#searchEmployeeNo').val('');
    $('#searchStatusCode').selectpicker('val', "-1")
    $('#searchLMSBusinessUnitId').selectpicker('val', "-1")
    $('#employeeRepeater').repeater('render');
});

function GridCourseClicked(id) {
    location.href = GetRootDirectory() + '/Employee/EmployeeCourseList/' + id;
}

function GridTrainingClicked(id) {
    location.href = GetRootDirectory() + '/Employee/CourseHistory/' + id;
}

// comment here 
//Function checks all tha validations and stores in model
function ConfirmCallback(result, btn) {
    var form = dialog.find('#entityform');
    debugger;
    form.bootstrapValidator({
        message: 'This value is not valid',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            FirstName: {
                validators: {
                    notEmpty: {
                        message: 'First Name is required.'
                    }
                }
            },
            LastName: {
                validators: {
                    notEmpty: {
                        message: 'Last Name is required.'
                    }
                }
            },
            EmployeeNo: {
                validators: {
                    notEmpty: {
                        message: 'EmployeeNo is required.'
                    }
                }
            },
            LMSDepartmentId: {
                validators: {
                    callback: {
                        message: 'Department is required.',
                        callback: function (value, validator, $field) {
                            /* Get the selected options */
                            var options = validator.getFieldElements('LMSDepartmentId').val();
                            return (options !== '-1');
                        }
                    }
                }
            },
            LMSJobTitleId: {
                validators: {
                    callback: {
                        message: 'Job Title is required.',
                        callback: function (value, validator, $field) {
                            /* Get the selected options */
                            var options = validator.getFieldElements('LMSJobTitleId').val();
                            return (options !== null && options !== '-1');
                        }
                    }
                }
            },
            StatusCode: {
                validators: {
                    callback: {
                        message: 'Status is required.',
                        callback: function (value, validator, $field) {
                            /* Get the selected options */
                            var options = validator.getFieldElements('StatusCode').val();
                            return (options !== '-1');
                        }
                    }
                }
            },
            Shift: {
                validators: {
                    callback: {
                        message: 'Shift is required.',
                        callback: function (value, validator, $field) {
                            /* Get the selected options */
                            var options = validator.getFieldElements('Shift').val();
                            return (options !== '-1');
                        }
                    }
                }
            },
            LMSBusinessUnitId: {
                validators: {
                    callback: {
                        // Changed  Business Unit to  Location 11/21/2016
                        //message: 'Business Unit is required.',
                        message: 'Location is required.',
                        callback: function (value, validator, $field) {
                            /* Get the selected options */
                            var options = validator.getFieldElements('LMSBusinessUnitId').val();
                            return (options !== '-1');
                        }
                    }
                }
            },
        }
    });
    var validator = form.data('bootstrapValidator');
    validator.validate();

    if (validator.isValid()) {
        var FirstName = dialog.find('#FirstName').val();
        var LastName = dialog.find('#LastName').val();
        var MiddleName = dialog.find('#MiddleName').val();
        var EmployeeNo = dialog.find('#EmployeeNo').val();
        var HireDate = dialog.find('#HireDate').datepicker();
        HireDate = dialog.find("#HireDate").data('datepicker').getFormattedDate('yyyy-mm-dd');
        var Shift = dialog.find('#Shift').val();
        var LMSDepartmentId = dialog.find('#LMSDepartmentId').val();
        var LMSBusinessUnitId = dialog.find('#LMSBusinessUnitId').val();
        var LMSJobTitleId = dialog.find('#LMSJobTitleId').val();
        var StatusCode = dialog.find('#StatusCode').val();
        debugger;
        var hidden = dialog.find('#txtId');
        var id = 0;
        if (hidden && hidden.val()) {
            if (!isNaN(hidden.val())) {
                id = hidden.val();
            }
        }
        debugger;
        var model = {
            LMSEmployeeId: id,
            FirstName: FirstName,
            LastName: LastName,
            MiddleName: MiddleName,
            EmployeeNo: EmployeeNo,
            HireDate: HireDate,
            StrHireDate: HireDate,
            LMSDepartmentId: LMSDepartmentId,
            LMSBusinessUnitId: LMSBusinessUnitId,
            LMSJobTitleId: LMSJobTitleId,
            StatusCode: StatusCode,
            Shift: Shift
        };
        $.ajax({
            type: 'post',
            url: GetRootDirectory() + '/Employee/SaveEmployee',
            data: model
        })
        .done(function (data) {
            if (data && data.isSuccess) {
                NextFunctionality(data.LMSEmployeeId, btn);
            }
            else {
                dialog.modal('hide');
                bootbox.alert(data.message);
            }
        })
        .fail(function (x, y, x) {
            alert("error");
        });
    }
    else
        return false;
}

// when next button is clicked - adds Initial Orietation courses and goes to Employee Course 
function NextFunctionality(LMSEmployeeId, btn) {
    if (btn === 'confirm') {
        //dialog.find('#txtId').val(LMSEmployeeId);
        dialog.modal('hide');
        $('#employeeRepeater').repeater('render');
    }
    else {


        $('#OrientLMSEmployeeId').val(LMSEmployeeId);
        var div = $('#orientpopup').html();
        dialog = bootbox.dialog({
            message: div,
            //size: 'large',
            buttons: {
                cancel: {
                    label: '<i class="fa fa-times"></i> Cancel'
                },
                confirm: {
                    label: '<i class="fa fa-check"></i> Confirm',
                    callback: function (result) {
                        var form = dialog.find('#orientform');
                        form.bootstrapValidator({
                            message: 'This value is not valid',
                            feedbackIcons: {
                                valid: 'glyphicon glyphicon-ok',
                                invalid: 'glyphicon glyphicon-remove',
                                validating: 'glyphicon glyphicon-refresh'
                            },
                            fields: {
                                //OrientInstructorName: {
                                //    validators: {
                                //        notEmpty: {
                                //            message: 'Instructor Name is required.'
                                //        }
                                //    }
                                //},
                                LMSCourseId: {
                                    validators: {
                                        callback: {
                                            message: 'Course is required.',
                                            callback: function (value, validator, $field) {
                                                /* Get the selected options */
                                                var options = validator.getFieldElements('LMSCourseId').val();
                                                return (options !== '-1');
                                            }
                                        }
                                    }
                                },
                            }
                        });
                        var validator = form.data('bootstrapValidator');
                        validator.validate();
                        if (validator.isValid()) {
                            var LMSEmployeeId = dialog.find('#OrientLMSEmployeeId').val();
                            var OrientInstructorName = dialog.find('#OrientInstructorName').val();
                            var OrientRemarks = dialog.find('#OrientRemarks').val();
                            var OrientEvaluation = dialog.find('#OrientEvaluation').val();
                            var CompletedDate = dialog.find('#orientCompletedDate').datepicker();
                            CompletedDate = dialog.find("#orientCompletedDate").data('datepicker').getFormattedDate('yyyy-mm-dd');
                            var model = {
                                LMSEmployeeId: LMSEmployeeId,
                                InstructorName: OrientInstructorName,
                                Remarks: OrientRemarks,
                                Evaluation: OrientEvaluation,
                                CompletedDate: CompletedDate
                            };
                            $.ajax({
                                type: 'post',
                                url: GetRootDirectory() + '/Employee/AddInitialOrientation',
                                data: model
                            })
                            .done(function (data) {
                                if (data && data.isSuccess) {
                                    dialog.modal('hide');
                                    //  $('#employeeRepeater').repeater('render');
                                    location.href = GetRootDirectory() + '/Employee/EmployeeCourseList/' + LMSEmployeeId
                                }
                                else {
                                    dialog.modal('hide');
                                    bootbox.alert(data.message);
                                }
                            })
                            .fail(function (x, y, x) {
                                alert("error");
                            });
                        }
                        else
                            return false;
                    }
                }
            },
            onEscape: function () {
                this.modal('hide');
            }
        })
        dialog.find('#orientCompletedDate').datepicker({ autoclose: true, todayHighlight: true, todayBtn: "linked" });
        dialog.find('#orientCompletedDate').datepicker("setDate", new Date());



        ////Commented as we need to show popup
        //var model = {
        //    LMSEmployeeId: LMSEmployeeId,
        //};
        //$.ajax({
        //    type: 'post',
        //    url: GetRootDirectory() + '/Employee/AddInitialOrientation',
        //    data: model
        //})
        //.done(function (data) {
        //    if (data && data.isSuccess) {
        //        dialog.modal('hide');
        //        //$('#employeeCoursesRepeater').repeater('render');
        //        location.href = GetRootDirectory() + '/Employee/EmployeeCourseList/' + LMSEmployeeId
        //    }
        //    else {
        //        dialog.modal('hide');
        //        bootbox.alert(data.message);
        //    }
        //})
        //.fail(function (x, y, x) {
        //    alert("error");
        //});





    }
}