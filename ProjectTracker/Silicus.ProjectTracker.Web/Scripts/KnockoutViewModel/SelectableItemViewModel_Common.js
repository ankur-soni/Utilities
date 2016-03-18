function ItemState(titleTextState, itemIdState, isSelectedState) {
    this.titleState = ko.observable(titleTextState);
    this.itemIdState = ko.observable(itemIdState);
    this.isSelectedState = ko.observable(isSelectedState);
}

function Item(titleText, itemId, isSelected) {
    this.title = ko.observable(titleText);
    this.itemId = ko.observable(itemId);
    this.isSelected = ko.observable(isSelected);
}

function ItemCountry(titleTextCountry, itemIdCountry, isSelectedCountry) {
    this.titleCountry = ko.observable(titleTextCountry);
    this.itemIdCountry = ko.observable(itemIdCountry);
    this.isSelectedCountry = ko.observable(isSelectedCountry);
}

var SelectableItemViewModelState = function (items) {
    // Data
    var self = this;
    self.isExcludedState = ko.observable(items.isExcludedState);
    self.filterState = ko.observable("");
    self.selectAllState = ko.observable(false);
    self.availableItemsState = ko.observableArray(ko.utils.arrayMap(items.AvailableItems, function (item) {
        return new ItemState(item.ItemName, item.ItemId, item.IsSelected);
    }));

    self.selectedItemsState = ko.computed(function () {
        return ko.utils.arrayFilter(self.availableItemsState(), function (item) {
            return item.isSelectedState();
        });
    });

    //filter the items using the filter text
    self.filteredItemsState = ko.dependentObservable(function () {
        var filter = this.filterState().toLowerCase();
        if (!filter) {
            return this.availableItemsState();
        } else {
            return ko.utils.arrayFilter(this.availableItemsState(), function (item) {
                return ko.utils.stringStartsWith(item.titleState().toLowerCase(), filter);
            });
        }
    }, self);

    // Operations
    self.removeItemState = function (removedItem) {
        ko.utils.arrayForEach(self.availableItemsState(), function (item) {
            if (item.titleState === removedItem.titleState) {

                $(".gencontainer .moverleft li").each(function () {
                    if ($(this).find("span").text() == removedItem.titleState()) {
                        $(this).removeClass('liselectedbgcolor');
                        $(this).addClass('liselectednobgcolor');
                    }
                });

                item.isSelectedState(false);
            }
        });
    };

    self.selectAllState.subscribe(function (newValue) {
        ko.utils.arrayForEach(this.filteredItemsState(), function (item) {
            item.isSelectedState(newValue);
        });
    }, self);
}

var SelectableItemViewModelCountry = function (items) {
    // Data
    var self = this;
    self.isExcludedCountry = ko.observable(items.isExcludedCountry);
    self.filterCountry = ko.observable("");
    self.selectAllCountry = ko.observable(false);
    self.availableItemsCountry = ko.observableArray(ko.utils.arrayMap(items.AvailableItems, function (item) {
        return new ItemCountry(item.ItemName, item.ItemId, item.IsSelected);
    }));

    self.selectedItemsCountry = ko.computed(function () {
        return ko.utils.arrayFilter(self.availableItemsCountry(), function (item) {
            return item.isSelectedCountry();
        });
    });

    //filter the items using the filter text
    self.filteredItemsCountry = ko.dependentObservable(function () {
        var filter = this.filterCountry().toLowerCase();
        if (!filter) {
            return this.availableItemsCountry();
        } else {
            return ko.utils.arrayFilter(this.availableItemsCountry(), function (item) {
                return ko.utils.stringStartsWith(item.titleCountry().toLowerCase(), filter);
            });
        }
    }, self);

    // Operations
    self.removeItemCountry = function (removedItem) {
        ko.utils.arrayForEach(self.availableItemsCountry(), function (item) {
            if (item.titleCountry === removedItem.titleCountry) {

                $(".gencontainer .moverleft li").each(function () {
                    if ($(this).find("span").text() == removedItem.titleCountry()) {
                        $(this).removeClass('liselectedbgcolor');
                        $(this).addClass('liselectednobgcolor');
                    }
                });

                item.isSelectedCountry(false);
            }
        });
    };

    self.selectAllCountry.subscribe(function (newValue) {
        ko.utils.arrayForEach(this.filteredItemsCountry(), function (item) {
            item.isSelectedCountry(newValue);
        });
    }, self);
}



var SelectableItemViewModelCounty1 = function (items) {
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
            return ko.utils.arrayFilter(this.availableItems(), function (item) {
                return ko.utils.stringStartsWith(item.title().toLowerCase(), filter);
            });
        }
    }, self);

    // Operations
    self.removeItem = function (removedItem) {
        ko.utils.arrayForEach(self.availableItems(), function (item) {
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
        ko.utils.arrayForEach(this.filteredItems(), function (item) {
            item.isSelected(newValue);
        });
    }, self);

}