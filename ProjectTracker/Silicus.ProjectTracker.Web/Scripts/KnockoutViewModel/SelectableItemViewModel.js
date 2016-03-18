function Item(titleText, ProjectId, isSelected, isActive, isAssigned) {
    this.title = ko.observable(titleText);
    this.itemId = ko.observable(ProjectId);
    this.isSelected = ko.observable(isSelected);
    if (isActive == false)
       this.isActive = ko.observable(false);
    else
        if (isAssigned == false)
            this.isActive = ko.observable(false);
        else
            this.isActive = ko.observable(true);
       
  }

var SelectableItemViewModel = function (items) {
    // Data
    var self = this;
    self.isExcluded = ko.observable(items.IsExcluded);
    self.filter = ko.observable("");
    self.selectAll = ko.observable(false);
    self.availableItems = ko.observableArray(ko.utils.arrayMap(items.AvailableItems, function (item) {
        return new Item(item.ProjectName, item.ProjectId, item.IsSelected,item.IsActive,item.IsAssigned);
    }));

    self.selectedItems = ko.computed(function () {
        return ko.utils.arrayFilter(self.availableItems(), function (item) {
            return item.isSelected();
        });
    });

    self.UpdateItems = function (items)
    {
        self.availableItems.removeAll();
        ko.utils.arrayForEach(items.AvailableItems, function (item) {
            var newItem = new Item(item.ProjectName, item.ProjectId, item.IsSelected, item.IsActive,item.IsAssigned);
            self.availableItems.push(newItem)
        });
    }

    //filter the items using the filter text
    self.filteredItems = ko.dependentObservable(function () {
        
        var filter = this.filter().toLowerCase();
        if (!filter) {
            return this.availableItems();
        } else {
            return ko.utils.arrayFilter(this.availableItems(), function (item) {
                return item.title().toLowerCase().indexOf(filter) !== -1;
            });
        }
    }, self);

    //self.Guaranteed = ko.computed(function () {
    //    var result;
    //    result = self.IsActive;
    //    if (!result) {
    //        self.IsActive(false);
    //    }
    //    return result;
    //}, self);

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

