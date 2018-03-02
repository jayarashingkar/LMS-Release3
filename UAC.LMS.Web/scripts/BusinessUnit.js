﻿// define the columns in your datasource
var columns = [
    //{
    //    label: 'ID',
    //    property: 'LMSBusinessUnitId',
    //    sortable: true
    //},
    {
        // Changed  Business Unit to  Location 11/21/2016
        //label: 'Business Unit Name',

        label: 'Location Name',
        property: 'BusinessUnitName',
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
        case 'LMSBusinessUnitId':
            // let's combine name and description into a single column
            customMarkup = '<div style="font-size:12px;">' + rowData.LMSBusinessUnitId + '</div>';
            break;
        case 'Edit':
            // let's combine name and description into a single column
            customMarkup = '<button onclick="GridEditClicked(' + rowData.LMSBusinessUnitId + ')" id="gridEdit" name="gridEdit" class="btn btn-info btn-sm"><i class="fa fa-pencil"></i></button>';
            break;
        case 'Delete':
            // let's combine name and description into a single column
            customMarkup = '<button onclick="GridDeleteClicked(' + rowData.LMSBusinessUnitId + ')" id="gridDelete" name="gridDelete" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>';
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
    item.attr('id', 'row' + helpers.rowData.LMSBusinessUnitId);

    callback();
}

// this example uses an API to fetch its datasource.
// the API handles filtering, sorting, searching, etc.
function customDataSource(options, callback) {
    // set options
    var pageIndex = options.pageIndex;
    var pageSize = options.pageSize;
    var options = {
        pageIndex: pageIndex,
        pageSize: pageSize,
        sortDirection: options.sortDirection,
        sortBy: options.sortProperty,
        filterBy: options.filter.value || '',
        searchBy: options.search || ''
    };
    // call API, posting options
    $.ajax({
        type: 'post',
        url: GetRootDirectory() + '/Grid/GetBusinessUnits',
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
                            BusinessUnitName: {
                                validators: {
                                    notEmpty: {
                                        // Changed  Business Unit to  Location 11/21/2016
                                        // message: 'Business Unit Name is required.'
                                        message: 'Location Name is required.'
                                    }
                                }
                            },
                        }
                    });
                    var validator = form.data('bootstrapValidator');
                    validator.validate();
                    if (validator.isValid()) {
                        var BusinessUnitName = dialog.find('#BusinessUnitName').val();
                        var hidden = dialog.find('#txtId');
                        var id = 0;
                        if (hidden && hidden.val()) {
                            if (!isNaN(hidden.val())) {
                                id = hidden.val();
                            }
                        }
                        var model = {
                            LMSBusinessUnitId: id,
                            BusinessUnitName: BusinessUnitName
                        };
                        $.ajax({
                            type: 'post',
                            url: GetRootDirectory() + '/Admin/SaveBusinessUnit',
                            data: model
                        })
                        .done(function (data) {
                            if (data && data.isSuccess) {
                                dialog.modal('hide');
                                $('#businessUnitRepeater').repeater('render');
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

    // checks for permission 

    dialog.find('#BusinessUnitName').focus();
    if (document.getElementById("hdnPermission").value == "ReadOnly") {
        dialog.find('button[data-bb-handler=confirm]').attr('disabled', 'disabled');
    }
    if (typeof param !== 'undefined' && param) {
        dialog.find('#txtId').val(param.LMSBusinessUnitId);
        dialog.find('#BusinessUnitName').val(param.BusinessUnitName);
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
        url: GetRootDirectory() + '/Admin/EditBusinessUnit',
        data: obj
    })
    .done(function (data) {
        if (data && data.entity && data.entity.LMSBusinessUnitId > 0) {
            $("#btnAdd").trigger("click", [{
                LMSBusinessUnitId: id,
                BusinessUnitName: data.entity.BusinessUnitName
            }]);
        }
    });
}


function GridDeleteClicked(id) {
    DeleteGridRow(id, GetRootDirectory() + '/Admin/DeleteLMSBusinessUnit', 'businessUnitRepeater');
}