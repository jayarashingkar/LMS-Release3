// define the columns in your datasource
var columns = [
    //{
    //    label: 'ID',
    //    property: 'LMSEmployeeCourseId',
    //    sortable: true
    //},
    //{
    //    label: 'Employee Name',
    //    property: 'EmployeeName',
    //    sortable: true
    //},
   {
       label: 'Course Code',
       property: 'CourseCode',
       sortable: true,
       width: '50px'
   },
    {
        label: 'Course Name',
        property: 'CourseName',
        sortable: true
    },
    {
        // Changed CourseLength to  Training Time 11/21/2016
        //label: 'Course Length',
        label: 'Training Time',
        property: 'CourseLength',
        sortable: true,
        width: '50px'
    },
    {
        label: 'Frequency',
        property: 'Frequency',
        sortable: true,
        width: '50px'
    },
    {
        label: 'Initial Orientation',
        property: 'IsInitialOrientation',
        sortable: true,
        width: '50px'
    },
    //{
    //    label: 'Instructor Name',
    //    property: 'InstructorName',
    //    sortable: true
    //},

    // Removed Previous Date 11/21/2016
    //{
    //    label: 'Previous Date',
    //    property: 'PreviousDateInFormat',
    //    sortable: true,
    //    width: '50px'
    //},
    {
        label: 'Completed Date',
        property: 'CompletedDateInFormat',
        sortable: true,
        width: '50px'
    },
    {
        label: 'Due Date',
        property: 'DueDateInFormat',
        sortable: true,
        width: '50px'
    },
    {
        label: 'Edit',
        property: 'Edit',
        width: '50px'
    },
    {
        label: 'Delete',
        property: 'Delete',
        width: '50px'
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
        case 'LMSEmployeeCourseId':
            // let's combine name and description into a single column
            customMarkup = '<div style="font-size:12px;">' + rowData.LMSEmployeeCourseId + '</div>';
            break;
        case
        'IsInitialOrientation':
            // let's combine name and description into a single column
            customMarkup = (helpers.item.text() && helpers.item.text() === 'true') ? 'Yes' : 'No';
            break;
        case
        'Frequency':
            // let's combine name and description into a single column
            customMarkup = (helpers.item.text() && helpers.item.text() === '0') ? '-' : helpers.item.text();
            break;
        case 'Edit':
            // let's combine name and description into a single column
            customMarkup = '<button onclick="GridEditClicked(' + rowData.LMSEmployeeCourseId + ')" id="gridEdit" name="gridEdit" class="btn btn-info btn-sm"><i class="fa fa-pencil"></i></button>';
            break;
        case 'Delete':
            // let's combine name and description into a single column
            customMarkup = '<button onclick="GridDeleteClicked(' + rowData.LMSEmployeeCourseId + ')" id="gridDelete" name="gridDelete" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>';
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
    item.attr('id', 'row' + helpers.rowData.LMSEmployeeCourseId);

    callback();
}

// this example uses an API to fetch its datasource.
// the API handles filtering, sorting, searching, etc.
function customDataSource(options, callback) {
    // set options
    var pageIndex = options.pageIndex;
    var pageSize = options.pageSize;
    var search = '';
    if ($('#LMSEmployeeId').val())
        search += ';' + 'LMSEmployeeId:' + $('#LMSEmployeeId').val();
    if ($('#searchCourseName').val())
        search += ';' + 'searchCourseName:' + $('#searchCourseName').val();
    if ($('#searchCourseCode').val())
        search += ';' + 'searchCourseCode:' + $('#searchCourseCode').val();
    if (typeof $('input[name=IsCouresType]:checked').val() !== 'undefined')
        search += ';' + 'CourseType:' + $('input[name=IsCouresType]:checked').val();
    //var searchLMSJobTitleId = $('#searchLMSJobTitleId').val();
    //if (searchLMSJobTitleId && searchLMSJobTitleId !== '-1')
    //    search += ';' + 'searchLMSJobTitleId:' + searchLMSJobTitleId;
    if (typeof $('input[name=IsJobTitle]:checked').val() !== 'undefined')
        search += ';' + 'IsJobTitle:' + $('#hdnJobTitleId').val();
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
        url: GetRootDirectory() + '/Grid/GetEmployeeCourses',
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
                label: '<i class="fa fa-times"></i> Cancel'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Confirm',
                callback: function (result) {
                    var form = dialog.find('#entityform');
                    form.bootstrapValidator({
                        message: 'This value is not valid',
                        feedbackIcons: {
                            valid: 'glyphicon glyphicon-ok',
                            invalid: 'glyphicon glyphicon-remove',
                            validating: 'glyphicon glyphicon-refresh'
                        },
                        fields: {
                            //InstructorName: {
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
                        var LMSEmployeeId = dialog.find('#LMSEmployeeId').val();
                        var LMSCourseId = dialog.find('#LMSCourseId').val();
                        var Frequency = dialog.find('#Frequency').val();
                        var Evaluation = dialog.find('#Evaluation').val();
                        var Remarks = dialog.find('#Remarks').val();
                        var InstructorName = dialog.find('#InstructorName').val();

                        // Removed Previous Date 11/21/2016
                        //var PreviousDate = dialog.find('#PreviousDate').datepicker();
                        //PreviousDate = dialog.find("#PreviousDate").data('datepicker').getFormattedDate('yyyy-mm-dd');
                        var CompletedDate = dialog.find('#CompletedDate').datepicker();
                        CompletedDate = dialog.find("#CompletedDate").data('datepicker').getFormattedDate('yyyy-mm-dd');
                        // Removed Previous Date 11/21/2016
                        var PreviousDate = CompletedDate;
                        var DueDate = dialog.find('#DueDate').datepicker();
                        DueDate = dialog.find("#DueDate").data('datepicker').getFormattedDate('yyyy-mm-dd');

                        var hidden = dialog.find('#txtId');
                        var id = 0;
                        if (hidden && hidden.val()) {
                            if (!isNaN(hidden.val())) {
                                id = hidden.val();
                            }
                        }
                        var model = {
                            LMSEmployeeCourseId: id,
                            LMSEmployeeId: LMSEmployeeId,
                            LMSCourseId: LMSCourseId,
                            Frequency: Frequency,
                            Evaluation: Evaluation,
                            Remarks: Remarks,
                            InstructorName: InstructorName,
                            // Removed Previous Date 11/21/2016
                            //PreviousDate: PreviousDate,
                            PreviousDate: CompletedDate,
                            CompletedDate: CompletedDate,
                            DueDate: DueDate
                        };
                        $.ajax({
                            type: 'post',
                            url: GetRootDirectory() + '/Employee/SaveEmployeeCourse',
                            data: model
                        })
                        .done(function (data) {
                            if (data && data.isSuccess) {
                                dialog.modal('hide');
                                $('#employeeCoursesRepeater').repeater('render');
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
    if (document.getElementById("hdnPermission").value == "ReadOnly") {
        dialog.find('button[data-bb-handler=confirm]').attr('disabled', 'disabled');
    }
    dialog.find('#LMSCourseId').on('change', function () {
        CourseChanged(this);
    });
    dialog.find('#CompletedDate').on('change', function (e) {
        $('.datepicker').hide();
        dialog.find('#DueDate').datepicker("update", "");
        var CompletedDate = dialog.find("#CompletedDate").data('datepicker').getFormattedDate('yyyy-mm-dd');
        var Frequency = dialog.find('#Frequency').val();
        if (CompletedDate && Frequency && Frequency > 0) {
            var model = { CompletedDate: CompletedDate, Frequency: Frequency };
            $.ajax({
                type: 'post',
                url: GetRootDirectory() + '/Employee/GetComputedDueDate',
                data: model
            })
            .done(function (data) {
                //alert(data);
                if (data) {
                    dialog.find('#DueDate').datepicker("update", data);
                }
            })
            .fail(function (x, y, x) {
                alert("error");
            });
        }

    });
    if (typeof param !== 'undefined' && param)
        dialog.find('#LMSCourseId').attr('disabled', 'disabled');
    dialog.find('#LMSCourseId').attr("data-live-search", "true");
    dialog.find('#LMSCourseId').selectpicker();
    // Removed Previous Date 11/21/2016
    // dialog.find('#PreviousDate').datepicker({ autoclose: true, todayHighlight: true, todayBtn: "linked" });
    //dialog.find('#CompletedDate').datepicker("setDate", new Date());

    dialog.find('#CompletedDate').datepicker({ autoclose: true, todayHighlight: true, todayBtn: "linked" });
    dialog.find('#DueDate').datepicker({ autoclose: true, todayHighlight: true, todayBtn: "linked" });
    if (typeof param !== 'undefined' && param) {
        dialog.find('#txtId').val(param.LMSEmployeeCourseId);
        dialog.find('#LMSEmployeeId').val(param.LMSEmployeeId);
        dialog.find('#LMSCourseId').selectpicker('val', param.LMSCourseId);
        dialog.find('#InstructorName').val(param.InstructorName);
        dialog.find('#Evaluation').val(param.Evaluation);
        dialog.find('#Remarks').val(param.Remarks);
        // Removed Previous Date 11/21/2016
        //dialog.find('#PreviousDate').datepicker("update", param.PreviousDateInFormat);
        dialog.find('#CompletedDate').datepicker("update", param.CompletedDateInFormat);
        dialog.find('#DueDate').datepicker("update", param.DueDateInFormat);
        dialog.find('#LMSCourseId').trigger('change');
    }
});

$('#btnAddOrient').on('click', function (event, param) {
    //var LMSEmployeeId = $('#LMSEmployeeId').val();
    //var model = {
    //    LMSEmployeeId: LMSEmployeeId,
    //    InstructorName: "",
    //    Remarks: "",
    //    Evaluation: "",
    //};
    //$.ajax({
    //    type: 'post',
    //    url: GetRootDirectory() + '/Employee/AddInitialOrientation',
    //    data: model
    //})
    //.done(function (data) {
    //    if (data && data.isSuccess) {
    //        //dialog.modal('hide');
    //        $('#employeeCoursesRepeater').repeater('render');
    //    }
    //    else {
    //        //dialog.modal('hide');
    //        bootbox.alert(data.message);
    //    }
    //})
    //.fail(function (x, y, x) {
    //    alert("error");
    //});

    // removed pop up before adding initial orientation courses

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
                        var LMSEmployeeId = $('#LMSEmployeeId').val();
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
                                $('#employeeCoursesRepeater').repeater('render');
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
    if (document.getElementById("hdnPermission").value == "ReadOnly") {
        dialog.find('button[data-bb-handler=confirm]').attr('disabled', 'disabled');
    }
});

$('#btnAddCourse').on('click', function (event, param) {

    var model = { jobTitleId: $('#hdnJobTitleId').val(), employeeId: $('#LMSEmployeeId').val() };
    $.ajax({
        type: 'post',
        url: GetRootDirectory() + '/Employee/AddTitleCourses',
        data: model
    })
    .done(function (data) {
        if (data && data.isSuccess) {
            //dialog.modal('hide');
            $('#employeeCoursesRepeater').repeater('render');
        }
        else {
            //dialog.modal('hide');
            bootbox.alert(data.message);
        }
    })
    .fail(function (x, y, x) {
        alert("error");
    });

    //var model = { jobTitleId: $('#hdnJobTitleId').val(), employeeId: $('#LMSEmployeeId').val() };
    //$.ajax({
    //    type: 'post',
    //    url: GetRootDirectory() + '/Employee/GetTitleCourses',
    //    data: model
    //})
    //.done(function (data) {
    //    var msg = '';
    //    if (data && data.lstLMSCourse) {
    //        var length = data.lstLMSCourse.length;
    //        if (length > 0) {
    //            for (var i = 0; i < length; i++) {
    //                msg += "CourseCode : " + data.lstLMSCourse[i].CourseCode + " - CourseName : " + data.lstLMSCourse[i].CourseName + '</br>';
    //            }
    //        }
    //    }
    //    AddJobTitleCourse(msg);
    //});
});

function AddJobTitleCourse(msg) {
    if (msg && msg !== '') {
        var model = { jobTitleId: $('#hdnJobTitleId').val(), employeeId: $('#LMSEmployeeId').val() };
        var dialog = bootbox.dialog({
            message: "JobTitle Contains the following Courses. Do you want to add it? <br/><br/>" + msg,
            buttons: {
                cancel: {
                    label: '<i class="fa fa-times"></i> Cancel'
                },
                confirm: {
                    label: '<i class="fa fa-check"></i> Confirm',
                    callback: function (result) {
                        $.ajax({
                            type: 'post',
                            url: GetRootDirectory() + '/Employee/AddTitleCourses',
                            data: model
                        })
                        .done(function (data) {
                            if (data && data.isSuccess) {
                                dialog.modal('hide');
                                $('#employeeCoursesRepeater').repeater('render');
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
                }
            },
            onEscape: function () {
                this.modal('hide');
            }
        });
    }
    else
        bootbox.alert("No Course is mapped with employee's JobTitle / Course already added.");

}


$(document).ready(function () {
    $('#btnCancel').on('click', function () {
        if (typeof dialog !== 'undefined' && dialog)
            dialog.modal('hide');
    });

    // using select picker with mulitple selection and select all, deselect all options
    $('#searchLMSJobTitleId').attr('multiple', '');
    $('#searchLMSJobTitleId').attr('data-actions-box', 'true');
    $('#searchLMSJobTitleId').selectpicker();
});

function GridEditClicked(id) {
    var obj = { id: id };
    $.ajax({
        type: 'post',
        url: GetRootDirectory() + '/Employee/EditEmployeeCourse',
        data: obj
    })
    .done(function (data) {
        if (data && data.entity && data.entity.LMSEmployeeCourseId > 0) {
            $("#btnAdd").trigger("click", [{
                LMSEmployeeCourseId: id,
                LMSEmployeeId: data.entity.LMSEmployeeId,
                LMSCourseId: data.entity.LMSCourseId,
                InstructorName: data.entity.InstructorName,
                Evaluation: data.entity.Evaluation,
                Remarks: data.entity.Remarks,
                // Removed Previous Date 11/21/2016
                // PreviousDate: data.entity.PreviousDate,
                //PreviousDateInFormat: data.entity.PreviousDateInFormat,

                PreviousDate: data.entity.CompletedDate,
                CompletedDate: data.entity.CompletedDate,
                CompletedDateInFormat: data.entity.CompletedDateInFormat,
                DueDate: data.entity.DueDate,
                DueDateInFormat: data.entity.DueDateInFormat,
            }]);
        }
    });
}

function GridDeleteClicked(id) {
    DeleteGridRow(id, GetRootDirectory() + '/Employee/DeleteEmployeeCourse', 'employeeCoursesRepeater');
}

$('#btnSearch').on('click', function () {
    $('#employeeCoursesRepeater').repeater('render');
});

$('#btnClear').on('click', function () {
    $('#searchCourseName').val('');
    $('#searchCourseCode').val('');
    $('#searchLMSJobTitleId').selectpicker('val', "-1");
    $('input[name=IsCouresType]:checked').parent().attr('class', 'radio-custom radio-inline');
    $('input[name=IsCouresType]:checked').prop('checked', false);
    $('input[name=IsJobTitle]:checked').parent().attr('class', 'radio-custom radio-inline');
    $('input[name=IsJobTitle]:checked').prop('checked', false);
    $('#employeeCoursesRepeater').repeater('render');
});

function CourseChanged(ele) {
    var obj = { id: $(ele).val() };
    $.ajax({
        type: 'post',
        url: GetRootDirectory() + '/Admin/EditCourse',
        data: obj
    })
    .done(function (data) {
        if (data && data.entity && data.entity.LMSCourseId > 0) {
            dialog.find('#CourseCode').val(data.entity.CourseCode);
            dialog.find('#CourseLength').val(data.entity.CourseLength);
            dialog.find('#IsReoccuring').val((data.entity.IsReocurring) ? "Yes" : "No");
            dialog.find('#Frequency').val(data.entity.Frequency);
            dialog.find('#IsInitialOrientation').val((data.entity.IsInitialOrientation) ? "Yes" : "No");
            dialog.find('#CompletedDate').trigger('change');
        }
    });
}