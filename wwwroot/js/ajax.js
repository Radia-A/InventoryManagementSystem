$(document).ready(function () {
    $('#filterForm').on('submit', function (e) {
        e.preventDefault();

        let formData = $(this).serialize();

        $.ajax({
            url: '/Products/FilteredList',
            type: 'GET',
            data: formData,
            success: function (result) {
                $('#productResults').html(result);
            },
            error: function () {
                alert('Something went wrong while loading products.');
            }
        });
    });
});
