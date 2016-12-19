(function () {
    'use strict';
    function getsearchContain(searchString) {

        var url = "/FrameworxProject/Details";
        $.get(url, { searchString: searchString }, function (data) {

        });

    }
    function searchComponent(value) {
        var url = "/FrameworxProject/SearchComponent";
        $.get(url, { searchString: value }, function (data) {
            $("#containerDiv").html(data);
        }).done(function () {
            $('#containerDiv').unblock();
        });
    }

    $(document).ready(
        function () {
            var timer;
            $("#searchString").on('keyup', function () {
                $('#containerDiv').unblock();
                $('#containerDiv').block({
                    message: '<h1>Searching</h1>',
                    css: {
                        border: 'none',
                        padding: '15px',
                        backgroundColor: '#fff',
                        '-webkit-border-radius': '10px',
                        '-moz-border-radius': '10px',
                        color: '#000'
                    }
                });
                clearTimeout(timer);
                var ms = 1000; // milliseconds
                var val = this.value.trim();
                timer = setTimeout(function () {
                    searchComponent(val);
                }, ms);
            });


            $('#searchString').on('input propertychange', function () {
                var $this = $(this);
                var visible = Boolean($this.val());
                $this.siblings('.form-control-clear').toggleClass('hidden', !visible);
            }).trigger('propertychange');

            $('.form-control-clear').click(function () {
                $(this).siblings('input[type="text"]').val('')
                  .trigger('propertychange').trigger('keyup').focus();
            });
        }
        );

})();
