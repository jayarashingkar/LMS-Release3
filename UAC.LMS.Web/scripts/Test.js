// define the columns in your datasource
var columns = [
    {
        label: 'ID',
        property: 'TestModel1Id',
        sortable: true
    },
    {
        label: 'MyProperty 1',
        property: 'MyProperty1',
        sortable: true
    },
    {
        label: 'MyProperty 2',
        property: 'MyProperty2',
        sortable: true
    },
    {
        label: 'MyProperty 3',
        property: 'MyProperty3',
        sortable: true
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
        case 'TestModel1Id':
            // let's combine name and description into a single column
            customMarkup = '<div style="font-size:12px;">' + rowData.TestModel1Id + '</div>';
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
    item.attr('id', 'row' + helpers.rowData.TestModel1Id);

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
        url: GetRootDirectory() + '/LMS/GetData',
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

function LoadGrid(id) {
    // initialize the repeater
    var repeater = $('#' + id);
    repeater.repeater({
        list_selectable: false, // (single | multi)
        list_noItemsHTML: 'nothing to see here... move along',

        // override the column output via a custom renderer.
        // this will allow you to output custom markup for each column.
        list_columnRendered: customColumnRenderer,

        // override the row output via a custom renderer.
        // this example will use this to add an "id" attribute to each row.
        list_rowRendered: customRowRenderer,

        // setup your custom datasource to handle data retrieval;
        // responsible for any paging, sorting, filtering, searching logic
        dataSource: customDataSource
    });
}