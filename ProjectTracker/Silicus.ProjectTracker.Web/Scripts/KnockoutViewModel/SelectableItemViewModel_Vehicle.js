function Item(titleText, itemId, brand, isSelected) {
    this.title = ko.observable(titleText);
    this.brand = ko.observable(brand);
    this.isSelected = ko.observable(isSelected);
    this.itemId = ko.observable(itemId);
}

var SelectableItemViewModel_Vehicle = function (items) {
    // debugger;
    // Data
    
    var self = this;
    self.filter = ko.observable("");
    self.isExcluded = ko.observable(items.IsExcluded);
    self.filterKeyword = ko.observable("");
    self.selectAll = ko.observable(false);

    self.availableItems_Vehicle = ko.observableArray(ko.utils.arrayMap(items.AvailableItemsVehicleModel, function(item) {
        return new Item(item.ItemName, item.ItemId, item.Brand, item.IsSelected);
    }));

    self.selectedItems_Vehicle = ko.computed(function() {
        return ko.utils.arrayFilter(self.availableItems_Vehicle(), function(item) {
            return item.isSelected();
        });
    });

    //filter the items using the filter text
    self.filteredItems_Vehicle = ko.dependentObservable(function() {

        var filterKeyword = this.filterKeyword().toLowerCase();
        var selectedBrand = self.filter().toLowerCase();

        if (filterKeyword != '') {

            if (selectedBrand) {
                return ko.utils.arrayFilter(this.availableItems_Vehicle(), function(item) {
                    return ((ko.utils.stringStartsWith(item.brand().toLowerCase(), selectedBrand.toLowerCase())) && (item.title().toLowerCase().indexOf(filterKeyword.toLowerCase()) !== -1));
                });
            } else {

                return ko.utils.arrayFilter(this.availableItems_Vehicle(), function (item) {
                    return (item.title().toLowerCase().indexOf(filterKeyword.toLowerCase()) !== -1);
                });
            }
        }

        if (!selectedBrand) {
            return this.availableItems_Vehicle();
        } else {

            if (selectedBrand.toLowerCase() == "brand") {
                return this.availableItems_Vehicle();
            } else {
                return ko.utils.arrayFilter(this.availableItems_Vehicle(), function(item) {
                    return (item.brand().toLowerCase().indexOf(selectedBrand.toLowerCase()) !== -1);
                });
            }
        }
    }, self);

    // Operations
    self.removeItem_Vehicle = function(removedItem) {
        ko.utils.arrayForEach(self.availableItems_Vehicle(), function(item) {
            if (item.title === removedItem.title) {

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
        ko.utils.arrayForEach(this.filteredItems_Vehicle(), function (item) {
            item.isSelected(newValue);
        });
    }, self);
}