function Item(titleText,itemId, isSelected) {
    this.title = ko.observable(titleText);
    this.itemId = ko.observable(itemId);
    this.isSelected = ko.observable(isSelected);
}

var SelectableItemViewModel = function (items) {
    // Data
    var self = this;
    self.isExcluded = ko.observable(items.IsExcluded);
    self.filter = ko.observable("");
    self.selectAll = ko.observable(false);
    self.availableItems = ko.observableArray(ko.utils.arrayMap(items.AvailableItems, function (item) {
        return new Item(item.ItemName, item.ItemId, item.IsSelected);
    }));

    self.selectedItems = ko.computed(function () {
        return ko.utils.arrayFilter(self.availableItems(), function (item) {
            return item.isSelected();
        });
    });

    //filter the items using the filter text
    self.filteredItems = ko.dependentObservable(function () {
        
        var filter = this.filter().toLowerCase();
        if (!filter) {
            return this.availableItems();
        } else {

            var filterFrom = $("#hfFilterType").val();
            $("#hfFilterType").val('text');
            // console.log(" filter from == " + filterFrom);
            if (filterFrom == 'select') {
                return ko.utils.arrayFilter(this.availableItems(), function (item) {
                    return ko.utils.stringStartsWith(item.title().toLowerCase(), filter);
                });
            } else {
                return ko.utils.arrayFilter(this.availableItems(), function (item) {
                    return item.title().toLowerCase().indexOf(filter) !== -1;
                });
            }
        }
    }, self);

    // Operations
    self.removeItem = function (removedItem) {
        ko.utils.arrayForEach(self.availableItems(), function (item) {
            if (item.title === removedItem.title) {
                //$(item).parent("li").removeClass('liselectedbgcolor');
                //$(item).parent("li").addClass("liselectednobgcolor");
                
                $(".gencontainer .moverleft li").each(function () {
                    if ($(this).find("span").text() == removedItem.title()) {
                        $(this).removeClass('liselectedbgcolor');
                        $(this).addClass('liselectednobgcolor');
                    }
                });

                item.isSelected(false);                
            }
        });
    };

    self.selectAll.subscribe(function (newValue) {
        ko.utils.arrayForEach(this.filteredItems(), function (item) {
            item.isSelected(newValue);
        });
    }, self);

}
