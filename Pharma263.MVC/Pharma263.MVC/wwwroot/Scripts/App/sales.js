var currentItemPrice;
var tempStock;
$.datepicker.setDefaults({
    dateFormat: "dd/mm/yy"
});
$(".mydatepicker").datepicker({
    dateFormat: "dd/mm/yy",
    autoclose: true,
    onSelect: function (dateText, inst) {
        // Parse the selected date
        var selectedDate = $.datepicker.parseDate("dd/mm/yy", dateText);

        // Get the current time
        var currentTime = new Date();

        // Set the time part of the selected date to the current time
        selectedDate.setHours(currentTime.getHours());
        selectedDate.setMinutes(currentTime.getMinutes());
        selectedDate.setSeconds(currentTime.getSeconds());

        // Update the input field value with the adjusted date
        $(this).val($.datepicker.formatDate("dd/mm/yy", selectedDate));
    }
});
$.ajax({
    type: "GET",
    url: "/Customer/GetCustomer",
    datatype: "Json",
    success: function (data) {
        // Clear existing options from select element
        $('#Customer').empty();
        // Add empty option as first option
        $('#Customer').append($('<option>').text('Select Customer').attr('value', ''));
        $.each(data, function (index, value) {
            $('#Customer').append($('<option>').text(value.name).attr('value', value.id));
            //$("#Customer").append('<option value="' + value.Id + '">' + value.Name + "</option>");            
        });
        // Initialize Select2 plugin on select element
        $('#Customer').select2();
    },
});

$.ajax({
    type: "GET",
    url: "/Admin/GetStocks",
    datatype: "Json",
    success: function (data) {
        $("#SMedicine").empty();
        $('#SMedicine').append($('<option>').text('Select Medicine').attr('value', ''));
        $.each(data, function (index, value) {
            $("#SMedicine").append($('<option>').text(value.name).attr('value', value.id));
        });
        $('#SMedicine').select2();
        LoadStockMedicineDetails($("#SMedicine option:selected").val());
    },
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
    url: "/Admin/GetSaleStatus",
    datatype: "Json",
    success: function (data) {
        $.each(data, function (index, value) {
            $("#SaleStatus").append(
                '<option value="' + value.id + '">' + value.name + "</option>"
            );
        });
    },
});

$("#SMedicine").change(function () {
    LoadStockMedicineDetails($("#SMedicine option:selected").val());
    blankme("SMedicine");
});
$("#SQuantity").change(function () {
    blankme("SQuantity");
    stockQuantityCheckerVal();
});
tempOrder = [];
var a = 1;
$("#SaleaddToList").click(function (e) {
    e.preventDefault();
    if (Saleadd_validation() && stockQuantityCheckerVal()) {
        var productId = $("#SMedicine option:selected").val(),
            productName = $("#SMedicine option:selected").text(),
            price = currentItemPrice,
            quantity = $("#SQuantity").val(),
            // Change this to use percentage instead of amount
            discountPercent = parseFloat($("#SDiscountPercent").val()) || 0,
            detailsTableBody = $("#detailsTable tbody");

        // Calculate the actual discount amount based on percentage
        var subtotal = parseFloat(price) * parseInt(quantity);
        var discountAmount = subtotal * (discountPercent / 100);
        var netAmount = subtotal - discountAmount;

        var data = {
            id: parseInt(productId),
            ref: a++,
            quantity: parseInt(quantity),
        };
        tempOrder.push(data);
        console.log(tempOrder);

        var productItem =
            `<tr>
                <td>${productId}</td>
                <td>${productName}</td>
                <td>${parseFloat(price).toFixed(2)}</td>
                <td><input type="text" value="${discountPercent.toFixed(2)}" class="discount-percent-input" placeholder="%" /></td>
                <td>${quantity}</td>
                <td class="amount">${netAmount.toFixed(2)}</td>
                <td class="discount-amount" style="display:none;">${discountAmount.toFixed(2)}</td>
                <td><a data-itemId="${productId}" href="#" class="sdeleteItem"><i class="fa fa-trash"></i></a></td>
            </tr>`;

        detailsTableBody.append(productItem);

        // Bind event handlers for the new row
        detailsTableBody.find("tr:last .discount-percent-input").on("input", function () {
            updateRowAmountFromPercent($(this).closest("tr"));
        });

        SalecalculateSum();

        $("#SQuantity").val("");
        $("#SDiscountPercent").val("");
        LoadStockMedicineDetails($("#SMedicine option:selected").val());
        blankme("SSubTotal");
        blankme("SGrandTotal");
    }
});

function updateRowAmountFromPercent(row) {
    var price = parseFloat(row.find("td:eq(2)").text());
    var quantity = parseInt(row.find("td:eq(4)").text());
    var discountPercent = parseFloat(row.find(".discount-percent-input").val()) || 0;

    // Calculate discount amount from percentage
    var subtotal = price * quantity;
    var discountAmount = subtotal * (discountPercent / 100);

    // Ensure discount isn't more than subtotal
    if (discountAmount > subtotal) {
        discountAmount = subtotal;
    }

    var netAmount = subtotal - discountAmount;

    // Update the visible amount and hidden discount amount
    row.find(".amount").text(netAmount.toFixed(2));

    // Update or create the hidden discount amount cell
    if (row.find(".discount-amount").length > 0) {
        row.find(".discount-amount").text(discountAmount.toFixed(2));
    } else {
        row.append($("<td>").addClass("discount-amount").css("display", "none").text(discountAmount.toFixed(2)));
    }

    // Recalculate totals
    SalecalculateSum();
}

$("#SBtnSave").click(function (e) {
    e.preventDefault();

    // Disable the button to prevent multiple clicks
    $(this).prop('disabled', true);

    if (submitValidation()) {
        var orderArr = [];
        orderArr.length = 0;
        $.each($("#detailsTable tbody tr"), function () {
            orderArr.push({
                StockId: parseInt($(this).find("td:eq(0)").html()),
                MedicineName: $(this).find("td:eq(1)").html(),
                Price: parseFloat($(this).find("td:eq(2)").html()),
                // Use the hidden discount amount instead of the visible percentage
                Discount: parseFloat($(this).find(".discount-amount").text()) || 0,
                Quantity: parseInt($(this).find("td:eq(4)").html()),
                Amount: parseFloat($(this).find("td:eq(5)").html()),
            });
        });

        var customerId = parseInt($("#Customer").val());
        var rawDate = $("#SDate").val();
        var parts = rawDate.split("/");
        var saleDateOriginal = new Date(parts[2], parts[1] - 1, parts[0]);
        var saleDate = `${saleDateOriginal.getFullYear()}-${(saleDateOriginal.getMonth() + 1).toString().padStart(2, '0')}-${saleDateOriginal.getDate().toString().padStart(2, '0')}T${saleDateOriginal.getHours().toString().padStart(2, '0')}:${saleDateOriginal.getMinutes().toString().padStart(2, '0')}:${saleDateOriginal.getSeconds().toString().padStart(2, '0')}`;

        var paymentMethodId = parseInt($("#PaymentMethod").val());
        var total = parseFloat($("#SSubTotal").text());
        var notes = $("#SNotes").val();
        var saleStatusId = parseInt($("#SaleStatus").val());
        var discount = 0;
        var grandTotal = parseFloat($("#SGrandTotal").text());

        var data = JSON.stringify({
            CustomerId: customerId,
            SalesDate: saleDate,
            PaymentMethodId: paymentMethodId,
            Total: total,
            Notes: notes,
            SaleStatusId: saleStatusId,
            Discount: discount,
            GrandTotal: grandTotal,
            Items: orderArr,
        });
        console.log(data);
        $.when(saveSale(data))
            .then(function (response) {
                //console.log(response);
                //toastr.success(response.message);
                location.href = "/Sale/Index";
            })
            .fail(function (err) {
                toastr.error(response.message);
            })
            .always(function () {
                // Re-enable the button after the request is complete
                $("#SBtnSave").prop('disabled', false);
            });
    } else {
        // Re-enable the button if validation fails
        $("#SBtnSave").prop('disabled', false);
    }
});

$("#QBtnSave").click(function (e) {
    e.preventDefault();

    // Disable the button to prevent multiple clicks
    $(this).prop('disabled', true);

    if (submitQuotationValidation()) {
        var orderArr = [];
        orderArr.length = 0;
        $.each($("#detailsTable tbody tr"), function () {
            orderArr.push({
                StockId: parseInt($(this).find("td:eq(0)").html()),
                MedicineName: $(this).find("td:eq(1)").html(),
                Price: parseFloat($(this).find("td:eq(2)").html()),
                // Use the hidden discount amount instead of the visible percentage
                Discount: parseFloat($(this).find(".discount-amount").text()) || 0,
                Quantity: parseInt($(this).find("td:eq(4)").html()),
                Amount: parseFloat($(this).find("td:eq(5)").html()),
            });
        });

        var customerId = parseInt($("#Customer").val());
        var rawDate = $("#SDate").val();
        var parts = rawDate.split("/");
        var saleDateOriginal = new Date(parts[2], parts[1] - 1, parts[0]);
        var saleDate = `${saleDateOriginal.getFullYear()}-${(saleDateOriginal.getMonth() + 1).toString().padStart(2, '0')}-${saleDateOriginal.getDate().toString().padStart(2, '0')}T${saleDateOriginal.getHours().toString().padStart(2, '0')}:${saleDateOriginal.getMinutes().toString().padStart(2, '0')}:${saleDateOriginal.getSeconds().toString().padStart(2, '0')}`;

        var total = parseFloat($("#SSubTotal").text());
        var notes = $("#SNotes").val();
        var discount = parseFloat($("#SDiscount").val());
        var grandTotal = parseFloat($("#SGrandTotal").text());

        var data = JSON.stringify({
            CustomerId: customerId,
            QuotationDate: saleDate,
            Total: total,
            Notes: notes,
            Discount: discount,
            GrandTotal: grandTotal,
            Items: orderArr,
        });
        console.log(data);
        $.when(saveQuotation(data))
            .then(function (response) {
                //console.log(response);
                //toastr.success(response.message);
                location.href = "/Quotation/Index";
            })
            .fail(function (err) {
                toastr.error(response.message);
            })
            .always(function () {
                // Re-enable the button after the request is complete
                $("#QBtnSave").prop('disabled', false);
            });
    } else {
        // Re-enable the button if validation fails
        $("#QBtnSave").prop('disabled', false);
    }
});

$("#SBtnUpdate").click(function (e) {
    e.preventDefault();
    if (submitValidation()) {
        var orderArr = [];
        orderArr.length = 0;
        $.each($("#detailsTable tbody tr"), function () {
            orderArr.push({
                StockId: parseInt($(this).find("td:eq(0)").html()),
                MedicineName: $(this).find("td:eq(1)").html(),
                Price: parseFloat($(this).find("td:eq(2)").html()),
                Quantity: parseInt($(this).find("td:eq(3)").html()),
                Amount: parseFloat($(this).find("td:eq(4)").html()),
            });
        });
        var rawDate = $("#SDate").val();
        var parts = rawDate.split("/");
        var saleDateOriginal = new Date(parts[2], parts[1] - 1, parts[0]);
        var saleDate = `${saleDateOriginal.getFullYear()}-${(saleDateOriginal.getMonth() + 1).toString().padStart(2, '0')}-${saleDateOriginal.getDate().toString().padStart(2, '0')}T${saleDateOriginal.getHours().toString().padStart(2, '0')}:${saleDateOriginal.getMinutes().toString().padStart(2, '0')}:${saleDateOriginal.getSeconds().toString().padStart(2, '0')}`;

        var data = JSON.stringify({
            Id: $("#SBtnUpdate").attr("data-sale-Id"),
            CustomerId: parseInt($("#Customer").val()),
            //SaleCode: $("#SCode").val(),
            SalesDate: saleDate,
            PaymentMethodId: parseInt($("#PaymentMethod").val()),
            Total: parseFloat($("#SSubTotal").text()),
            Notes: $("#SNotes").val(),
            SaleStatusId: parseInt($("#SaleStatus").val()),
            Discount: parseFloat($("#SDiscount").val()).toFixed(2),
            GrandTotal: parseFloat($("#SGrandTotal").text()).toFixed(2),
            Items: orderArr,
        });
        console.log(data);
        $.when(UpdateSale(data))
            .then(function (response) {
                //console.log(response);
                //toastr.success(response.message);
                location.href = "/Sale/Index";
            })
            .fail(function (err) {
                toastr.error(response.message);
            });
    }
});

$("#QBtnUpdate").click(function (e) {
    e.preventDefault();
    if (submitQuotationValidation()) {
        var orderArr = [];
        orderArr.length = 0;
        $.each($("#detailsTable tbody tr"), function () {
            orderArr.push({
                StockId: parseInt($(this).find("td:eq(0)").html()),
                MedicineName: $(this).find("td:eq(1)").html(),
                Price: parseFloat($(this).find("td:eq(2)").html()),
                // Use the hidden discount amount instead of the visible percentage
                Discount: parseFloat($(this).find(".discount-amount").text()) || 0,
                Quantity: parseInt($(this).find("td:eq(4)").html()),
                Amount: parseFloat($(this).find("td:eq(5)").html()),
            });
        });
        var rawDate = $("#SDate").val();
        var parts = rawDate.split("/");
        var saleDateOriginal = new Date(parts[2], parts[1] - 1, parts[0]);
        var saleDate = `${saleDateOriginal.getFullYear()}-${(saleDateOriginal.getMonth() + 1).toString().padStart(2, '0')}-${saleDateOriginal.getDate().toString().padStart(2, '0')}T${saleDateOriginal.getHours().toString().padStart(2, '0')}:${saleDateOriginal.getMinutes().toString().padStart(2, '0')}:${saleDateOriginal.getSeconds().toString().padStart(2, '0')}`;

        var data = JSON.stringify({
            Id: $("#QBtnUpdate").attr("data-quotation-Id"),
            CustomerId: parseInt($("#Customer").val()),
            //SaleCode: $("#SCode").val(),
            QuotationDate: saleDate,
            //PaymentMethodId: parseInt($("#PaymentMethod").val()),
            Total: parseFloat($("#SSubTotal").text()),
            Notes: $("#SNotes").val(),
            //SaleStatusId: parseInt($("#SaleStatus").val()),
            Discount: parseFloat($("#SDiscount").val()).toFixed(2),
            GrandTotal: parseFloat($("#SGrandTotal").text()).toFixed(2),
            Items: orderArr,
        });
        console.log(data);
        $.when(UpdateQuotation(data))
            .then(function (response) {
                //console.log(response);
                //toastr.success(response.message);
                location.href = "/Quotation/Index";
            })
            .fail(function (err) {
                toastr.error(response.message);
            });
    }
});



$(document).on("click", "a.sdeleteItem", function (e) {
    e.preventDefault();
    var $self = $(this);
    var ref = $(this).attr("data-itemid");
    var index = tempOrder.findIndex(function (a) {
        return a.ref == ref;
    });
    if (index > -1) {
        tempOrder.splice(index, 1);
    }
    $(this)
        .parents("tr")
        .css("background-color", "#1f306f")
        .fadeOut(800, function () {
            $(this).remove();
            SalecalculateSum();
            blankme("SSubTotal");
            LoadStockMedicineDetails($("#SMedicine option:selected").val());
        });
});
function saveSale(data) {
    return $.ajax({
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        url: "/Sale/AddSale",
        data: data,
    });
}

function saveQuotation(data) {
    return $.ajax({
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        url: "/Quotation/AddQuotation",
        data: data,
    });
}
function UpdateSale(data) {
    return $.ajax({
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        url: "/Sale/EditSale",
        data: data,
    });
}

function UpdateQuotation(data) {
    return $.ajax({
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        url: "/Quotation/EditQuotation",
        data: data,
    });
}
function SalecalculateSum() {
    var total = 0;
    var grandTotal = 0;

    $("#detailsTable tbody tr").each(function () {
        var price = parseFloat($(this).find("td:eq(2)").text());
        var quantity = parseInt($(this).find("td:eq(4)").text());
        var discountPercent = parseFloat($(this).find(".discount-percent-input").val()) || 0;

        // Calculate discount amount from percentage
        var subtotal = price * quantity;
        var discountAmount = subtotal * (discountPercent / 100);

        // Ensure discount isn't more than subtotal
        if (discountAmount > subtotal) {
            discountAmount = subtotal;
        }

        var amount = subtotal - discountAmount;

        $(this).find("td.amount").text(amount.toFixed(2));

        // Update or create the hidden discount amount
        if ($(this).find(".discount-amount").length > 0) {
            $(this).find(".discount-amount").text(discountAmount.toFixed(2));
        } else {
            $(this).append($("<td>").addClass("discount-amount").css("display", "none").text(discountAmount.toFixed(2)));
        }

        total += subtotal;
        grandTotal += amount;
    });

    $("#SSubTotal").text(total.toFixed(2));
    $("#SGrandTotal").text(grandTotal.toFixed(2));
}
function Saleadd_validation() {
    var medicine = document.getElementById("SMedicine").value;
    var quantity = document.getElementById("SQuantity").value;
    if (quantity == "" || medicine == "") {
        if (quantity == "") {
            document.getElementById("error_SQuantity").style.display = "block";
        } else {
            document.getElementById("error_SQuantity").style.display = "none";
        }
        if (medicine == "") {
            document.getElementById("error_SMedicine").style.display = "block";
        } else {
            document.getElementById("error_SMedicine").style.display = "none";
        }
        return !1;
    } else {
        return !0;
    }
}

function updateRowAmount(row) {
    var price = parseFloat(row.find("td:eq(2)").text());
    var quantity = parseInt(row.find(".quantity-input").val()) || 0;
    var discount = parseFloat(row.find(".discount-input").val()) || 0;

    // Recalculate the row amount
    var netAmount = (price * quantity) - discount;
    if (netAmount < 0) netAmount = 0; // Ensure no negative amounts

    // Update the amount cell
    row.find(".amount").text(netAmount.toFixed(2));

    // Recalculate totals
    SalecalculateSum();
}

function SDiscountAmount() {
    blankme("SDiscount");
    blankme("SGrandTotal");
    var b = parseFloat($("#SDiscount").val()).toFixed(2);
    if (isNaN(b)) return;
    var a = parseFloat($("#SSubTotal").text()).toFixed(2);
    $("#SGrandTotal").text(a - b);
}
function submitValidation() {
    var customer = document.getElementById("Customer").value;
    //var saleCode = document.getElementById("SCode").value;
    var saleDate = document.getElementById("SDate").value;
    var paymentmethod = document.getElementById("PaymentMethod").value;
    var sStaus = document.getElementById("SaleStatus").value;
    var total = parseFloat($("#SSubTotal").text());
    var gtotal = parseFloat($("#SGrandTotal").text());
    if (
        customer == "" ||
        sStaus == "" ||
        saleDate == "" ||
        paymentmethod == "" ||
        total == "" ||
        total == 0.0 ||
        isNaN(total) ||
        gtotal == "" ||
        gtotal == 0.0 ||
        isNaN(gtotal)
    ) {
        if (sStaus == "") {
            document.getElementById("error_SaleStatus").style.display = "block";
        } else {
            document.getElementById("error_SaleStatus").style.display = "none";
        }
        if (customer == "") {
            document.getElementById("error_Customer").style.display = "block";
        } else {
            document.getElementById("error_Customer").style.display = "none";
        }
        if (saleDate == "") {
            document.getElementById("error_SDate").style.display = "block";
        } else {
            document.getElementById("error_SDate").style.display = "none";
        }
        if (paymentmethod == "") {
            document.getElementById("error_PaymentMethod").style.display = "block";
        } else {
            document.getElementById("error_PaymentMethod").style.display = "none";
        }
        if (total == "" || total === 0.0 || isNaN(total)) {
            document.getElementById("error_SSubTotal").style.display = "block";
        } else {
            document.getElementById("error_SSubTotal").style.display = "none";
        }
        if (gtotal == "" || gtotal === 0.0 || isNaN(gtotal)) {
            document.getElementById("error_SGrandTotal").style.display = "block";
        } else {
            document.getElementById("error_SGrandTotal").style.display = "none";
        }
        return !1;
    } else {
        return !0;
    }
}

function submitQuotationValidation() {
    var customer = document.getElementById("Customer").value;
    //var saleCode = document.getElementById("SCode").value;
    var saleDate = document.getElementById("SDate").value;
    //var paymentmethod = document.getElementById("PaymentMethod").value;
    //var sStaus = document.getElementById("SaleStatus").value;
    var total = parseFloat($("#SSubTotal").text());
    var gtotal = parseFloat($("#SGrandTotal").text());
    if (
        customer == "" ||
        /*sStaus == "" ||*/
        saleDate == "" ||
        /*paymentmethod == "" ||*/
        total == "" ||
        total == 0.0 ||
        isNaN(total) ||
        gtotal == "" ||
        gtotal == 0.0 ||
        isNaN(gtotal)
    ) {
        //if (sStaus == "") {
        //    document.getElementById("error_SaleStatus").style.display = "block";
        //} else {
        //    document.getElementById("error_SaleStatus").style.display = "none";
        //}
        if (customer == "") {
            document.getElementById("error_Customer").style.display = "block";
        } else {
            document.getElementById("error_Customer").style.display = "none";
        }
        if (saleDate == "") {
            document.getElementById("error_SDate").style.display = "block";
        } else {
            document.getElementById("error_SDate").style.display = "none";
        }
        //if (paymentmethod == "") {
        //    document.getElementById("error_PaymentMethod").style.display = "block";
        //} else {
        //    document.getElementById("error_PaymentMethod").style.display = "none";
        //}
        if (total == "" || total === 0.0 || isNaN(total)) {
            document.getElementById("error_SSubTotal").style.display = "block";
        } else {
            document.getElementById("error_SSubTotal").style.display = "none";
        }
        if (gtotal == "" || gtotal === 0.0 || isNaN(gtotal)) {
            document.getElementById("error_SGrandTotal").style.display = "block";
        } else {
            document.getElementById("error_SGrandTotal").style.display = "none";
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
function stockQuantityCheckerVal() {
    var val = document.getElementById("SQuantity").value;
    if (tempStock == 0 || tempStock == "") return;
    if (val > tempStock) {
        document.getElementById("error_SQuantitycheck").style.display = "block";
        return !1;
    } else {
        document.getElementById("error_SQuantitycheck").style.display = "none";
        return !0;
    }
}
function tempStockCounter(id) {
    if (id == "" || id == 0) return 0;
    if (tempOrder.length == 0) return 0;
    var result = tempOrder
        .filter(function (a) {
            if (a.id == id) return a;
        })
        .reduce(function (a, b) {
            return a + b.quantity;
        }, 0);
    return result;
}
//function LoadStockMedicineDetails(id) {
//    if (id == "") {
//        var details = $("#medicineDetails").html(``);
//        return;
//    }
//    $.ajax({
//        type: "GET",
//        url: "/Medicine/GetStockMedicineById",
//        datatype: "Json",
//        data: { id: id },
//        success: function (data) {
//            $.each(data, function (index, value) {
//                var tempstockCount = tempStockCounter(value.id);
//                var details = $("#medicineDetails").html(
//                    `<p style="font-size:11px"> selling price : <strong>${value.sellingPrice
//                    }</strong>, Stock: <strong>${value.totalQuantity - tempstockCount
//                    }</strong> </p>`
//                );
//                currentItemPrice = value.sellingPrice;
//                origialStock = value.totalQuantity;
//                tempStock = value.TotalQuantity - tempstockCount;
//                console.log(tempstockCount);
//            });
//        },
//    });
//}
function LoadStockMedicineDetails(id) {
    if (id == "") {
        var details = $("#medicineDetails").html(``);
        return;
    }

    $.ajax({
        type: "GET",
        url: "/Medicine/GetStockMedicineById",
        datatype: "Json",
        data: { id: id },
        success: function (data) {
            if (data) {
                var value = data; // Assuming you expect a single object in the data array
                var sellingPrice = parseFloat(value.sellingPrice);
                var totalQuantity = parseInt(value.totalQuantity);
                if (!isNaN(sellingPrice) && !isNaN(totalQuantity)) {
                    var tempstockCount = tempStockCounter(value.id);
                    var details = $("#medicineDetails").html(
                        `<p style="font-size:11px"> selling price : <strong>${sellingPrice
                        }</strong>, Stock: <strong>${totalQuantity - tempstockCount
                        }</strong> </p>`
                    );
                    currentItemPrice = sellingPrice;
                    origialStock = totalQuantity;
                    tempStock = totalQuantity - tempstockCount;
                    console.log(tempstockCount);
                } else {
                    console.log("Invalid sellingPrice or totalQuantity.");
                }
            } else {
                console.log("No data returned from the server.");
            }
        },
        error: function (error) {
            console.log("Error in the AJAX request:", error);
        },
    });
}
