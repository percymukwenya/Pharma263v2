var currentItemPrice;
$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: "/Supplier/GetSupplier",
        datatype: "Json",
        success: function (data) {
            $.each(data, function (index, value) {
                $("#Supplier").append(
                    '<option value="' + value.id + '">' + value.name + "</option>"
                );
            });
        },
    });

    $("#Medicine").select2({
        placeholder: "Search for a medicine...",
        allowClear: true,
        minimumInputLength: 2,
        ajax: {
            type: "GET",
            url: "/Medicine/SearchMedicines", // Absolute URL to API 
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    query: params.term || '',
                    page: params.page || 1
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;

                return {
                    results: $.map(data, function (item) {
                        var text = item.name;
                        if (item.brand) {
                            text += ' (' + item.brand + ')';
                        }

                        return {
                            id: item.id,
                            text: text,
                            item: item
                        };
                    }),
                    pagination: {
                        more: data.length === 20
                    }
                };
            },
            cache: true
        }
    });

    $.ajax({
        type: "GET",
        url: "/Admin/GetPaymentMethod",
        datatype: "Json",
        success: function (data) {
            $.each(data, function (index, value) {
                $("#PaymentMethod").append(
                    '<option value="' + value.id + '">' + value.name + "</option>"
                );
            });
        },
    });

    $.ajax({
        type: "GET",
        url: "/Admin/GetPurchaseStatus",
        datatype: "Json",
        success: function (data) {
            $.each(data, function (index, value) {
                $("#PurchaseStatus").append(
                    '<option value="' + value.id + '">' + value.name + "</option>"
                );
            });
        },
    });

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

    $("#Quantity").change(function () {
        blankme("Quantity");
    });
    $("#addToList").click(function (e) {
        e.preventDefault();
        if (add_validation()) {
            var productId = $("#Medicine option:selected").val(),
                productName = $("#Medicine option:selected").text(),
                price = currentItemPrice,
                quantity = $("#Quantity").val(),
                batchNumber = $("<input style='width: 100px;'>").attr("type", "text").val($("#BatchNumber").val()),
                expiryDate = $("<input style='width: 80px;'>").attr("type", "text").val($("#ExpiryDate").val()).datepicker(),
                buyingPrice = $("<input style='width: 50px;'>").attr("type", "text").val($("#BuyingPrice").val()).addClass("buyingPrice"),
                sellingPrice = $("<input style='width: 50px;'>").attr("type", "text").val($("#SellingPrice").val()),
                // Changed to display as percentage input with % symbol
                discountInput = $("<input style='width: 50px;'>").attr("type", "text").val("0").addClass("discountPercent").attr("placeholder", "%"),
                detailsTableBody = $("#detailsTable tbody");

            var bpVal = parseFloat(buyingPrice.val());
            var qtyVal = parseInt(quantity);
            var discVal = parseFloat(discountInput.val());
            var discPercentVal = parseFloat(discountInput.val());
            if (isNaN(bpVal)) bpVal = 0;
            if (isNaN(qtyVal)) qtyVal = 0;
            if (isNaN(discPercentVal)) discPercentVal = 0;

            // Calculate discount amount based on percentage
            var discountAmount = ((bpVal * qtyVal) * (discPercentVal / 100)).toFixed(2);
            var amount = ((bpVal * qtyVal) - discountAmount).toFixed(2);

            var productItem = $("<tr>")
                .append($("<td>").text(productId))
                .append($("<td>").text(productName))
                .append($("<td>").append(batchNumber))
                .append($("<td>").append(expiryDate))
                .append($("<td>").append(buyingPrice))
                .append($("<td>").append(sellingPrice))
                .append($("<td>").append(discountInput))  // discount cell added here
                .append($("<td>").addClass("quantity").text(quantity))
                .append($("<td>").addClass("amount").text(amount))
                // Add a hidden cell to store the calculated discount amount
                .append($("<td>").addClass("discountAmount").css("display", "none").text(discountAmount))
                .append($("<td>").html('<a data-itemId="0" href="#" class="deleteItem"><i class="fa fa-trash"></i></a>'));
            detailsTableBody.append(productItem);

            // Define a helper to recalculate row amount
            function recalcRowAmount(row) {
                var bp = parseFloat(row.find("td:eq(4) input").val());
                var qty = parseInt(row.find("td:eq(7)").text());
                var discPercent = parseFloat(row.find("td:eq(6) input").val());
                if (isNaN(bp)) bp = 0;
                if (isNaN(qty)) qty = 0;
                if (isNaN(discPercent)) discPercent = 0;

                // Calculate the discount amount based on percentage
                var discAmount = (bp * qty) * (discPercent / 100);
                if (discAmount < 0) discAmount = 0;

                var newAmt = (bp * qty) - discAmount;
                if (newAmt < 0) newAmt = 0;

                row.find("td.amount").text(newAmt.toFixed(2));
                // Update the hidden discount amount cell
                row.find("td.discountAmount").text(discAmount.toFixed(2));
                calculateSum();
            }

            // Bind input events: recalc when buying price or discount changes
            buyingPrice.on("input", function () {
                recalcRowAmount($(this).closest("tr"));
            });
            discountInput.on("input", function () {
                recalcRowAmount($(this).closest("tr"));
            });

            calculateSum();

            // Clear input fields after adding
            $("#Quantity").val("");
            $("#BatchNumber").val("");
            $("#ExpiryDate").val("");
            $("#BuyingPrice").val("");
            $("#SellingPrice").val("");

        }
    });
    //$(document).on("input", "#BuyingPrice", function () {
    //    calculateSum();
    //});
    $("#BtnSave").click(function (e) {
        e.preventDefault();

        // Disable the button to prevent multiple clicks
        $(this).prop('disabled', true);

        if (submitValidation()) {
            var orderArr = [];
            orderArr.length = 0;
            $.each($("#detailsTable tbody tr"), function () {
                var $row = $(this);
                var medicineId = parseInt($row.find("td:eq(0)").text());
                var medicineName = $row.find("td:eq(1)").text();
                var batchNo = $row.find("td:eq(2) input").val();
                var rawDate = $row.find("td:eq(3) input").length > 0 ? $row.find("td:eq(3) input").val() : $row.find("td:eq(3)").text();
                // (Format rawDate from dd/mm/yyyy to ISO as in your existing code)

                var parts = rawDate.split("/");
                var expiryDateOriginal = new Date(parts[2], parts[1] - 1, parts[0]);
                var expiryDate = `${expiryDateOriginal.getFullYear()}-${(expiryDateOriginal.getMonth() + 1).toString().padStart(2, '0')}-${expiryDateOriginal.getDate().toString().padStart(2, '0')}T${expiryDateOriginal.getHours().toString().padStart(2, '0')}:${expiryDateOriginal.getMinutes().toString().padStart(2, '0')}:${expiryDateOriginal.getSeconds().toString().padStart(2, '0')}`;

                var buyingPrice = parseFloat($row.find("td:eq(4)").find("input").val());
                var sellingPrice = parseFloat($row.find("td:eq(5)").find("input").val());

                // Get discount amount from the hidden cell instead of calculating from percentage here
                var discount = parseFloat($row.find("td.discountAmount").text());

                var quantity = parseInt($row.find("td:eq(7)").text());
                var amount = parseFloat($row.find("td:eq(8)").text());

                orderArr.push({
                    MedicineId: medicineId,
                    MedicineName: medicineName,
                    BatchNo: batchNo,
                    ExpiryDate: expiryDate, // Convert to appropriate ISO format
                    BuyingPrice: buyingPrice,
                    SellingPrice: sellingPrice,
                    Quantity: quantity,
                    Discount: discount,   // new per-item discount field
                    Amount: amount,
                    Price: buyingPrice    // or whichever unit price you wish to pass
                });
            });

            var supplierId = parseInt($("#Supplier").val());
            var rawDate = $("#Date").val();
            var parts = rawDate.split("/");
            var purchaseDate = new Date(parts[2], parts[1] - 1, parts[0]);
            var customFormattedDate = `${purchaseDate.getFullYear()}-${(purchaseDate.getMonth() + 1).toString().padStart(2, '0')}-${purchaseDate.getDate().toString().padStart(2, '0')}T${purchaseDate.getHours().toString().padStart(2, '0')}:${purchaseDate.getMinutes().toString().padStart(2, '0')}:${purchaseDate.getSeconds().toString().padStart(2, '0')}`;
            var paymentMethodId = parseInt($("#PaymentMethod").val());
            var total = parseFloat($("#SubTotal").text());
            var notes = $("#Notes").val();
            var purchaseStatusId = parseInt($("#PurchaseStatus").val());
            var grandTotal = parseFloat($("#GrandTotal").text()).toFixed(2);

            var data = JSON.stringify({
                Id: $("#BtnUpdate").attr("data-purchase-id"),
                SupplierId: supplierId,
                PurchaseDate: customFormattedDate,
                PaymentMethodId: paymentMethodId,
                Total: total,
                Notes: notes,
                PurchaseStatusId: purchaseStatusId,
                Discount: 0,
                GrandTotal: grandTotal,
                Items: orderArr,
            });
            console.log(data);
            $.when(saveOrder(data))
                .then(function (response) {
                    //toastr.success(response.message);
                    location.href = "/Purchase/Index";
                })
                .fail(function (err) {
                    //toastr.error(response.message);
                    console.log(err);
                })
                .always(function () {
                    // Re-enable the button after the request is complete
                    $("#BtnSave").prop('disabled', false);
                });
        } else {
            // Re-enable the button if validation fails
            $("#BtnSave").prop('disabled', false);
        }
    });
    $("#BtnUpdate").click(function (e) {
        e.preventDefault();
        if (submitValidation()) {
            var orderArr = [];
            orderArr.length = 0;
            $.each($("#detailsTable tbody tr"), function () {
                
                var $row = $(this);
                var medicineId = parseInt($row.find("td:eq(0)").text());
                var medicineName = $row.find("td:eq(1)").text();
                var batchNo = $row.find("td:eq(2) input").length > 0
                    ? $row.find("td:eq(2) input").val()
                    : $row.find("td:eq(2)").text();
                var rawDateData = $row.find("td:eq(3) input").length > 0
                    ? $row.find("td:eq(3) input").val()
                    : $row.find("td:eq(3)").text();
                var buyingPrice = $row.find("td:eq(4) input").length > 0
                    ? parseFloat($row.find("td:eq(4) input").val())
                    : parseFloat($row.find("td:eq(4)").text());
                var sellingPrice = $row.find("td:eq(5) input").length > 0
                    ? parseFloat($row.find("td:eq(5) input").val())
                    : parseFloat($row.find("td:eq(5)").text());

                // For update, we need to handle both new rows with hidden discount amount and existing rows
                var discountAmount;

                // If we have the hidden cell, use that value
                if ($row.find("td.discountAmount").length > 0) {
                    discountAmount = parseFloat($row.find("td.discountAmount").text());
                } else {
                    // For existing rows, calculate from the percentage in the input
                    var discPercent = $row.find("td:eq(6) input").length > 0
                        ? parseFloat($row.find("td:eq(6) input").val())
                        : 0;

                    var qty = parseInt($row.find("td:eq(7)").text());
                    discountAmount = (buyingPrice * qty) * (discPercent / 100);
                }

                var quantity = parseInt($row.find("td:eq(7)").text());
                var amount = parseFloat($row.find("td:eq(8)").text());

                var rawDate = rawDateData;
                var parts = rawDate.split("/");
                var expiryDateOriginal = new Date(parts[2], parts[1] - 1, parts[0]);
                var expiryDate = `${expiryDateOriginal.getFullYear()}-${(expiryDateOriginal.getMonth() + 1).toString().padStart(2, '0')}-${expiryDateOriginal.getDate().toString().padStart(2, '0')}T${expiryDateOriginal.getHours().toString().padStart(2, '0')}:${expiryDateOriginal.getMinutes().toString().padStart(2, '0')}:${expiryDateOriginal.getSeconds().toString().padStart(2, '0')}`;
                

                orderArr.push({
                    MedicineId: medicineId,
                    MedicineName: medicineName,
                    BatchNo: batchNo,
                    ExpiryDate: expiryDate,
                    BuyingPrice: buyingPrice,
                    SellingPrice: sellingPrice,
                    Quantity: quantity,
                    Discount: discountAmount,
                    Amount: amount,
                    Price: buyingPrice
                });

            });

            var supplierId = parseInt($("#Supplier").val());
            var rawDate = $("#Date").val();
            var parts = rawDate.split("/");
            var purchaseDate = new Date(parts[2], parts[1] - 1, parts[0]);
            var customFormattedDate = `${purchaseDate.getFullYear()}-${(purchaseDate.getMonth() + 1).toString().padStart(2, '0')}-${purchaseDate.getDate().toString().padStart(2, '0')}T${purchaseDate.getHours().toString().padStart(2, '0')}:${purchaseDate.getMinutes().toString().padStart(2, '0')}:${purchaseDate.getSeconds().toString().padStart(2, '0')}`;
            var paymentMethodId = parseInt($("#PaymentMethod").val());
            var total = parseFloat($("#SubTotal").text());
            var notes = $("#Notes").val();
            var purchaseStatusId = parseInt($("#PurchaseStatus").val());
            var discount = 0;          
            var grandTotal = parseFloat($("#GrandTotal").text()).toFixed(2);

            var data = JSON.stringify({
                Id: $("#BtnUpdate").attr("data-purchase-id"),
                SupplierId: supplierId,
                PurchaseDate: customFormattedDate,
                PaymentMethodId: paymentMethodId,
                Total: total,
                Notes: notes,
                PurchaseStatusId: purchaseStatusId,
                Discount: 0,
                GrandTotal: grandTotal,
                Items: orderArr,
            });
            console.log(data);
            $.when(UpdateOrder(data))
                .then(function (response) {
                    //console.log(response);
                    //toastr.success(response.message);
                    location.href = "/Purchase/Index";
                })
                .fail(function (err) {
                    //toastr.error(response.message);
                    console.log(err);
                });
        }
    });
    function saveOrder(data) {
        return $.ajax({
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST",
            url: "/Purchase/Purchase",
            data: data,
        });
    }
    function UpdateOrder(data) {
        return $.ajax({
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST",
            url: "/Purchase/UpdatePurchase",
            data: data,
        });
    }
    $(document).on("click", "a.deleteItem", function (e) {
        e.preventDefault();
        var $self = $(this);
        $(this)
            .parents("tr")
            .css("background-color", "#1f306f")
            .fadeOut(800, function () {
                $(this).remove();
                calculateSum();
                blankme("SubTotal");
            });
    });
});

// functions
function LoadMedicineDetails(id) {
    if (id == "") {
        var details = $("#medicineDetails").html(``);
        return;
    }
    $.ajax({
        type: "GET",
        url: "/Medicine/GetMedicinesById",
        dataType: "Json",
        data: { id: id },
        success: function (data) {
            $.each(data, function (index, value) {
                var details = $("#medicineDetails").html(
                    `<p style="font-size:11px">Buying price : <strong>${value.buyingPrice}</strong>, Batch No: <strong>${value.batchNo}</strong> </p>`
                );
                currentItemPrice = value.buyingPrice;
                console.log(value.buyingPrice, value.batchNo);
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });
}
function calculateSum() {
    var sum = 0;
    $(".amount").each(function () {
        var value = $(this).text();
        if (!isNaN(value) && value.length !== 0) {
            sum += parseFloat(value);
        }
    });
    if (sum == 0.0) {
        $("#Discount").text("0");
        $("#GrandTotal").text("0");
    }
    $("#SubTotal").text(sum.toFixed(2));
    $("#GrandTotal").text(sum.toFixed(2));

    var b = parseFloat($("#Discount").val()).toFixed(2);
    if (isNaN(b)) return;
    var a = parseFloat($("#SubTotal").text()).toFixed(2);
    $("#GrandTotal").text(a - b);
}
$(".amount").each(function () {
    calculateSum();
});
function DiscountAmount() {
    blankme("Discount");
    blankme("GrandTotal");
    var b = parseFloat($("#Discount").val()).toFixed(2);
    if (isNaN(b)) return;
    var a = parseFloat($("#SubTotal").text()).toFixed(2);
    $("#GrandTotal").text(a - b);
}
function add_validation() {
    var medicine = document.getElementById("Medicine").value;
    var quantity = document.getElementById("Quantity").value;
    if (quantity == "" || medicine == "") {
        if (quantity == "") {
            document.getElementById("error_Quantity").style.display = "block";
        } else {
            document.getElementById("error_Quantity").style.display = "none";
        }
        if (medicine == "") {
            document.getElementById("error_Medicine").style.display = "block";
        } else {
            document.getElementById("error_Medicine").style.display = "none";
        }
        return !1;
    } else {
        return !0;
    }
}
function blankme(id) {
    var val = document.getElementById(id).value;
    var error_id = "error_" + id;
    if (val == "" || val === 0.0) {
        document.getElementById(error_id).style.display = "block";
    } else {
        document.getElementById(error_id).style.display = "none";
    }
}
function submitValidation() {
    var supplier = document.getElementById("Supplier").value;
    //var purcahseCode = document.getElementById("Code").value;
    var purcahseDate = document.getElementById("Date").value;
    var paymentmethod = document.getElementById("PaymentMethod").value;
    var pStaus = document.getElementById("PurchaseStatus").value;
    var total = parseFloat($("#SubTotal").text());
    var gtotal = parseFloat($("#GrandTotal").text());
    if (
        supplier == "" ||
        pStaus == "" ||
        purcahseDate == "" ||
        paymentmethod == "" ||
        total == "" ||
        total == 0.0 ||
        isNaN(total) ||
        gtotal == "" ||
        gtotal == 0.0 ||
        isNaN(gtotal)
    ) {
        if (pStaus == "") {
            document.getElementById("error_PurchaseStatus").style.display = "block";
        } else {
            document.getElementById("error_PurchaseStatus").style.display = "none";
        }
        if (supplier == "") {
            document.getElementById("error_Supplier").style.display = "block";
        } else {
            document.getElementById("error_Supplier").style.display = "none";
        }
        if (purcahseDate == "") {
            document.getElementById("error_Date").style.display = "block";
        } else {
            document.getElementById("error_Date").style.display = "none";
        }
        if (paymentmethod == "") {
            document.getElementById("error_PaymentMethod").style.display = "block";
        } else {
            document.getElementById("error_PaymentMethod").style.display = "none";
        }
        if (total == "" || total === 0.0 || isNaN(total)) {
            document.getElementById("error_SubTotal").style.display = "block";
        } else {
            document.getElementById("error_SubTotal").style.display = "none";
        }
        if (gtotal == "" || gtotal === 0.0 || isNaN(gtotal)) {
            document.getElementById("error_GrandTotal").style.display = "block";
        } else {
            document.getElementById("error_GrandTotal").style.display = "none";
        }
        return !1;
    } else {
        return !0;
    }
}
