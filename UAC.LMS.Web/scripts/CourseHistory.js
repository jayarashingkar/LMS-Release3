// define the columns in your datasource
var columns = [
    {
        label: 'Course Name',
        property: 'CourseName',
        width: '50px',
        sortable: true
    },
    {
        label: 'Course Code',
        property: 'CourseCode',
        width: '50px',
        sortable: true
    },
    {
        label: 'Completion Date',
        property: 'CompletedDateInFormat',
        width: '50px',
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
    item.attr('id', 'row' + helpers.rowData.LMSCourseHistoryId);

    callback();
}

// this example uses an API to fetch its datasource.
// the API handles filtering, sorting, searching, etc.

function customDataSource(options, callback) {
    // set options
    var pageIndex = options.pageIndex;
    var pageSize = options.pageSize;

    var search = '';
    if ($('#hdnEmployeeId').val())
        search += ';' + 'hdnEmployeeId:' + $('#hdnEmployeeId').val();

    if ($('#LMSCourseName').val())
        search += ';' + 'LMSCourseName:' + $('#LMSCourseName').val();

    if ($('#LMSCourseCode').val())
        search += ';' + 'LMSCourseCode:' + $('#LMSCourseCode').val();

    if ($('#searchFromDate').val()) {
        var searchFromDate = $('#searchFromDate').datepicker();
        searchFromDate = $("#searchFromDate").data('datepicker').getFormattedDate('yyyy-mm-dd');
        if (searchFromDate && searchFromDate !== '')
            search += ';' + 'searchFromDate:' + searchFromDate;
    }

    if ($('#searchToDate').val()) {
        var searchToDate = $('#searchToDate').datepicker();
        searchToDate = $("#searchToDate").data('datepicker').getFormattedDate('yyyy-mm-dd');
        if (searchToDate && searchToDate !== '')
            search += ';' + 'searchToDate:' + searchToDate;
    }

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
        url: GetRootDirectory() + '/Grid/GetEmpCourseHistory',
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

// comment here 
//on Search Button the Grid is loaded with Course History with render options
$('#btnSearch').on('click', function () {
    $('#HistoryRepeater').repeater('render');
});

// comment here 
// on clear button the Grid is loaded with all the Course History 
$('#btnClear').on('click', function () {
    $('#LMSCourseName').selectpicker('val', '-1');
    $('#LMSCourseCode').selectpicker('val', '-1');
    $('#searchFromDate').val('');
    $('#searchToDate').val('');
    $('#HistoryRepeater').repeater('render');
});

// comment here 

$(document).ready(function () {
    $('#searchFromDate').datepicker({ autoclose: true, todayHighlight: true, todayBtn: "linked" });
    $('#searchToDate').datepicker({ autoclose: true, todayHighlight: true, todayBtn: "linked" });
    $('#LMSCourseName').selectpicker();
    $('#LMSCourseCode').selectpicker();

});

