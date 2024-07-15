class KendoObject {
    constructor(elementId, kendoElementType) {
        this.elementId = elementId;
        this.kendoElementType = kendoElementType;
        this.kendoConstractor = $(this.elementId).data(this.kendoElementType);
        this.dataSource = $(this.elementId).data(kendoElementType).dataSource;
        this.data = $(this.elementId).data(kendoElementType).dataSource.data();


    }
    setHeight(height) {
        debugger;
        this.kendoConstractor.setOptions({
            height: height
        });
    }

    refreshGrid() {
        this.dataSource.read();
    }

    getAllTrRow() {
        return $(this.elementId).data(this.kendoElementType).element.children('div.k-grid-content').children().children('tbody').children()
    }

    rowActive(rowActive) {
        this.rowActivate = rowActive;

    }

    cellRemoveDisabledByClass(cellNumber, fromClass) {
        var cellSelected = $($(this.rowActivate).children()[cellNumber]);
        var acceptBtn = cellSelected.children(fromClass)
        $(acceptBtn).removeAttr('disabled');
    }

    changeClassToClass(cellNumber, fromClass, ToClass, title, functionDelete, disabled = true) {

        var cellSelected = $($(this.rowActivate).children()[cellNumber]);
        var acceptBtn = cellSelected.children(fromClass)
        $(acceptBtn).removeAttr('class');
        $(acceptBtn).removeAttr('onclick');
        $(acceptBtn).addClass(ToClass);
        $(acceptBtn).empty();
        $(acceptBtn).append(`<span>${title}</span>`);
        if (disabled) {
            $(acceptBtn).attr('disabled', 'disabled');
        }
        $(acceptBtn).attr('onclick', functionDelete);
        $(acceptBtn).attr('title', title);
    }

    removeElementByClass(cellNumber, btnClassName = "") {
        var cellSelect = $($(this.rowActivate).children()[cellNumber]);
        if (btnClassName !== "") {
            cellSelect.children(btnClassName).remove()
        } else {
            cellSelect.empty()
        }
    }

    addClassToRowsSelected(rows, className, cellNumber = 2) {

        for (cellNumber; cellNumber < $(rows).children().length; cellNumber++) {
            $($(rows).children()[cellNumber]).addClass(className)
        }

    }

    removeClassFromAllRows(rows, className, cellNumber = 2) {


        var tr = this.getAllTrRow().find('.' + className)
        for (var i = 0; i < tr.length; i++) {
            $(tr[i]).removeClass('hoverTd')
        }
    }

    resetStyleTreeList(rows) {

        for (var i = 0; i <= rows.length; i++) {

            this.kendoConstractor.expand($("#treelist tbody>tr:eq(0)"))
            if ($(rows[i]).hasClass('k-treelist-group')) {
                $(rows[i]).find('.fa-address-card').remove()
            } else {
                $(rows[i]).find('.fa-sitemap').remove()
            }
        }

        var tr = this.getAllTrRow();
        for (var i = 1; i <= tr.length; i++) {
            if ($(tr[i]).hasClass('k-treelist-group')) {
                var row = this.kendoConstractor.element.find("tr[data-uid=" + $(tr[i]).attr("data-uid") + "]")
                this.kendoConstractor.collapse(row)
            }

        }

    }

    getselectedUID() {
        var uid = $(this.elementId).data(this.kendoElementType).select().data().uid;
        return uid
    }

    getSelectTrRow() {
        return $(this.elementId).data(this.kendoElementType).element.children('div.k-grid-content').children().children('tbody').children('.k-state-selected')
    }

    focusToFirstNumberColumRowSelected(numberCell) {
        var trr = $(this.elementId).data(this.kendoElementType).select()[0]
        $(trr)[0].cells[numberCell].click()
    }


    getCellOfSelectedRow(numberCell) {
        var trr = $(this.elementId).data(this.kendoElementType).select()[0]
        return $(trr)[0].cells[numberCell]
    }

    focusToColumRowActived(numberCell) {
        var cellSelected = this.getCellOfRowActive(numberCell)
        $(cellSelected).click()
    }

    getCellOfRowActive(cellNumber) {
        var cellSelected = $($(this.rowActivate).children()[cellNumber]);
        return cellSelected;
    }

    getElementOfCellNumberByClassName(numberCell, classElement) {

        var trr = $(this.elementId).data(this.kendoElementType).select()[0]

        var cellObject = $(trr).cells[numberCell];

        return $(cellObject).children(classElement)

    }
    getElementOfCellNumberByClassNameInRowActive(numberCell, classElement) {

        var cellObject = this.getCellOfRowActive(numberCell)

        return $(cellObject).children(classElement)

    }
    setValueToCellNumberByClassName(numberCell, classElement, value) {

        var cell = this.getElementOfCellNumberByClassNameInRowActive(numberCell, classElement)

        cell.html(value)
    }
    enableSelectedRow(cellNumber) {
        var allRowGrid = $(this.elementId).data(this.kendoElementType).element.children('div.k-grid-content').children().children('tbody').children()

        for (var i = 0; i < allRowGrid.length; i++) {
            var item = $(allRowGrid[i])
            $(allRowGrid[i]).removeClass('k-alt')
            if (item.hasClass('k-state-selected')) {
                $(item.children()[1]).children('td').css({
                    "background-color": "#e5e5e5 !important;"
                })
                $(item.children()[cellNumber]).children().removeAttr('disabled', 'disabled')
            } else {
                $(item.children()[cellNumber]).children().attr('disabled', 'disabled')
            }
            console.log(item)
        }

    }

    removeAltFromGrid() {
        var allTrRow = this.getAllTrRow();
        for (var i = 0; i < allTrRow.length; i++) {
            $(allTrRow[i]).removeClass('k-alt')

        }
    }


    getKendoDataItems(ismultiSelects = false) {

        return ismultiSelects ? $(this.elementId).data(this.kendoElementType).dataItems() : $(this.elementId).data(this.kendoElementType).dataItem();
    }

    getDataItemFromSelectedRow() {
        var uid = $(this.elementId).data(this.kendoElementType).select().data().uid;
        var filteredlist = $(this.elementId).data(this.kendoElementType).dataSource.data().filter(a => a.uid == uid)[0]
        return filteredlist;
    }

    getOneFieldFromDataITems(fieldName) {
        return this.getKendoDataItems(true).map(a => a[fieldName])
    }

    convertTimeDurationToMinute(timeDuration) {
        var timeString = timeDuration;
        var timeArray = timeString.split(":");
        var hours = parseInt(timeArray[0]);
        var minute = parseInt(timeArray[1]);
        var totalMinute = hours * 60 + minute;
        return totalMinute;
    }

    getSumNumberFromObject(fieldName) {
        let sum = 0;
        var ArrayItems = this.getOneFieldFromDataITems(fieldName)
        ArrayItems.forEach(function (item) {
            sum += item;
        })
        return sum;

    }

    getSumStringToNumberFromObject(fieldName) {

        let sum = 0;
        var ArrayItems = this.data.map(a => a[fieldName])
        ArrayItems.forEach(function (item) {
            sum += parseFloat(item);
        })
        return sum;

    }

    getSumDataWithCondition(fieldName, filterList = []) {
        /*
        filterlist=[{
                FieldName:"IsEffective",
                operation:"!==",
                value:1
            }]
         * */
        let sum = 0;
        var dataFiltered = this.filterData(filterList)
        var ArrayItems = dataFiltered.map(a => a[fieldName])
        ArrayItems.forEach(function (item) {
            sum += parseFloat(item);
        })
        return sum;

    }

    filterData(filterList) {
        var serachdata = this.data;
        filterList.forEach(function (x) {

            let op = x.operation
            switch (op) {
                case '==':
                    serachdata = serachdata.filter(c => c[x.FieldName] == x.value);
                    break;

                case '!==':
                    serachdata = serachdata.filter(c => c[x.FieldName] !== x.value)
                    break;
            }
        })
        return serachdata;
    }




    filterDataWithOperationParent(filterList) {
        if (filterList.operationParent == "or") {
            var datafinal = filterListOR(filterList.conditionParent)
            return datafinal;
        } else {
            var datafinal = filterListAnd(filterList.conditionParent)
            return datafinal;
        }

    }

    filterListOR = (filterdata) => {
        filterdata.forEach(function (x) {
            var serachdata = [];
            let op = x.operation
            switch (op) {
                case '==':
                    var item = this.data.filter(c => c[x.FieldName] == x.value);
                    if (item.length > 0) serachdata.push(item)
                    break;

                case '!==':
                    var item = this.data.filter(c => c[x.FieldName] !== x.value)
                    if (item.length > 0) serachdata.push(item)
                    break;
            }
        })
        return serachdata;
    }
    filterListAnd = (filterdata) => {
        filterdata.forEach(function (x) {
            var serachdata = this.data;
            let op = x.operation
            switch (op) {
                case '==':
                    serachdata = serachdata.filter(c => c[x.FieldName] == x.value);
                    break;

                case '!==':
                    serachdata = serachdata.filter(c => c[x.FieldName] !== x.value)
                    break;
            }
        })
        return serachdata;
    }


    removeBtnEdit(cellNumber, btnClassName = "") {
        var cellSelect = $($(this.rowActivate).children()[cellNumber]);
        if (btnClassName !== "") {
            cellSelect.children(btnClassName).remove()
        } else {
            cellSelect.empty()
        }
    }

    changeAcceptBtnToDelete(cellNumber, acceptBtnClassName) {

        var cellSelected = $($(this.rowActivate).children()[cellNumber]);
        var acceptBtn = cellSelected.children(acceptBtnClassName)
        $(acceptBtn).removeAttr('class');
        $(acceptBtn).addClass('btn btn-danger btn-sm btn-ksc-menu ');
        $(acceptBtn).empty();
        $(acceptBtn).append(`<span>X</span>`);
        $(acceptBtn).attr('disabled', 'disabled');
        $(acceptBtn).attr('onclick', "RemoveBtn(this,event)");
        $(acceptBtn).attr('title', 'انصراف از تایید');
    }

    changeAcceptBtnToDeleteOptional(cellNumber, acceptBtnClassName, functionDelete, disabled = true) {

        var cellSelected = $($(this.rowActivate).children()[cellNumber]);
        var acceptBtn = cellSelected.children(acceptBtnClassName)
        $(acceptBtn).removeAttr('class');
        $(acceptBtn).removeAttr('onclick');
        $(acceptBtn).addClass('btn btn-danger btn-sm btn-ksc-menu ');
        $(acceptBtn).empty();
        $(acceptBtn).append(`<span>X</span>`);
        if (disabled) {
            $(acceptBtn).attr('disabled', 'disabled');
        }
        $(acceptBtn).attr('onclick', functionDelete);
        $(acceptBtn).attr('title', 'انصراف از تایید');
    }

    changeTextColorRowsSelected(rows, className) {

        for (var j = 2; j < $(rows).children().length; j++) {
            $($(rows).children()[j]).addClass(className)
        }

    }

    getSumTimeDurationToMinute(fieldName) {
        let sum = 0;
        let ArrayItems = this.getOneFieldFromDataITems(fieldName)
        ArrayItems.forEach(function (item) {
            let timeString = item;
            let timeArray = timeString.split(":");
            let hours = parseInt(timeArray[0]);
            let minute = parseInt(timeArray[1]);
            let totalMinute = hours * 60 + minute;

            sum += totalMinute;
        })
        return sum;

    }

    getSumTimeMinute(fieldName) {
        let sum = 0;
        var ArrayItems = this.getOneFieldFromDataITems(fieldName)
        ArrayItems.forEach(function (item) {

            sum += item;
        })
        return sum;
    }

    selectBackRow(cellNumber = 0) {
        var allRowGrid = this.kendoConstractor.element.children('div.k-grid-content').children().children('tbody').children()
        var isfindSelect = false;
        for (i = 0; i < allRowGrid.length; i++) {
            var item = $(allRowGrid[i])

            if (item.hasClass('k-state-selected')) {
                if (i == 0) return;
                item.removeClass('k-state-selected')
                $(allRowGrid[i - 1]).addClass('k-state-selected')
                $($(this.kendoConstractor.select().first()[0]).children()[cellNumber]).click()
                $($(this.kendoConstractor.select().first()[0]).children()[cellNumber]).find(".k-input-inner").focus()
                return;
            }
            console.log(item)
        }

    }

    moveToNextRowAndCurrentCell(cellNumber = 0) {
        var allRowGrid = this.kendoConstractor.element.children('div.k-grid-content').children().children('tbody').children()

        if (allRowGrid.hasClass('k-state-selected') == false) {
            $(allRowGrid[0]).addClass('k-state-selected')
        } else {
            var countGrid = allRowGrid.length;
            for (let i = 0; i < countGrid; i++) {
                var item = $(allRowGrid[i])
                if (item.hasClass('k-state-selected')) {
                    if (i == countGrid - 1) return;
                    item.removeClass('k-state-selected')
                    $(allRowGrid[i + 1]).addClass('k-state-selected')

                    $($(this.kendoConstractor.select().first()[0]).children()[cellNumber]).click()
                    $($(this.kendoConstractor.select().first()[0]).children()[cellNumber]).find(".k-input-inner").focus()
                    return;
                }
            }
        }

    }

    findTrWithFilterBox(text, allTr, classhover) {
        if (text !== '') {

            for (var i = 1; i < allTr.length; i++) {
                if ($(allTr[i]).hasClass('k-treelist-group')) {
                    $(allTr[i]).find('.fa-address-card').remove()
                } else {
                    $(allTr[i]).find('.fa-sitemap').remove()
                }
                var truid = $(allTr[i]).attr('data-uid');
                var selectData = this.data.filter(a => a.uid == truid)[0];
                if (selectData.Name.includes(text)) {

                    var selectPArent = this.data.filter(a => a.id == selectData.ParentId)[0]
                    if (selectPArent !== undefined) {
                        var row = this.kendoConstractor.element.find("tr[data-uid=" + selectPArent.uid + "]")
                        this.kendoConstractor.expand(row)
                    }

                    this.addClassToRowsSelected(allTr[i], classhover, 0)

                }
            }
        } else {

        }
    }
}

class kscGrid extends kendo.ui.Grid {

    _firstUid;
    constructor(element, options) {
        if (options.autoBind == undefined || options.autoBind == true) {
            options.autoBind = false
        }

        if (options.removeAlt == undefined) {
            options.removeAlt = false
        }

        if (options.singleCollapsed == undefined) {
            options.singleCollapsed = false
        }

        super($(element), options);
        if (options.change == undefined) {
            var customFunction = function () {

                if (this.getOptions().selectable == true) {
                    if (this._firstUid == this.getselectedUID()) return;
                    this._firstUid = this.getselectedUID()
                }

            }
            this.bind("change", customFunction)

        }
        this.content.scrollLeft(9999999)

        if (this.getOptions().removeAlt == true) {
            this.removeAltFromGrid();
        }

        if (this.getOptions().singleCollapsed == true) {
            this.GridCollapseRow()
        }
        this._firstUid = "";
    }


    GridCollapseRow() {
        $(".k-hierarchy-cell a.k-icon").bind('click', function (e) {
            var isExpand = $(e.target).hasClass("k-i-collapse");
            if (!isExpand) {
                var allMasterRows = this.tbody.find(">tr.k-master-row");
                for (var i = 0; i < allMasterRows.length; i++) {
                    if (allMasterRows.eq(i).next('tr.k-detail-row').length > 0) {
                        grid.collapseRow(allMasterRows.eq(i));
                    }
                }
            }
        });
    }


    scrollleft() {
        this.content.scrollLeft(9999999)
    }

    setHeight(height) {
        this.setOptions({
            height: height
        });
    }

    bindChange(nameFunction) {
        this.unbind("change")
        var customFunction = function () {
            debugger;
            if (this.getOptions().selectable == true) {
                if (this._firstUid == this.getselectedUID()) return;
                this._firstUid = this.getselectedUID()
            }
            return eval(nameFunction)
        }
        this.bind("change", customFunction)
    }

    bindDataBound(nameFunction) {
        //var customFunction = function () {
        //    debugger;
        //    if (this.getOptions().selectable == true) {
        //        if (firstuid == this.getselectedUID()) return;
        //        firstuid = this.getselectedUID()
        //    }
        //    return eval(nameFunction)
        //}
        this.bind("dataBound", customFunction)
    }

    refreshGrid() {
        this.dataSource.read();
    }

    getAllTrRow() {
        return this.element.children('div.k-grid-content').children().children('tbody').children()
    }

    rowActive(rowActive) {
        this.rowActivate = rowActive;

    }

    cellRemoveDisabledByClass(cellNumber, fromClass) {
        var cellSelected = $($(this.rowActivate).children()[cellNumber]);
        var acceptBtn = cellSelected.children(fromClass)
        $(acceptBtn).removeAttr('disabled');
    }

    changeClassToClass(cellNumber, fromClass, ToClass, title, functionDelete, disabled = true) {

        var cellSelected = $($(this.rowActivate).children()[cellNumber]);
        var acceptBtn = cellSelected.children(fromClass)
        $(acceptBtn).removeAttr('class');
        $(acceptBtn).removeAttr('onclick');
        $(acceptBtn).addClass(ToClass);
        $(acceptBtn).empty();
        $(acceptBtn).append(`<span>${title}</span>`);
        if (disabled) {
            $(acceptBtn).attr('disabled', 'disabled');
        }
        $(acceptBtn).attr('onclick', functionDelete);
        $(acceptBtn).attr('title', title);
    }

    removeElementByClass(cellNumber, btnClassName = "") {
        var cellSelect = $($(this.rowActivate).children()[cellNumber]);
        if (btnClassName !== "") {
            cellSelect.children(btnClassName).remove()
        } else {
            cellSelect.empty()
        }
    }

    addClassToRowsSelected(rows, className, cellNumber = 2) {

        for (cellNumber; cellNumber < $(rows).children().length; cellNumber++) {
            $($(rows).children()[cellNumber]).addClass(className)
        }

    }

    removeClassFromAllRows(rows, className, cellNumber = 2) {


        var tr = this.getAllTrRow().find('.' + className)
        for (var i = 0; i < tr.length; i++) {
            $(tr[i]).removeClass('hoverTd')
        }
    }

    resetStyleTreeList(rows) {

        for (var i = 0; i <= rows.length; i++) {

            this.expand($("#treelist tbody>tr:eq(0)"))
            if ($(rows[i]).hasClass('k-treelist-group')) {
                $(rows[i]).find('.fa-address-card').remove()
            } else {
                $(rows[i]).find('.fa-sitemap').remove()
            }
        }

        var tr = this.getAllTrRow();
        for (var i = 1; i <= tr.length; i++) {
            if ($(tr[i]).hasClass('k-treelist-group')) {
                var row = this.element.find("tr[data-uid=" + $(tr[i]).attr("data-uid") + "]")
                this.collapse(row)
            }

        }

    }


    getSelectTrRow() {
        return this.element.children('div.k-grid-content').children().children('tbody').children('.k-state-selected')
    }

    focusToFirstNumberColumRowSelected(numberCell) {
        var trr = this.select()[0]
        $(trr)[0].cells[numberCell].click()
    }


    getCellOfSelectedRow(numberCell) {
        var trr = this.select()[0]
        return $(trr)[0].cells[numberCell]
    }

    focusToColumRowActived(numberCell) {
        var cellSelected = this.getCellOfRowActive(numberCell)
        $(cellSelected).click()
    }

    getCellOfRowActive(cellNumber) {
        var cellSelected = $($(this.rowActivate).children()[cellNumber]);
        return cellSelected;
    }

    getElementOfCellNumberByClassName(numberCell, classElement) {

        var trr = this.select()[0]

        var cellObject = $(trr).cells[numberCell];

        return $(cellObject).children(classElement)

    }
    getElementOfCellNumberByClassNameInRowActive(numberCell, classElement) {

        var cellObject = this.getCellOfRowActive(numberCell)

        return $(cellObject).children(classElement)

    }
    setValueToCellNumberByClassName(numberCell, classElement,value) {

        var cell = this.getElementOfCellNumberByClassNameInRowActive(numberCell, classElement)
        
        cell.html(value)
    }
    enableSelectedRow(cellNumber) {
        var allRowGrid = this.element.children('div.k-grid-content').children().children('tbody').children()

        for (var i = 0; i < allRowGrid.length; i++) {
            var item = $(allRowGrid[i])
            $(allRowGrid[i]).removeClass('k-alt')
            if (item.hasClass('k-state-selected')) {
                $(item.children()[1]).children('td').css({
                    "background-color": "#e5e5e5 !important;"
                })
                $(item.children()[cellNumber]).children().removeAttr('disabled', 'disabled')
            } else {
                $(item.children()[cellNumber]).children().attr('disabled', 'disabled')
            }
            console.log(item)
        }

    }

    removeAltFromGrid() {
        var allTrRow = this.getAllTrRow();
        for (var i = 0; i < allTrRow.length; i++) {
            $(allTrRow[i]).removeClass('k-alt')

        }
    }

    selectBackRow(cellNumber = 0) {
        var allRowGrid = this.element.children('div.k-grid-content').children().children('tbody').children()
        var isfindSelect = false;
        for (i = 0; i < allRowGrid.length; i++) {
            var item = $(allRowGrid[i])

            if (item.hasClass('k-state-selected')) {
                if (i == 0) return;
                item.removeClass('k-state-selected')
                $(allRowGrid[i - 1]).addClass('k-state-selected')
                $($(this.kendoConstractor.select().first()[0]).children()[cellNumber]).click()
                $($(this.kendoConstractor.select().first()[0]).children()[cellNumber]).find(".k-input-inner").focus()
                return;
            }
            console.log(item)
        }

    }

    moveToNextRowAndCurrentCell(cellNumber = 0) {
        var allRowGrid = this.element.children('div.k-grid-content').children().children('tbody').children()

        if (allRowGrid.hasClass('k-state-selected') == false) {
            $(allRowGrid[0]).addClass('k-state-selected')
        } else {
            var countGrid = allRowGrid.length;
            for (let i = 0; i < countGrid; i++) {
                var item = $(allRowGrid[i])
                if (item.hasClass('k-state-selected')) {
                    if (i == countGrid - 1) return;
                    item.removeClass('k-state-selected')
                    $(allRowGrid[i + 1]).addClass('k-state-selected')

                    $($(this.kendoConstractor.select().first()[0]).children()[cellNumber]).click()
                    $($(this.kendoConstractor.select().first()[0]).children()[cellNumber]).find(".k-input-inner").focus()
                    return;
                }
            }
        }

    }



    ////----------------------------------------data SourceMEthod-----------------------------------------------/////
    get getViewData() {
        return this.dataSource.view();
    }
    get getDataSource() {
        return this.dataSource;
    }

    get getData() {
        return this.dataSource.data();
    }


    getselectedUID() {
        var uid = this.select().data().uid;
        return uid
    }


    getKendoDataItems(ismultiSelects = false) {

        return ismultiSelects ? $(this.elementId).data(this.kendoElementType).dataItems() : $(this.elementId).data(this.kendoElementType).dataItem();
    }

    getDataItemFromSelectedRow() {
        var uid = $(this.elementId).data(this.kendoElementType).select().data().uid;
        var filteredlist = $(this.elementId).data(this.kendoElementType).dataSource.data().filter(a => a.uid == uid)[0]
        return filteredlist;
    }

    getOneFieldFromDataITems(fieldName) {
        return this.getKendoDataItems(true).map(a => a[fieldName])
    }

    convertTimeDurationToMinute(timeDuration) {
        var timeString = timeDuration;
        var timeArray = timeString.split(":");
        var hours = parseInt(timeArray[0]);
        var minute = parseInt(timeArray[1]);
        var totalMinute = hours * 60 + minute;
        return totalMinute;
    }

    getSumNumberFromObject(fieldName) {
        let sum = 0;
        var ArrayItems = this.getOneFieldFromDataITems(fieldName)
        ArrayItems.forEach(function (item) {
            sum += item;
        })
        return sum;

    }

    getSumStringToNumberFromObject(fieldName) {

        let sum = 0;
        var ArrayItems = this.data.map(a => a[fieldName])
        ArrayItems.forEach(function (item) {
            sum += parseFloat(item);
        })
        return sum;

    }

    getSumDataWithCondition(fieldName, filterList = []) {
        /*
        filterlist=[{
                FieldName:"IsEffective",
                operation:"!==",
                value:1
            }]
         * */
        let sum = 0;
        var dataFiltered = this.filterData(filterList)
        var ArrayItems = dataFiltered.map(a => a[fieldName])
        ArrayItems.forEach(function (item) {
            sum += parseFloat(item);
        })
        return sum;

    }

    filterData(filterList) {
        var serachdata = this.data;
        filterList.forEach(function (x) {

            let op = x.operation
            switch (op) {
                case '==':
                    serachdata = serachdata.filter(c => c[x.FieldName] == x.value);
                    break;

                case '!==':
                    serachdata = serachdata.filter(c => c[x.FieldName] !== x.value)
                    break;
            }
        })
        return serachdata;
    }




    filterDataWithOperationParent(filterList) {
        if (filterList.operationParent == "or") {
            var datafinal = filterListOR(filterList.conditionParent)
            return datafinal;
        } else {
            var datafinal = filterListAnd(filterList.conditionParent)
            return datafinal;
        }

    }

    filterListOR = (filterdata) => {
        filterdata.forEach(function (x) {
            var serachdata = [];
            let op = x.operation
            switch (op) {
                case '==':
                    var item = this.data.filter(c => c[x.FieldName] == x.value);
                    if (item.length > 0) serachdata.push(item)
                    break;

                case '!==':
                    var item = this.data.filter(c => c[x.FieldName] !== x.value)
                    if (item.length > 0) serachdata.push(item)
                    break;
            }
        })
        return serachdata;
    }
    filterListAnd = (filterdata) => {
        filterdata.forEach(function (x) {
            var serachdata = this.data;
            let op = x.operation
            switch (op) {
                case '==':
                    serachdata = serachdata.filter(c => c[x.FieldName] == x.value);
                    break;

                case '!==':
                    serachdata = serachdata.filter(c => c[x.FieldName] !== x.value)
                    break;
            }
        })
        return serachdata;
    }


    getSumTimeDurationToMinute(fieldName) {
        let sum = 0;
        let ArrayItems = this.getOneFieldFromDataITems(fieldName)
        ArrayItems.forEach(function (item) {
            let timeString = item;
            let timeArray = timeString.split(":");
            let hours = parseInt(timeArray[0]);
            let minute = parseInt(timeArray[1]);
            let totalMinute = hours * 60 + minute;

            sum += totalMinute;
        })
        return sum;

    }

    getSumTimeMinute(fieldName) {
        let sum = 0;
        var ArrayItems = this.getOneFieldFromDataITems(fieldName)
        ArrayItems.forEach(function (item) {

            sum += item;
        })
        return sum;
    }



}
