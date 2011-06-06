$(document).ready(function () {
  
    var viewProducts = new ViewProducts();
          
});

var ViewProducts = function () {
    this.gridViewProducts = null;
    this.init();
};

ViewProducts.prototype = {
    init: function () {
        var self = this;
        self.initViewProducts();
    },
    initViewProducts: function () { 

        this.gridViewProducts = $('#productsList').jqGrid({
            colNames:  ['ID', 'Item name', 'Item description', 'Item unit price'],
            colModel:  [
            { name: 'ID', index: 'ID', hidden: true, search: false },
            { name: 'ProductName', index: 'ProductName', width: 325, sortable: true, searchoptions: { sopt: ['eq', 'ne', 'cn']} },
            { name: 'ProductDescription', index: 'ProductDescription', width: 285, sortable: true, searchoptions: { sopt: ['eq', 'ne', 'cn'] }, search: true },
            { name: 'UnitPrice', index: 'UnitPrice', width: 150, sortable: true, searchoptions: { sopt: ['eq', 'ge', 'le'] }, search: true }
            ],
            pager: jQuery('#productsListPager'),
            sortname: 'ProductName',
            rowNum: 10,
            rowList: [10, 20, 50],
            sortorder: "asc",
            width: 1000,
            height: 400,
            datatype: 'json',
            caption: 'Products list',
            viewrecords: true,
            mtype: 'Post',
            jsonReader: {
                root: "Rows",
                page: "Page",
                total: "Total",
                records: "Records",
                repeatitems: false,
                userdata: "UserData"
            },
            gridComplete: function () {
               var ids = jQuery("#productsList").jqGrid('getDataIDs');
            },
            url: "/Product/GetData"
        }).navGrid('#productsListPager', { view: false, del: false, add: false, edit: false },
       {}, // default settings for edit
       {}, // default settings for add
       {}, // delete instead that del:false we need this
       {closeOnEscape: true, multipleSearch: true, closeAfterSearch: true }, // search options
       {} /* view parameters*/

     )
    }
}