// define the columns in your datasource

var columns = [
    //{
    //    label: 'ID',
    //    property: 'LMSJobTitleId',
    //    sortable: true
    //},
    {
        label: 'Audit Id',
        property: 'LMSAuditId',
        width: '50px'
    },
    {
        label: 'Transaction Date',
        property: 'TransactionDateInFormat',
        width: '50px'

    },
    {
        label: 'User Name',
        property: 'UserName',
        width: '50px'
    },
    {
        label: 'Full Name',
        property: 'FullName',
        width: '50px'
    },
    {
        label: 'Action',
        property: 'Action',
        width: '50px'
    },
    {
        label: 'Description',
        property: 'Description'
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
        case 'LMSAuditId':
            // let's combine name and description into a single column
            customMarkup = '<div style="font-size:12px;">' + rowData.LMSAuditId + '</div>';
            break;

        case 'TransactionDate':
            // let's combine name and description into a single column                      
            //
            customMarkup = '<div style="font-size:12px;">' + rowData.TransactionDate + '</div>';

            // customMarkup = '<div style="font-size:12px;">' + rowData.TransactionDateInFormat + '</div>';
            break;

        case 'UserName':
            // let's combine name and description into a single column
            var date = new Date();
            customMarkup = '<div style="font-size:12px;">' + rowData.UserName + '</div>';
            break;
        case 'FullName':
            // let's combine name and description into a single column
            customMarkup = '<div style="font-size:12px;">' + rowData.FullName + '</div>';
            break;
        case 'Action':
            // let's combine name and description into a single column
            customMarkup = '<div style="font-size:12px;">' + rowData.Action + '</div>';
            break;
        case 'Description':
            // let's combine name and description into a single column
            customMarkup = '<div style="font-size:12px;">' + rowData.Description + '</div>';
            break;
            //case 'Edit':
            //    // let's combine name and description into a single column
            //    customMarkup = '<button onclick="GridEditClicked(' + rowData.LMSJobTitleId + ')" id="gridEdit" name="gridEdit" class="btn btn-info btn-sm"><i class="fa fa-pencil"></i></button>';
            //    break;
            //case 'Delete':
            //    // let's combine name and description into a single column
            //    customMarkup = '<button onclick="GridDeleteClicked(' + rowData.LMSJobTitleId + ')" id="gridDelete" name="gridDelete" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>';
            //    break;
        default:
            // otherwise, just use the existing text value
            customMarkup = helpers.item.text();
            break;
    }

    helpers.item.html(customMarkup);

    callback();
}

function formatDate(date) {
    return "date";
}

function customRowRenderer(helpers, callback) {
    // let's get the id and add it to the "tr" DOM element
    var item = helpers.item;
    item.attr('id', 'row' + helpers.rowData.LMSAuditId);

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
        search += ';' + 'searchUserName:' + $('#searchUserName').val();

    if ($('#searchFullName').val())
        search += ';' + 'searchFullName:' + $('#searchFullName').val();

    if ($('#searchDate').val()) {
        var DueDate = $('#searchDate').datepicker();
        DueDate = $("#searchDate").data('datepicker').getFormattedDate('yyyy-mm-dd');
        if (DueDate && DueDate !== '')
            search += ';' + 'searchDate:' + DueDate;
    }

    if ($('#searchDesc').val())
        search += ';' + 'searchDesc:' + $('#searchDesc').val();

    var options = {
        pageIndex: options.pageIndex,
        pageSize: options.pageSize,
        sortDirection: options.sortDirection,
        sortBy: options.sortProperty,
        filterBy: options.filter.value || '',
        searchBy: search || ''
    };
    // call API, posting options
    $.ajax({
        type: 'post',
        url: GetRootDirectory() + '/Grid/GetAudit',
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


$('#btnSearch').on('click', function () {
    $('#AuditRepeater').repeater('render');
});

$('#btnClear').on('click', function () {
    $('#searchUserName').val('');
    $('#searchFullName').val('');
    $('#searchDate').val('');
    $('#AuditRepeater').repeater('render');
});

$(document).ready(function () {
    $('#searchDate').datepicker({ autoclose: true, todayHighlight: true, todayBtn: "linked" });
});

