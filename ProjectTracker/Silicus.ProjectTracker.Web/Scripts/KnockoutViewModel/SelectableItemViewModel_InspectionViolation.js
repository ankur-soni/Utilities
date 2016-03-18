function Item(titleTextViolFamily,violationFamily, itemIdViolFamily, isSelectedViolFamily) {
    this.titleViolation = ko.observable(titleTextViolFamily);
    this.violationFamily = ko.observable(violationFamily);
    this.itemIdViolation = ko.observable(itemIdViolFamily);
    this.isSelectedViolation = ko.observable(isSelectedViolFamily);
}

var SelectableItemViewModelInspectionViolation = function (items) {
    // debugger;
    // Data
    var self = this;
    self.filter = ko.observable("");
    self.isExcluded = ko.observable(items.IsExcluded);
    self.filterKeyword = ko.observable("");
    self.selectAll = ko.observable(false);

    self.availableItemsViolation = ko.observableArray(ko.utils.arrayMap(items.AvailableItemsInspectionViolation, function (item) {
        
        return new Item(item.ItemName,item.ViolationFamily, item.ItemId, item.IsSelected);
    }));

    self.selectedItemsViolation = ko.computed(function () {
        return ko.utils.arrayFilter(self.availableItemsViolation(), function (item) {
            return item.isSelectedViolation();
        });
    });

    //filter the items using the filter text
    self.filteredItemsViolation = ko.dependentObservable(function () {
        // debugger;
        var filter = this.filter().toLowerCase();
        var filterKeyword = this.filterKeyword().toLowerCase();
        var selectedFamily = $("#violationDropdownSelect").val();
        var selectedFilterKeywords = $("#txtmoverbox1").val();
        


        //if (!filterKeyword) {


        //} else {
        //    if (selectedFamily.toLowerCase() == "Select Family") {
        //        return this.availableItemsViolation();
        //    }
        //    else if(selectedFamily.toLowerCase() != "Select Family")
        //        return ko.utils.arrayFilter(this.availableItemsViolation(), function (item) {
        //            return item.violationFamily().toLowerCase() === selectedFamily.toLowerCase();
        //        });
        //    }
            
      
        if (!filter)

        {
            return this.availableItemsViolation();
        }
        else
        {
            // debugger;
            if (filter.toLowerCase() == "Select Family...".toLowerCase()) {
                return this.availableItemsViolation();
            }
            else if (selectedFamily.toLowerCase() == filter.toLowerCase() && selectedFamily.toLowerCase() != "Select Family...".toLowerCase())
            {
                return ko.utils.arrayFilter(this.availableItemsViolation(), function (item) {

                return item.violationFamily().toLowerCase() === selectedFamily.toLowerCase();

                 
                });
               
            }
           else {
                return ko.utils.arrayFilter(this.availableItemsViolation(), function (item) {

                    //return item.violationFamily().toLowerCase() === selectedFamily.toLowerCase();

                    return item.titleViolation().toLowerCase().indexOf(filter) !== -1;
                });
            }
        }
    }, self);

    // Operations
    self.removetitleViolation = function (removedItem) {
        // debugger;
        ko.utils.arrayForEach(self.availableItemsViolation(), function (item) {
            if (item.titleViolation === removedItem.titleViolation) {

                $(".gencontainer .moverleft li").each(function () {
                    if ($(this).find("span").text() == removedItem.titleViolation()) {
                        $(this).removeClass('liselectedbgcolor');
                        $(this).addClass('liselectednobgcolor');
                    }
                });

                item.isSelectedViolation(false);
            }
        });
    };

    self.selectAll.subscribe(function (newValue) {
        ko.utils.arrayForEach(this.filteredItemsViolation(), function (item) {
            item.isSelectedViolation(newValue);
        });
    }, self);
}
