
$(document).ready(function () {

    $.ajax({
        type: "GET",
        url: "/Admin/GetCustomerType",
        datatype: "Json",
        success: function (data) {
            $.each(data, function (index, value) {
                $("#CustomerType").append(
                    '<option value="' + value.id + '">' + value.name + "</option>"
                );
            });
        },
    });
});
