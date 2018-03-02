// define the columns in your datasource
var columns = [
    //{
    //    label: 'ID',
    //    property: 'LMSCourseId',
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
        
        label: 'Training Time',
        property: 'CourseLength',
        sortable: true,
        width: '50px'
    },
    {
        label: 'Reoccuring',
        property: 'IsReocurring',
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
        case 'LMSCourseId':
            // let's combine name and description into a single column
            customMarkup = '<div style="font-size:12px;">' + rowData.LMSCourseId + '</div>';
            break;
        case 'Edit':
            // let's combine name and description into a single column
            customMarkup = '<button onclick="GridEditClicked(' + rowData.LMSCourseId + ')" id="gridEdit" name="gridEdit" class="btn btn-info btn-sm"><i class="fa fa-pencil"></i></button>';
            break;
        case 'Delete':
            // let's combine name and description into a single column
            customMarkup = '<button onclick="GridDeleteClicked(' + rowData.LMSCourseId + ')" id="gridDelete" name="gridDelete" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>';
            break;
        case
            'IsInitialOrientation':
            // let's combine name and description into a single column
            customMarkup = (helpers.item.text() && helpers.item.text() === 'true') ? 'Yes' : 'No';
            break;
        case
            'IsReocurring':
            // let's combine name and description into a single column
            customMarkup = (helpers.item.text() && helpers.item.text() === 'true') ? 'Yes' : 'No';
            break;
        case
        'Frequency':
            // let's combine name and description into a single column
            customMarkup = (helpers.item.text() && helpers.item.text() === '0') ? '-' : helpers.item.text();
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
    item.attr('id', 'row' + helpers.rowData.LMSCourseId);

    callback();
}

// this example uses an API to fetch its datasource.
// the API handles filtering, sorting, searching, etc.
function customDataSource(options, callback) {
    // set options
    var pageIndex = options.pageIndex;
    var pageSize = options.pageSize;
    var search = '';
    if ($('#searchCourseName').val())
        search += ';' + 'searchCourseName:' + $('#searchCourseName').val();
    if ($('#searchCourseCode').val())
        search += ';' + 'searchCourseCode:' + $('#searchCourseCode').val();
    if (typeof $('input[name=IsCouresType]:checked').val() !== 'undefined')
        search += ';' + 'CourseType:' + $('input[name=IsCouresType]:checked').val();
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
        url: GetRootDirectory() + '/Grid/GetCourses',
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
                            CourseName: {
                                validators: {
                                    notEmpty: {
                                        message: 'Course Name is required.'
                                    }
                                }
                            },
                            CourseCode: {
                                validators: {
                                    notEmpty: {
                                        message: 'Course Code is required.'
                                    }
                                }
                            },
                            //IsReocurring: {
                            //    validators: {
                            //        callback: {
                            //            message: 'Frequency is required.',
                            //            callback: function (value, validator, $field) {
                            //                var ret = true;
                            //                var Frequency = validator.getFieldElements('Frequency').val();
                            //                var IsReocurring = dialog.find('input[name=IsReocurring]:checked').val();
                            //                console.log('Frequency : ' + Frequency);
                            //                if (IsReocurring == 'yes' && !Frequency) {
                            //                    ret = false;
                            //                }
                            //                return { "valid": ret };
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    });
                    var validator = form.data('bootstrapValidator');
                    validator.validate();
                    if (validator.isValid()) {

                        var frevalid = dialog.find('#Frequency').val();
                        var isrevalid = dialog.find('input[name=IsReocurring]:checked').val();
                        if (isrevalid === 'yes' && !frevalid) {
                            alert('Frequency is required.');
                            return false;
                        }
                        var CourseName = dialog.find('#CourseName').val();
                        var CourseCode = dialog.find('#CourseCode').val();
                        var CourseLength = dialog.find('#CourseLength').val();
                        var IsReocurring = false;
                        if (typeof dialog.find('input[name=IsReocurring]:checked').val() !== 'undefined' && dialog.find('input[name=IsReocurring]:checked').val() === 'yes')
                            IsReocurring = true;
                        var Frequency = dialog.find('#Frequency').val();
                        var IsInitialOrientation = dialog.find('#IsInitialOrientation').prop('checked');
                        var hidden = dialog.find('#txtId');
                        var id = 0;
                        if (hidden && hidden.val()) {
                            if (!isNaN(hidden.val())) {
                                id = hidden.val();
                            }
                        }
                        var LMSJobTitleIds = dialog.find('#LMSJobTitleId').val();
                        var model = {
                            LMSCourseId: id,
                            CourseName: CourseName,
                            CourseCode: CourseCode,
                            CourseLength: CourseLength,
                            IsReocurring: IsReocurring,
                            Frequency: Frequency,
                            IsInitialOrientation: IsInitialOrientation
                        };
                        $.ajax({
                            type: 'post',
                            url: GetRootDirectory() + '/Admin/SaveCourse',
                            data: { model: model, LMSJobTitleIds: LMSJobTitleIds }
                        })
                        .done(function (data) {
                            if (data && data.isSuccess) {
                                dialog.modal('hide');
                                $('#courseRepeater').repeater('render');
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
    dialog.find('input[type=radio][name=IsReocurring]').change(function () {
        if ($(this).val() === 'no') {
            dialog.find("#Frequency").val('');
            dialog.find("#Frequency").attr("disabled", "disabled");
        }
        else
            dialog.find("#Frequency").removeAttr("disabled");
    });
    dialog.find('#LMSJobTitleId').attr('multiple', '');
    dialog.find('#LMSJobTitleId').attr('data-actions-box', 'true');
    dialog.find('#LMSJobTitleId').selectpicker();
    if (document.getElementById("hdnPermission").value == "ReadOnly") {
        dialog.find('button[data-bb-handler=confirm]').attr('disabled', 'disabled');                
    }
    if (typeof param !== 'undefined' && param) {
        dialog.find('#txtId').val(param.LMSCourseId);
        dialog.find('#CourseName').val(param.CourseName);
        dialog.find('#CourseCode').val(param.CourseCode);
        dialog.find('#CourseLength').val(param.CourseLength);
        dialog.find('#Frequency').val(param.Frequency);
        if (param.IsReocurring) {
            var yesChk = dialog.find("input[name='IsReocurring'][value='yes']");
            yesChk.prop("checked", true);
            yesChk.parent().addClass("checked");
        }
        else {
            var noChk = dialog.find("input[name='IsReocurring'][value='no']");
            noChk.prop("checked", true);
            noChk.parent().addClass("checked");
            dialog.find("#Frequency").val('');
            dialog.find("#Frequency").attr("disabled", "disabled");
        }

        if (param.IsInitialOrientation)
            dialog.find('#IsInitialOrientation').parent().addClass("checked");
        dialog.find('#IsInitialOrientation').prop('checked', param.IsInitialOrientation);
        //JobTitleIds
        if (param.JobTitleIds && param.JobTitleIds.length > 0) {
            dialog.find('#LMSJobTitleId').selectpicker('val', param.JobTitleIds);
        }      
    }
    else {
        dialog.find("#Frequency").attr("disabled", "disabled");
        var yesChk = dialog.find("input[name='IsReocurring'][value='no']");
        yesChk.prop("checked", true);
        yesChk.parent().addClass("checked");
    }
});

$(document).ready(function () {
    $('#btnCancel').on('click', function () {
        if (typeof dialog !== 'undefined' && dialog)
            dialog.modal('hide');
    });
});

function GridEditClicked(id) {
    var obj = { id: id };
    $.ajax({
        type: 'post',
        url: GetRootDirectory() + '/Admin/EditCourse',
        data: obj
    })
    .done(function (data) {
        if (data && data.entity && data.entity.LMSCourseId > 0) {
            $("#btnAdd").trigger("click", [{
                LMSCourseId: id,
                CourseName: data.entity.CourseName,
                CourseCode: data.entity.CourseCode,
                CourseLength: data.entity.CourseLength,
                IsReocurring: data.entity.IsReocurring,
                Frequency: data.entity.Frequency,
                IsInitialOrientation: data.entity.IsInitialOrientation,
                JobTitleIds: data.jobTitleIds
            }]);
        }
    });
}

function GridDeleteClicked(id) {
    DeleteGridRow(id, GetRootDirectory() + '/Admin/DeleteCourse', 'courseRepeater');
}

$('#btnSearch').on('click', function () {
    $('#courseRepeater').repeater('render');
});

$('#btnClear').on('click', function () {
    $('#searchCourseName').val('');
    $('#searchCourseCode').val('');
    $('input[name=IsCouresType]:checked').parent().attr('class', 'radio-custom radio-inline')
    $('input[name=IsCouresType]:checked').prop('checked', false);
    $('#courseRepeater').repeater('render');
});