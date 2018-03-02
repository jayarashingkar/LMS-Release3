// define the columns in your datasource
var columns = [
    //{
    //    label: 'ID',
    //    property: 'UserId',
    //    sortable: true
    //},
      //{
      //    label: 'Employee No',
      //    property: 'EmployeeNo',
      //    sortable: true,
      //    width: '50px'
      //},
    {
        label: 'User Name',
        property: 'UserName',
        sortable: true
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
        case 'UserId':
            // let's combine name and description into a single column
            customMarkup = '<div style="font-size:12px;">' + rowData.UserId + '</div>';
            break;
        case 'Edit':
            // let's combine name and description into a single column
            customMarkup = '<button onclick="GridEditClicked(' + rowData.UserId + ')" id="gridEdit" name="gridEdit" class="btn btn-info btn-sm"><i class="fa fa-pencil"></i></button>';
            break;
        case 'Delete':
            // let's combine name and description into a single column
            customMarkup = '<button onclick="GridDeleteClicked(' + rowData.UserId + ')" id="gridDelete" name="gridDelete" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>';
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
    item.attr('id', 'row' + helpers.rowData.UserId);

    callback();
}

// this example uses an API to fetch its datasource.
// the API handles filtering, sorting, searching, etc.
function customDataSource(options, callback) {
    // set options
    var pageIndex = options.pageIndex;
    var pageSize = options.pageSize;
    var search = '';
    if ($('#searchUserName').val())
        search += ';' + 'UserName:' + $('#searchUserName').val();
    if ($('#searchLastName').val())
        search += ';' + 'LastName:' + $('#searchLastName').val();
    if ($('#searchFirstName').val())
        search += ';' + 'FirstName:' + $('#searchFirstName').val();
    if ($('#searchEmployeeNo').val())
        search += ';' + 'EmployeeNo:' + $('#searchEmployeeNo').val();
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
        url: GetRootDirectory() + '/Grid/GetUserLogins',
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
                            UserName: {
                                validators: {
                                    notEmpty: {
                                        message: 'User Name is required.'
                                    }
                                }
                            },
                            Password: {
                                validators: {
                                    notEmpty: {
                                        message: 'Password is required.'
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
                            PermissionLevel: {
                                validators: {
                                    callback: {
                                        message: 'Permission Level is required.',
                                        callback: function (value, validator, $field) {
                                            /* Get the selected options */
                                            var options = validator.getFieldElements('PermissionLevel').val();
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
                        var UserName = dialog.find('#UserName').val();
                        var UserType = dialog.find('#UserType').val();
                        var Password = dialog.find('#Password').val();
                        var FirstName = dialog.find('#FirstName').val();
                        var LastName = dialog.find('#LastName').val();
                        var Email = dialog.find('#Email').val();
                        var EmployeeNo = dialog.find('#EmployeeNo').val();
                        var IssueDate = dialog.find('#IssueDate').datepicker();
                        IssueDate = dialog.find("#IssueDate").data('datepicker').getFormattedDate('yyyy-mm-dd');
                        var PermissionLevel = dialog.find('#PermissionLevel').val();

                        var hidden = dialog.find('#txtId');
                        var id = 0;
                        if (hidden && hidden.val()) {
                            if (!isNaN(hidden.val())) {
                                id = hidden.val();
                            }
                        }
                        var model = {
                            UserId: id,
                            FirstName: FirstName,
                            LastName: LastName,
                            EmployeeNo: EmployeeNo,
                            IssueDate: IssueDate,
                            IssueDateInFormat: IssueDate,
                            PermissionLevel: PermissionLevel,
                            UserName: UserName,
                            Password: Password,
                            UserType: UserType,
                            Email: Email
                        };
                        $.ajax({
                            type: 'post',
                            url: GetRootDirectory() + '/Admin/SaveUserLogin',
                            data: model
                        })
                        .done(function (data) {
                            if (data && data.isSuccess) {
                                dialog.modal('hide');
                                $('#userLoginsRepeater').repeater('render');
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
    });
    if (document.getElementById("hdnPermission").value == "ReadOnly") {
        dialog.find('button[data-bb-handler=confirm]').attr('disabled', 'disabled');
    }
    dialog.find('#PermissionLevel').selectpicker();
    dialog.find('#IssueDate').datepicker({ autoclose: true, todayHighlight: true, todayBtn: "linked" });
    if (typeof param !== 'undefined' && param) {
        dialog.find('#txtId').val(param.UserId);
        dialog.find('#UserName').val(param.UserName);
        dialog.find("#UserName").attr("disabled", "disabled");
        dialog.find('#Password').attr('disabled', true);
        dialog.find('#FirstName').val(param.FirstName);
        dialog.find('#Email').val(param.Email);
        dialog.find('#LastName').val(param.LastName);
        dialog.find('#EmployeeNo').val(param.EmployeeNo);
        dialog.find('#MiddleName').val(param.MiddleName);
        dialog.find('#IssueDate').datepicker("update", param.IssueDateInFormat);
        dialog.find('#PermissionLevel').selectpicker('val', param.PermissionLevel);
    }
    else {
        dialog.find('#IssueDate').datepicker("setDate", new Date());
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
        url: GetRootDirectory() + '/Admin/EditUserLogin',
        data: obj
    })
    .done(function (data) {
        if (data && data.entity && data.entity.UserId > 0) {
            $("#btnAdd").trigger("click", [{
                UserId: id,
                FirstName: data.entity.FirstName,
                LastName: data.entity.LastName,
                EmployeeNo: data.entity.EmployeeNo,
                IssueDate: data.entity.IssueDate,
                IssueDateInFormat: data.entity.IssueDateInFormat,
                UserName: data.entity.UserName,
                PermissionLevel: data.entity.PermissionLevel,
                Email: data.entity.Email
            }]);
        }
    });
}

function GridDeleteClicked(id) {
    DeleteGridRow(id, GetRootDirectory() + '/Admin/DeleteUserLogins', 'userLoginsRepeater');
}

$('#btnSearch').on('click', function () {
    $('#userLoginsRepeater').repeater('render');
});

$('#btnClear').on('click', function () {
    $('#searchUserName').val('');
    $('#searchFirstName').val('');
    $('#searchLastName').val('');
    $('#searchEmployeeNo').val('');
    $('#userLoginsRepeater').repeater('render');
    
});