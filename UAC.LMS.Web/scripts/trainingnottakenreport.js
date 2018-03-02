// define the columns in your datasource
var columns = [
    //{
    //    label: 'ID',
    //    property: 'LMSEmployeeId',
    //    sortable: true
    //},
    {
        label: 'Employee Name',
        property: 'EmployeeName',
        sortable: true
    },
    {
        label: 'Empl#',
        property: 'EmployeeNo',
        sortable: true
    },
    {
        label: 'Training Event',
        property: 'TrainingEvent',
        sortable: true
    },
    {
        label: 'Frq(Months)',
        property: 'Frequency',
        sortable: true
    },
    {
        label: 'Course No',
        property: 'CourseNo',
        sortable: true
    },
    {
        label: 'Department',
        property: 'Department',
        sortable: true
    },
    {
        label: 'Date Completed',
        property: 'CompletedDate',
        sortable: true
    },
    {
        label: 'Date Due',
        property: 'DueDate',
        sortable: true
    },
    {
        label: 'Days Remain',
        property: 'DaysRemain',
        sortable: true
    },
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
        case 'CompletedDate':
            // let's combine name and description into a single column
            customMarkup = rowData.CompletedDateInFormat;
            break;
        case 'DueDate':
            // let's combine name and description into a single column
            customMarkup = rowData.DueDateInFormat;
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
    item.attr('id', 'row' + helpers.rowData.EmployeeNo);

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
        url: GetRootDirectory() + '/Grid/GetTrainingnotTaken',
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
$(document).ready(function () {
    $('#btnExport').on('click', function () {
        document.getElementById('reportForm').submit();
    });
});